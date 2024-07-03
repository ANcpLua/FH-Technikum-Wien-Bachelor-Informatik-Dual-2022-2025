#ifndef TWMAILER_CLIENT_H
#define TWMAILER_CLIENT_H

#include <string>
#include <memory>

class TwMailerClient
{
public:
    TwMailerClient(const std::string& serverAddress, uint16_t serverPort);
    ~TwMailerClient();

    void run();

private:
    bool isAuthenticated_;

    void connect();
    void disconnect();
    void sendCommand(const std::string& command);
    std::string receiveResponse();

    void handleLogin();
    void handleSend();
    void handleList();
    void handleRead();
    void handleDelete();
    void handleQuit();

    std::string getInput(const std::string& prompt);
    std::string getMultiLineInput(const std::string& prompt);
    bool validateEmail(const std::string& email);
    bool validateSubject(const std::string& subject);

    class Impl;
    std::unique_ptr<Impl> pImpl;
};

#endif // TWMAILER_CLIENT_H
