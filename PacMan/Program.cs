using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            Console.Clear();
            Console.WriteLine(game);

            do
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        game.MoveBy(-1, 0);
                        break;

                    case ConsoleKey.RightArrow:
                        game.MoveBy(+1, 0);
                        break;

                    case ConsoleKey.UpArrow:
                        game.MoveBy(0, -1);
                        break;

                    case ConsoleKey.DownArrow:
                        game.MoveBy(0, +1);
                        break;
                }

                Console.Clear();
                Console.WriteLine(game);

                if (game.Won)
                {
                    Console.WriteLine("You win!");
                    break;
                }

                if (game.Lost)
                {
                    Console.Write("You lose!");
                    break;
                }

            } while (true);
        }
    }
}
