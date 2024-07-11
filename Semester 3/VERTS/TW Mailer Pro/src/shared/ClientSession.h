#ifndef CLIENT_SESSION_H
#define CLIENT_SESSION_H

#include "../server/CommandHandler.h"
#include <atomic>
#include <memory>
#include <string>

class CommandHandler;

class ClientSession : public std::enable_shared_from_this<ClientSession>
{
public:
    ClientSession (int clientSocket, const std::string &clientIp,
                   std::shared_ptr<CommandHandler> commandHandler);

    virtual ~ClientSession ();

    virtual void start ();

    virtual void stop ();

    virtual void sendResponse (const std::string &response);

    virtual std::string receiveCommand ();

    virtual bool
    isAuthenticated () const
    {
        return authenticated_;
    }

    virtual void
    setAuthenticated (bool auth)
    {
        authenticated_ = auth;
    }

    virtual const std::string &
    getUsername () const
    {
        return username_;
    }

    virtual void
    setUsername (const std::string &username)
    {
        username_ = username;
    }

    virtual const std::string &
    getClientIp () const
    {
        return clientIp_;
    }

private:
    virtual void processCommands ();

    int clientSocket_;
    std::string clientIp_;
    std::shared_ptr<CommandHandler> commandHandler_;
    std::atomic<bool> authenticated_;
    std::string username_;
    std::atomic<bool> running_;
};

#endif