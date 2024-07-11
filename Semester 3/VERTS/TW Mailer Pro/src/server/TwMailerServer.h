#ifndef TWMAILER_SERVER_H
#define TWMAILER_SERVER_H

#include "../shared/Configuration.h"
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
    TwMailerServer ();

    ~TwMailerServer ();

    void start ();

    void stop ();

    bool isRunning () const;

    static void setSuppressErrorMessages (bool suppress);

private:
    void acceptConnections ();

    void handleClient (int clientSocket, const std::string &clientIp);

    void cleanupConnections ();

    uint16_t port_;
    std::string mailSpoolDirectory_;
    std::atomic<bool> running_;
    int serverSocket_;
    std::shared_ptr<AuthenticationService> authService_;
    std::shared_ptr<MailService> mailService_;
    std::shared_ptr<CommandHandler> commandHandler_;
    std::vector<std::thread> clientThreads_;
    std::mutex clientThreadsMutex_;
    static constexpr int MAX_CONNECTIONS = 100;
    static bool suppressErrorMessages;

    void loadConfiguration ();
};

#endif
