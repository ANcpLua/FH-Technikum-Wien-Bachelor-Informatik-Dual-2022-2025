#include <stdio.h>
#include <stdlib.h>

void invalidInput()
{
    printf("\nInput invalid! Try again: ");   // (-_-) zzz
}

enum waggonTypen // zug typen Passenger, Sleeper, Diner
{
    passenger='P', sleeper='S', diner='D'
};

struct carriage         // datentyp       waggontypen+ 20-130 plätze
{
    enum waggonTypen type;
    int capacity;
};

struct train                // datentyp vom datentypen              10 züge(typ+plätze) + länge
{
    struct carriage carriages[10];
    int length;
};

void printCarriage(struct carriage waggonType)         // auswahl waggontypen+plätze
{
    char typ;    // ermöglicht buchstabenauswahl fürs switchcase
    switch(waggonType.type)  // zugriff auf den waggontypen im struct mit .          JA ICH WEIß DIE NAMEN SIND NICHT SO GUT GEWÄHLT (° ͜ʖ °)╭∩╮
    {
        case passenger: typ='P';
        break;

        case sleeper: typ='S';
        break;

        case diner: typ='D';
        break;
    }
    printf("[%c:%03d]", typ, waggonType.capacity);     // print waggontypen und inhalt durch . aufs struct     ezzzzzz   \(ᵔᵕᵔ)/

}


void printTrain(struct train *trainInhalt) // POINTERRRRRR:  pointer auf unser train struct mit zugriff auf die länge, BIF-dual, Praesenz 12 Strings, Pdf.Folie.19 Structs verstecken, dass sie komplexe und viele Daten enthalten
                                            // Lösung: Pointer auf structs verwenden  ,  trainInhalt->lenght ist eine Verkürzung von (*trainInhalt).length
{
    if(trainInhalt->length == 0)     //  kein waggon = 0 , dann error
    {
        printf("\nTrain: No Carriages!");
    }
        else                              // wenns eine länge gibt dann zwischenspeichern und mit for abmessen dann printen
        {
            printf("\nTrain: ");

            int trainLength = trainInhalt->length;

            for(int i=0; i<trainLength; i++)
            {
                printf("%d:", i);
                printCarriage(trainInhalt->carriages[i]);  // print waggontypen und inhalt
                    if(i<trainLength-1)     // einfach ein - bindestrich (╯°□°）╯
                    {
                        printf("-");
                    }
            }
            printf(" Length: %d", trainLength); // waggonlänge pointer auf länge im struct train
        }
}

char getMenu()      //   乁(◔ ౪◔)⊃━☆ﾟ.*･｡ﾟ             wenn vertippt, schleife bis user mental rdy ist, bei richtig eingabe returnen !!!!!!!!magic!!!!!
{
    char eingabe;

    printf("\nChoose Action: print train (p), new carriage (n), print stats (s) or exit (x): ");
    scanf(" %c", &eingabe);

    while(eingabe!= 'p' && eingabe != 'n' && eingabe != 's' && eingabe != 'x')
    {
        invalidInput();
        scanf(" %c", &eingabe);  // zwischentest1 (　-_･) ︻デ═一 ╭∩╮
    }
        return eingabe;
}

struct carriage getCarriage()       // Bob der Baumeister    waggontypen + kapazität 20-130
{
    char input;
    int capacity;
    enum waggonTypen type;

    printf("\nChoose type of carriage: passenger (p), sleeper (s) or diner (d): ");            // waggontypen abfragen durch einlesen(scanf) des inputs(char,%c)
    scanf(" %c", &input);  //  ╭∩╮

    while(input != 'p' && input != 's' && input !='d')                         // wenn vertippt, schleife bis user mental rdy ist
    {
        invalidInput();
        scanf(" %c", &input);   //  ╭∩╮
    }
    if(input == 'p'|| input == 's'|| input == 'd') // wenn user dann rdy ist, wird eingeladen 20-130 & auf capacity, wenn user größe nicht kennt reapeat mit schleif und osmanischer schelle -- zeile 103
    {
        printf("\nChoose capacity (20 - 130): ");
        scanf("%d", &capacity);

            while(20 > capacity || 130 < capacity)
            {
                invalidInput();
                scanf("%d", &capacity);
            }
            if(input == 'p')
            {
                type=passenger;             // zwischenspeichern von input abhänging von eingabe, damit wir später returnen können ┬─┬⃰͡ (ᵔᵕᵔ͜ )
            }
                else if(input == 's')
                {
                    type=sleeper;
                }
                    else if(input == 'd')
                    {
                        type=diner;
                    }
    }
    struct carriage inhalt = {type, capacity};    // kurz Miraculix roleplay: datentyp mit gespeicherten werten erstellen
    return inhalt;   // in asterix mund schnell - bauchi macht fauchi, return des spezifischen inhalt
}

