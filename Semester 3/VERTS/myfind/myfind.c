#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/wait.h>
#include <dirent.h>
#include <sys/stat.h>
#include <limits.h>
#include <stdbool.h>

#define PATH_MAX_LENGTH 256
#define PIPE_BUFFER_SIZE 4096

void searchDirectory(const char *searchPath, const char *fileName, bool caseSensitive, bool recursive, int pipeFileDescriptor) {
    // Open the directory
    DIR *directoryHandle = opendir(searchPath);

    if (!directoryHandle) {
        // Handle error opening directory
        char errorMessage[PIPE_BUFFER_SIZE];
        snprintf(errorMessage, sizeof(errorMessage), "Invalid directory: %s\n", searchPath);
        write(pipeFileDescriptor, errorMessage, strlen(errorMessage));
        return;
    }

    // Define the function to compare file names
    int (*compareFunction)(const char *, const char *) = (caseSensitive) ? strcmp : strcasecmp;

    // Read each entry in the directory
    struct dirent *directoryEntry;
    while ((directoryEntry = readdir(directoryHandle)) != NULL) {
        // Ignore '.' and '..' directories
        if (!strcmp(directoryEntry->d_name, ".") || !strcmp(directoryEntry->d_name, "..")) {
            continue;
        }

        // Check if the entry is a directory and recurse if requested
        if (directoryEntry->d_type == DT_DIR && recursive) {
            char newPath[PATH_MAX_LENGTH];
            snprintf(newPath, sizeof(newPath), "%s/%s", searchPath, directoryEntry->d_name);
            searchDirectory(newPath, fileName, caseSensitive, recursive, pipeFileDescriptor);
            continue;
        }

        // Check if the entry matches the filename
        if (compareFunction(fileName, directoryEntry->d_name) == 0) {
            char resolvedPath[PATH_MAX_LENGTH];
            snprintf(resolvedPath, sizeof(resolvedPath), "%s/%s", searchPath, directoryEntry->d_name);
            char outputMessage[PIPE_BUFFER_SIZE];
            snprintf(outputMessage, sizeof(outputMessage), "%d: %s: %s\n", getpid(), directoryEntry->d_name, resolvedPath);
            write(pipeFileDescriptor, outputMessage, strlen(outputMessage));
        }
    }

    // Close the directory
    closedir(directoryHandle);
}

int main(int argc, char *argv[]) {
    // Initialize variables
    bool caseSensitive = true;
    bool recursive = false;
    char searchPath[PATH_MAX_LENGTH];
    int option;

    // Parse command-line arguments
    while ((option = getopt(argc, argv, "iR")) != -1) {
        switch (option) {
            case 'i':
                caseSensitive = false;
                break;
            case 'R':
                recursive = true;
                break;
            case '?':
            default:
                fprintf(stderr, "Error: Invalid option\n");
                return EXIT_FAILURE;
        }
    }

    // Get the search path
    if (optind < argc) {
        strncpy(searchPath, argv[optind++], PATH_MAX_LENGTH);
    } else {
        fprintf(stderr, "Error: Search path not specified\n");
        return EXIT_FAILURE;
    }

    // Create pipes and fork child processes
    int pipeFileDescriptors[2]; // Only one pipe needed at a time

    for (int i = optind; i < argc; i++) {
        // Create a pipe for communication
        if (pipe(pipeFileDescriptors) == -1) {
            perror("pipe");
            return EXIT_FAILURE;
        }

        // Fork a child process
        pid_t childProcessId = fork();
        if (childProcessId == -1) {
            perror("fork");
            return EXIT_FAILURE;
        }

        if (childProcessId == 0) { // Child process
            // Close the read end of the pipe (child only needs to write)
            close(pipeFileDescriptors[0]);

            // Call the searchDirectory function to search for the specified file
            searchDirectory(searchPath, argv[i], caseSensitive, recursive, pipeFileDescriptors[1]);

            // Close the write end of the pipe (child is done writing)
            close(pipeFileDescriptors[1]);

            // Exit the child process
            exit(0);
        } else { // Parent process
            // Close the write end of the pipe (parent only needs to read)
            close(pipeFileDescriptors[1]);

            // Read from the pipe
            char buffer[PIPE_BUFFER_SIZE];
            ssize_t bytesRead;
            while ((bytesRead = read(pipeFileDescriptors[0], buffer, PIPE_BUFFER_SIZE - 1)) > 0) {
                buffer[bytesRead] = '\0'; // Null-terminate the string
                printf("%s", buffer);
            }

             // Close the read end of the pipe
            close(pipeFileDescriptors[0]);

            // Wait for the child process to finish
            waitpid(childProcessId, NULL, 0);
        }
    }

    return EXIT_SUCCESS;
}
