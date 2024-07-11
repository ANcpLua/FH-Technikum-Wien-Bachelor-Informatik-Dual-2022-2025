CXX = g++
CXXFLAGS = -Wall -Wextra -std=c++17 -g
INCLUDES = -I/usr/local/include -I../src/client -I../src/shared -I../src/server -I.
LIBS = -L/usr/local/lib -lgtest -lgmock -lpthread -lldap -llber

SRC_DIR = ../src
OBJ_DIR = obj
TEST_DIR = .

COMMON_SRC = $(filter-out $(SRC_DIR)/client/main_client.cpp $(SRC_DIR)/server/main_server.cpp, $(wildcard $(SRC_DIR)/client/*.cpp $(SRC_DIR)/shared/*.cpp $(SRC_DIR)/server/*.cpp))
TEST_SRC = $(wildcard $(TEST_DIR)/*.cpp)
MOCK_HEADERS = $(wildcard $(TEST_DIR)/*Mock*.h)

COMMON_OBJS = $(patsubst $(SRC_DIR)/%.cpp,$(OBJ_DIR)/%.o,$(COMMON_SRC))
TEST_OBJS = $(patsubst $(TEST_DIR)/%.cpp,$(OBJ_DIR)/%.o,$(TEST_SRC))

.PHONY: all clean test create_dirs

all: create_dirs test_runner

create_dirs:
	mkdir -p $(OBJ_DIR)/client $(OBJ_DIR)/shared $(OBJ_DIR)/server

test_runner: $(COMMON_OBJS) $(TEST_OBJS)
	$(CXX) $(CXXFLAGS) $(INCLUDES) -o $@ $^ $(LIBS)

$(OBJ_DIR)/%.o: $(SRC_DIR)/%.cpp
	@mkdir -p $(@D)
	$(CXX) $(CXXFLAGS) $(INCLUDES) -c $< -o $@

$(OBJ_DIR)/%.o: $(TEST_DIR)/%.cpp $(MOCK_HEADERS)
	@mkdir -p $(@D)
	$(CXX) $(CXXFLAGS) $(INCLUDES) -c $< -o $@

clean:
	rm -rf $(OBJ_DIR) test_runner

test: test_runner
	./test_runner

print-%:
	@echo '$*=$($*)'

DEPFILES := $(COMMON_OBJS:.o=.d) $(TEST_OBJS:.o=.d)
$(DEPFILES):

include $(wildcard $(DEPFILES))
