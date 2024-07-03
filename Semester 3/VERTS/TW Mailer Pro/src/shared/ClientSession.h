#ifndef CLIENT_SESSION_H
#define CLIENT_SESSION_H

#include <atomic>
#include <memory>
#include <string>

class CommandHandler;

class ClientSession : public std::enable_shared_from_this<ClientSession>
{
public:
    ClientSession (int clientSocket, const std::string &clientIp,
                   std::shared_ptr<CommandHandler> commandHandler);
    ~ClientSession ();

    void start ();
    void stop ();
    void sendResponse (const std::string &response);
    std::string receiveCommand ();

    bool
    isAuthenticated () const
    {
        return authenticated_;
    }
    void
    setAuthenticated (bool auth)
    {
        authenticated_ = auth;
    }
    const std::string &
    getUsername () const
    {
        return username_;
    }
    void
    setUsername (const std::string &username)
    {
        username_ = username;
    }
    const std::string &
    getClientIp () const
    {
        return clientIp_;
    }

private:
    void processCommands ();

    int clientSocket_;
    std::string clientIp_;
    std::shared_ptr<CommandHandler> commandHandler_;
    std::atomic<bool> authenticated_;
    std::string username_;
    std::atomic<bool> running_;
};

#endif // CLIENT_SESSION_H
