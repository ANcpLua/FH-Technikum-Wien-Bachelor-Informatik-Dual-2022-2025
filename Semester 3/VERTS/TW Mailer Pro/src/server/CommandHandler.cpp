#include "../shared/ClientSession.h"
#include "../shared/Logger.h"
#include "../shared/TwMailerExceptions.h"
#include "AuthenticationService.h"
#include "CommandHandler.h"
#include "MailService.h"
#include <algorithm>
#include <cctype>
#include <sstream>

CommandHandler::CommandHandler (AuthenticationService &authService,
                                MailService &mailService)
        : authService_ (authService), mailService_ (mailService)
{
    commandMap_["LOGIN"] = [this] (const auto &args, auto &session) {
        this->handleLogin (args, session);
    };
    commandMap_["SEND"] = [this] (const auto &args, auto &session) {
        this->handleSend (args, session);
    };
    commandMap_["LIST"] = [this] (const auto &args, auto &session) {
        this->handleList (args, session);
    };
    commandMap_["READ"] = [this] (const auto &args, auto &session) {
        this->handleRead (args, session);
    };
    commandMap_["DEL"] = [this] (const auto &args, auto &session) {
        this->handleDelete (args, session);
    };
    commandMap_["QUIT"] = [this] (const auto &args, auto &session) {
        this->handleQuit (args, session);
    };
}

void
CommandHandler::handleCommand (const std::string &command,
                               ClientSession &session)
{
    try
    {
        std::istringstream iss (command);
        std::string cmd;
        iss >> cmd;
        std::transform (cmd.begin (), cmd.end (), cmd.begin (), ::toupper);

        std::vector<std::string> args;
        std::string arg;
        while (iss >> arg)
        {
            args.push_back (arg);
        }

        LOG_INFO ("Received command: " + cmd
                  + " from user: " + session.getUsername ());

        auto it = commandMap_.find (cmd);
        if (it != commandMap_.end ())
        {
            it->second (args, session);
        }
        else
        {
            throw TwMailerException ("Unknown command: " + cmd);
        }
    }
    catch (const TwMailerException &e)
    {
        LOG_WARNING ("Command error: " + std::string (e.what ()));
        session.sendResponse ("ERR " + std::string (e.what ()) + "\n");
    }
    catch (const std::exception &e)
    {
        LOG_ERROR ("Unexpected error handling command: "
                   + std::string (e.what ()));
        session.sendResponse ("ERR An unexpected error occurred\n");
    }
}

void
CommandHandler::handleLogin (const std::vector<std::string> &args,
                             ClientSession &session)
{
    if (args.size () != 2)
    {
        session.sendResponse (
                "ERR Invalid number of arguments for LOGIN\n");
        return;
    }

    const std::string &username = args[0];
    const std::string &password = args[1];

    if (authService_.authenticate (username, password, session.getClientIp ()))
    {
        session.setAuthenticated (true);
        session.setUsername (username);
        session.sendResponse ("OK Login successful\n");
        LOG_INFO ("User " + username + " logged in successfully");
    }
    else
    {
        int remainingAttempts
                = authService_.getRemainingAttempts (session.getClientIp ());
        if (remainingAttempts > 0)
        {
            session.sendResponse ("ERR Login failed. "
                                  + std::to_string (remainingAttempts)
                                  + " attempts remaining.\n");
        }
        else
        {
            session.sendResponse (
                    "ERR Login failed. IP has been blacklisted.\n");
        }
        LOG_WARNING ("Login failed for user " + username);
    }
}

void
CommandHandler::handleSend (const std::vector<std::string> &args,
                            ClientSession &session)
{
    if (!session.isAuthenticated ())
    {
        session.sendResponse ("ERR Not authenticated\n");
        return;
    }

    if (args.size () < 3)
    {
        session.sendResponse ("ERR Invalid number of arguments for SEND\n");
        return;
    }

    const std::string &receiver = args[0];
    const std::string &subject = args[1];
    std::string body = args[2];
    for (size_t i = 3; i < args.size (); ++i)
    {
        body += " " + args[i];
    }

    if (mailService_.sendMail (session.getUsername (), receiver, subject, body))
    {
        mailService_.sendMail (session.getUsername (),
                               session.getUsername (), subject, body);
        session.sendResponse ("OK Mail sent\n");
        LOG_INFO ("Mail sent from " + session.getUsername () + " to "
                  + receiver);
    }
    else
    {
        session.sendResponse ("ERR Failed to send mail\n");
        LOG_ERROR ("Failed to send mail from " + session.getUsername ()
                   + " to " + receiver);
    }
}

void
CommandHandler::handleList (const std::vector<std::string> & /* args */,
                            ClientSession &session)
{
    if (!session.isAuthenticated ())
    {
        session.sendResponse ("ERR Not authenticated\n");
        return;
    }

    auto mailList = mailService_.listMails (session.getUsername ());
    std::ostringstream oss;
    oss << "OK " << mailList.size () << "\n";
    for (const auto &mail : mailList)
    {
        oss << mail << "\n";
    }
    session.sendResponse (oss.str ());
    LOG_INFO ("Listed " + std::to_string (mailList.size ()) + " mails for user "
              + session.getUsername ());
}

void
CommandHandler::handleRead (const std::vector<std::string> &args,
                            ClientSession &session)
{
    if (!session.isAuthenticated ())
    {
        session.sendResponse ("ERR Not authenticated\n");
        return;
    }

    if (args.size () != 1)
    {
        session.sendResponse ("ERR Invalid number of arguments for READ\n");
        return;
    }

    size_t mailId;
    try
    {
        mailId = std::stoul (args[0]);
    }
    catch (const std::exception &)
    {
        session.sendResponse ("ERR Invalid mail ID\n");
        return;
    }

    std::string mailContent
            = mailService_.readMail (session.getUsername (), mailId);
    if (!mailContent.empty ())
    {
        session.sendResponse ("OK\n" + mailContent + "\n");
        LOG_INFO ("Mail " + std::to_string (mailId) + " read by user "
                  + session.getUsername ());
    }
    else
    {
        session.sendResponse ("ERR Mail not found\n");
        LOG_WARNING ("Mail " + std::to_string (mailId)
                     + " not found for user " + session.getUsername ());
    }
}

void
CommandHandler::handleDelete (const std::vector<std::string> &args,
                              ClientSession &session)
{
    if (!session.isAuthenticated ())
    {
        session.sendResponse ("ERR Not authenticated\n");
        return;
    }

    if (args.size () != 1)
    {
        session.sendResponse ("ERR Invalid number of arguments for DEL\n");
        return;
    }

    size_t mailId;
    try
    {
        mailId = std::stoul (args[0]);
    }
    catch (const std::exception &)
    {
        session.sendResponse ("ERR Invalid mail ID\n");
        return;
    }

    if (mailService_.deleteMail (session.getUsername (), mailId))
    {
        session.sendResponse ("OK Mail deleted\n");
        LOG_INFO ("Mail " + std::to_string (mailId) + " deleted by user "
                  + session.getUsername ());
    }
    else
    {
        session.sendResponse ("ERR Failed to delete mail\n");
        LOG_WARNING ("Failed to delete mail " + std::to_string (mailId)
                     + " for user " + session.getUsername ());
    }
}

void
CommandHandler::handleQuit (const std::vector<std::string> & /* args */,
                            ClientSession &session)
{
    session.sendResponse ("OK Goodbye\n");
    LOG_INFO ("User " + session.getUsername () + " disconnected");
    session.stop ();
}
