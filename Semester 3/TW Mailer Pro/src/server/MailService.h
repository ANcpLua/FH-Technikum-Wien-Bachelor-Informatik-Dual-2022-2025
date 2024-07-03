#ifndef MAIL_SERVICE_H
#define MAIL_SERVICE_H

#include <string>
#include <vector>
#include <filesystem>

class MailService
{
public:
    explicit MailService(const std::string& mailSpoolDirectory);

    bool sendMail(
            const std::string& sender,
            const std::string& receiver,
            const std::string& subject,
            const std::string& body
    );

    std::vector<std::string> listMails(const std::string& user) const;
    std::string readMail(const std::string& user, size_t mailId) const;
    bool deleteMail(const std::string& user, size_t mailId);

private:
    std::filesystem::path getUserMailbox(const std::string& user) const;
    std::filesystem::path getMailPath(const std::string& user, size_t mailId) const;
    size_t getNextMailId(const std::string& user);

    std::filesystem::path mailSpoolDirectory_;
};

#endif // MAIL_SERVICE_H
