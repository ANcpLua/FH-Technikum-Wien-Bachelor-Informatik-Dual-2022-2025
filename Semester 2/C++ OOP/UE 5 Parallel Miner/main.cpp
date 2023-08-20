#include <iostream>
#include <algorithm>
#include <vector>

#include <thread>
#include <mutex>
#include <chrono>

#include "World.h"
#include "roboFabrik.h"
#include "roboter.h"

using namespace std;


mutex gMutex; // Ein Mutex wird verwendet, um kritische Abschnitte im Code zu schützen.

// Die Funktion Play wird von jedem Thread ausgeführt und simuliert das Minen von Werten durch einen Roboter.
void Play(roboFabrik* robot, World* world, pair<int, double>* score, vector<roboFabrik*>& all_robots) {
    auto thread_start = chrono::high_resolution_clock::now(); // thread_start -> aktuelle Zeit. Diese Variable wird verwendet, um die Dauer jedes Threads zu messen.
    while (true) {
        try {
            gMutex.lock();      // versucht, den Mutex gMutex zu sperren. Wenn der Mutex bereits gesperrt ist, wird der aktuelle Thread blockiert, bis der Mutex vom anderen Thread freigegeben wird.
            int dx = rand() % 5;
            int dy = rand() % 5;
            int dz = rand() % 10;
            score->first += robot->mine(*world, dx, dy, dz, all_robots); // Diese Zeile ruft die mine-Methode des roboFabrik-Objekts auf, auf das durch den Parameter robot gezeigt wird.
            // Die Methode nimmt fünf Argumente entgegen: eine Referenz auf ein World-Objekt, drei Ganzzahlen für die Bewegung in x-, y- und z-Richtung und eine Referenz auf einen Vektor von roboFabrik-Zeigern.
            // Die Methode gibt einen Integer-Wert zurück, der die Menge der von dem Roboter abgebauten Ressourcen darstellt. Dieser Wert wird zum ersten Element des von dem Parameter score gezeigten Paares hinzugefügt.
            bool is_board_full = world->nothingToMine();
            // Diese Zeilen rufen die nothingToMine-Methode des durch den Parameter world gezeigten World-Objekts auf.
            // Die Methode gibt einen booleschen Wert zurück, der angibt, ob im virtuellen Raum noch Ressourcen abgebaut werden können.
            // Wenn der virtuelle Raum leer ist, wird die Schleife mit einem break-Statement verlassen. Andernfalls wird der Thread für 1 Mikrosekunde mit der sleep_for-Methode des this_thread-Namespace angehalten. Schließlich wird der Mutex gMutex entsperrt.
            this_thread::sleep_for(chrono::microseconds(1)); //
            gMutex.unlock();

            if (is_board_full) {break;} // falls true -> break

        } catch (const exception& e) {
            gMutex.unlock();
            cerr << "Ein Fehler ist beim Ausführen der Roboter-Aktionen aufgetreten: " << e.what() << endl;
        }
    }
    auto thread_end = chrono::high_resolution_clock::now(); // Diese Zeilen berechnen die Dauer des Threads und speichern sie im zweiten Element des durch den Parameter score gezeigten Paares.
    chrono::duration<double> thread_duration = thread_end - thread_start;
    score->second = thread_duration.count();
}


int main() {
    srand(time(nullptr));
    const int kNumRobots = 5;


    // Erstellt einen Vektor namens robot_scores mit kNumRobots Elementen. Jedes Element des Vektors ist ein Paar von Ganzzahlen und Doubles,
    // die die Punkte und die Zeit, die von jedem Roboter benötigt wird, darstellen. Jedes Paar wird mit den Werten 0 und 0.0 initialisiert.
    vector<pair<int, double>> robot_scores(kNumRobots, make_pair(0, 0.0));

    // Erstellt einen Vektor namens all_robots mit kNumRobots Elementen. Jedes Element des Vektors ist ein Zeiger auf ein roboFabrik-Objekt,
    vector<roboFabrik *> all_robots(kNumRobots);


    // Erstelle fünf Roboterobjekte
    all_robots[0] = new roboter(1, 2, 1);
    all_robots[1] = new roboter(3, 2, 5);
    all_robots[2] = new roboter(3, 1, 4);
    all_robots[3] = new roboter(2, 1, 6);
    all_robots[4] = new roboter(4, 2, 7);

    World world;
    int total_world_score = world.getTotalScore();
    cout << "\n" << "Gesamtpunktzahl der Welt am Anfang: " << total_world_score << endl << "\n";


    auto program_start_time = chrono::high_resolution_clock::now();
    vector<thread> threads;
    // Erstelle einen Thread für jeden Roboter
    for (int i = 0; i < kNumRobots; i++) {threads.emplace_back(Play, all_robots[i], &world, &robot_scores[i], ref(all_robots));}
    // Warte, bis alle Threads abgeschlossen sind
    for (auto &t: threads) {t.join();}
    chrono::duration<double> program_duration = chrono::high_resolution_clock::now() - program_start_time;


    // Zeige die finale Welt mit den Positionen der Roboter an
    world.PrintWorld(all_robots);

    // Zeige die finalen Punktzahlen und Zeiten der Roboter an
    for (int i = 0; i < kNumRobots; i++) {
        cout << "Roboter " << i << " Punkte: " << robot_scores[i].first << ", Zeit: " << robot_scores[i].second
             << " Sekunden" << endl << endl;}

    // Zeige, welche Roboter überlebt haben
    cout << "Lebende Roboter: ";
    for (int i = 0; i < kNumRobots; i++) {
        if (all_robots[i]->getHealth() > 0) {
            cout << "Roboter " << i << ", ";
        }
    }
    cout << endl;


    cout << "\n" << "Programmdauer: " << program_duration.count() << " Sekunden" << endl;


    // Lösche alle Roboterobjekte
    for (roboFabrik *robot: all_robots) {
        delete robot;
    }

    return 0;
}