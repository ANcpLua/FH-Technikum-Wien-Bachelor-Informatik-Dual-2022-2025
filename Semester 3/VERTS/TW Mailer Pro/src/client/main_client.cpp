#include "../shared/Logger.h"
#include "TwMailerClient.h"
#include <iostream>

int
main (int argc, char *argv[])
{
    if (argc != 3)
    {
        std::cerr << "Usage: " << argv[0] << " <server_address> <server_port>"
                  << std::endl;
        return 1;
    }

    std::string serverAddress = argv[1];
    uint16_t serverPort = static_cast<uint16_t> (std::stoi (argv[2]));

    try
    {
        Logger::getInstance ().initialize ("twmailer_client.log");

        TwMailerClient client (serverAddress, serverPort);
        client.run ();
    }
    catch (const std::exception &e)
    {
        std::cerr << "Error: " << e.what () << std::endl;
        LOG_CRITICAL ("Client error: " + std::string (e.what ()));
        return 1;
    }

    return 0;
}