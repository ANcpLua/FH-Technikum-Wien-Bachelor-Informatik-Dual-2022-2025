#include <iostream>
#include <cmath>

using namespace std;

int a=0;
float V=0;
float A=0;
float ri=0;
float ru=0;

void dodekaeder ()
{
	V=pow(a,3)*(15+7*sqrt(5))/4.0;
	A=3*pow(a,2)*sqrt(5*(5+2*sqrt(5)));
	ru=a*sqrt(3)*(1+sqrt(5))/4.0;
	ri=a*sqrt(10*(25+11*sqrt(5)))/20.0;
}

int
main (int argc, char *argv[])
{
  a = 2;
  dodekaeder ();
  cout << "Volumen:" << V << " Oberfläche:"<<A << " Inkreisradius:" << ri << " Umkreisradius:" <<ru << endl;
  return 0;
}