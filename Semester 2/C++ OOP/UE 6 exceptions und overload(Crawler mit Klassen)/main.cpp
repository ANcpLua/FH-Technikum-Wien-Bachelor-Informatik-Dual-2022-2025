#include<iostream>
#include<cstdlib>//rand() srand system() windows
#include<ctime>//zeit f�rs seed
#include<assert.h>// PUNKTE
#include<limits>// max 1 move
#include<stdexcept>// for exceptions


const int SIZE=10;

int re_size1=1;
int re_size2=0;

class Player
{
public:
    Player(int x,int y,int life,int relics,int mana,int energie,int rage)
    {
        this->setX(x);
        this->setY(y);
        this->setLife(life);
        this->setRelics(relics);
        this->setMana(mana);
        this->setEnergie(energie);
        this->setRage(rage);
    } Player()
    {
        this->setX(0);
        this->setY(0);
        this->setLife(0);
        this->setRelics(0);
        this->setMana(0);
        this->setEnergie(0);
        this->setRage(0);
    } Player(const Player& other)
    {
        this->x=other.x;
        this->y=other.y;
        this->life=other.life;
        this->relics=other.relics;
        this->mana=other.mana;
        this->energie=other.energie;
        this->rage=other.rage;
    }


    friend std::ostream& operator<<(std::ostream& os, const Player& player)
    {
        os << "Player(x: " << player.x << ", y: " << player.y << ", life: " << player.life
           << ", relics: " << player.relics << ", mana: " << player.mana
           << ", energie: " << player.energie << ", rage: " << player.rage << ")";
        return os;
    }



    int getX()const
    {
        return x;
    } void setX(int x)
    {
        this->x=x;
    } int getY()const
    {
        return y;
    } void setY(int y)
    {
        this->y=y;
    } int getLife()const
    {
        return life;
    } void setLife(int life)
    {
        this->life=life;
    } int getRelics()const
    {
        return relics;
    } void setRelics(int relics)
    {
        this->relics=relics;
    } int getMana()const
    {
        return mana;
    } void setMana(int mana)
    {
        this->mana=mana;
    } int getEnergie()const
    {
        return energie;
    } void setEnergie(int energie)
    {
        this->energie=energie;
    } int getRage()const
    {
        return rage;
    } void setRage(int rage)
    {
        this->rage=rage;
    }
    private:
    int x,y,life,relics,mana,energie,rage;
};

class Enemy
{
public:
    Enemy(int x,int y)
    {
        this->setX(x);
        this->setY(y);
    }

    Enemy()
    {
        this->setX(0);
        this->setY(0);
    } Enemy(const Enemy& other)
    {
        this->x=other.x;
        this->y=other.y;
    }
    int getX()const
    {
        return x;
    } void setX(int x)
    {
        this->x=x;
    } int getY()const
    {
        return y;
    } void setY(int y)
    {
        this->y=y;
    } private:
    int x,y;
};

class Game
{
public:

    Game()
    {

        player.setLife(3);
    }

     const Player& getPlayer() const {
        return player;
    }

    void initializeWorld()
    {
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                world[i][j] = randomlyGenerateField();
            }
        }

        // Place player and enemy
        int px = rand() % SIZE;
        int py = rand() % SIZE;
        world[px][py] = '@';
        player.setX(px);
        player.setY(py);

        int ex = rand() % SIZE;
        int ey = rand() % SIZE;
        while (true)
        {
            if (world[ex][ey] != ' ' || (ex == px && ey == py))
            {
                throw std::logic_error("Trying to place the enemy or player on an occupied or invalid field");
                ex = rand() % SIZE;
                ey = rand() % SIZE;
            }
            else
            {
                break;
            }
        }
        world[ex][ey] = 'E';
        enemy.setX(ex);
        enemy.setY(ey);

    }

   static char randomlyGenerateField()
    {
        int random=rand()%10;
        return(random<5)?' ':(random<7)?'X':(random<9)?'R':'F';
    }
