#include "Point.h"

Point::Point(int x, int y)
{
    // std::cout << "[*] Point Constructor" << std::endl;

    this->x = x;
    this->y = y;
}

Point::~Point()
{
    // std::cout << "[*] Point Destructor" << std::endl;
}

std::istream& operator >> (std::istream& is, Point& p)
{
    std::cout << "x: ";
    std::cin >> p.x;
    std::cout << "y: ";
    std::cin >> p.y;

    return is;
}

std::ostream& operator << (std::ostream& os, Point& p)
{
    std::cout << p.x << "/" << p.y;

    return os;
}

bool Point::operator == (Point& p)
{
    return this->x == p.x && this->y == p.y;
}

void Point::operator ++ ()
{
    this->x += 1;
    this->y += 1;
}

void Point::operator ++ (int i)
{
    this->x += 1;
    this->y += 1;
}

Point* Point::operator + (Point& p)
{
    return new Point(this->x + p.x, this->y + p.y);
}
