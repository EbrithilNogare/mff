
all: false-sharing

.PHONY: all run clean

false-sharing: false-sharing.c
	$(CC) $(CFLAGS) -O3 -Wall -pthread -o $@ $< -lpapi

run: false-sharing
	./false-sharing 0 1 2 3 4

clean:
	rm -f false-sharing

