#include "Configuration.h"
#include <algorithm>
#include <fstream>
#include <sstream>
#include <stdexcept>

Configuration &
Configuration::getInstance ()
{
    static Configuration instance;
    return instance;
}

void
Configuration::loadFromFile (const std::string &filename)
{
    std::ifstream file (filename);
    if (!file.is_open ())
    {
        throw std::runtime_error ("Unable to open configuration file: "
                                  + filename);
    }
    std::string line;
    while (std::getline (file, line))
    {
        line.erase (0, line.find_first_not_of (" \t"));
        line.erase (line.find_last_not_of (" \t") + 1);
        if (line.empty () || line[0] == '#')
        {
            continue;
        }
        auto delimiterPos = line.find ('=');
        if (delimiterPos != std::string::npos)
        {
            std::string key = line.substr (0, delimiterPos);
            std::string value = line.substr (delimiterPos + 1);
            key.erase (0, key.find_first_not_of (" \t"));
            key.erase (key.find_last_not_of (" \t") + 1);
            value.erase (0, value.find_first_not_of (" \t"));
            value.erase (value.find_last_not_of (" \t") + 1);
            settings_[key] = value;
        }
    }
}

void
Configuration::parseCommandLine (int argc, char *argv[])
{
    for (int i = 1; i < argc; ++i)
    {
        std::string arg = argv[i];
        if (arg.substr (0, 2) == "--")
        {
            size_t pos = arg.find ('=');
            if (pos != std::string::npos)
            {
                std::string key = arg.substr (2, pos - 2);
                std::string value = arg.substr (pos + 1);
                settings_[key] = value;
            }
            else if (i + 1 < argc)
            {
                std::string key = arg.substr (2);
                std::string value = argv[++i];
                settings_[key] = value;
            }
        }
    }
}

std::string
Configuration::getString (const std::string &key,
                          const std::string &defaultValue) const
{
    auto it = settings_.find (key);
    return (it != settings_.end ()) ? it->second : defaultValue;
}

int
Configuration::getInt (const std::string &key, int defaultValue) const
{
    auto it = settings_.find (key);
    return (it != settings_.end ()) ? std::stoi (it->second) : defaultValue;
}

bool
Configuration::getBool (const std::string &key, bool defaultValue) const
{
    auto it = settings_.find (key);
    if (it != settings_.end ())
    {
        std::string value = it->second;
        std::transform (value.begin (), value.end (), value.begin (), ::tolower);
        return (value == "true" || value == "1" || value == "yes");
    }
    return defaultValue;
}

void
Configuration::setString (const std::string &key, const std::string &value)
{
    settings_[key] = value;
}