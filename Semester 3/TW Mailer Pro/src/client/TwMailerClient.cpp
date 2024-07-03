#include "Logger.h"
#include "TwMailerClient.h"
#include "TwMailerExceptions.h"
#include <algorithm>
#include <arpa/inet.h>
#include <cctype>
#include <cstring>
#include <iostream>
#include <netinet/in.h>
#include <regex>
#include <sstream>
#include <sys/socket.h>
#include <unistd.h>

class TwMailerClient::Impl
{
public:
    Impl (const std::string &serverAddress, uint16_t serverPort)
            : serverAddress_ (serverAddress), serverPort_ (serverPort), socket_ (-1)
    {
    }

    std::string serverAddress_;
    uint16_t serverPort_;
    int socket_;
};

TwMailerClient::TwMailerClient (const std::string &serverAddress,
                                uint16_t serverPort)
        : isAuthenticated_ (false),
          pImpl (std::make_unique<Impl> (serverAddress, serverPort))
{
}

TwMailerClient::~TwMailerClient () { disconnect (); }

void
TwMailerClient::run ()
{
    try
    {
        connect ();
        std::cout << "Connected to TwMailer server." << std::endl;

        while (true)
        {
            if (!isAuthenticated_)
            {
                std::cout << "\nAvailable commands: LOGIN, QUIT" << std::endl;
            }
            else
            {
                std::cout << "\nAvailable commands: SEND, LIST, "
                             "READ, DEL, QUIT"
                          << std::endl;
            }

            std::string command = getInput ("Enter command: ");
            std::transform (command.begin (), command.end (), command.begin (),
                            ::toupper);

            if (command == "LOGIN")
            {
                if (isAuthenticated_)
                {
                    std::cout << "You are already logged in." << std::endl;
                }
                else
                {
                    handleLogin ();
                }
            }
            else if (command == "QUIT")
            {
                handleQuit ();
                break;
            }
            else if (!isAuthenticated_)
            {
                std::cout << "You need to log in first." << std::endl;
            }
            else if (command == "SEND")
            {
                handleSend ();
            }
            else if (command == "LIST")
            {
                handleList ();
            }
            else if (command == "READ")
            {
                handleRead ();
            }
            else if (command == "DEL")
            {
                handleDelete ();
            }
            else
            {
                std::cout << "Unknown command. Please try again." << std::endl;
            }
        }
    }
    catch (const TwMailerException &e)
    {
        std::cerr << "TwMailer error: " << e.what () << std::endl;
    }
    catch (const std::exception &e)
    {
        std::cerr << "Unexpected error: " << e.what () << std::endl;
    }
}

void
TwMailerClient::connect ()
{
    pImpl->socket_ = socket (AF_INET, SOCK_STREAM, 0);
    if (pImpl->socket_ == -1)
    {
        throw NetworkException ("Failed to create socket");
    }

    sockaddr_in serverAddr{};
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons (pImpl->serverPort_);

    int res = inet_pton (AF_INET, pImpl->serverAddress_.c_str (),
                         &serverAddr.sin_addr);
    if (res == 0)
    {
        throw NetworkException ("Invalid address format");
    }
    else if (res < 0)
    {
        throw NetworkException ("Address conversion error: "
                                + std::string (strerror (errno)));
    }

    if (::connect (pImpl->socket_, (struct sockaddr *)&serverAddr,
                   sizeof (serverAddr))
        < 0)
    {
        throw NetworkException ("Connection failed: "
                                + std::string (strerror (errno)));
    }
}

void
TwMailerClient::disconnect ()
{
    if (pImpl->socket_ != -1)
    {
        close (pImpl->socket_);
        pImpl->socket_ = -1;
    }
}

void
TwMailerClient::sendCommand (const std::string &command)
{
    std::cout << "DEBUG: Sending command:\n" << command << std::endl;
    ssize_t bytesSent
            = send (pImpl->socket_, command.c_str (), command.length (), 0);
    if (bytesSent == -1)
    {
        throw NetworkException ("Failed to send command");
    }
    if (static_cast<size_t> (bytesSent) != command.length ())
    {
        throw NetworkException ("Failed to send entire command");
    }
}

