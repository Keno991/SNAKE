using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    class Program
    {

        private const string SNAKECHAR = "o";
        static void Main(string[] args)
        {
            List<List<string>> fieldOfPlay = new List<List<string>>();
            ConsoleKeyInfo key = default;
            ConsoleKeyInfo previouskey = default;
            var snakeBody = new SnakeQueue<int[]>(new Queue<int[]>(new List<int[]>() { new int[2] { 1, 1 }, new int[2] { 1, 2 }, new int[2] { 1, 3 } }));

            var foodCoord = new List<int[]> { new int[2] { 3, 1 }, new int[2] { 4, 6 } };

            fieldOfPlay.Add(new List<string> { "+", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "+" });

            for (int i = 0; i < 8; i++)
            {
                fieldOfPlay.Add(new List<string> { "|", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "|" });
            }

            fieldOfPlay.Add(new List<string> { "+", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "+" });

            fieldOfPlay[1][1] = "o";
            fieldOfPlay[1][2] = "o";
            fieldOfPlay[1][3] = "o";

            fieldOfPlay[3][1] = "x";
            fieldOfPlay[4][6] = "x";

            Task.Run(() =>
            {
                while (true)
                {
                    key = System.Console.ReadKey(true);
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    GenerateFood(fieldOfPlay, foodCoord);

                    Thread.Sleep(5000);
                }
            });

            while (true)
                {

                    Render(fieldOfPlay);
                    MoveSnake(fieldOfPlay, key, previouskey, snakeBody, foodCoord);
                    Console.Clear();
                }
        }


        private static void Render(List<List<string>> fop)
        {
            foreach (var row in fop)
            {
                Console.WriteLine(String.Join(string.Empty, row.ToArray()));
            }
        }

        private static void MoveSnake(List<List<string>> fop, ConsoleKeyInfo key, ConsoleKeyInfo previousKey, SnakeQueue<int[]> snakeBody, List<int[]> foodCoord)
        {
            if (key == default(ConsoleKeyInfo))
            {
                var firstElement = snakeBody.Dequeue();
                fop[firstElement[0]][firstElement[1]] = " ";
                
                var currentLast = snakeBody.Last ?? new int[2] { 1, 3 };
                currentLast[1] = currentLast[1] + 1;
                snakeBody.Enqueue(currentLast);
                fop[currentLast[0]][currentLast[1]] = SNAKECHAR;
                Thread.Sleep(200);
            }
            else
            {
                var firstElement = snakeBody.Peek();
                var currentLast = snakeBody.Last ?? new int[2] { 1, 3 };
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        fop[firstElement[0]][firstElement[1]] = " ";

                        currentLast[0] = currentLast[0] + 1;
                        break;
                    case ConsoleKey.UpArrow:
                        fop[firstElement[0]][firstElement[1]] = " ";

                        currentLast[0] = currentLast[0] - 1;
                        break;
                    case ConsoleKey.LeftArrow:
                        fop[firstElement[0]][firstElement[1]] = " ";

                        currentLast[1] = currentLast[1] - 1;
                        break;
                    case ConsoleKey.RightArrow:
                        fop[firstElement[0]][firstElement[1]] = " ";

                        currentLast[1] = currentLast[1] + 1;
                        break;

                    default:
                        fop[firstElement[0]][firstElement[1]] = " ";
                        
                        currentLast[1] = currentLast[1] + 1;
                        break;
                }
                var isFood = CheckForFood(foodCoord, currentLast);

                if (!isFood)
                {
                    snakeBody.Dequeue();
                }
                
                snakeBody.Enqueue(currentLast);
                fop[currentLast[0]][currentLast[1]] = SNAKECHAR;
                Thread.Sleep(200);
            }

            previousKey = key;
        }

        private static void GenerateFood(List<List<string>> fop, List<int[]> FoodCoord)
        {
            int minX = 1;
            int maxX = 8;
            int miny = 1;
            int maxY = 14;

            int xCoord = 0;
            int yCoord = 0;

            var rnd = new Random();

            while (true)
            {
                xCoord = rnd.Next(minX, maxX);
                yCoord = rnd.Next(miny, maxY);

                if(fop[xCoord][yCoord] == " ")
                {
                    if (FoodCoord.Any())
                    {
                        FoodCoord.Remove(FoodCoord.First());
                    }
                    FoodCoord.Add(new int[2] { xCoord, yCoord });
                    fop[xCoord][yCoord] = "x";
                    return;
                }
            }
        }

        private static bool CheckForFood(List<int[]> FoodCoord, int[] snakeHead) 
        {
            foreach (var item in FoodCoord)
            {
                if(item[0] == snakeHead[0] && item[1] == snakeHead[1])
                {
                    FoodCoord.Remove(item);
                    return true;
                }
            }
            return false;
        }
    }
}
