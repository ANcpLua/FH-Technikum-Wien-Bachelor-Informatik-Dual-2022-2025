#include "TwMailerServerTests.h"
#include "MockTwMailerServer.h"
#include "../src/server/CommandHandler.h"
#include "../src/shared/TwMailerExceptions.h"
#include "../src/shared/Configuration.h"
#include "../src/shared/Logger.h"
#include <filesystem>
#include <stdexcept>
#include <gtest/gtest.h>
#include <gmock/gmock.h>

namespace fs = std::filesystem;

using ::testing::_;
using ::testing::Return;
using ::testing::ReturnRef;
using ::testing::Throw;
using ::testing::ElementsAre;
using ::testing::HasSubstr;
using ::testing::IsTrue;
using ::testing::IsFalse;
using ::testing::Eq;
using ::testing::IsEmpty;
using ::testing::ElementsAreArray;
using ::testing::AllOf;
using ::testing::Ge;
using ::testing::Le;

constexpr const char *kTestClientIp = "127.0.0.1";
constexpr const char *kTestUsername = "test_user";

void TwMailerServerTests::SetUp()
{
    fs::path exe_path = fs::canonical("/proc/self/exe");
    fs::path project_root = exe_path.parent_path().parent_path();
    fs::path config_path = project_root / "conf" / "twmailer.conf";
    Configuration::getInstance().loadFromFile(config_path.string());

    fs::path log_dir = project_root / Configuration::getInstance().getString("log_directory", "logs");
    if (!fs::exists(log_dir))
    {
        fs::create_directories(log_dir);
    }

    fs::path log_file = log_dir / "test_twmailer_server.log";
    Logger::getInstance().initialize(log_file.string());

    AuthenticationService::Config config;
    config.MAIL_SPOOL_DIRECTORY = "/tmp/test_mail_spool";
    config.BLACKLIST_FILE_PATH = "/tmp/test_blacklist.txt";
    config.MAX_LOGIN_ATTEMPTS = 3;
    config.BLACKLIST_DURATION = std::chrono::minutes(15);
    config.ATTEMPT_RESET_DURATION = std::chrono::minutes(5);

    mock_auth_service_ = std::make_shared<testing::MockAuthenticationService>(
        "ldap://ldap.technikum-wien.at",
        "uid=%s,ou=people,dc=technikum-wien,dc=at",
        config);
    mock_mail_service_ = std::make_shared<testing::MockMailService>("/tmp/test_mail_spool");
    mock_command_handler_ = std::make_shared<testing::MockCommandHandler>(*mock_auth_service_, *mock_mail_service_);
    client_session_ = std::make_unique<testing::MockClientSession>(0, kTestClientIp, mock_command_handler_);
}

void TwMailerServerTests::TearDown()
{
    mock_auth_service_.reset();
    mock_mail_service_.reset();
    mock_command_handler_.reset();
    client_session_.reset();
}

TEST_F(TwMailerServerTests, AuthenticationSucceeds)
{
    SecureString password("password");
    EXPECT_CALL(*mock_auth_service_,
                authenticate(kTestUsername, testing::Truly([&password](const SecureString &p) {
                    return p.c_str() == password.c_str();
                }),
                kTestClientIp))
        .WillOnce(Return(true));

    bool is_authenticated = mock_auth_service_->authenticate(kTestUsername, password, kTestClientIp);
    EXPECT_THAT(is_authenticated, IsTrue()) << "Authentication should succeed with correct credentials";
}

TEST_F(TwMailerServerTests, AuthenticationFails)
{
    SecureString wrong_password("wrong_password");
    EXPECT_CALL(*mock_auth_service_,
                authenticate(kTestUsername, testing::Truly([&wrong_password](const SecureString &p) {
                    return p.c_str() == wrong_password.c_str();
                }),
                kTestClientIp))
        .WillOnce(Return(false));

    bool is_authenticated = mock_auth_service_->authenticate(kTestUsername, wrong_password, kTestClientIp);
    EXPECT_THAT(is_authenticated, IsFalse()) << "Authentication should fail with incorrect credentials";
}

TEST_F(TwMailerServerTests, CreateUserDirectoryIsCalled)
{
    EXPECT_CALL(*mock_auth_service_, createUserDirectory(kTestUsername)).Times(1);
    mock_auth_service_->createUserDirectory(kTestUsername);
}

TEST_F(TwMailerServerTests, SendMailSucceeds)
{
    std::string sender = kTestUsername;
    std::string receiver = "recipient@example.com";
    std::string subject = "Test Subject";
    std::string body = "Test Body";

    EXPECT_CALL(*mock_mail_service_, sendMail(sender, receiver, subject, body))
        .WillOnce(Return(true));

    bool is_mail_sent = mock_mail_service_->sendMail(sender, receiver, subject, body);
    EXPECT_THAT(is_mail_sent, IsTrue());
}

