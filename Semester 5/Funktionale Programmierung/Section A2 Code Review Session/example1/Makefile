PROGRAM   = example
CXX       = clang++
CXXFLAGS  = -g -std=c++17 -Wall -lm

$(PROGRAM): example.cpp
	$(CXX) example.cpp -o $(PROGRAM)

.PHONY: clean 

clean:
	-rm -f *.o $(PROGRAM) 
