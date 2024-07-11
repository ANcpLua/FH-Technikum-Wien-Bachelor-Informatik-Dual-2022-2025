#ifndef TWMAILER_SERVER_TESTS_H
#define TWMAILER_SERVER_TESTS_H

#include <gtest/gtest.h>
#include <gmock/gmock.h>
#include "../src/shared/Configuration.h"
#include "../src/shared/Logger.h"
#include "../src/server/TwMailerServer.h"
#include "MockTwMailerServer.h"
#include <memory>

class TwMailerServerTests : public ::testing::Test
{
protected:
    void SetUp() override;

    void TearDown() override;

    std::shared_ptr <testing::MockAuthenticationService> mock_auth_service_;
    std::shared_ptr <testing::MockMailService> mock_mail_service_;
    std::shared_ptr <testing::MockCommandHandler> mock_command_handler_;
    std::unique_ptr <testing::MockClientSession> client_session_;
};

#endif
