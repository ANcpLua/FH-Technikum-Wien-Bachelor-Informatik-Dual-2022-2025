#include <stdio.h>
#include <string.h>
#define anz 10

// calculates and adds bonus for each value in the input array that is above the given threshold
double praemien(double betraege[], int anzahl, double grenze, double praemie) {
double uberprufen=0; // variable to hold the value being checked
double praemieGesamt=0; // variable to accumulate the total bonus
for(int i = 0; i < anzahl; i++) { // loop through each element in the array
    uberprufen = betraege[i];

    if (uberprufen >= grenze) { // check if the value is above the threshold
        praemieGesamt+=praemie; // add the bonus to the total
        betraege[i] += praemie; // add the bonus to the value
    }

}
return praemieGesamt; // return the total bonus
}

// input function to get values into the array
int eingabe(double betraege[], int max) {
int i=0;
for(; i < max; i++) {
printf("Zahl %d: ", i+1);
scanf("%lf", &betraege[i]);
if(betraege[i] < 0) // break loop if input value is negative
break;
}

return i; // return the number of elements in the array
}

// function to calculate the sum of all values in the array
double sum(double betraege[], int anzahl) {
double retsum = 0;
for(int i = 0; i < anzahl; i++) {
    retsum += betraege[i];
}

return retsum;
}

int main() {
double arr[anz] = {0}; // array to hold input values
double grenze, praemie, gesamt; // variables for threshold, bonus, and total bonus
int n;
n = eingabe(arr, anz); // get input values into the array
printf("Untergrenze: ");
scanf("%lf", &grenze); // get threshold value
printf("Praemie: ");
scanf("%lf", &praemie); // get bonus value

printf("Summe vor Praemien: %.2f\n", sum(arr, n)); // print sum of values before bonus
gesamt = praemien(arr, n, grenze, praemie); // calculate and add bonus
printf("Praemien gesamt: %.2f\n", gesamt); // print total bonus
printf("Summe nach Praemien: %.2f\n", sum(arr, n)); // print sum of values after bonus

return 0;
}