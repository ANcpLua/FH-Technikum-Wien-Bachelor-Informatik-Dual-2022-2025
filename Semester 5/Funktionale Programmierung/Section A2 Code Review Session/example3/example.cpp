#include <iostream>
#include <cmath>

using namespace std;

int a=0;
float V=0;
float A=0;
float ri=0;
float ru=0;

void tetraeder ()
{
	V=pow(a,3)*sqrt(2)/12;
	A=pow(a,2)*sqrt(3);
	ru=a*sqrt(6)/4;
	ri=a*sqrt(6)/12;
}

int
main (int argc, char *argv[])
{
  a = 2;
  tetraeder ();
  cout << "Volumen:" << V << " Oberfläche:"<<A << " Inkreisradius:" << ri << " Umkreisradius:" <<ru << endl;
  return 0;
}