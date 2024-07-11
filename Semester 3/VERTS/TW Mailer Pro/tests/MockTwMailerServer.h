#ifndef MOCK_TW_MAILER_SERVER_H
#define MOCK_TW_MAILER_SERVER_H

#include "../src/server/AuthenticationService.h"
#include "../src/server/CommandHandler.h"
#include "../src/server/MailService.h"
#include "../src/server/SecureString.h"
#include "../src/shared/ClientSession.h"
#include <filesystem>
#include <gmock/gmock.h>
#include <gtest/gtest.h>
#include <memory>
#include <string>
#include <vector>

namespace testing
{

class MockAuthenticationService : public AuthenticationService
{
public:
    MockAuthenticationService(const std::string &ldapUri,
                              const std::string &ldapBindDnFormat,
                              const Config &config)
        : AuthenticationService(ldapUri, ldapBindDnFormat, config)
    {}

    MOCK_METHOD(bool, authenticate, (const std::string &username, const SecureString &password, const std::string &clientIp), (override));
    MOCK_METHOD(void, createUserDirectory, (const std::string &username), (override));
    MOCK_METHOD(int, getRemainingAttempts, (const std::string &clientIp), (const, override));
};

class MockMailService : public MailService
{
public:
    MockMailService(const std::string &mail_spool_directory)
        : MailService(mail_spool_directory)
    {}

    MOCK_METHOD(bool, sendMail, (const std::string &sender, const std::string &receiver, const std::string &subject, const std::string &body), (override));
    MOCK_METHOD(std::vector<std::string>, listMails, (const std::string &user), (const, override));
    MOCK_METHOD(std::string, readMail, (const std::string &user, size_t mailId), (const, override));
    MOCK_METHOD(bool, deleteMail, (const std::string &user, size_t mailId), (override));
};

class MockCommandHandler : public CommandHandler
{
public:
    MockCommandHandler(AuthenticationService &auth_service,
                       MailService &mail_service)
        : CommandHandler(auth_service, mail_service)
    {}

    MOCK_METHOD(void, handleCommand, (const std::string &command, ClientSession &session), (override));
    MOCK_METHOD(void, handleLogin, (const std::vector<std::string> &args, ClientSession &session), (override));
    MOCK_METHOD(void, handleSend, (const std::vector<std::string> &args, ClientSession &session), (override));
    MOCK_METHOD(void, handleList, (const std::vector<std::string> &args, ClientSession &session), (override));
    MOCK_METHOD(void, handleRead, (const std::vector<std::string> &args, ClientSession &session), (override));
    MOCK_METHOD(void, handleDelete, (const std::vector<std::string> &args, ClientSession &session), (override));
    MOCK_METHOD(void, handleQuit, (const std::vector<std::string> &args, ClientSession &session), (override));
};

class MockClientSession : public ClientSession
{
public:
    MockClientSession(int client_socket, const std::string &client_ip,
                      std::shared_ptr<CommandHandler> command_handler)
        : ClientSession(client_socket, client_ip, command_handler)
    {}

    MOCK_METHOD(void, processCommands, (), (override));
    MOCK_METHOD(void, sendResponse, (const std::string &response), (override));
    MOCK_METHOD(std::string, receiveCommand, (), (override));
    MOCK_METHOD(bool, isAuthenticated, (), (const, override));
    MOCK_METHOD(void, setAuthenticated, (bool authenticated), (override));
    MOCK_METHOD(const std::string &, getUsername, (), (const, override));
    MOCK_METHOD(void, setUsername, (const std::string &username), (override));
    MOCK_METHOD(const std::string &, getClientIp, (), (const, override));
};

} // namespace testing

#endif
