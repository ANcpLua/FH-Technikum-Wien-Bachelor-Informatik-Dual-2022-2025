#include <iostream>
#include <cmath>

using namespace std;

int a=0;
int V=0;
int A=0;
float d=0;
float ri=0;
float ru=0;

void wuerfel ()
{
	V=pow(a,3);
	A=6*pow(a,2);
	d=a*sqrt(3);
	ru=a/2*sqrt(3);
	ri=a/2;
}

int
main (int argc, char *argv[])
{
  a = 2;
  wuerfel ();
  cout << "Volumen:" << V << " Oberfläche:"<<A << " Diagonale:" << d << " Inkreisradius:" << ri << " Umkreisradius:" <<ru << endl;
  return 0;
}