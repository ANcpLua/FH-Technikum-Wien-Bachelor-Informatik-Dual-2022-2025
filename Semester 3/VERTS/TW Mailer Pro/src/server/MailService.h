#ifndef MAILSERVICE_H
#define MAILSERVICE_H

#include <filesystem>
#include <string>
#include <vector>

class MailService
{
public:
    MailService (const std::string &mailSpoolDirectory);

    virtual ~MailService () = default;

    virtual bool sendMail (const std::string &sender, const std::string &receiver,
                           const std::string &subject, const std::string &body)
    = 0;

    virtual std::vector<std::string>
    listMails (const std::string &user) const = 0;

    virtual std::string readMail (const std::string &user,
                                  size_t mailId) const = 0;

    virtual bool deleteMail (const std::string &user, size_t mailId) = 0;

protected:
    std::filesystem::path mailSpoolDirectory_;
};

class MailServiceImpl : public MailService
{
public:
    MailServiceImpl (const std::string &mailSpoolDirectory);

    bool sendMail (const std::string &sender, const std::string &receiver,
                   const std::string &subject, const std::string &body) override;

    std::vector<std::string> listMails (const std::string &user) const override;

    std::string readMail (const std::string &user, size_t mailId) const override;

    bool deleteMail (const std::string &user, size_t mailId) override;

private:
    std::filesystem::path getUserMailbox (const std::string &user) const;

    std::filesystem::path getMailPath (const std::string &user,
                                       size_t mailId) const;

    bool saveEmail (const std::filesystem::path &path, const std::string &from,
                    const std::string &to, const std::string &subject,
                    const std::string &body);

    std::string
    generateSequentialFilename (const std::filesystem::path &mailboxPath) const;

    void renumberMails (const std::string &user);
};

#endif