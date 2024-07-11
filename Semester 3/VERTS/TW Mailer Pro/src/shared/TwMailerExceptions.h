#ifndef TWMAILER_EXCEPTIONS_H
#define TWMAILER_EXCEPTIONS_H

#include <stdexcept>
#include <string>

class TwMailerException : public std::runtime_error
{
public:
    explicit TwMailerException (const std::string &message)
            : std::runtime_error (message)
    {
    }
};

class AuthenticationException : public TwMailerException
{
public:
    explicit AuthenticationException (const std::string &message)
            : TwMailerException ("Authentication error: " + message)
    {
    }
};

class MailException : public TwMailerException
{
public:
    explicit MailException (const std::string &message)
            : TwMailerException ("Mail error: " + message)
    {
    }
};

class ConfigurationException : public TwMailerException
{
public:
    explicit ConfigurationException (const std::string &message)
            : TwMailerException ("Configuration error: " + message)
    {
    }
};

class NetworkException : public TwMailerException
{
public:
    explicit NetworkException (const std::string &message)
            : TwMailerException ("Network error: " + message)
    {
    }
};

#endif