TEST_F(TwMailerServerTests, SendMailFails)
{
    std::string sender = kTestUsername;
    std::string receiver = "recipient@example.com";
    std::string subject = "Test Subject";
    std::string body = "Test Body";

    EXPECT_CALL(*mock_mail_service_, sendMail(sender, receiver, subject, body))
        .WillOnce(Return(false));

    bool is_mail_sent = mock_mail_service_->sendMail(sender, receiver, subject, body);
    EXPECT_THAT(is_mail_sent, IsFalse());
}

TEST_F(TwMailerServerTests, ListMailsReturnsExistingMails)
{
    std::vector<std::string> expected_mail_list = {"Mail1", "Mail2", "Mail3"};
    EXPECT_CALL(*mock_mail_service_, listMails(kTestUsername))
        .WillOnce(Return(expected_mail_list));

    std::vector<std::string> actual_mail_list = mock_mail_service_->listMails(kTestUsername);
    EXPECT_THAT(actual_mail_list, ElementsAreArray(expected_mail_list));
}

TEST_F(TwMailerServerTests, ListMailsReturnsEmptyListWhenNoMails)
{
    std::vector<std::string> empty_mail_list;
    EXPECT_CALL(*mock_mail_service_, listMails(kTestUsername))
        .WillOnce(Return(empty_mail_list));

    std::vector<std::string> result = mock_mail_service_->listMails(kTestUsername);
    EXPECT_THAT(result, IsEmpty());
}

TEST_F(TwMailerServerTests, ReadExistingMailReturnsCorrectContent)
{
    const std::string expected_mail_content = "This is a test mail content";
    const size_t mail_id = 1;
    EXPECT_CALL(*mock_mail_service_, readMail(kTestUsername, mail_id))
        .WillOnce(Return(expected_mail_content));

    std::string actual_mail_content = mock_mail_service_->readMail(kTestUsername, mail_id);
    EXPECT_THAT(actual_mail_content, Eq(expected_mail_content));
}

TEST_F(TwMailerServerTests, ReadNonExistentMailThrowsException)
{
    const size_t non_existent_mail_id = 999;
    EXPECT_CALL(*mock_mail_service_, readMail(kTestUsername, non_existent_mail_id))
        .WillOnce(Throw(std::runtime_error("Mail not found")));

    EXPECT_THAT([&]() {
        mock_mail_service_->readMail(kTestUsername, non_existent_mail_id);
    }, testing::Throws<std::runtime_error>());
}

TEST_F(TwMailerServerTests, DeleteExistingMailSucceeds)
{
    const size_t mail_id = 1;
    EXPECT_CALL(*mock_mail_service_, deleteMail(kTestUsername, mail_id))
        .WillOnce(Return(true));

    bool is_deleted = mock_mail_service_->deleteMail(kTestUsername, mail_id);
    EXPECT_THAT(is_deleted, IsTrue());
}

TEST_F(TwMailerServerTests, DeleteNonExistentMailFails)
{
    const size_t non_existent_mail_id = 999;
    EXPECT_CALL(*mock_mail_service_, deleteMail(kTestUsername, non_existent_mail_id))
        .WillOnce(Return(false));

    bool is_deleted = mock_mail_service_->deleteMail(kTestUsername, non_existent_mail_id);
    EXPECT_THAT(is_deleted, IsFalse());
}

TEST_F(TwMailerServerTests, GetRemainingAttemptsReturnsCorrectValue)
{
    const int expected_attempts = 2;
    EXPECT_CALL(*mock_auth_service_, getRemainingAttempts(kTestClientIp))
        .WillOnce(Return(expected_attempts));

    int actual_attempts = mock_auth_service_->getRemainingAttempts(kTestClientIp);
    EXPECT_THAT(actual_attempts, Eq(expected_attempts));
}

TEST_F(TwMailerServerTests, SecureStringConstructorCreatesCorrectString)
{
    SecureString secureStr("test");
    EXPECT_THAT(std::string(secureStr.c_str()), Eq("test"));
}

TEST_F(TwMailerServerTests, HandleCommandProcessesListCorrectly)
{
    std::string list_command = "LIST";
    EXPECT_CALL(*mock_command_handler_, handleCommand(list_command, testing::Ref(*client_session_)))
        .Times(1);

    mock_command_handler_->handleCommand(list_command, *client_session_);
}

TEST_F(TwMailerServerTests, HandleCommandProcessesReadCorrectly)
{
    std::string read_command = "READ 1";
    EXPECT_CALL(*mock_command_handler_, handleCommand(read_command, testing::Ref(*client_session_)))
        .Times(1);

    mock_command_handler_->handleCommand(read_command, *client_session_);
}

TEST_F(TwMailerServerTests, HandleCommandProcessesDeleteCorrectly)
{
    std::string delete_command = "DEL 1";
    EXPECT_CALL(*mock_command_handler_, handleCommand(delete_command, testing::Ref(*client_session_)))
        .Times(1);

    mock_command_handler_->handleCommand(delete_command, *client_session_);
}

