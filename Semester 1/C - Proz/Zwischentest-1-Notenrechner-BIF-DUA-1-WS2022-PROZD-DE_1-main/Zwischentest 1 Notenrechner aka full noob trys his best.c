#include <stdio.h>
#include <stdlib.h>

double zt = 0;               //zwischentest
double at = 0;              //abschlusstest
double ub = 0;              //übungsbsp
double total = 0;           // gesamtPunkte auf englisch
double summeTotal=0;             // summe aus zt+at+übungen
double percent=0;                     // percent wenn positiv



double punkteLesen()            //return value funktion für punkte
{
    double ret=0;
    printf("\nPunkte: ");
    scanf("%lf", &ret);
    return ret;
}


void results() // funktion für ergebnis nicht negativ 
{
    printf("\nGesamtpunkte absolut: %.2f", summeTotal);
    printf("\nGesamtpunkte %%: %.2f %%", percent);
}




int main()
{
    int loop=1;      // loop
    int abschlusstestEingetragen=0;                 // checkt ob eh nur 1 abschlusstest indem er true false macht
    char c=' ';              // c ist wurscht wie es heißt definiert mit ' ' weil buchstabe/character




    while (loop)        //kommt in schleife richtig=1 führt aus, scannt zeichen ein, speichert in c ein.
    {

        printf("\nZT Punkte: %.2lf", zt);
        printf("\nAT Punkte: %.2lf", at);
        printf("\nUE Punkte: %.2lf", ub);
        printf("\nErreichbare Gesamtpunkte: %.2lf", total);
        printf("\nTyp: ");
        scanf(" %c", &c);

        double input=0; // punkte eingabe funktion

        if (c=='z')
        {
            input=punkteLesen();    // funktion gleichsetzen weniger fehlerlastig
            if (input >=0 && input <= 10)
            {
                zt+=input;
                total+=10;
            }
            else
                printf("\nungueltiger Bereich: min 0.00, max 10.00");

        }
        else if(c=='a')
        {
            if(abschlusstestEingetragen==0)   // 0 steht für nicht eingetragen, da noch nicht eingtragen gehts weiter
            {
                input=punkteLesen();
                if (input >= 0 && input <= 70)
                {
                    at+=input;
                    abschlusstestEingetragen=1;   // variable jetzt nicht mehr 0 deshalb wird in der nächsten schleife oben auf else print!
                    total+=70;
                }
                else
                    printf("\nungueltiger Bereich: min 0.00, max 70.00");


            }
            else if (abschlusstestEingetragen!=0)
            {
                printf("\nnur ein Abschlusstest erlaubt");
            }
        }
        else if(c=='u')
        {
            input=punkteLesen();
            if (input >= 0 && input <= 0.50)
            {
                ub+=input;
                if (ub>10.00)

                    ub=10.00;
            }
            else
                printf("\nungueltiger Bereich: min 0.00, max 0.50");
        }
        else if(c=='=')
        {

            double negativCheck=0;        // summe aus zt und at - wenn das % negativ ist leider die ubungspunkte umsonst
            double percentnegativ=0;          // negative % ausgabe

            negativCheck=zt+at;
            percentnegativ=negativCheck/total*100;

            summeTotal=zt+at+ub;
            percent=summeTotal/total*100;


            if(total==0)
            {
                printf("\nGesamtpunkte absolut: 0.00");
                printf("\nGesamtpunkte %%: 0.00 %%");
                printf("\nNote: 0");
                break;
            }


            if (100*negativCheck < 55*total)
            {

                printf("\nGesamtpunkte absolut: %.2f", negativCheck);
                printf("\nGesamtpunkte %%: %.2f %%", percentnegativ);
                printf("\nNote: 5");
                break;
            }
            else if  (100*summeTotal >= 55*total && 100*summeTotal < 67*total)
            {
                results();   // gesampt+%
                printf("\nNote: 4");
                break;
            }
            else if  (100*summeTotal >= 67*total && 100*summeTotal < 78*total)
            {
                results();
                printf("\nNote: 3");
                break;

            }
            else if (100*summeTotal >= 78*total && 100*summeTotal < 89*total)
            {
                results();
                printf("\nNote: 2");
                break;

            }
            else if (100*summeTotal/89 >=total)
            {
                results();
            printf("\nNote: 1");
            break;
            }
        }
    }

    return 0;
}
