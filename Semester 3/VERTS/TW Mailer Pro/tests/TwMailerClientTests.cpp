#include <gmock/gmock.h>
#include <gtest/gtest.h>
#include "TwMailerClientTests.h"
#include <filesystem>

using ::testing::_;
using ::testing::AtLeast;
using ::testing::Return;
using ::testing::Throw;
using ::testing::StrEq;
using ::testing::Eq;
using ::testing::HasSubstr;
using ::testing::AllOf;
using ::testing::Not;
using ::testing::IsEmpty;

void
TwMailerClientTest::SetUp ()
{
    fs::path exe_path = fs::canonical ("/proc/self/exe");
    fs::path project_root = exe_path.parent_path ().parent_path ();

    fs::path config_path = project_root / "conf" / "twmailer.conf";
    Configuration::getInstance ().loadFromFile (config_path.string ());

    fs::path log_dir
            = project_root
              / Configuration::getInstance ().getString ("log_directory", "logs");
    if (!fs::exists (log_dir))
    {
        fs::create_directories (log_dir);
    }
    fs::path log_file = log_dir / "test_twmailer_client.log";
    Logger::getInstance ().initialize (log_file.string ());

    client_ = std::make_unique<MockTwMailerClient> (
            Configuration::getInstance ().getString ("server_address", "localhost"),
            static_cast<uint16_t> (
                    Configuration::getInstance ().getInt ("port", 8080)));
}

void
TwMailerClientTest::TearDown ()
{
}

TEST_F(TwMailerClientTest, ConfigurationTest)
{
    EXPECT_THAT(Configuration::getInstance().getString("server_address"), StrEq("localhost"));
    EXPECT_THAT(Configuration::getInstance().getInt("port"), Eq(8080));
    EXPECT_THAT(Configuration::getInstance().getString("mail_spool_directory"), StrEq("mail_spool"));
}

TEST_F(TwMailerClientTest, HandleLogin_Success)
{
    EXPECT_CALL(*client_, getInput("Username: ")).WillOnce(Return("testuser"));
    EXPECT_CALL(*client_, getMaskedInput("Password: ")).WillOnce(Return("password123"));
    EXPECT_CALL(*client_, sendCommand("LOGIN\ntestuser\npassword123\n", _)).Times(1);
    EXPECT_CALL(*client_, receiveResponse(_)).WillOnce(Return("OK\n"));

    testing::internal::CaptureStdout();
    client_->handleLogin();
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(client_->isAuthenticated_, Eq(true));
    EXPECT_THAT(output, HasSubstr("Login successful"));
}

TEST_F(TwMailerClientTest, HandleSend_Success)
{
    EXPECT_CALL(*client_, getInput("Receiver: ")).WillOnce(Return("receiver@example.com"));
    EXPECT_CALL(*client_, getInput("Subject: ")).WillOnce(Return("Test Subject"));
    EXPECT_CALL(*client_, validateEmail("receiver@example.com")).WillOnce(Return(true));
    EXPECT_CALL(*client_, validateSubject("Test Subject")).WillOnce(Return(true));

    std::string input = "Test message\n.\n";
    std::istringstream input_stream(input);
    std::streambuf* orig_cin = std::cin.rdbuf(input_stream.rdbuf());

    EXPECT_CALL(*client_, sendCommand("SEND receiver@example.com Test Subject Test message\n", false)).Times(1);
    EXPECT_CALL(*client_, receiveResponse(false)).WillOnce(Return("OK\n"));

    testing::internal::CaptureStdout();
    client_->handleSend();
    std::cin.rdbuf(orig_cin);
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(output, HasSubstr("OK"));
}

TEST_F(TwMailerClientTest, HandleList_Success)
{
    EXPECT_CALL(*client_, sendCommand("LIST", _)).Times(1);
    EXPECT_CALL(*client_, receiveResponse(_)).WillOnce(Return("OK\n2\nMail 1\nMail 2\n"));

    testing::internal::CaptureStdout();
    client_->handleList();
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(output, AllOf(
        HasSubstr("OK"),
        HasSubstr("2"),
        HasSubstr("Mail 1"),
        HasSubstr("Mail 2")
    ));
}

TEST_F(TwMailerClientTest, HandleRead_Success)
{
    EXPECT_CALL(*client_, getInput("Enter mail ID: ")).WillOnce(Return("1"));
    EXPECT_CALL(*client_, sendCommand("READ 1", _)).Times(1);
    EXPECT_CALL(*client_, receiveResponse(_))
        .WillOnce(Return("OK\n\nFrom: sender@example.com\nSubject: Test\nBody: Test message\n\n"));

    testing::internal::CaptureStdout();
    client_->handleRead();
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(output, AllOf(
        HasSubstr("From: sender@example.com"),
        HasSubstr("Subject: Test"),
        HasSubstr("Body: Test message")
    ));
}

