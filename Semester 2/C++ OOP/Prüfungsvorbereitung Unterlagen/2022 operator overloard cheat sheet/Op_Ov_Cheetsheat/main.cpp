#include <iostream>

#include "Point.h"

int main()
{
    Point* p1 = new Point(7, 3);
    Point* p2 = new Point(3, 7);

    Point* p3 = *p1 + *p2;

    std::cout << "p1: " << *p1 << std::endl;
    std::cout << "p2: " << *p2 << std::endl;
    std::cout << "p3: " << *p3 << std::endl;

    std::cin >> *p3;

    std::cout << "p3: " << *p3 << std::endl;

    (*p3)++;

    std::cout << "p3: " << *p3 << std::endl;

    ++(*p3);

    std::cout << "p3: " << *p3 << std::endl;

    Point* p4 = new Point(7, 3);

    std::cout << "p1 == p2: " << ((*p1 == *p2) ? "TRUE" : "FALSE") << std::endl;
    std::cout << "p1 == p4: " << ((*p1 == *p4) ? "TRUE" : "FALSE") << std::endl;

    delete p1;
    delete p2;
    delete p3;
    delete p4;

    return 0;
}
