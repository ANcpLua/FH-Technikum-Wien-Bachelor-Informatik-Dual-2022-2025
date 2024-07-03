#include "../shared/Configuration.h"
#include "../shared/Logger.h"
#include "../shared/TwMailerExceptions.h"
#include "TwMailerServer.h"
#include <filesystem>
#include <iostream>

namespace fs = std::filesystem;

int
main (int argc, char *argv[])
{
    try
    {
        fs::path exe_path = fs::canonical ("/proc/self/exe");
        fs::path project_root = exe_path.parent_path ().parent_path ();
        fs::path config_path = project_root / "conf" / "twmailer.conf";

        auto &config = Configuration::getInstance ();
        config.loadFromFile (config_path.string ());
        config.parseCommandLine (argc, argv);

        fs::path log_dir
                = fs::path (config.getString ("log_directory", "./logs"));
        if (!fs::exists (log_dir))
        {
            fs::create_directories (log_dir);
        }

        fs::path log_file
                = log_dir / config.getString ("log_file", "twmailer.log");
        Logger::getInstance ().initialize (log_file.string ());

        uint16_t port = static_cast<uint16_t> (config.getInt ("port", 8080));
        fs::path mail_spool_dir
                = fs::path (config.getString ("mail_spool_directory", "./mail_spool"))
                        .lexically_normal ();
        if (mail_spool_dir.is_relative ())
        {
            mail_spool_dir = project_root / mail_spool_dir;
        }

        std::string ldapUri
                = config.getString ("ldap_uri", "ldap://ldap.technikum-wien.at");
        std::string ldapBindDnFormat = config.getString (
                "ldap_bind_dn_format", "uid=%s,ou=people,dc=technikum-wien,dc=at");

        if (!fs::exists (mail_spool_dir))
        {
            fs::create_directories (mail_spool_dir);
        }

        TwMailerServer server (port, mail_spool_dir.string (), ldapUri,
                               ldapBindDnFormat);
        server.start ();

        std::cout << "TwMailer server started on port " << port << std::endl;
        std::cout << "Mail spool directory: " << mail_spool_dir << std::endl;
        std::cout << "Press Enter to stop the server" << std::endl;

        std::cin.get ();
        server.stop ();
    }
    catch (const TwMailerException &e)
    {
        std::cerr << "TwMailer error: " << e.what () << std::endl;
        return 1;
    }
    catch (const std::exception &e)
    {
        std::cerr << "Unexpected error: " << e.what () << std::endl;
        return 1;
    }

    return 0;
}