std::string
TwMailerClient::receiveResponse ()
{
    char buffer[4096];
    std::string response;
    ssize_t bytesReceived;

    do
    {
        bytesReceived = recv (pImpl->socket_, buffer, sizeof (buffer) - 1, 0);
        if (bytesReceived > 0)
        {
            buffer[bytesReceived] = '\0';
            response += buffer;
        }
        else if (bytesReceived == 0)
        {
            throw NetworkException ("Server closed the connection");
        }
        else
        {
            throw NetworkException ("Failed to receive response: "
                                    + std::string (strerror (errno)));
        }
    }
    while (bytesReceived == sizeof (buffer) - 1);

    std::cout << "DEBUG: Received response: " << response << std::endl;
    return response;
}

void
TwMailerClient::handleLogin ()
{
    std::string username = getInput ("Username: ");
    std::string password = getInput ("Password: ");

    sendCommand ("LOGIN\n" + username + "\n" + password + "\n");
    std::string response = receiveResponse ();
    std::cout << response << std::endl;

    if (response.substr (0, 2) == "OK")
    {
        isAuthenticated_ = true;
        std::cout << "Login successful. You can now use other commands."
                  << std::endl;
    }
    else
    {
        std::cout << "Login failed. Please try again." << std::endl;
    }

    void TwMailerClient::handleSend ()
    {
        std::string receiver = getInput ("Receiver: ");
        if (!validateEmail (receiver))
        {
            std::cout << "Invalid email address. Please try again." << std::endl;
            return;
        }

        std::string subject = getInput ("Subject: ");
        if (!validateSubject (subject))
        {
            std::cout << "Invalid subject. Must be 80 characters or less."
                      << std::endl;
            return;
        }

        std::cout << "Message (end with a line containing only '.'):" << std::endl;
        std::string body;
        std::string line;
        while (std::getline (std::cin, line))
        {
            if (line == ".")
            {
                break;
            }
            body += line + "\n";
        }

        if (!body.empty () && body.back () == '\n')
        {
            body.pop_back ();
        }

        std::ostringstream command;
        command << "SEND\n"
                << receiver << "\n"
                << subject << "\n"
                << body << "\n.\n";
        sendCommand (command.str ());
        std::string response = receiveResponse ();
        std::cout << response << std::endl;
    }

    void TwMailerClient::handleList ()
    {
        sendCommand ("LIST");
        std::string response = receiveResponse ();
        std::cout << response << std::endl;
    }

    void TwMailerClient::handleRead ()
    {
        std::string mailId = getInput ("Enter mail ID: ");
        try
        {
            size_t id = std::stoul (mailId);
            sendCommand ("READ " + mailId);
            std::string response = receiveResponse ();
            std::cout << response << std::endl;
        }
        catch (const std::exception &)
        {
            std::cout << "Invalid mail ID. Please enter a number." << std::endl;
        }
    }

    void TwMailerClient::handleDelete ()
    {
        std::string mailId = getInput ("Enter mail ID to delete: ");
        try
        {
            size_t id = std::stoul (mailId);
            sendCommand ("DEL " + mailId);
            std::string response = receiveResponse ();
            std::cout << response << std::endl;
        }
        catch (const std::exception &)
        {
            std::cout << "Invalid mail ID. Please enter a number." << std::endl;
        }
    }

    void TwMailerClient::handleQuit ()
    {
        sendCommand ("QUIT");
        std::string response = receiveResponse ();
        std::cout << response << std::endl;
    }

    std::string TwMailerClient::getInput (const std::string &prompt)
    {
        std::string input;
        std::cout << prompt;
        std::getline (std::cin, input);
        return input;
    }

    std::string TwMailerClient::getMultiLineInput (const std::string &prompt)
    {
        std::cout << prompt << std::endl;
        std::string input, line;
        while (std::getline (std::cin, line) && line != ".")
        {
            input += line + "\n";
        }
        return input;
    }

    bool TwMailerClient::validateEmail (const std::string &email)
    {
        const std::regex pattern ("(\\w+)(\\.|_)?(\\w*)@(\\w+)(\\.(\\w+))+");
        return std::regex_match (email, pattern);
    }

    bool TwMailerClient::validateSubject (const std::string &subject)
    {
        return subject.length () <= 80;
    }
}