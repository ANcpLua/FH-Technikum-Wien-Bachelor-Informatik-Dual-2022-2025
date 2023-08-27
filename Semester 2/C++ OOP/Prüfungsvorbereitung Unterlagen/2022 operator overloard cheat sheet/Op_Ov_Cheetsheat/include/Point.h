#ifndef POINT_H
#define POINT_H

#include <iostream>

class Point
{
    public:
        Point(int x, int y);
        virtual ~Point();

        // istream und ostream Operatoren müssen entweder als FRIEND oder außerhalb der Klasse definiert werden (siehe Tutorium)
        friend std::istream& operator >> (std::istream& is, Point& p);
        friend std::ostream& operator << (std::ostream& os, Point& p);

        // vergleicht die x- & y-Koordinaten zweier Punkte
        bool operator == (Point& p);

        // erhöht x- & y-Koordinaten +1 (ohne int i: Preinkrement, Standardfall)
        // ermöglicht die Schreibweise: ++p
        void operator ++ ();

        // erhöht x- & y-Koordinaten +1 (mit int i: Postinkrement)
        // ermöglicht die Schreibweise: p++
        void operator ++ (int i);

        // erstellt einen neuen Punkt mit den x- & y-Koordinaten zweier gegebener Punkte
        Point* operator + (Point& p);

    protected:
        int x;
        int y;

    private:
};

#endif // POINT_H
