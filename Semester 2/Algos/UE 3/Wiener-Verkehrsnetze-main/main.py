import heapq
import re
import time
from collections import defaultdict



def read_graph(filename):
    graph = defaultdict(list)
    with open(filename, 'r') as file:
        for line in file:
            parts = line.strip().split(':')
            line_name = parts[0]
            stations_and_costs = re.findall(r'"(.*?)" (\d+)', parts[1])
            for i in range(len(stations_and_costs) - 1):
                station_a, cost_a = stations_and_costs[i]
                station_b, cost_b = stations_and_costs[i + 1]
                cost = int(cost_a)
                graph[station_a].append((station_b, cost, line_name))
                graph[station_b].append((station_a, cost, line_name))
    return graph


def shortest_path(graph, start, goal):
    line_switch_penalty = 5  # extra kosten bei wechsel weil sonst 30ger und 31ger buggy ist, zahl kann man adjusten nach wie viel will ich zahlen für umsteigen :) ganz cool
    queue = [(0, start, [], None)]
    seen = set()
    while queue:
        (cost, current, path, previous_line) = heapq.heappop(queue)
        if current not in seen:
            seen.add(current)
            path = path + [(current, cost, previous_line)]
            if current == goal:
                return path
            for next_station, next_cost, line_name in graph[current]:
                switch_cost = line_switch_penalty if previous_line and line_name != previous_line else 0
                heapq.heappush(queue, (cost + next_cost + switch_cost, next_station, path, line_name))


def find_path(filename_graph, start, goal):
    graph = read_graph(filename_graph)
    path = shortest_path(graph, start, goal)
    if path:
        if start == goal:
            print("Start und Ziel sind dieselbe Station.")
            cost = 0
        else:
            previous_line = None
            for i, (station, cost, line_name) in enumerate(path[1:]):
                if line_name != previous_line:
                    if i != 0:  # Add this condition to skip printing the first line change
                        print(f"Umstieg auf Linie {line_name} an Station {station}")
                print(f"Fahre von {path[i][0]} nach {station} mit Linie {line_name} (Kosten: {cost})")
                previous_line = line_name
            print(f"Ende: {goal}")
    else:
        print("Kein Pfad gefunden")

    print(f"Gesamtkosten: {cost}")


def get_user_input():
    start_station = input("Bitte geben Sie die Startstation ein: ")
    end_station = input("Bitte geben Sie die Zielstation ein: ")
    return start_station, end_station


if __name__ == "__main__":
    while True:
        start, end = get_user_input()
        print(f"Start: {start}")

        start_time = time.time()  # Erfasse die Startzeit
        find_path("filename_graph start ziel.txt", start, end)
        end_time = time.time()  # Erfasse die Endzeit

        elapsed_time = end_time - start_time
        print(f"Laufzeit: {elapsed_time:.5f} Sekunden")

        user_choice = input("Möchten Sie weiterfahren (w), neu anfangen (n) oder beenden (x)? ").lower()

        if user_choice == "w":
            start = end  # Set the previous end station as the new start station
        elif user_choice == "n":
            continue  # Start over with new start and end stations
        elif user_choice == "x":
            break  # Exit the loop and end the program
        else:
            print("Ungültige Eingabe. Das Programm wird beendet.")
            break