TEST_F(TwMailerServerTests, HandleCommandProcessesQuitCorrectly)
{
    std::string quit_command = "QUIT";
    EXPECT_CALL(*mock_command_handler_, handleCommand(quit_command, testing::Ref(*client_session_)))
        .Times(1);

    mock_command_handler_->handleCommand(quit_command, *client_session_);
}

TEST_F(TwMailerServerTests, HandleInvalidCommandReturnsError)
{
    std::string invalid_command = "INVALID_COMMAND";
    EXPECT_CALL(*mock_command_handler_, handleCommand(invalid_command, testing::Ref(*client_session_)))
        .WillOnce(::testing::Invoke([](const std::string &, ClientSession &session) {
            session.sendResponse("ERR Invalid command");
        }));

    EXPECT_CALL(*client_session_, sendResponse("ERR Invalid command")).Times(1);
    mock_command_handler_->handleCommand(invalid_command, *client_session_);
}

TEST_F(TwMailerServerTests, AuthenticationFailsWithEmptyCredentials)
{
    SecureString empty_password("");
    EXPECT_CALL(*mock_auth_service_, authenticate("", testing::Truly([](const SecureString &p) {
        return p.length() == 0;
    }), kTestClientIp)).WillOnce(Return(false));

    bool is_authenticated = mock_auth_service_->authenticate("", empty_password, kTestClientIp);
    EXPECT_THAT(is_authenticated, IsFalse());
}

TEST_F(TwMailerServerTests, SendMailToNonExistentUserFails)
{
    std::string sender = kTestUsername;
    std::string non_existent_receiver = "nonexistent@example.com";
    std::string subject = "Test Subject";
    std::string body = "Test Body";

    EXPECT_CALL(*mock_mail_service_, sendMail(sender, non_existent_receiver, subject, body))
        .WillOnce(Return(false));

    bool is_mail_sent = mock_mail_service_->sendMail(sender, non_existent_receiver, subject, body);
    EXPECT_THAT(is_mail_sent, IsFalse());
}

TEST_F(TwMailerServerTests, ListMailsWithInvalidUserReturnsEmpty)
{
    std::string invalid_user = "invalid_user";
    EXPECT_CALL(*mock_mail_service_, listMails(invalid_user))
        .WillOnce(Return(std::vector<std::string>()));

    std::vector<std::string> result = mock_mail_service_->listMails(invalid_user);
    EXPECT_THAT(result, IsEmpty());
}

TEST_F(TwMailerServerTests, ConcurrentAuthenticationAttemptsAreThreadSafe)
{
    const int num_threads = 5;
    std::vector<std::thread> threads;
    std::atomic<int> successful_auths(0);

    for (int i = 0; i < num_threads; ++i)
    {
        threads.emplace_back([this, &successful_auths, i]() {
            SecureString password("password");
            std::string username = "user" + std::to_string(i);
            std::string client_ip = "192.168.0." + std::to_string(i);

            EXPECT_CALL(*mock_auth_service_,
                        authenticate(username, testing::Truly([&password](const SecureString &p) {
                            return p.c_str() == password.c_str();
                        }), client_ip))
                .WillOnce(Return(true));

            bool is_authenticated = mock_auth_service_->authenticate(username, password, client_ip);
            if (is_authenticated)
            {
                successful_auths++;
            }
        });
    }

    for (auto &thread : threads)
    {
        thread.join();
    }

    EXPECT_THAT(successful_auths.load(), Eq(num_threads));
}

TEST_F(TwMailerServerTests, AuthenticationFailsAfterMaxAttempts)
{
    SecureString password("wrong_password");
    EXPECT_CALL(*mock_auth_service_, authenticate(kTestUsername, _, kTestClientIp)).Times(3).WillRepeatedly(Return(false));

    for (int i = 0; i < 3; i++)
    {
        bool is_authenticated = mock_auth_service_->authenticate(kTestUsername, password, kTestClientIp);
        EXPECT_THAT(is_authenticated, IsFalse());
    }

    EXPECT_CALL(*mock_auth_service_, getRemainingAttempts(kTestClientIp)).WillOnce(Return(0));
    int remaining_attempts = mock_auth_service_->getRemainingAttempts(kTestClientIp);
    EXPECT_THAT(remaining_attempts, Eq(0));
}

TEST_F(TwMailerServerTests, SendMailWithEmptyFieldsFails)
{
    std::string sender = kTestUsername;
    std::string receiver = "";
    std::string subject = "";
    std::string body = "";

    EXPECT_CALL(*mock_mail_service_, sendMail(sender, receiver, subject, body)).WillOnce(Return(false));
    bool is_mail_sent = mock_mail_service_->sendMail(sender, receiver, subject, body);
    EXPECT_THAT(is_mail_sent, IsFalse());
}