void displayGame()const
    {
        for (int i = 0; i < SIZE; i++)
        {
            std::cout << "+---";
        }
        std::cout << "+" << std::endl;

        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                std::cout << "| " << world[i][j] << " ";
            }
            std::cout << "|" << std::endl;

            for (int j = 0; j < SIZE; j++)
            {
                std::cout << "+---";
            }
            std::cout << "+" << std::endl;
        }
        std::cout << "Player lives left: " << player.getLife() << std::endl;
        std::cout << "Relics collected: " << player.getRelics() << std::endl;
        std::cout << "Rage: " << player.getRage() << std::endl;
        std::cout << "Energie: " << player.getEnergie() << std::endl;
        std::cout << "Mana: " << player.getMana() << std::endl;

    }


    void movePlayerLevel(char direction, int& level)
{
    int new_x = player.getX();
    int new_y = player.getY();

    while (true)
    {
        std::cout << "Enter direction: ";
        std::cin >> direction;
        std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');

        if (!(direction == 'a' || direction == 'A' || direction == 'w' || direction == 'W' || direction == 's' || direction == 'S' || direction == 'd' || direction == 'D'))
        {
            throw std::invalid_argument("Invalid direction");
        }

        switch (direction)
        {
        case 'W':
        case 'w':
            new_x--;
            break;
        case 'A':
        case 'a':
            new_y--;
            break;
        case 'S':
        case 's':
            new_x++;
            break;
        case 'D':
        case 'd':
            new_y++;
            break;
        default:
            throw std::invalid_argument("Invalid direction");
        }

        if (new_x < 0 || new_x >= SIZE || new_y < 0 || new_y >= SIZE)
        {
            throw std::out_of_range("Player is trying to move out of bounds");
        }

        break;
    }


        if (world[new_x][new_y] != ' ')
        {
            numEvents++;

            if (world[new_x][new_y] == 'R')
            {
                player.setRelics(player.getRelics() + 1);

                int rand_num = rand() % 3;
                std::string itemType;
                if (rand_num == 0)
                {
                    player.setEnergie(player.getEnergie() + 1);
                    itemType = "energie item";
                }
                else if (rand_num == 1)
                {
                    player.setMana(player.getMana() + 1);
                    itemType = "mana item";
                }
                else
                {
                    player.setRage(player.getRage() + 1);
                    itemType = "Rage item";
                }

                std::cout << "You found a relic you also found a " << itemType << ", its magic power helps you prevent " << itemType << "traps\n";

                arr[0] = { "Found a Relic and a random Item" };
                resizeArray(arr, re_size2, re_size1+1);
                system("pause");
            }
            else if (world[new_x][new_y] == 'F')
            {
                player.setLife(player.getLife() + 1);

                int rand_num1 = rand() % 3;
                std::string itemType;
                if (rand_num1 == 0)
                {
                    player.setEnergie(player.getEnergie() + 1);
                    itemType = "energie item";
                }
                else if (rand_num1 == 1)
                {
                    player.setMana(player.getMana() + 1);
                    itemType = "mana item";
                }
                else
                {
                    player.setRage(player.getRage() + 1);
                    itemType = "Rage item";
                }

                std::cout << "You found a fountain and gained 1 hp you also found a " << itemType << ", its magic power helps you prevent " << itemType << "traps\n";

                arr[0] = { "Found a Fountain and a random Item" };
                resizeArray(arr, re_size2, re_size1+1);
                system("pause");
            }
            else if (world[new_x][new_y] == 'X')
            {
                int random1 = rand() % 3;
                std::string trapType;
                int requiredResource;
                std::string resourceType;
                int rangeStart, rangeEnd;

                if (random1 == 0)
                {
                    trapType = "ENERGY-Trap";
                    requiredResource = player.getEnergie();
                    resourceType = "ENERGY";
                    rangeStart = 1;
                    rangeEnd = 5;
                }
                else if (random1 == 1)
                {
                    trapType = "MANA-Trap";
                    requiredResource = player.getMana();
                    resourceType = "MANA";
                    rangeStart = 1;
                    rangeEnd = 10;
                }
                else if (random1 == 2)
                {
                    trapType = "RAGE-Trap";
                    requiredResource = player.getRage();
                    resourceType = "RAGE";
                    rangeStart = 1;
                    rangeEnd = 100;
                }

                arr[0] = { "Stepped on a " + trapType + "!" };
                std::cout << "Stepped on a " << trapType << "!" << std::endl;

                if (requiredResource > 0)
                {
                    std::cout << "Use your " << resourceType << " to prevent the Trial." << std::endl;
                    std::cout << "Use " << resourceType << " 1 for yes, 0 for no." << std::endl;
                    bool resource1 = false;
                    std::cin >> resource1;

                    if (resource1)
                    {
                        std::cout << "You have used your " << resourceType << ", the Trial has been prevented." << std::endl;
                        if (resourceType == "ENERGY")
                        {
                            player.setEnergie(player.getEnergie() - 1);
                        }
                        else if (resourceType == "MANA")
                        {
                            player.setMana(player.getMana() - 1);
                        }
                        else if (resourceType == "RAGE")
                        {
                            player.setRage(player.getRage() - 1);
                        }
                        system("pause");
                    }
                    else
                    {
                        std::cout << "You didn't use your " << resourceType << ", the Trial wants you to guess a number between " << rangeStart << "-" << rangeEnd << "." << std::endl;
                        int userGuess;
                        std::cin >> userGuess;
                        assert(userGuess >= rangeStart && userGuess <= rangeEnd && "Invalid input, Restart the game and learn from your mistake.\n");
                        int randomNumber = rand() % 1 + 1;
                        if (userGuess == randomNumber)
                        {
                            std::cout << "Congratulations! You guessed the number right and saved your HP." << std::endl;
                            system("pause");
                        }
                        else
                        {
                            std::cout << "Sorry, the number was " << randomNumber << ", you lost 1 HP." << std::endl;
                            player.setLife(player.getLife() - 1);
                            system("pause");
                        }
                    }
                }
                else
                {
                    std::cout << "You don't have enough " << resourceType << " to prevent the Trial. The Trial wants you to guess a number between " << rangeStart << "-" << rangeEnd << "." << std::endl;
                    int userGuess;
                    std::cin >> userGuess;
                    assert(userGuess >= rangeStart && userGuess <= rangeEnd && "Invalid input, Restart the game and learn from your mistake.\n");
                    int randomNumber = rand() % 1 + 1;
                    if (userGuess == randomNumber)
                    {
                        std::cout << "Congratulations! You guessed the number right and saved your HP." << std::endl;
                        system("pause");
                    }
                    else
                    {
                        std::cout << "Sorry, the number was " << randomNumber << ", you lost 1 HP." << std::endl;
                        player.setLife(player.getLife() - 1);
                        system("pause");
                    }
                }
                world[new_x][new_y] = ' ';


            }
        }
        // Check if new position contains an enemy
        if (new_x == enemy.getX() && new_y == enemy.getY())
        {
            std::cout << "You got caught by the enemy!" << std::endl;
            player.setLife(player.getLife() - 1);
            arr[0] = { " Got caught by the enemy!!" };
            resizeArray(arr, re_size2, re_size1+1);
            system("pause");
            getchar();
        }


        // Move player to new position
        world[player.getX()][player.getY()] = ' ';
        world[new_x][new_y] = '@';
        player.setX(new_x);
        player.setY(new_y);

        // Enemy new position

        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j <SIZE; j++)
            {
                if (world[i][j] == 'E')

                {
                    world[i][j] = ' ';
                }
            }

        }
        //enemy random movement
        int ex = rand() % SIZE;
        int ey = rand() % SIZE;
        while (world[ex][ey] != ' ')
        {
            ex = rand() % SIZE;
            ey = rand() % SIZE;
        }

        world[ex][ey] = 'E';
        enemy.setX(ex);
        enemy.setY(ey);

        if (player.getRelics() == 5)
        {

            std::cout << "Congratulations! You found all the relics and win!!" << std::endl;
            level=4;
            gameOver = true;
        }

        // Check if game is over
        if (player.getLife() <= 0)
        {
            std::cout << "Game over! You ran out of lives." << std::endl;
            gameOver = true;
        }
        else if (level == 1 && player.getRelics() == 3 ) //
        {


            std::cout << "Congratulations! You found all the relics. See what happens on the next level!!" << std::endl;
            system("pause");

            level = 2;
            initializeWorld();
        }
        else if (level == 2 ) //
        {
            int ex1 = rand() % SIZE;
            int ey1 = rand() % SIZE;
            while (world[ex1][ey1] != ' ')
            {
                ex1 = rand() % SIZE;
                ey1 = rand() % SIZE;
            }

            world[ex1][ey1] = 'E';
            enemy.setX(ex1);
            enemy.setY(ey1);

        }

        else if (numEvents == 100)
        {
            std::cout << "Game over! You reached the maximum number of events." << std::endl;
            gameOver = true;
        }

    }

    void resizeArray(std::string*& arr, int& size, int newSize)
    {
        std::string* newArr = new std::string[newSize];

        for (int i = 0; i < size; i++)
        {
            newArr[i] = arr[i];
        }
        delete[] arr;
        arr = newArr;
        size = newSize;
    }
   void DisplayEvents()const
    {
        std::cout << "Event Log :" << std::endl;
        for (int i = 0; i < re_size1; i++)
        {

            std::cout << arr[i] << std::endl;
        }
    }
    bool isGameOver() const
    {
        return gameOver;
    }

    virtual ~Game()
    {
        delete[] arr;
        arr = 0;
    }


