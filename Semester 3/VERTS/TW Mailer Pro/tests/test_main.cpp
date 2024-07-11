#include <gtest/gtest.h>
#include <gmock/gmock.h>
#include <iostream>

class TestEventListener : public testing::EmptyTestEventListener
{
public:
    void OnTestProgramStart(const testing::UnitTest &unit_test) override
    {
        std::cout << "Starting test program. Total test suites: "
                  << unit_test.total_test_suite_count() << std::endl;
    }

    void OnTestSuiteStart(const testing::TestSuite &test_suite) override
    {
        std::cout << "Starting test suite: " << test_suite.name() << std::endl;
    }

    void OnTestStart(const testing::TestInfo &test_info) override
    {
        std::cout << "Starting test: " << test_info.name() << std::endl;
    }
};

int main(int argc, char **argv)
{
    std::cout << "Initializing Google Test..." << std::endl;
    ::testing::InitGoogleTest(&argc, argv);

    std::cout << "Initializing Google Mock..." << std::endl;
    ::testing::InitGoogleMock(&argc, argv);

    ::testing::TestEventListeners &listeners = ::testing::UnitTest::GetInstance()->listeners();
    listeners.Append(new TestEventListener);

    std::cout << "Running all tests..." << std::endl;
    return RUN_ALL_TESTS();
}