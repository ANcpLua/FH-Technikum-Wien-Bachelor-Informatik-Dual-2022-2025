#ifndef LOGGER_H
#define LOGGER_H

#include <fstream>
#include <mutex>
#include <string>

enum LogLevel
{
    DEBUG,
    INFO,
    WARNING,
    ERROR,
    CRITICAL
};

class Logger
{
public:
    static Logger &getInstance ();

    void initialize (const std::string &logFilePath);
    void log (LogLevel level, const std::string &message);

    void
    debug (const std::string &message)
    {
        log (LogLevel::DEBUG, message);
    }
    void
    info (const std::string &message)
    {
        log (LogLevel::INFO, message);
    }
    void
    warning (const std::string &message)
    {
        log (LogLevel::WARNING, message);
    }
    void
    error (const std::string &message)
    {
        log (LogLevel::ERROR, message);
    }
    void
    critical (const std::string &message)
    {
        log (LogLevel::CRITICAL, message);
    }

private:
    Logger () = default;
    Logger (const Logger &) = delete;
    Logger &operator= (const Logger &) = delete;

    std::string levelToString (LogLevel level) const;
    std::string getCurrentTimestamp () const;

    std::ofstream logFile_;
    std::mutex mutex_;
};

#define LOG_DEBUG(message) Logger::getInstance ().debug (message)
#define LOG_INFO(message) Logger::getInstance ().info (message)
#define LOG_WARNING(message) Logger::getInstance ().warning (message)
#define LOG_ERROR(message) Logger::getInstance ().error (message)
#define LOG_CRITICAL(message) Logger::getInstance ().critical (message)

#endif // LOGGER_H
