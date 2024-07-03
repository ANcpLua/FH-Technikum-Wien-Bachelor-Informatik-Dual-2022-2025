#ifndef TWMAILER_SERVER_H
#define TWMAILER_SERVER_H

#include <atomic>
#include <cstdint>
#include <memory>
#include <mutex>
#include <string>
#include <thread>
#include <vector>

class AuthenticationService;
class MailService;
class CommandHandler;

class TwMailerServer
{
public:
    TwMailerServer (uint16_t port, const std::string &mailSpoolDirectory,
                    const std::string &ldapUri,
                    const std::string &ldapBindDnFormat);
    ~TwMailerServer ();

    void start ();
    void stop ();
    bool isRunning () const;

private:
    void acceptConnections ();
    void handleClient (int clientSocket, const std::string &clientIp);
    void cleanupConnections ();

    uint16_t port_;
    std::string mailSpoolDirectory_;
    std::atomic<bool> running_;
    int serverSocket_;

    std::unique_ptr<AuthenticationService> authService_;
    std::unique_ptr<MailService> mailService_;
    std::shared_ptr<CommandHandler> commandHandler_;

    std::vector<std::thread> clientThreads_;
    std::mutex clientThreadsMutex_;

    static constexpr int MAX_CONNECTIONS = 100;
};

#endif // TWMAILER_SERVER_H
