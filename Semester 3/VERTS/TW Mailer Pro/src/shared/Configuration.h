#ifndef CONFIGURATION_H
#define CONFIGURATION_H

#include <string>
#include <unordered_map>

class Configuration
{
public:
    static Configuration &getInstance ();

    void loadFromFile (const std::string &filename);
    void parseCommandLine (int argc, char *argv[]);

    std::string getString (const std::string &key,
                           const std::string &defaultValue = "") const;
    int getInt (const std::string &key, int defaultValue = 0) const;
    bool getBool (const std::string &key, bool defaultValue = false) const;

private:
    Configuration () = default;
    Configuration (const Configuration &) = delete;
    Configuration &operator= (const Configuration &) = delete;

    std::unordered_map<std::string, std::string> settings_;
};

#endif // CONFIGURATION_H
