#ifndef AUTHENTICATIONSERVICE_H
#define AUTHENTICATIONSERVICE_H

#include "SecureString.h"
#include <chrono>
#include <ldap.h>
#include <mutex>
#include <string>
#include <unordered_map>
#include <vector>

class AuthenticationService
{
public:
    struct Config
    {
        std::string MAIL_SPOOL_DIRECTORY;
        std::string BLACKLIST_FILE_PATH;
        int MAX_LOGIN_ATTEMPTS;
        std::chrono::minutes BLACKLIST_DURATION;
        std::chrono::minutes ATTEMPT_RESET_DURATION;
    };

    AuthenticationService (const std::string &ldapUri,
                           const std::string &ldapBindDnFormat,
                           const Config &config);

    virtual ~AuthenticationService ();

    virtual bool authenticate (const std::string &username,
                               const SecureString &password,
                               const std::string &clientIp)
    = 0;

    virtual void createUserDirectory (const std::string &username) = 0;

    virtual int getRemainingAttempts (const std::string &clientIp) const = 0;

protected:
    std::string ldapUri_;
    std::string ldapBindDnFormat_;
    LDAP *ldapHandle_;
    Config config_;
};

class AuthenticationServiceImpl : public AuthenticationService
{
public:
    AuthenticationServiceImpl (const std::string &ldapUri,
                               const std::string &ldapBindDnFormat,
                               const Config &config);

    ~AuthenticationServiceImpl () override;

    bool authenticate (const std::string &username, const SecureString &password,
                       const std::string &clientIp) override;

    void createUserDirectory (const std::string &username) override;

    int getRemainingAttempts (const std::string &clientIp) const override;

    void initialize ();

    void cleanup ();

private:
    mutable std::mutex mutex_;
    std::unordered_map<std::string,
            std::vector<std::chrono::system_clock::time_point> >
            loginAttempts_;
    std::unordered_map<std::string, std::chrono::system_clock::time_point>
            blacklist_;

    bool ldapAuthenticate (const std::string &username,
                           const SecureString &password);

    void blacklistIp (const std::string &clientIp);

    bool isIpBlacklisted (const std::string &clientIp) const;

    void cleanupBlacklist ();

    void loadBlacklist ();

    void saveBlacklist () const;

    void ensureBlacklistDirectoryExists () const;
};

#endif