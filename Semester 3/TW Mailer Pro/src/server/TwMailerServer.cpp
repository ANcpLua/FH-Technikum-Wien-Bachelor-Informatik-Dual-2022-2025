#include "../shared/ClientSession.h"
#include "../shared/Logger.h"
#include "AuthenticationService.h"
#include "CommandHandler.h"
#include "MailService.h"
#include "TwMailerServer.h"
#include <algorithm>
#include <arpa/inet.h>
#include <netinet/in.h>
#include <stdexcept>
#include <sys/socket.h>
#include <unistd.h>

TwMailerServer::TwMailerServer (uint16_t port,
                                const std::string &mailSpoolDirectory,
                                const std::string &ldapUri,
                                const std::string &ldapBindDnFormat)
        : port_ (port), mailSpoolDirectory_ (mailSpoolDirectory), running_ (false),
          serverSocket_ (-1)
{
    authService_
            = std::make_unique<AuthenticationService> (ldapUri, ldapBindDnFormat);
    mailService_ = std::make_unique<MailService> (mailSpoolDirectory);
    commandHandler_
            = std::make_shared<CommandHandler> (*authService_, *mailService_);
}

TwMailerServer::~TwMailerServer () { stop (); }

void
TwMailerServer::start ()
{
    if (running_)
    {
        Logger::getInstance ().log (LogLevel::WARNING,
                                    "Server is already running.");
        return;
    }

    serverSocket_ = socket (AF_INET, SOCK_STREAM, 0);
    if (serverSocket_ == -1)
    {
        throw std::runtime_error ("Failed to create server socket.");
    }

    sockaddr_in serverAddr{};
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_addr.s_addr = INADDR_ANY;
    serverAddr.sin_port = htons (port_);

    if (bind (serverSocket_, (struct sockaddr *)&serverAddr, sizeof (serverAddr))
        < 0)
    {
        close (serverSocket_);
        throw std::runtime_error ("Failed to bind server socket.");
    }

    if (listen (serverSocket_, 10) < 0)
    {
        close (serverSocket_);
        throw std::runtime_error ("Failed to listen on server socket.");
    }

    running_ = true;
    Logger::getInstance ().log (LogLevel::INFO, "Server started on port "
                                                + std::to_string (port_));
    acceptConnections ();
}

void
TwMailerServer::stop ()
{
    running_ = false;
    if (serverSocket_ != -1)
    {
        close (serverSocket_);
        serverSocket_ = -1;
    }
    cleanupConnections ();
    Logger::getInstance ().log (LogLevel::INFO, "Server stopped.");
}

bool
TwMailerServer::isRunning () const
{
    return running_;
}

void
TwMailerServer::acceptConnections ()
{
    while (running_)
    {
        sockaddr_in clientAddr{};
        socklen_t clientAddrLen = sizeof (clientAddr);
        int clientSocket = accept (serverSocket_, (struct sockaddr *)&clientAddr,
                                   &clientAddrLen);
        if (clientSocket < 0)
        {
            if (running_)
            {
                Logger::getInstance ().log (
                        LogLevel::ERROR, "Failed to accept client connection.");
            }
            continue;
        }

        {
            std::lock_guard<std::mutex> lock (clientThreadsMutex_);
            if (clientThreads_.size () >= MAX_CONNECTIONS)
            {
                Logger::getInstance ().log (
                        LogLevel::WARNING,
                        "Maximum connections reached. Rejecting new connection.");
                close (clientSocket);
                continue;
            }

            std::string clientIp = inet_ntoa (clientAddr.sin_addr);
            clientThreads_.emplace_back ([this, clientSocket, clientIp] () {
                this->handleClient (clientSocket, clientIp);
            });
        }
    }
}

void
TwMailerServer::handleClient (int clientSocket, const std::string &clientIp)
{
    auto session = std::make_shared<ClientSession> (clientSocket, clientIp,
                                                    commandHandler_);
    session->start ();
}

void
TwMailerServer::cleanupConnections ()
{
    std::lock_guard<std::mutex> lock (clientThreadsMutex_);
    for (auto &thread : clientThreads_)
    {
        if (thread.joinable ())
        {
            thread.join ();
        }
    }
    clientThreads_.clear ();
}
