#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// define a struct called array that has two int pointers: ID and arr
typedef struct array 
{
int* ID;
int* arr;
}array;

// define a struct called vector3 that has three ints: c0, c1, and dif
typedef struct vector3 
{
int c0;
int c1;
int dif;
}vector3;

// allocate memory for the arrays in the struct and populate them with values
array setupArray(int _length0, int _length1) 
{
array newArray;
newArray.ID = (int*)malloc((_length0+_length1)*sizeof(int));
newArray.arr = (int*)malloc((_length0+_length1)*sizeof(int));
int counter = 0;
for (int i = 0; i < _length0; i++) 
{
scanf(" %d", &newArray.arr[counter]);
newArray.ID[counter] = 0;
counter++;
}
for (counter = counter; counter < _length0+_length1; counter++) 
{
scanf(" %d", &newArray.arr[counter]);
newArray.ID[counter] = 1;
}
return newArray;
}

// get the length of one of the arrays
int setupLength() 
{
int i;
scanf(" %d", &i);
return i;
}

// free the memory of the arrays
void freeArray(array* _arr) 
{
free(_arr->ID);
free(_arr->arr);
}

// swap two values
void swap(int* _a, int* _b) 
{
*_a = *_a + *_b;
*_b = *_a - *_b;
*_a = *_a - *_b;
}

// sort the subarray
void sortSub(array* _arr, int _length) 
{
for (int i = 0; i < _length-1; i++) 
{
for (int j = 0; j < _length-i-1; j++) 
{
if (_arr->arr[j] > _arr->arr[j+1]) 
{
swap(&_arr->arr[j], &_arr->arr[j+1]);
swap(&_arr->ID[j], &_arr->ID[j+1]);
}
}
}
}

// find the smallest difference between elements with different IDs
void calculateSmallestDiff(array* _arr, int _length) 
{
sortSub(_arr, _length);
// initialize smallest to 0, 999999, 999999
vector3 smallest = {0, 999999, 999999};
for (int i = 0; i < _length-1; i++) 
{
if (_arr->ID[i] != _arr->ID[i+1]) 
{
// set new candidate. cleaner version below
vector3 candidate = {0, 0, 0};
if (_arr->ID[i] == 1) 
{
                candidate.c0 = _arr->arr[i + 1];
                candidate.c1 = _arr->arr[i];
                candidate.dif = abs(_arr->arr[i] - _arr->arr[i + 1]);

            } else 
			{
                candidate.c0 = _arr->arr[i];
                candidate.c1 = _arr->arr[i + 1];
                candidate.dif = abs(_arr->arr[i] - _arr->arr[i + 1]);
            }

            if (candidate.dif < smallest.dif)
            {
                smallest = candidate;
            }
        }
    }
    printf(": %d (%d, %d)", smallest.dif, smallest.c0, smallest.c1);
}





// declare main function
int main()
{

    // use `setupLength` function to get length of first array
    int length0 = setupLength();

    // use `setupLength` function to get length of second array
    int length1 = setupLength();

    // create a new array and get a pointer to it
    array newArray = setupArray(length0, length1), *nAPtr = &newArray;

    // find the smallest difference between elements in the two arrays
    calculateSmallestDiff(nAPtr, length0+length1);

    // free memory that was allocated for the array
    freeArray(nAPtr);

    // return 0 to indicate successful execution of the program
    return 0;
}



