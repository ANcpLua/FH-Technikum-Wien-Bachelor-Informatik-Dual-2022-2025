#include "../shared/Logger.h"
#include "../shared/TwMailerExceptions.h"
#include "MailService.h"
#include <algorithm>
#include <filesystem>
#include <fstream>
#include <sstream>

namespace fs = std::filesystem;

MailService::MailService (const std::string &mailSpoolDirectory)
        : mailSpoolDirectory_ (fs::absolute (mailSpoolDirectory))
{
    fs::create_directories (mailSpoolDirectory_);
    LOG_INFO ("Mail spool directory set to: " + mailSpoolDirectory_.string ());
}

bool
MailService::sendMail (const std::string &sender, const std::string &receiver,
                       const std::string &subject, const std::string &body)
{
    try
    {
        fs::path receiverMailbox = getUserMailbox (receiver);
        fs::create_directories (receiverMailbox);
        size_t mailId = getNextMailId (receiver);
        fs::path mailPath = getMailPath (receiver, mailId);

        LOG_INFO ("Attempting to save email at: " + mailPath.string ());

        std::ofstream mailFile (mailPath);
        if (!mailFile.is_open ())
        {
            LOG_ERROR ("Failed to create mail file: " + mailPath.string ());
            throw MailException ("Failed to create mail file");
        }

        mailFile << "From: " << sender << "\n"
                 << "To: " << receiver << "\n"
                 << "Subject: " << subject << "\n\n"
                 << body;

        LOG_INFO ("Mail saved successfully at: " + mailPath.string ());
        return true;
    }
    catch (const std::exception &e)
    {
        LOG_ERROR ("Error sending mail: " + std::string (e.what ()));
        return false;
    }
}

std::vector<std::string>
MailService::listMails (const std::string &user) const
{
    std::vector<std::string> mailList;
    fs::path userMailbox = getUserMailbox (user);

    if (!fs::exists (userMailbox))
    {
        fs::create_directories (userMailbox);
        LOG_INFO ("Created mailbox for user: " + user);
    }

    LOG_INFO ("Listing mails for user: " + user
              + " in directory: " + userMailbox.string ());

    if (fs::exists (userMailbox) && fs::is_directory (userMailbox))
    {
        size_t mailId = 1;
        for (const auto &entry : fs::directory_iterator (userMailbox))
        {
            if (fs::is_regular_file (entry))
            {
                LOG_INFO ("Found mail file: " + entry.path ().string ());
                std::ifstream mailFile (entry.path ());
                std::string line;
                std::string subject;
                while (std::getline (mailFile, line))
                {
                    if (line.rfind ("Subject: ", 0) == 0)
                    {
                        subject = line.substr (9);
                        break;
                    }
                }
                mailList.push_back (std::to_string (mailId) + ". " + subject);
                ++mailId;
            }
        }
    }
    else
    {
        LOG_WARNING ("User mailbox not found or not a directory: "
                     + userMailbox.string ());
    }

    LOG_INFO ("Found " + std::to_string (mailList.size ()) + " mails for user "
              + user);
    return mailList;
}

std::string
MailService::readMail (const std::string &user, size_t mailId) const
{
    fs::path mailPath = getMailPath (user, mailId);

    LOG_INFO ("Attempting to read mail: " + mailPath.string ());

    if (!fs::exists (mailPath))
    {
        LOG_WARNING ("Mail not found: " + mailPath.string ());
        return "";
    }

    std::ifstream mailFile (mailPath);
    std::ostringstream contentStream;
    contentStream << mailFile.rdbuf ();
    LOG_INFO ("Successfully read mail: " + mailPath.string ());
    return contentStream.str ();
}

bool
MailService::deleteMail (const std::string &user, size_t mailId)
{
    fs::path mailPath = getMailPath (user, mailId);

    LOG_INFO ("Attempting to delete mail: " + mailPath.string ());

    if (!fs::exists (mailPath))
    {
        LOG_WARNING ("Mail not found for deletion: " + mailPath.string ());
        return false;
    }

    try
    {
        fs::remove (mailPath);
        LOG_INFO ("Mail deleted successfully: " + mailPath.string ());
        return true;
    }
    catch (const std::exception &e)
    {
        LOG_ERROR ("Error deleting mail: " + std::string (e.what ()));
        return false;
    }
}

fs::path
MailService::getUserMailbox (const std::string &user) const
{
    fs::path userMailbox = mailSpoolDirectory_ / user;
    LOG_INFO ("User mailbox path: " + userMailbox.string ());
    return userMailbox;
}

fs::path
MailService::getMailPath (const std::string &user, size_t mailId) const
{
    fs::path mailPath
            = getUserMailbox (user) / (std::to_string (mailId) + ".eml");
    LOG_INFO ("Mail path: " + mailPath.string ());
    return mailPath;
}

size_t
MailService::getNextMailId (const std::string &user)
{
    fs::path userMailbox = getUserMailbox (user);
    size_t maxId = 0;

    for (const auto &entry : fs::directory_iterator (userMailbox))
    {
        if (fs::is_regular_file (entry))
        {
            std::string filename = entry.path ().filename ().string ();
            size_t dotPos = filename.find ('.');
            if (dotPos != std::string::npos)
            {
                try
                {
                    size_t id = std::stoul (filename.substr (0, dotPos));
                    maxId = std::max (maxId, id);
                }
                catch (const std::exception &e)
                {
                    LOG_WARNING ("Invalid mail file name: " + filename);
                }
            }
        }
    }

    LOG_INFO ("Next mail ID for user " + user + ": "
              + std::to_string (maxId + 1));
    return maxId + 1;
}
