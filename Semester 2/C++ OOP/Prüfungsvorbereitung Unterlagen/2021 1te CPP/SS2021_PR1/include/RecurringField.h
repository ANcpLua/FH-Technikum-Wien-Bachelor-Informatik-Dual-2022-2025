#ifndef RECURRINGFIELD_H
#define RECURRINGFIELD_H

#include "Field.h"

class RecurringField : public Field
{
    public:
        RecurringField();
        virtual ~RecurringField();
        bool execute();

    protected:

    private:
};

#endif // RECURRINGFIELD_H
