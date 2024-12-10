#include<stdio.h>
#define MULT(a, b) (a * b)

int main() {
	int r = MULT(-16 + 3, 3 + 2);
	int r1 = MULT(2 + 4, 3 + 1);
	printf("%d %d", r, r1);
 		
	return 0;
}
