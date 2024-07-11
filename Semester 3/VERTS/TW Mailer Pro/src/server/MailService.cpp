#include "../shared/Logger.h"
#include "../shared/TwMailerExceptions.h"
#include "MailService.h"
#include <algorithm>
#include <chrono>
#include <filesystem>
#include <fstream>
#include <map>
#include <sstream>

namespace fs = std::filesystem;

MailService::MailService (const std::string &mailSpoolDirectory)
        : mailSpoolDirectory_ (fs::absolute (mailSpoolDirectory))
{
    fs::create_directories (mailSpoolDirectory_);
    LOG_INFO ("Mail spool directory set to: " + mailSpoolDirectory_.string ());
}

MailServiceImpl::MailServiceImpl (const std::string &mailSpoolDirectory)
        : MailService (mailSpoolDirectory)
{
}

bool
MailServiceImpl::sendMail (const std::string &sender,
                           const std::string &receiver,
                           const std::string &subject, const std::string &body)
{
    try
    {
        fs::path senderMailbox = getUserMailbox (sender);
        fs::create_directories (senderMailbox);
        std::string filename = generateSequentialFilename (senderMailbox);
        fs::path mailPath = senderMailbox / filename;

        LOG_INFO ("Attempting to save email at: " + mailPath.string ());

        if (saveEmail (mailPath, sender, receiver, subject, body))
        {
            LOG_INFO ("Mail saved successfully at: " + mailPath.string ());
            return true;
        }
        else
        {
            LOG_ERROR ("Failed to save email at: " + mailPath.string ());
            return false;
        }
    }
    catch (const std::exception &e)
    {
        LOG_ERROR ("Error sending mail: " + std::string (e.what ()));
        return false;
    }
}

std::vector<std::string>
MailServiceImpl::listMails (const std::string &user) const
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

    std::map<size_t, std::string> mailMap;
    for (const auto &entry : fs::directory_iterator (userMailbox))
    {
        if (fs::is_regular_file (entry))
        {
            std::string filename = entry.path ().filename ().string ();
            size_t mailId = std::stoul (filename);
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
            mailMap[mailId] = subject;
        }
    }

    for (const auto &[id, subject] : mailMap)
    {
        mailList.push_back (std::to_string (id) + ". " + subject);
    }

    LOG_INFO ("Found " + std::to_string (mailList.size ()) + " mails for user "
              + user);
    return mailList;
}

std::string
MailServiceImpl::readMail (const std::string &user, size_t mailId) const
{
    fs::path mailPath = getMailPath (user, mailId);

    if (mailPath.empty () || !fs::exists (mailPath))
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
MailServiceImpl::deleteMail (const std::string &user, size_t mailId)
{
    fs::path mailPath = getMailPath (user, mailId);
    if (mailPath.empty () || !fs::exists (mailPath))
    {
        LOG_WARNING ("Mail not found for deletion: " + mailPath.string ());
        return false;
    }

    try
    {
        fs::remove (mailPath);
        LOG_INFO ("Mail deleted successfully: " + mailPath.string ());
        renumberMails (user);
        return true;
    }
    catch (const std::exception &e)
    {
        LOG_ERROR ("Error deleting mail: " + std::string (e.what ()));
        return false;
    }
}

fs::path
MailServiceImpl::getUserMailbox (const std::string &user) const
{
    fs::path userMailbox = mailSpoolDirectory_ / user;
    LOG_INFO ("User mailbox path: " + userMailbox.string ());
    return userMailbox;
}

fs::path
MailServiceImpl::getMailPath (const std::string &user, size_t mailId) const
{
    fs::path userMailbox = getUserMailbox (user);
    std::string mailIdStr = std::to_string (mailId) + ".eml";

    for (const auto &entry : fs::directory_iterator (userMailbox))
    {
        if (fs::is_regular_file (entry) && entry.path ().filename () == mailIdStr)
        {
            return entry.path ();
        }
    }
    LOG_WARNING ("Mail not found for user " + user + " with ID "
                 + std::to_string (mailId));
    return fs::path ();
}

bool
MailServiceImpl::saveEmail (const fs::path &path, const std::string &from,
                            const std::string &to, const std::string &subject,
                            const std::string &body)
{
    std::ofstream file (path);
    if (!file.is_open ())
    {
        LOG_ERROR ("Failed to open file for writing: " + path.string ());
        return false;
    }

    file << "From: " << from << "\n"
         << "To: " << to << "\n"
         << "Subject: " << subject << "\n\n"
         << body;

    if (file.fail ())
    {
        LOG_ERROR ("Failed to write to file: " + path.string ());
        return false;
    }

    return true;
}

std::string
MailServiceImpl::generateSequentialFilename (const fs::path &mailboxPath) const
{
    size_t maxId = 0;
    for (const auto &entry : fs::directory_iterator (mailboxPath))
    {
        if (fs::is_regular_file (entry))
        {
            std::string filename = entry.path ().filename ().string ();
            size_t id = std::stoul (filename);
            if (id > maxId)
            {
                maxId = id;
            }
        }
    }
    return std::to_string (maxId + 1) + ".eml";
}

void
MailServiceImpl::renumberMails (const std::string &user)
{
    fs::path userMailbox = getUserMailbox (user);
    std::map<size_t, fs::path> mailMap;
    for (const auto &entry : fs::directory_iterator (userMailbox))
    {
        if (fs::is_regular_file (entry))
        {
            size_t mailId = std::stoul (entry.path ().filename ().string ());
            mailMap[mailId] = entry.path ();
        }
    }

    size_t newId = 1;
    for (const auto &[oldId, path] : mailMap)
    {
        if (oldId != newId)
        {
            fs::path newPath = userMailbox / (std::to_string (newId) + ".eml");
            fs::rename (path, newPath);
        }
        newId++;
    }
}