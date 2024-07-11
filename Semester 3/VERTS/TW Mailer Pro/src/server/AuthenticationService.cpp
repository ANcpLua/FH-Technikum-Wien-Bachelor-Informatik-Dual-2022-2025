#include "../shared/Logger.h"
#include "../shared/TwMailerExceptions.h"
#include "AuthenticationService.h"
#include <algorithm>
#include <chrono>
#include <cstring>
#include <filesystem>
#include <fstream>
#include <iomanip>
#include <ldap.h>
#include <sstream>
#include <stdexcept>
#include <vector>

namespace fs = std::filesystem;

AuthenticationService::AuthenticationService (
        const std::string &ldapUri, const std::string &ldapBindDnFormat,
        const Config &config)
        : ldapUri_ (ldapUri), ldapBindDnFormat_ (ldapBindDnFormat),
        ldapHandle_ (nullptr), config_ (config)
{
    int rc = ldap_initialize (&ldapHandle_, ldapUri_.c_str ());
    if (rc != LDAP_SUCCESS)
    {
        throw std::runtime_error ("Failed to initialize LDAP: "
                                  + std::string (ldap_err2string (rc)));
    }
    int version = LDAP_VERSION3;
    ldap_set_option (ldapHandle_, LDAP_OPT_PROTOCOL_VERSION, &version);
}

AuthenticationService::~AuthenticationService ()
{
    if (ldapHandle_)
    {
        ldap_unbind_ext (ldapHandle_, nullptr, nullptr);
    }
}

AuthenticationServiceImpl::AuthenticationServiceImpl (
        const std::string &ldapUri, const std::string &ldapBindDnFormat,
        const Config &config)
        : AuthenticationService (ldapUri, ldapBindDnFormat, config)
{
}

AuthenticationServiceImpl::~AuthenticationServiceImpl () { cleanup (); }

void
AuthenticationServiceImpl::initialize ()
{
    ensureBlacklistDirectoryExists ();
    loadBlacklist ();
}

void
AuthenticationServiceImpl::cleanup ()
{
    saveBlacklist ();
}

bool
AuthenticationServiceImpl::authenticate (const std::string &username,
                                         const SecureString &password,
                                         const std::string &clientIp)
{
    std::lock_guard<std::mutex> lock (mutex_);
    cleanupBlacklist ();

    if (isIpBlacklisted (clientIp))
    {
        Logger::getInstance ().log (LogLevel::WARNING,
                                    "Authentication attempt from blacklisted IP: "
                                    + clientIp);
        return false;
    }

    auto &attempts = loginAttempts_[clientIp];
    auto now = std::chrono::system_clock::now ();

    attempts.erase (std::remove_if (attempts.begin (), attempts.end (),
                                    [this, now] (const auto &attempt) {
                                        return now - attempt
                                               > config_.ATTEMPT_RESET_DURATION;
                                    }),
                    attempts.end ());

    int remainingAttempts = config_.MAX_LOGIN_ATTEMPTS - attempts.size ();

    if (remainingAttempts <= 0)
    {
        blacklistIp (clientIp);
        Logger::getInstance ().log (
                LogLevel::WARNING,
                "IP blacklisted due to too many failed attempts: " + clientIp);
        return false;
    }

    attempts.push_back (now);

    bool authenticated = ldapAuthenticate (username, password);
    if (authenticated)
    {
        loginAttempts_.erase (clientIp);
        Logger::getInstance ().log (
                LogLevel::INFO, "Successful authentication for user: " + username);
        createUserDirectory (username);
    }
    else
    {
        Logger::getInstance ().log (LogLevel::WARNING,
                                    "Failed authentication attempt for user: "
                                    + username + ". Remaining attempts: "
                                    + std::to_string (remainingAttempts - 1));
    }

    return authenticated;
}

void
AuthenticationServiceImpl::blacklistIp (const std::string &clientIp)
{
    blacklist_[clientIp] = std::chrono::system_clock::now ();
    saveBlacklist ();
}

bool
AuthenticationServiceImpl::isIpBlacklisted (const std::string &clientIp) const
{
    auto it = blacklist_.find (clientIp);
    if (it != blacklist_.end ())
    {
        return std::chrono::system_clock::now () - it->second
               <= config_.BLACKLIST_DURATION;
    }
    return false;
}

int
AuthenticationServiceImpl::getRemainingAttempts (
        const std::string &clientIp) const
{
    std::lock_guard<std::mutex> lock (mutex_);
    auto it = loginAttempts_.find (clientIp);
    if (it == loginAttempts_.end ())
    {
        return config_.MAX_LOGIN_ATTEMPTS;
    }
    return std::max (0, config_.MAX_LOGIN_ATTEMPTS
                        - static_cast<int> (it->second.size ()));
}