TEST_F(TwMailerClientTest, ValidEmailValidation)
{
    EXPECT_CALL(*client_, validateEmail("valid.email@example.com")).WillOnce(Return(true));
    EXPECT_THAT(client_->validateEmail("valid.email@example.com"), Eq(true));
}

TEST_F(TwMailerClientTest, InvalidSubjectValidation)
{
    std::string long_subject(81, 'a');
    EXPECT_CALL(*client_, validateSubject(long_subject)).WillOnce(Return(false));
    EXPECT_THAT(client_->validateSubject(long_subject), Eq(false));
}


TEST_F(TwMailerClientTest, HandleLogin_Failure)
{
    EXPECT_CALL(*client_, getInput("Username: ")).WillOnce(Return("testuser"));
    EXPECT_CALL(*client_, getMaskedInput("Password: ")).WillOnce(Return("wrongpassword"));
    EXPECT_CALL(*client_, sendCommand("LOGIN\ntestuser\nwrongpassword\n", _)).Times(1);
    EXPECT_CALL(*client_, receiveResponse(_)).WillOnce(Return("ERR\n"));

    testing::internal::CaptureStdout();
    client_->handleLogin();
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(client_->isAuthenticated_, Eq(false));
    EXPECT_THAT(output, HasSubstr("Login failed"));
}

TEST_F(TwMailerClientTest, HandleSend_InvalidEmail)
{
    EXPECT_CALL(*client_, getInput("Receiver: ")).WillOnce(Return("invalid_email"));
    EXPECT_CALL(*client_, validateEmail("invalid_email")).WillOnce(Return(false));

    testing::internal::CaptureStdout();
    client_->handleSend();
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(output, HasSubstr("Invalid email address"));
}

TEST_F(TwMailerClientTest, HandleList_EmptyInbox)
{
    EXPECT_CALL(*client_, sendCommand("LIST", _)).Times(1);
    EXPECT_CALL(*client_, receiveResponse(_)).WillOnce(Return("OK\n0\n"));

    testing::internal::CaptureStdout();
    client_->handleList();
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(output, AllOf(
        HasSubstr("OK"),
        HasSubstr("0"),
        Not(HasSubstr("Mail"))
    ));
}

TEST_F(TwMailerClientTest, HandleDelete_Success)
{
    EXPECT_CALL(*client_, getInput("Enter mail ID to delete: ")).WillOnce(Return("1"));
    EXPECT_CALL(*client_, sendCommand("DEL 1", _)).Times(1);
    EXPECT_CALL(*client_, receiveResponse(_)).WillOnce(Return("OK\n"));

    testing::internal::CaptureStdout();
    client_->handleDelete();
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(output, HasSubstr("OK"));
}

TEST_F(TwMailerClientTest, HandleDelete_InvalidMailId)
{
    EXPECT_CALL(*client_, getInput("Enter mail ID to delete: ")).WillOnce(Return("invalid"));

    testing::internal::CaptureStdout();
    client_->handleDelete();
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(output, HasSubstr("Invalid mail ID"));
}

TEST_F(TwMailerClientTest, HandleQuit_Success)
{
    EXPECT_CALL(*client_, sendCommand("QUIT", _)).Times(1);
    EXPECT_CALL(*client_, receiveResponse(_)).WillOnce(Return("OK\nGoodbye\n"));

    testing::internal::CaptureStdout();
    client_->handleQuit();
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(output, AllOf(
        HasSubstr("OK"),
        HasSubstr("Goodbye")
    ));
}

TEST_F(TwMailerClientTest, HandleQuit_Failure)
{
    EXPECT_CALL(*client_, sendCommand("QUIT", _)).Times(1);
    EXPECT_CALL(*client_, receiveResponse(_)).WillOnce(Return("ERR\n"));

    testing::internal::CaptureStdout();
    client_->handleQuit();
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(output, HasSubstr("ERR"));
}

TEST_F(TwMailerClientTest, ValidSubjectValidation)
{
    EXPECT_CALL(*client_, validateSubject("Valid Subject")).WillOnce(Return(true));
    EXPECT_THAT(client_->validateSubject("Valid Subject"), Eq(true));
}

TEST_F(TwMailerClientTest, HandleReadWithInvalidMailId)
{
    EXPECT_CALL(*client_, getInput("Enter mail ID: ")).WillOnce(Return("invalid_id"));

    testing::internal::CaptureStdout();
    client_->handleRead();
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(output, HasSubstr("Invalid mail ID"));
}

TEST_F(TwMailerClientTest, HandleQuitSendsQuitCommand)
{
    EXPECT_CALL(*client_, sendCommand("QUIT", true)).Times(1);
    EXPECT_CALL(*client_, receiveResponse(true)).WillOnce(Return("OK\nGoodbye\n"));

    testing::internal::CaptureStdout();
    client_->handleQuit(true);
    std::string output = testing::internal::GetCapturedStdout();

    EXPECT_THAT(output, AllOf(
        HasSubstr("OK"),
        HasSubstr("Goodbye")
    ));
}
