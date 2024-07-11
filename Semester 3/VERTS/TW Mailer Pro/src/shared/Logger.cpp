#include "Logger.h"
#include <chrono>
#include <iomanip>
#include <iostream>

Logger &
Logger::getInstance ()
{
    static Logger instance;
    return instance;
}

void
Logger::initialize (const std::string &logFilePath)
{
    std::lock_guard<std::mutex> lock (mutex_);
    logFile_.open (logFilePath, std::ios::app);
    if (!logFile_.is_open ())
    {
        throw std::runtime_error ("Failed to open log file: " + logFilePath);
    }
}

void
Logger::log (LogLevel level, const std::string &message)
{
    std::lock_guard<std::mutex> lock (mutex_);
    std::string logEntry = getCurrentTimestamp () + " [" + levelToString (level)
                           + "] " + message + "\n";
    logFile_ << logEntry;
    logFile_.flush ();
    std::cout << logEntry;
}

std::string
Logger::levelToString (LogLevel level) const
{
    switch (level)
    {
        case LogLevel::DEBUG:
            return "DEBUG";
        case LogLevel::INFO:
            return "INFO";
        case LogLevel::WARNING:
            return "WARNING";
        case LogLevel::ERROR:
            return "ERROR";
        case LogLevel::CRITICAL:
            return "CRITICAL";
        default:
            return "UNKNOWN";
    }
}

std::string
Logger::getCurrentTimestamp () const
{
    auto now = std::chrono::system_clock::now ();
    auto in_time_t = std::chrono::system_clock::to_time_t (now);
    std::stringstream ss;
    ss << std::put_time (std::localtime (&in_time_t), "%Y-%m-%d %H:%M:%S");
    return ss.str ();
}