#Compiler and Linker Configurations
CXX = g++
CXXFLAGS = -Wall -Wextra -std=c++17 -g -Isrc/server -Isrc/shared
LDFLAGS = -lpthread -lldap -llber

#Define the directories
SRC_DIR = src
BUILD_DIR = build
BIN_DIR = bin

#Automatically collect all.cpp files in the SRC_DIR subdirectories
SERVER_SOURCES = $(wildcard $(SRC_DIR)/server/*.cpp)
CLIENT_SOURCES = $(wildcard $(SRC_DIR)/client/*.cpp)
SHARED_SOURCES = $(wildcard $(SRC_DIR)/shared/*.cpp)

# Automatically generate .o files in BUILD_DIR for each .cpp file
SERVER_OBJECTS = $(SERVER_SOURCES:$(SRC_DIR)/%.cpp=$(BUILD_DIR)/%.o)
CLIENT_OBJECTS = $(CLIENT_SOURCES:$(SRC_DIR)/%.cpp=$(BUILD_DIR)/%.o)
SHARED_OBJECTS = $(SHARED_SOURCES:$(SRC_DIR)/%.cpp=$(BUILD_DIR)/%.o)

# Target binaries
EXEC_SERVER = $(BIN_DIR)/twmailer-server
EXEC_CLIENT = $(BIN_DIR)/twmailer-client

# Define phony targets for make commands
.PHONY: all clean directories

# Default target builds both client and server
all: directories $(EXEC_SERVER) $(EXEC_CLIENT)

# Create necessary directories
directories:
	@mkdir -p $(BUILD_DIR)/server $(BUILD_DIR)/client $(BUILD_DIR)/shared $(BIN_DIR)

# Rule for building the server binary
$(EXEC_SERVER): $(SERVER_OBJECTS) $(SHARED_OBJECTS)
	$(CXX) $^ -o $@ $(LDFLAGS)

# Rule for building the client binary
$(EXEC_CLIENT): $(CLIENT_OBJECTS) $(filter-out $(BUILD_DIR)/shared/ClientSession.o,$(SHARED_OBJECTS))
	$(CXX) $^ -o $@ $(LDFLAGS)

# General rule for converting .cpp files to .o files
$(BUILD_DIR)/%.o: $(SRC_DIR)/%.cpp
	@mkdir -p $(@D)
	$(CXX) $(CXXFLAGS) -c $< -o $@

# Clean up the build (remove object files and executables)
clean:
	@echo "Cleaning up..."
	@rm -rf $(BUILD_DIR) $(BIN_DIR)

help:
	@echo "Usage: make [target]"
	@echo "Targets:"
	@echo "  all        - Builds the client and server binaries"
	@echo "  clean      - Removes binaries and object files"
	@echo "  help       - Displays this help message"
