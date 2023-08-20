import csv   #https://docs.python.org/3/library/csv.html
import pickle  #https://docs.python.org/3/library/pickle.html
import sys          #https://docs.python.org/3/library/sys.html
import tkinter as tk        #https://docs.python.org/3/library/tkinter.html
from tkinter import filedialog
from colorama import init, Fore, Style          #https://docs.python.org/3/library/tkinter.html


# Gebe es keine Angabe mit Bewertungsmatrix :( hätte man so viel besser machen können.
#from datetime import datetime      import matplotlib.dates as dates      import matplotlib.pyplot as plt         from pprint import pprint 
#import pandas as pd           import yfinance as yf f



init()


class Stock:
    def __init__(stock, kuerzel, name, wkn):
        stock.kuerzel = kuerzel
        stock.name = name
        stock.wkn = wkn
        stock.kursD = []

    def ADD(stock, date, open_price, high_price, low_price, close_price, volume, adj_close):
        if len(stock.kursD) >= 30:
            stock.kursD.pop(0)
        stock.kursD.append((date, open_price, high_price, low_price, close_price, volume, adj_close))

    def data(stock):
        return stock.kursD[-1] if stock.kursD else None


def custom_hash(key, capacity):
    hash_value = 0
    for char in key:
        hash_value = (31 * hash_value + ord(char)) % capacity
    return hash_value


class HashTable:
    def __init__(HashTable, capacity=1003):
        HashTable.capacity = capacity
        HashTable.size = 0
        HashTable.buckets = [(None, None)] * HashTable.capacity

    def __len__(HashTable):
        return HashTable.size

    def __contains__(HashTable, key):
        index = HashTable.quadSond(key)
        return HashTable.buckets[index] is not None and HashTable.buckets[index] != (None, None)

    def quadSond(HashTable, key):
        index = custom_hash(key, HashTable.capacity)
        i = 1
        while HashTable.buckets[index] is not None and HashTable.buckets[index][0] != key:
            index = (index + i ** 2) % HashTable.capacity
            i += 1
            if i >= HashTable.capacity:
                break
        return index

    def __getitem__(HashTable, key):
        for k, v in HashTable.bucket(key):
            if k == key:
                return v
        raise KeyError(key)

    def bucket(HashTable, key):
        index = HashTable.quadSond(key)
        return [HashTable.buckets[index]] if HashTable.buckets[index] is not None else []

    def __setitem__(HashTable, key, value):
        index = HashTable.quadSond(key)
        collisions = 0
        while HashTable.buckets[index] is not None and HashTable.buckets[index] != (None, None):
            if HashTable.buckets[index][0] == key:
                break
            index = (index + collisions ** 2) % HashTable.capacity
            collisions += 1
            if collisions >= HashTable.capacity:
                raise Exception("Hash table is full.")
        if HashTable.buckets[index] == (None, None):
            HashTable.size += 1
        HashTable.buckets[index] = (key, value)
        if HashTable.size > 0.8 * HashTable.capacity and collisions > 5:
            HashTable.resize()

    def __delitem__(HashTable, key):
        index = HashTable.quadSond(key)
        if HashTable.buckets[index] is not None:
            HashTable.buckets[index] = (None, None)
            HashTable.size -= 1
        else:
            raise KeyError(key)

    def resize(HashTable):
        HashTable.capacity *= 2
        new_buckets = [(None, None)] * HashTable.capacity
        for bucket in HashTable.buckets:
            if bucket != (None, None):
                for key, value in HashTable.bucket(bucket[0]):
                    index = custom_hash(key, HashTable.capacity)
                    new_buckets[index] = (key, value)
        HashTable.buckets = new_buckets

    def values(HashTable):
        return [item for bucket in HashTable.buckets if bucket is not None for _, item in [bucket]]


def handler(prompt):
    while True:
        data = input(f"Enter the stock {prompt}: ").strip()
        if not data:
            print("Input can't be empty. Please try again.")
            continue
        return data


def add(stocks):
    print(Fore.YELLOW, end='')

    while True:
        kuerzel = handler('kuerzel')
        if kuerzel in stocks:
            print(f"Error: {kuerzel} is already in use. Please choose a different kuerzel.")
        else:
            break
    while True:
        name = handler('name')
        if any(stock and stock.name == name for stock in stocks.values()):
            print(f"Error: {name} is already in use. Please choose a different name.")
        else:
            break

    wkn = handler('WKN')
    stocks[kuerzel] = Stock(kuerzel, name, wkn)
    print(f"Stock named {name} with the kuerzel {kuerzel} added successfully.")


def delete(stocks):
    print(Fore.GREEN, end='')
    kuerzel = handler('kuerzel')

    if kuerzel in stocks:
        del stocks[kuerzel]
        print(f"Stock with kuerzel {kuerzel} deleted successfully.")
    else:
        print(f"Stock with kuerzel {kuerzel} not found.")


