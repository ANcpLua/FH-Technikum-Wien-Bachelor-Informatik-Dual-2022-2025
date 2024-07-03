#ifndef COMMAND_HANDLER_H
#define COMMAND_HANDLER_H

#include <string>
#include <vector>
#include <unordered_map>
#include <functional>
#include <memory>

class AuthenticationService;
class MailService;
class ClientSession;

class CommandHandler
{
public:
    CommandHandler(AuthenticationService& authService, MailService& mailService);
    void handleCommand(const std::string& command, ClientSession& session);

private:
    void handleLogin(const std::vector<std::string>& args, ClientSession& session);
    void handleSend(const std::vector<std::string>& args, ClientSession& session);
    void handleList(const std::vector<std::string>& args, ClientSession& session);
    void handleRead(const std::vector<std::string>& args, ClientSession& session);
    void handleDelete(const std::vector<std::string>& args, ClientSession& session);
    void handleQuit(const std::vector<std::string>& args, ClientSession& session);

    AuthenticationService& authService_;
    MailService& mailService_;

    using CommandFunction = std::function<void(const std::vector<std::string>&, ClientSession&)>;
    std::unordered_map<std::string, CommandFunction> commandMap_;
};

#endif // COMMAND_HANDLER_H
