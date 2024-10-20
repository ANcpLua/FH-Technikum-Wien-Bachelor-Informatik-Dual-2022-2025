#include <iostream>
#include <cmath>

using namespace std;

int a=0;
float V=0;
float A=0;
float ri=0;
float ru=0;

void oktaeder ()
{
	V=pow(a,3)*sqrt(2)/3;
	A=2*pow(a,2)*sqrt(3);
	ru=a*sqrt(2)/2;
	ri=a*sqrt(6)/6;
}

int
main (int argc, char *argv[])
{
  a = 2;
  oktaeder ();
  cout << "Volumen:" << V << " Oberfläche:"<<A << " Inkreisradius:" << ri << " Umkreisradius:" <<ru << endl;
  return 0;
}