def importCSV(stocks):
    print(Fore.CYAN, end='')
    root = tk.Tk()
    file_path = filedialog.askopenfilename(defaultextension=".csv", filetypes=[("CSV Files", "*.csv")])
    root.destroy()
    if not file_path or not file_path.lower().endswith('.csv'):
        print("Invalid file type. Please select a CSV file. Importing stock data aborted.")
        return
    kuerzel = handler('kuerzel')
    if kuerzel in stocks:
        stock = stocks[kuerzel]
        with open(file_path, newline='') as csvfile:
            reader = csv.DictReader(csvfile)
            for row in reader:
                stock.ADD(row['Date'], float(row['Open']), float(row['High']), float(row['Low']),
                          float(row['Close']), float(row['Volume']), float(row['Adj Close']))
        print(f"Stock {kuerzel} data imported successfully.")
    else:
        print(f"Stock with kuerzel {kuerzel} not found.")


def search(stocks):
    print(Fore.MAGENTA, end='')
    found = False
    while not found:
        search_key = input("Enter the stock Kuerzel or name: ").strip().lower()
        if not search_key:
            print("Input can't be empty. Please try again.")
            continue

        for stock in stocks.values():
            if stock is None:
                continue

            if search_key == stock.kuerzel.lower() or search_key == stock.name.lower():
                found = True
                print(f"Stock Kuerzel: {stock.kuerzel} Name: {stock.name}")
                latest_data = stock.data()
                if latest_data:
                    print("\nLatest stock data:")
                    print(
                        f"Date: {latest_data[0]}\nOpen: {latest_data[1]}\nHigh: {latest_data[2]}\nLow: {latest_data[3]}"
                        f"\nClose: {latest_data[4]}\nVolume: {latest_data[5]}\nAdj Close: {latest_data[6]}")
                else:
                    print("No stock data available.")
                break

        if not found:
            print(f"Stock with Kuerzel or name {search_key} not found.")
            choice = input("Do you want to try searching again? [Y/N]: ").strip().lower()
            if choice != 'y':
                break


def printASCII(stock, width=60, height=15):
    print(Fore.BLUE, end='')

    if not stock.kursD:
        print(f"No price data found for stock {stock.kuerzel}. Please import the CSV file with stock prices.")
        return

    adj_closes = [kursD[6] for kursD in stock.kursD]
    min_price = min(adj_closes)
    max_price = max(adj_closes)
    price_range = max_price - min_price

    chart = [[' ' for _ in range(width)] for _ in range(height)]

    for idx, adj_close in enumerate(adj_closes):
        col = min(int(idx * width / (len(adj_closes) - 1)), width - 1)
        row = int((1 - (adj_close - min_price) / price_range) * (height - 1))
        chart[row][col] = '*'

    print(
        f"ASCII Chart for Stock name  {stock.name} Kuerzel {stock.kuerzel} - Closing price (Last 30 days): LEFT to RIGHT(most recent)")
    for row in chart:
        print(''.join(row))
    print()


def plot(stocks):
    print(Fore.BLUE, end='')
    kuerzel = handler('Kuerzel')
    try:
        stock = stocks[kuerzel]
    except KeyError:
        stock = None

    if stock:
        if stock.kursD:
            printASCII(stock, width=60, height=15)
        else:
            print(f"No data data found for stock {kuerzel}. Please import the CSV file with stock data.")
    else:
        print(f"Stock with Kuerzel {kuerzel} not found.")


def save(stocks):
    print(Fore.RED, end='')
    root = tk.Tk()
    file_path = filedialog.asksaveasfilename(defaultextension=".pickle", filetypes=[("Pickle Files", "*.pickle")])
    root.destroy()
    if file_path:
        with open(file_path, 'wb') as handle:
            pickle.dump(stocks, handle, protocol=pickle.HIGHEST_PROTOCOL)
            print("Stock data saved successfully.")
    else:
        print("No valid file selected. Stock data not saved.")


def load():
    print(Fore.YELLOW, end='')
    root = tk.Tk()
    file_path = filedialog.askopenfilename(defaultextension=".pickle", filetypes=[("Pickle Files", "*.pickle")])
    root.destroy()
    if not file_path or not file_path.lower().endswith('.pickle'):
        print("Invalid file type. Please select a Pickle file. Loading stock data aborted.")
        return
    with open(file_path, 'rb') as handle:
        stocks = pickle.load(handle)
        print("Stock data loaded successfully.")
        return stocks


def main():
    stocks = HashTable()

    while True:
        print(
            f"\nMenu: {Fore.YELLOW}1. ADD{Style.RESET_ALL} {Fore.GREEN}2. DEL{Style.RESET_ALL} {Fore.CYAN}3. IMPORT{Style.RESET_ALL} {Fore.MAGENTA}4. SEARCH{Style.RESET_ALL} {Fore.BLUE}5. PLOT{Style.RESET_ALL} {Fore.RED}6. SAVE{Style.RESET_ALL} {Fore.YELLOW}7. LOAD{Style.RESET_ALL} {Fore.GREEN}8. EXIT{Style.RESET_ALL}")
        choice = input("Enter your choice: ")
        _ = (add(stocks) if choice == "1" else
             delete(stocks) if choice == "2" else
             importCSV(stocks) if choice == "3" else
             search(stocks) if choice == "4" else
             plot(stocks) if choice == "5" else
             save(stocks) if choice == "6" else
             exec("stocks = load()") if choice == "7" else
             sys.exit() if choice == "8" else
             print(f"{Fore.RED}Invalid choice. Please try again.{Style.RESET_ALL}"))


if __name__ == "__main__":   # https://docs.python.org/3/library/__main__.html
    main()
