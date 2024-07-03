CC=gcc
CFLAGS=-Wall -Wextra -std=c11 -pedantic -g -D_GNU_SOURCE -Wno-format-truncation
LDFLAGS=
SOURCES=myfind.c
OBJECTS=$(SOURCES:.c=.o)
EXECUTABLE=myfind

all: $(SOURCES) $(EXECUTABLE)

$(EXECUTABLE): $(OBJECTS)
	$(CC) $(LDFLAGS) $(OBJECTS) -o $@

.c.o:
	$(CC) $(CFLAGS) -c $< -o $@

clean:
	rm -f $(OBJECTS) $(EXECUTABLE)

