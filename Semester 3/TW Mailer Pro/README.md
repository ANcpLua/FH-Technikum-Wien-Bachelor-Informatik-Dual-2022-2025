#!/bin/bash

# Define the base directory (current directory)
BASE_DIR=$(pwd)

mkdir -p "$BASE_DIR/src/server"
mkdir -p "$BASE_DIR/src/client"
mkdir -p "$BASE_DIR/src/shared"
mkdir -p "$BASE_DIR/include"
mkdir -p "$BASE_DIR/build/server"
mkdir -p "$BASE_DIR/build/client"
mkdir -p "$BASE_DIR/build/shared"
mkdir -p "$BASE_DIR/bin"
mkdir -p "$BASE_DIR/conf"
mkdir -p "$BASE_DIR/logs"
mkdir -p "$BASE_DIR/mail_spool"

echo "TwMailer directory structure has been created successfully."
echo "Project root: $BASE_DIR"
echo ""
echo "Directory structure:"
echo "├── src"
echo "│   ├── server"
echo "│   ├── client"
echo "│   └── shared"
echo "├── include"
echo "├── build"
echo "│   ├── server"
echo "│   ├── client"
echo "│   └── shared"
echo "├── bin"
echo "├── conf"
echo "├── logs"
echo "└── mail_spool"
