#include <stdio.h>
#include <stdlib.h>


int encode(int h, int m)
{
    h*=100;
    return h+m;
}

int h = 0, m = 0, min=0;

int read()
{
    int encoded = 0;
    printf("hours: ");
    scanf(" %2d", &h);
    printf("minutes: ");
    scanf(" %2d", &min);

    encoded = encode(h, min);
    return encoded;
}

int hours(int t)
{
    int hours = 0;
    hours = t / 100;
    return hours;
}

int minutes(int t)
{
    int minutes = 0;
    minutes = t % 100;
    return minutes;
}

void print(int t)
{
    printf("%02d:%02d", hours(t), minutes(t));
}


int add(int t1, int t2)
{
    h = hours(t1) + hours(t2);
    min = minutes(t1) + minutes(t2);

    if (min >= 60)
    {
        min %= 60;
        h++;
    }

    if (h >= 24)
    {
        h %= 24;
    }

    return encode(h, min);
}

int total_minutes(int t)
{
    int h = 0, min = 0;
    h = hours(t);
    min = minutes(t);

    for (int i = 0; i < h; i++)
    {
        min += 60;
    }
    return min;
}


void Test_1()
{
    int h, m;
    printf("h: ");
    scanf("%d", &h);
    printf("m: ");
    scanf("%d", &m);
    printf("encoded: %04d", encode(h, m));
}

void Test_2()
{
    printf("read: %04d", read());
}

void Test_3()
{
    int t;
    printf("hours: %02d\n", hours(t = read()));
    printf("minutes: %02d\n", minutes(t));
}

void Test_4()
{
    print(read());
}

void Test_5()
{
    int t1 = read();
    int t2 = read();
    printf("add: ");
    print(t1);
    printf(" + ");
    print(t2);
    printf(" = ");
    print(add(t1, t2));
}


void Test_6()
{
    int t = read();
    printf("read: %d\n", total_minutes(t));
}



int main()
{
    /* Input */
    int num;
    /* List of all available tests */
    void (*tests[])() = { Test_1,Test_2,Test_3,Test_4,Test_5,Test_6};
    int num_of_tests = (int)(sizeof(tests) / sizeof(void (*)()));

    /* Read test number */
    printf("Test #: ");
    scanf("%d", &num);

    /* Check valid range */
    if (num >= 1 && num <= num_of_tests)
    {
        /* Call the selected test */
        printf("Executing Test_%d.\n", num);
        tests[num - 1]();
    }
    else
    {
        /* Print error */
        printf("Invalid test number!");
    }

    return 0;
}
