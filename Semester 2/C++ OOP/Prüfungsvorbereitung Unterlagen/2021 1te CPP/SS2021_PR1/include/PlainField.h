#ifndef PLAINFIELD_H
#define PLAINFIELD_H

#include "Field.h"

class PlainField : public Field
{
    public:
        PlainField();
        virtual ~PlainField();
        bool execute();

    protected:

    private:
};

#endif // PLAINFIELD_H
