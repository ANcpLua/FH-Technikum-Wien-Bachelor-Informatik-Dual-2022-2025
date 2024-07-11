#ifndef TWMAILER_CLIENT_TESTS_H
#define TWMAILER_CLIENT_TESTS_H

#include "../src/shared/Configuration.h"
#include "../src/shared/Logger.h"
#include "../tests/MockTwMailerClient.h"
#include <filesystem>
#include <gmock/gmock.h>
#include <gtest/gtest.h>
#include <memory>

namespace fs = std::filesystem;

class TwMailerClientTest : public ::testing::Test
{
protected:
    void SetUp () override;

    void TearDown () override;

    std::unique_ptr<MockTwMailerClient> client_;
};

#endif
