#include <iostream>
#include <cmath>

using namespace std;

int a=0;
float V=0;
float A=0;
float ri=0;
float ru=0;

void ikosaeder ()
{
	V=5*pow(a,3)*(3+sqrt(5))/12;
	A=5*pow(a,2)*sqrt(3);
	ru=a/4.0*sqrt(2*(5+sqrt(5)));
	ri=a*sqrt(3)*(3+sqrt(5))/12;
}

int
main (int argc, char *argv[])
{
  a = 2;
  ikosaeder ();
  cout << "Volumen:" << V << " Oberfläche:"<<A << " Inkreisradius:" << ri << " Umkreisradius:" <<ru << endl;
  return 0;
}