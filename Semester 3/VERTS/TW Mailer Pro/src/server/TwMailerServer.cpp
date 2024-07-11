#include "../shared/ClientSession.h"
#include "../shared/Configuration.h"
#include "../shared/TwMailerExceptions.h"
#include "AuthenticationService.h"
#include "CommandHandler.h"
#include "MailService.h"
#include "TwMailerServer.h"
#include <iostream>
#include <netinet/in.h>
#include <sys/socket.h>
#include <unistd.h>

bool TwMailerServer::suppressErrorMessages = false;

TwMailerServer::TwMailerServer () : running_ (false), serverSocket_ (-1)
{
    loadConfiguration ();
}

TwMailerServer::~TwMailerServer () { stop (); }

void
TwMailerServer::loadConfiguration ()
{
    auto &config = Configuration::getInstance ();
    port_ = static_cast<uint16_t> (config.getInt ("port", 8080));
    mailSpoolDirectory_ = config.getString ("mail_spool_directory", "mail_spool");

    std::string ldapUri
            = config.getString ("ldap_uri", "ldap://ldap.technikum-wien.at");
    std::string ldapBindDnFormat = config.getString (
            "ldap_bind_dn_format", "uid=%s,ou=people,dc=technikum-wien,dc=at");

    AuthenticationService::Config authConfig;
    authConfig.MAX_LOGIN_ATTEMPTS = config.getInt ("max_login_attempts", 3);
    authConfig.BLACKLIST_DURATION
            = std::chrono::minutes (config.getInt ("blacklist_duration_minutes", 60));
    authConfig.ATTEMPT_RESET_DURATION = std::chrono::minutes (
            config.getInt ("attempt_reset_duration_minutes", 5));
    authConfig.BLACKLIST_FILE_PATH
            = config.getString ("blacklist_file_path", "data/ip_blacklist.txt");
    authConfig.MAIL_SPOOL_DIRECTORY = mailSpoolDirectory_;

    authService_ = std::make_shared<AuthenticationServiceImpl> (
            ldapUri, ldapBindDnFormat, authConfig);
    mailService_ = std::make_shared<MailServiceImpl> (mailSpoolDirectory_);
    commandHandler_
            = std::make_shared<CommandHandlerImpl> (*authService_, *mailService_);
}

void
TwMailerServer::start ()
{
    if (running_)
    {
        if (!suppressErrorMessages)
        {
            std::cerr << "Server is already running." << std::endl;
        }
        return;
    }

    serverSocket_ = socket (AF_INET, SOCK_STREAM, 0);
    if (serverSocket_ == -1)
    {
        throw std::runtime_error ("Failed to create socket");
    }

    int reuse = 1;
    if (setsockopt (serverSocket_, SOL_SOCKET, SO_REUSEADDR, &reuse,
                    sizeof (reuse))
        < 0)
    {
        close (serverSocket_);
        throw std::runtime_error ("Failed to set SO_REUSEADDR");
    }

    sockaddr_in serverAddr = {};
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_addr.s_addr = INADDR_ANY;
    serverAddr.sin_port = htons (port_);

    if (bind (serverSocket_, reinterpret_cast<sockaddr *> (&serverAddr),
              sizeof (serverAddr))
        == -1)
    {
        close (serverSocket_);
        throw std::runtime_error ("Failed to bind socket");
    }

    if (listen (serverSocket_, MAX_CONNECTIONS) == -1)
    {
        close (serverSocket_);
        throw std::runtime_error ("Failed to listen on socket");
    }

    running_ = true;
    if (!suppressErrorMessages)
    {
        std::cout << "Welcome" << port_ << std::endl;
    }

    std::thread (&TwMailerServer::acceptConnections, this).detach ();
}

void
TwMailerServer::stop ()
{
    if (!running_)
    {
        return;
    }

    running_ = false;

    if (serverSocket_ != -1)
    {
        close (serverSocket_);
        serverSocket_ = -1;
    }

    cleanupConnections ();
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
        int clientSocket = accept (serverSocket_, nullptr, nullptr);
        if (clientSocket == -1)
        {
            if (running_ && !suppressErrorMessages)
            {
                std::cerr << "Failed to accept connection" << std::endl;
            }
            continue;
        }

        std::thread (&TwMailerServer::handleClient, this, clientSocket,
                     "Client IP")
                .detach ();
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

void
TwMailerServer::setSuppressErrorMessages (bool suppress)
{
    suppressErrorMessages = suppress;
}
