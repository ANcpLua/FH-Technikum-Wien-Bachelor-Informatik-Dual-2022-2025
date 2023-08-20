#include <stdio.h>

#define N 10

// Define the structure of ant
typedef struct 
{
  int position_x;
  int position_y;
  int direction;
  int dead;
} ant;

// Function to move the ant
void move_ant(ant *a, char field[N][N]) 
{
  // Check if ant is outside the field
  if (a->position_x < 0 || a->position_x == N || a->position_y < 0 || a->position_y == N) 
    {
    a->dead = 1;
    return;
  }
  // Change the color of the field if ant is on a white square
  if (field[a->position_x][a->position_y] == '.') 
  {
    field[a->position_x][a->position_y] = '#';
    a->direction++;
  }
  // Change the color of the field if ant is on a black square
  else 
  {
    field[a->position_x][a->position_y] = '.';
    a->direction--;
  }
  // Update the direction of the ant
  a->direction = (a->direction + 4) % 4;
  switch(a->direction) 
  {
    case 0:
      a->position_y--;
      break;
    case 1:
      a->position_x++;
      break;
    case 2:
      a->position_y++;
      break;
    case 3:
      a->position_x--;
      break;
  }
}

int main() 
{
  char field[N][N];
  // Initialize the field with dots
  for (int i = 0; i < N; i++) 
    {
    for (int j = 0; j < N; j++) 
    {
      field[i][j] = '.';
    }
  }
  int turns = 0;
  ant a = {0};
  printf("Number of turns: ");
  scanf("%d", &turns);
  printf("Start position: ");
  scanf("%d %d", &a.position_x, &a.position_y);
  a.dead = 0;
  a.direction = 3;
  // Move the ant for the given number of turns
  for (int i = 0; i < turns; i++) 
  {
    move_ant(&a, field);
    if (a.dead) 
    {
      break;
    }
  }
  // Update the final position of the ant
  if (!a.dead) 
  {
    if (field[a.position_x][a.position_y] == '.') 
    {
      field[a.position_x][a.position_y] = 'a';
    }
    else {
      field[a.position_x][a.position_y] = 'A';
    }
  }
  // Print the final field
  for (int i = 0; i < N; i++) 
  {
    for (int j = 0; j < N; j++) 
    {
      printf("%c", field[j][i]);
    }
    printf("\n");
  }
  return 0;
}
