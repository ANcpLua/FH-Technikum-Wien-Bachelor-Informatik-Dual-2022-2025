#ifndef SECURESTRING_H
#define SECURESTRING_H

#include <string>

class SecureString
{
public:
    SecureString (const std::string &str);

    ~SecureString ();

    const char *c_str () const;

    char *data ();

    size_t length () const;

private:
    std::string str_;
};

#endif