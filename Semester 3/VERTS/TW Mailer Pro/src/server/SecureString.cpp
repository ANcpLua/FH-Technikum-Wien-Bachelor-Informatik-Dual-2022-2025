#include "SecureString.h"
#include <cstring>

SecureString::SecureString (const std::string &str) : str_ (str) {}

SecureString::~SecureString ()
{
    volatile char *p = const_cast<volatile char *> (str_.data ());
    for (size_t i = 0; i < str_.length (); ++i)
    {
        p[i] = 0;
    }
}

const char *
SecureString::c_str () const
{
    return str_.c_str ();
}

char *
SecureString::data ()
{
    return &str_[0];
}

size_t
SecureString::length () const
{
    return str_.length ();
}