CXX = g++
CXXFLAGS = -Wall -Wextra -std=c++17 -g

INCLUDES = -I./src/client -I./src/shared -I./src/server

LIBS = -lpthread -lldap -llber

SRC_DIR = src
OBJ_DIR = obj
BIN_DIR = bin

CLIENT_SRCS = $(SRC_DIR)/client/main_client.cpp $(SRC_DIR)/client/TwMailerClient.cpp
SERVER_SRCS = $(SRC_DIR)/server/AuthenticationService.cpp \
              $(SRC_DIR)/server/CommandHandler.cpp \
              $(SRC_DIR)/server/MailService.cpp \
              $(SRC_DIR)/server/main_server.cpp \
              $(SRC_DIR)/server/TwMailerServer.cpp \
              $(SRC_DIR)/server/SecureString.cpp
SHARED_SRCS = $(SRC_DIR)/shared/ClientSession.cpp \
              $(SRC_DIR)/shared/Configuration.cpp \
              $(SRC_DIR)/shared/Logger.cpp

CLIENT_OBJS = $(patsubst $(SRC_DIR)/%.cpp,$(OBJ_DIR)/%.o,$(CLIENT_SRCS))
SERVER_OBJS = $(patsubst $(SRC_DIR)/%.cpp,$(OBJ_DIR)/%.o,$(SERVER_SRCS))
SHARED_OBJS = $(patsubst $(SRC_DIR)/%.cpp,$(OBJ_DIR)/%.o,$(SHARED_SRCS))

TARGET_CLIENT = $(BIN_DIR)/twmailer-client
TARGET_SERVER = $(BIN_DIR)/twmailer-server

.PHONY: all clean create_dirs

all: create_dirs $(TARGET_CLIENT) $(TARGET_SERVER)

create_dirs:
	mkdir -p $(OBJ_DIR)/client $(OBJ_DIR)/server $(OBJ_DIR)/shared $(BIN_DIR)

$(TARGET_CLIENT): $(CLIENT_OBJS) $(SHARED_OBJS) $(OBJ_DIR)/server/SecureString.o $(OBJ_DIR)/server/CommandHandler.o $(OBJ_DIR)/server/MailService.o
	$(CXX) $(CXXFLAGS) -o $@ $^ $(LIBS)

$(TARGET_SERVER): $(SERVER_OBJS) $(SHARED_OBJS)
	$(CXX) $(CXXFLAGS) -o $@ $^ $(LIBS)

$(OBJ_DIR)/%.o: $(SRC_DIR)/%.cpp
	@mkdir -p $(@D)
	$(CXX) $(CXXFLAGS) $(INCLUDES) -c $< -o $@

clean:
	rm -rf $(OBJ_DIR) $(BIN_DIR)
