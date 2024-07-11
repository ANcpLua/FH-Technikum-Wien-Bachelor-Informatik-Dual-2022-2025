#ifndef TWMAILER_CLIENT_H
#define TWMAILER_CLIENT_H

#include <cstdint>
#include <memory>
#include <string>

class MockTwMailerClient;

class TwMailerClient
{
public:
    TwMailerClient (const std::string &serverAddress, uint16_t serverPort);

    virtual ~TwMailerClient ();

    void run ();

protected:
    bool isAuthenticated_;

    virtual void connect ();

    virtual void disconnect ();

    virtual void sendCommand (const std::string &command, bool debug = true);

    virtual std::string receiveResponse (bool debug = true);

    virtual std::string getInput (const std::string &prompt);

    virtual std::string getMaskedInput (const std::string &prompt);

    virtual std::string getMultiLineInput (const std::string &prompt);

    virtual bool validateEmail (const std::string &email);

    virtual bool validateSubject (const std::string &subject);

    virtual void handleLogin ();

    virtual void handleSend (bool debug = false);

    virtual void handleList (bool debug = false);

    virtual void handleRead (bool debug = false);

    virtual void handleDelete (bool debug = false);

    virtual void handleQuit (bool debug = false);

private:
    class Impl;

    std::unique_ptr<Impl> pImpl;

    friend class MockTwMailerClient;
};

#endif