bool
AuthenticationServiceImpl::ldapAuthenticate (const std::string &username,
                                             const SecureString &password)
{
    std::string bindDn = ldapBindDnFormat_;
    size_t pos = bindDn.find ("%s");
    if (pos != std::string::npos)
    {
        bindDn.replace (pos, 2, username);
    }

    berval cred;
    cred.bv_val = const_cast<char *> (password.c_str ());

    cred.bv_len = password.length ();

    int rc = ldap_sasl_bind_s (ldapHandle_, bindDn.c_str (), LDAP_SASL_SIMPLE,
                               &cred, nullptr, nullptr, nullptr);

    Logger::getInstance ().log (
            LogLevel::DEBUG, "LDAP authentication attempt for user: " + username
                             + ", Result: " + std::string (ldap_err2string (rc)));

    return rc == LDAP_SUCCESS;
}

void
AuthenticationServiceImpl::cleanupBlacklist ()
{
    auto now = std::chrono::system_clock::now ();
    bool changed = false;

    for (auto it = blacklist_.begin (); it != blacklist_.end ();)
    {
        if (now - it->second > config_.BLACKLIST_DURATION)
        {
            it = blacklist_.erase (it);
            changed = true;
        }
        else
        {
            ++it;
        }
    }

    for (auto it = loginAttempts_.begin (); it != loginAttempts_.end ();)
    {
        it->second.erase (
                std::remove_if (it->second.begin (), it->second.end (),
                                [this, now] (const auto &attempt) {
                                    return now - attempt
                                           > config_.ATTEMPT_RESET_DURATION;
                                }),
                it->second.end ());
        if (it->second.empty ())
        {
            it = loginAttempts_.erase (it);
        }
        else
        {
            ++it;
        }
    }

    if (changed)
    {
        saveBlacklist ();
    }
}

void
AuthenticationServiceImpl::loadBlacklist ()
{
    std::ifstream file (config_.BLACKLIST_FILE_PATH);
    if (!file)
    {
        Logger::getInstance ().log (LogLevel::WARNING,
                                    "Could not open blacklist file for reading: "
                                    + config_.BLACKLIST_FILE_PATH);
        return;
    }

    std::string line;
    while (std::getline (file, line))
    {
        std::istringstream iss (line);
        std::string ip;
        std::string timeStr;
        if (iss >> ip >> timeStr)
        {
            std::tm tm = {};
            std::istringstream ss (timeStr);
            ss >> std::get_time (&tm, "%Y-%m-%d %H:%M:%S");
            auto timePoint
                    = std::chrono::system_clock::from_time_t (std::mktime (&tm));
            blacklist_[ip] = timePoint;
        }
    }
    Logger::getInstance ().log (LogLevel::INFO,
                                "Loaded IP blacklist from file: "
                                + config_.BLACKLIST_FILE_PATH);
}

void
AuthenticationServiceImpl::saveBlacklist () const
{
    std::ofstream file (config_.BLACKLIST_FILE_PATH);
    if (!file)
    {
        Logger::getInstance ().log (LogLevel::ERROR,
                                    "Could not open blacklist file for writing: "
                                    + config_.BLACKLIST_FILE_PATH);
        return;
    }

    for (const auto &entry : blacklist_)
    {
        auto timePoint = entry.second;
        auto timeT = std::chrono::system_clock::to_time_t (timePoint);
        file << entry.first << " "
             << std::put_time (std::localtime (&timeT), "%Y-%m-%d %H:%M:%S")
             << std::endl;
    }
    Logger::getInstance ().log (LogLevel::INFO,
                                "Saved IP blacklist to file: "
                                + config_.BLACKLIST_FILE_PATH);
}

void
AuthenticationServiceImpl::ensureBlacklistDirectoryExists () const
{
    fs::path filePath = config_.BLACKLIST_FILE_PATH;
    fs::path dir = filePath.parent_path ();

    if (!fs::exists (dir))
    {
        fs::create_directories (dir);
        Logger::getInstance ().log (LogLevel::INFO,
                                    "Created directory for blacklist file: "
                                    + dir.string ());
    }

    if (!fs::exists (filePath))
    {
        std::ofstream file (filePath);
        if (file)
        {
            Logger::getInstance ().log (LogLevel::INFO, "Created blacklist file: "
                                                        + filePath.string ());
        }
        else
        {
            Logger::getInstance ().log (LogLevel::ERROR,
                                        "Failed to create blacklist file: "
                                        + filePath.string ());
        }
    }
}

void
AuthenticationServiceImpl::createUserDirectory (const std::string &username)
{
    fs::path userDir = fs::path (config_.MAIL_SPOOL_DIRECTORY) / username;
    if (!fs::exists (userDir))
    {
        fs::create_directories (userDir);
        Logger::getInstance ().log (LogLevel::INFO, "Created user directory: "
                                                    + userDir.string ());
    }
}