// Import required headers
#include <stdlib.h>
#include <stdio.h>

// Define structure of customers
typedef struct customers
{
char customer_id;
struct customers *next;
} customers;

// Define structure of shop
typedef struct shop
{
customers *all;
customers *k1;
customers *k2;
} shop;

// Function to create a customer
customers *create_customer(char customer)
{
customers *new_customers = malloc(sizeof(customers));
new_customers->customer_id = customer;
new_customers->next = NULL;
return new_customers;
}

// Function to add a customer to the back of a list
customers *add_back(customers *list, customers *element, int *count)
{
// If the list is empty, set list equal to element
if (list == NULL)
{
list = element;
}
// Otherwise traverse the list until the end is found
// and add the element to the end
else
{
customers *traveler = list;
while (traveler->next != NULL)
{
traveler = traveler->next;
}
traveler->next = element;
}
(*count)++;
return list;
}

// Function to add a customer to a list
customers *add_customer(customers *list, char customer_id, int *count)
{
customers *new_customer = create_customer(customer_id);
list = add_back(list, new_customer, count);
return list;
}

// Function to print a list of customers
void print_list(customers *list)
{
customers *p = list;
while (p != NULL)
{
printf("%c ", p->customer_id);
p = p->next;
}
printf("\n");
}

// Function to print the shop state
void print(int c_all, int c_k1, int c_k2, customers *all, customers *k1, customers *k2)
{
printf("Personen: %d\n", c_all);
print_list(all);
printf("Kassa 1: %d\n", c_k1);
print_list(k1);
printf("Kassa 2: %d\n", c_k2);
print_list(k2);
}

// Function to detach the first customer from a list
customers *detach_first(int *counter, customers *list)
{
customers *p = list;
if (p == NULL)
return NULL;
else
{
list = list->next;
p->next = NULL;
(*counter)--;
free(p);
return list;
}
}

// Function to move a customer from one list to another
customers *move_customer(int *c_from, int *c_to, customers *from, customers *to)
{
  customers *move = create_customer(from->customer_id);
  to = add_back(to, move, c_to);
  return to;
}

void free_all(customers *list)
{
    struct customers *p, *np;
    p = list;
    while (p != NULL)
    {
        np = p->next;
        free(p);
        p = np;
    }
}

int main()
{
    // Lists here
    shop *sim_shop = malloc(sizeof(shop));
    sim_shop->all = NULL;
    sim_shop->k1 = NULL;
    sim_shop->k2 = NULL;
    int c_all = 0, c_k1 = 0, c_k2 = 0;
    while (42)
    {
        char customer; //renamed from n
        printf(": ");
        scanf(" %c", &customer);

        if (customer == '-')
            break;

        // Add person
        sim_shop->all = add_customer(sim_shop->all, customer, &c_all);
    }

    int run = 1;
    while (run)
    {
        // Print here
        print(c_all, c_k1, c_k2, sim_shop->all, sim_shop->k1, sim_shop->k2);
        char c;
        printf(": ");
        scanf(" %c", &c);

        if (c == '-')
            break;

        switch (c)
        {
            case '-':
                run = 0;
                break;
            case 'a':
                // anstellen
                if (c_k1 > c_k2)
                {
                    sim_shop->k2=move_customer(&c_all, &c_k2, sim_shop->all, sim_shop->k2);
                    sim_shop->all = detach_first(&c_all, sim_shop->all);
                }

                else
                {
                    sim_shop->k1=move_customer(&c_all, &c_k1, sim_shop->all, sim_shop->k1);
                    sim_shop->all = detach_first(&c_all, sim_shop->all);
                }

                break;
            case '1':
                // kassa1
                sim_shop->k1 = detach_first(&c_k1, sim_shop->k1);
                break;
            case '2':

                // kassa2
                sim_shop->k2 = detach_first(&c_k2, sim_shop->k2);

                break;
        }
    }

    // free list
    free_all(sim_shop->all);
    free_all(sim_shop->k1);
    free_all(sim_shop->k2);
    free(sim_shop);
    return 0;
}