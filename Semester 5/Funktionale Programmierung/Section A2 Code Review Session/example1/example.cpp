#include <iostream>
#include <cmath>

using namespace std;

int a=0;
int b=0;
int c=0;
int V=0;
int A=0;
float d=0;

void quader ()
{
	V=a*b*c;
	A=2*(a*b+a*c+b*c);
	d=sqrt(pow(a,2)+pow(b,2)+pow(c,2));
}

int
main (int argc, char *argv[])
{
  a = 2;
  b = 3;
  c = 4;
  quader ();
  cout << "Volumen:" << V << " Oberfläche:"<<A << " Diagonale:" << d << endl;
  return 0;
}