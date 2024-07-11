#include "../server/CommandHandler.h"
#include "ClientSession.h"
#include "Logger.h"
#include "TwMailerExceptions.h"
#include <cstring>
#include <sstream>
#include <stdexcept>
#include <sys/socket.h>
#include <unistd.h>

ClientSession::ClientSession (int clientSocket, const std::string &clientIp,
                              std::shared_ptr<CommandHandler> commandHandler)
        : clientSocket_ (clientSocket), clientIp_ (clientIp),
        commandHandler_ (std::move (commandHandler)), authenticated_ (false),
        running_ (false)
{
    LOG_INFO ("New client session created for IP: " + clientIp);
}

ClientSession::~ClientSession () { stop (); }

void
ClientSession::start ()
{
    running_ = true;
    LOG_INFO ("Client session started for IP: " + clientIp_);
    processCommands ();
}

void
ClientSession::stop ()
{
    running_ = false;
    if (clientSocket_ != -1)
    {
        close (clientSocket_);
        clientSocket_ = -1;
    }
    LOG_INFO ("Client session stopped for IP: " + clientIp_);
}

void
ClientSession::sendResponse (const std::string &response)
{
    if (clientSocket_ == -1)
    {
        throw std::runtime_error ("Client socket is closed");
    }
    ssize_t bytesSent
            = send (clientSocket_, response.c_str (), response.length (), 0);
    if (bytesSent == -1)
    {
        LOG_ERROR ("Failed to send response to client: "
                   + std::string (strerror (errno)));
        throw std::runtime_error ("Failed to send response to client");
    }
    LOG_DEBUG ("Sent response to client: " + response);
}

std::string
ClientSession::receiveCommand ()
{
    if (clientSocket_ == -1)
    {
        throw std::runtime_error ("Client socket is closed");
    }
    char buffer[4096];
    ssize_t bytesRead = recv (clientSocket_, buffer, sizeof (buffer) - 1, 0);
    if (bytesRead == -1)
    {
        LOG_ERROR ("Failed to receive command from client: "
                   + std::string (strerror (errno)));
        throw std::runtime_error ("Failed to receive command from client");
    }
    if (bytesRead == 0)
    {
        return "";
    }
    buffer[bytesRead] = '\0';
    std::string command (buffer);

    std::istringstream iss (command);
    std::string commandType;
    iss >> commandType;
    LOG_DEBUG ("Received command type from client: " + commandType);

    return command;
}

void
ClientSession::processCommands ()
{
    try
    {
        while (running_)
        {
            std::string command = receiveCommand ();
            if (command.empty ())
            {
                break;
            }
            commandHandler_->handleCommand (command, *this);
        }
    }
    catch (const std::exception &e)
    {
        LOG_ERROR ("Error in client session: " + std::string (e.what ()));
    }
    LOG_INFO ("Client disconnected: " + clientIp_);
    stop ();
}