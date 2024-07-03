#ifndef AUTHENTICATION_SERVICE_H
#define AUTHENTICATION_SERVICE_H

#include "../shared/Logger.h"
#include <chrono>
#include <ldap.h>
#include <mutex>
#include <string>
#include <unordered_map>
#include <vector>

class AuthenticationService
{
public:
    AuthenticationService (const std::string &ldapUri,
                           const std::string &ldapBindDnFormat);
    ~AuthenticationService ();

    bool authenticate (const std::string &username, const std::string &password,
                       const std::string &clientIp);

    void blacklistIp (const std::string &clientIp);
    bool isIpBlacklisted (const std::string &clientIp) const;
    int getRemainingAttempts (const std::string &clientIp) const;

private:
    bool ldapAuthenticate (const std::string &username,
                           const std::string &password);

    void cleanupBlacklist ();

    std::string ldapUri_;
    std::string ldapBindDnFormat_;
    LDAP *ldapHandle_;

    std::unordered_map<std::string,
    std::vector<std::chrono::system_clock::time_point> >
            loginAttempts_;

    std::unordered_map<std::string, std::chrono::system_clock::time_point>
            blacklist_;

    mutable std::mutex mutex_;

    static constexpr int MAX_LOGIN_ATTEMPTS = 3;
    static constexpr std::chrono::minutes BLACKLIST_DURATION{ 1 };
    static constexpr std::chrono::minutes ATTEMPT_RESET_DURATION{ 5 };
};

#endif // AUTHENTICATION_SERVICE_H