private:
    char world[SIZE][SIZE];
    std::string* arr=new std::string[re_size1];
    Player player;
    Enemy enemy;
    int numEvents;
    bool gameOver;


};

int main()
{
    srand(time(0)); // seed the random number generator with the current time
    char direction = '\0';
    Game game;
    int level = 1;

    try
    {
        game.initializeWorld();
    }
    catch (std::logic_error& e)
    {
        std::cout << "An error occurred: " << e.what() << '\n' << std::endl; // virtual const char* what() const noexcept; e.what
    }

    std::cout << "Welcome to RedundantGame!" << std::endl;
    std::cout << "You start with three lives and zero energy, mana, or rage. Search for items to help you survive against enemies, traps, and trials(small help, 1 is your lucky number)." << std::endl;
    std::cout << "Use the 'w', 'a', 's', and 'd' keys to move up, left, down, and right, respectively." << std::endl;

    system("pause");
    getchar();

    while (!game.isGameOver())
    {

        try
        {
            game.displayGame();
            std::cout << game.getPlayer() << std::endl;  // Print the player's current state.
            game.movePlayerLevel(direction, level);
        }
        catch (std::invalid_argument& e)
        {
            std::cout << "An error occurred: " << e.what() << std::endl; // virtual const char* what() const noexcept;
        }
        catch (std::out_of_range& e)
        {
            std::cout << "An error occurred: " << e.what() << std::endl;
        }

    }

    game.DisplayEvents();
    std::cout << "END";

    return 0;
}
