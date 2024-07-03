#include "AuthenticationService.h"
#include "../shared/Logger.h"
#include <stdexcept>
#include <algorithm>
#include <vector>
#include <chrono>
#include <ldap.h>
#include <cstring>

AuthenticationService::AuthenticationService (const std::string &ldapUri,
                                              const std::string &ldapBindDnFormat)
        : ldapUri_ (ldapUri), ldapBindDnFormat_ (ldapBindDnFormat),
          ldapHandle_ (nullptr)
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

bool
AuthenticationService::authenticate (const std::string &username,
                                     const std::string &password,
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

    attempts.erase (
            std::remove_if (attempts.begin (), attempts.end (),
                            [now] (const auto &attempt)
                            { return now - attempt > ATTEMPT_RESET_DURATION; }),
            attempts.end ()
    );

    int remainingAttempts = MAX_LOGIN_ATTEMPTS - attempts.size ();

    if (remainingAttempts <= 0)
    {
        blacklistIp (clientIp);
        Logger::getInstance ().log (LogLevel::WARNING,
                                    "IP blacklisted due to too many failed attempts: "
                                    + clientIp);
        return false;
    }

    attempts.push_back (now);

    bool authenticated = ldapAuthenticate (username, password);
    if (authenticated)
    {
        loginAttempts_.erase (clientIp);
        Logger::getInstance ().log (LogLevel::INFO,
                                    "Successful authentication for user: "
                                    + username);
    }
    else
    {
        Logger::getInstance ().log (
                LogLevel::WARNING,
                "Failed authentication attempt for user: " + username
                + ". Remaining attempts: " + std::to_string (remainingAttempts - 1));
    }

    return authenticated;
}

int
AuthenticationService::getRemainingAttempts (const std::string &clientIp) const
{
    std::lock_guard<std::mutex> lock (mutex_);
    auto it = loginAttempts_.find (clientIp);
    if (it == loginAttempts_.end ())
    {
        return MAX_LOGIN_ATTEMPTS;
    }
    return std::max (0, MAX_LOGIN_ATTEMPTS - static_cast<int> (it->second.size ()));
}

void
AuthenticationService::blacklistIp (const std::string &clientIp)
{
    blacklist_[clientIp] = std::chrono::system_clock::now ();
}

bool
AuthenticationService::isIpBlacklisted (const std::string &clientIp) const
{
    auto it = blacklist_.find (clientIp);
    if (it != blacklist_.end ())
    {
        return std::chrono::system_clock::now () - it->second <= BLACKLIST_DURATION;
    }
    return false;
}

bool
AuthenticationService::ldapAuthenticate (const std::string &username,
                                         const std::string &password)
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
    return rc == LDAP_SUCCESS;
}

void
AuthenticationService::cleanupBlacklist ()
{
    auto now = std::chrono::system_clock::now ();
    for (auto it = blacklist_.begin (); it != blacklist_.end ();)
    {
        if (now - it->second > BLACKLIST_DURATION)
        {
            it = blacklist_.erase (it);
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
                                [now] (const auto &attempt)
                                { return now - attempt > ATTEMPT_RESET_DURATION; }),
                it->second.end ()
        );
        if (it->second.empty ())
        {
            it = loginAttempts_.erase (it);
        }
        else
        {
            ++it;
        }
    }
}