int getPosition(int length)  // position richtig zuordnen 0 1 2    0-10
{
    int position;

    printf("\nChoose position for the new carriage (0-%d): ", length);  // erste ist immer 0
    scanf("%d", &position);

    while(position < 0 || position > length)    // wenn falsch repeat bis richtig;    wenn bei C erste richtig reicht das, daher wenn kleiner als 0 invalidinput oder es gibt nur 3 gültige positionen
    {
        invalidInput();
        scanf("%d", &position);
    }
    return position;   // returnen die richtige position des zuges 0 1 2
}


int insertCarriage(struct train *richtigerWaggon, int position, struct carriage neuerWaggon)
{
    if(richtigerWaggon->length >= 10)  // waggonlänge muss 0-9 sein
    {
        return -1;
    }
    if(position > richtigerWaggon->length)  // gibt nur 0 1 2 daher wenn position größer als wagon mit pointer auf länge gibts error deshalb auch position < 0 davor checken , hätte auch && und < 0 machen können wenn in position nicht < 0 || wäre
    {
        return -2;
    }
    if(neuerWaggon.type == sleeper&&(richtigerWaggon->carriages[position].type!= passenger || richtigerWaggon->carriages[position+1].type!=passenger))  // fangt passengers ab
    {
        return -3;
    }
        for(int i = richtigerWaggon->length; i > position; i--) //iterierst rückwärts durchs array und schiebst des vorherige element auf die aktuelle position bis zur gewünschten position
        {
            richtigerWaggon->carriages[i] = richtigerWaggon->carriages[i-1];   
        }
            richtigerWaggon->carriages[position] = neuerWaggon;
            richtigerWaggon->length = richtigerWaggon->length+1;

            return 0;          // ( ◕◡◕)っ ♡,
}


int sumCapacity(struct train *cap, enum waggonTypen type)
{
    int summe = 0;
    for(int i = 0; i < cap->length; i++)
    {
        if(cap->carriages[i].type == type)
        {
            summe += cap->carriages[i].capacity;   //      durchgezählterwaggon pointer auf capacity
        }
    }
    return summe;
}

void printTrainStats(struct train *tStats)     // funktion printtrain + typen mit inhalt
{
    printTrain(tStats);
    printf("\nCapacities:");
    printf("\n  Passenger: %d", sumCapacity(tStats, 'P'));      // funktionsaufruf
    
    if(sumCapacity(tStats, 'P') < 65)
    {
        printf(" - invalid");
    }
    
    printf("\n  Sleeper: %d", sumCapacity(tStats, 'S'));
    printf("\n  Diner: %d", sumCapacity(tStats, 'D'));

    
}

int main()                      //       (づ｡◕‿‿◕｡)づ (づ｡◕‿‿◕｡)づ (づ｡◕‿‿◕｡)づ (づ｡◕‿‿◕｡)づ (づ｡◕‿‿◕｡)づ (づ｡◕‿‿◕｡)づ           
{
    struct train trainInhalt = {0};
    struct carriage waggonType = {0};
    char menue = 0;

    while(menue != 'x')                // alles außer x sonst ⌐╦╦═─(◡︵◡)
    {
        menue = getMenu();     // jetzt getmenu damit x abgefangen wird

        switch(menue)
        {
            case'p': printTrain(&trainInhalt);     // Zug infos
            break;

            case'n': waggonType = getCarriage(); //  p s oder d capacity und position

            int error = insertCarriage(&trainInhalt, getPosition(trainInhalt.length), waggonType);      //insertcarriage funktion Zeile 143

            switch(error)
            {
                case -1: printf("\nError: Train too long!");
                break;

                case -2: printf("\nError: Position not possible!");
                break;

                case -3: printf("\nError: Sleeper only possible directly before two passenger carriages!");
                break;
            }
            break;
            
            case 's': printTrainStats(&trainInhalt);     // PRINT ITTTTTTTTTTTTT   Zeile 45
            break;
        }
    }
    
    return 0;
    
}


// ( ◡‿◡✿)( ◡‿◡✿ ) ( ◡‿◡✿ )( ◡‿◡✿ )( ◡‿◡✿ )( ◡‿◡✿ )( ◡‿◡✿ )







