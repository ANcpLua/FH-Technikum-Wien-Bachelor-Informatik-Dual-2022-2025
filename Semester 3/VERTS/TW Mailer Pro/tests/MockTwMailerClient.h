#ifndef MOCK_TWMAILER_CLIENT_H
#define MOCK_TWMAILER_CLIENT_H

#include "../src/client/TwMailerClient.h"
#include <gmock/gmock.h>

class MockTwMailerClient : public TwMailerClient
{
public:
    MockTwMailerClient (const std::string &ldap_uri, uint16_t port)
            : TwMailerClient (ldap_uri, port)
    {}

    MOCK_METHOD (void, connect, (), (override));
    MOCK_METHOD (void, disconnect, (), (override));
    MOCK_METHOD (void, sendCommand, (const std::string &command, bool debug), (override));
    MOCK_METHOD (std::string, receiveResponse, (bool debug), (override));
    MOCK_METHOD (std::string, getInput, (const std::string &prompt), (override));
    MOCK_METHOD (std::string, getMaskedInput, (const std::string &prompt), (override));
    MOCK_METHOD (std::string, getMultiLineInput, (const std::string &prompt), (override));
    MOCK_METHOD (bool, validateEmail, (const std::string &email), (override));
    MOCK_METHOD (bool, validateSubject, (const std::string &subject), (override));

    using TwMailerClient::handleDelete;
    using TwMailerClient::handleList;
    using TwMailerClient::handleLogin;
    using TwMailerClient::handleQuit;
    using TwMailerClient::handleRead;
    using TwMailerClient::handleSend;

    using TwMailerClient::isAuthenticated_;
};

#endif
