#ifndef COMMAND_HANDLER_H
#define COMMAND_HANDLER_H

#include <functional>
#include <memory>
#include <string>
#include <unordered_map>
#include <vector>

class AuthenticationService;

class MailService;

class ClientSession;

class CommandHandler
{
public:
    CommandHandler (AuthenticationService &authService, MailService &mailService);

    virtual ~CommandHandler () = default;

    virtual void handleCommand (const std::string &command,
                                ClientSession &session)
    = 0;

protected:
    virtual void handleLogin (const std::vector<std::string> &args,
                              ClientSession &session)
    = 0;

    virtual void handleSend (const std::vector<std::string> &args,
                             ClientSession &session)
    = 0;

    virtual void handleList (const std::vector<std::string> &args,
                             ClientSession &session)
    = 0;

    virtual void handleRead (const std::vector<std::string> &args,
                             ClientSession &session)
    = 0;

    virtual void handleDelete (const std::vector<std::string> &args,
                               ClientSession &session)
    = 0;

    virtual void handleQuit (const std::vector<std::string> &args,
                             ClientSession &session)
    = 0;

    AuthenticationService &authService_;
    MailService &mailService_;
    using CommandFunction
            = std::function<void (const std::vector<std::string> &, ClientSession &)>;
    std::unordered_map<std::string, CommandFunction> commandMap_;
};

class CommandHandlerImpl : public CommandHandler
{
public:
    CommandHandlerImpl (AuthenticationService &authService,
                        MailService &mailService);

    void handleCommand (const std::string &command,
                        ClientSession &session) override;

protected:
    void handleLogin (const std::vector<std::string> &args,
                      ClientSession &session) override;

    void handleSend (const std::vector<std::string> &args,
                     ClientSession &session) override;

    void handleList (const std::vector<std::string> &args,
                     ClientSession &session) override;

    void handleRead (const std::vector<std::string> &args,
                     ClientSession &session) override;

    void handleDelete (const std::vector<std::string> &args,
                       ClientSession &session) override;

    void handleQuit (const std::vector<std::string> &args,
                     ClientSession &session) override;
};

#endif
