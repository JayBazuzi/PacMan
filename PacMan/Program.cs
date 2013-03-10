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
                        game.TryMoveTo(game.PlayerLocation.x - 1, game.PlayerLocation.y);
                        break;

                    case ConsoleKey.RightArrow:
                        game.TryMoveTo(game.PlayerLocation.x + 1, game.PlayerLocation.y);
                        break;

                    case ConsoleKey.UpArrow:
                        game.TryMoveTo(game.PlayerLocation.x, game.PlayerLocation.y - 1);
                        break;

                    case ConsoleKey.DownArrow:
                        game.TryMoveTo(game.PlayerLocation.x, game.PlayerLocation.y + 1);
                        break;
                }

                Console.Clear();
                Console.WriteLine(game);

                if (game.Won)
                {
                    Console.WriteLine("You win!");
                    break;
                }

            } while (true);
        }
    }
}

class Location
{
    internal int x;
    internal int y;
}


class Game
{
    internal enum Tile
    {
        _,
        o,
        O,
        l
    }

    internal Location PlayerLocation = new Location { x = 3, y = 3 };
    internal Location GhostLocation = new Location { x = 2, y = 7 };

    internal Tile[,] Tiles = new Tile[,] {
        { Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l,},
        { Tile.l, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.l, },
        { Tile.l, Tile.o, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.o, Tile.o, Tile.l, },
        { Tile.l, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.O, Tile.l, },
        { Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l,},
    };

    public int Big = 0;

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var y in Enumerable.Range(0, Tiles.GetLength(1)))
        {
            foreach (var x in Enumerable.Range(0, Tiles.GetLength(0)))
            {
                if (PlayerLocation.x == x && PlayerLocation.y == y)
                {
                    stringBuilder.Append(Big > 0 ? "C" : "c");
                }
                else if (GhostLocation.x == x && GhostLocation.y == y)
                {
                    stringBuilder.Append("G");
                }
                else
                {
                    switch (Tiles[x, y])
                    {
                        case Tile._:
                            stringBuilder.Append(" ");
                            break;

                        case Tile.o:
                            stringBuilder.Append(".");
                            break;

                        case Tile.O:
                            stringBuilder.Append("*");
                            break;

                        case Tile.l:
                            stringBuilder.Append("|");
                            break;

                        default:
                            throw new Exception();
                    }
                }
            }
            stringBuilder.AppendLine();

        }
        return stringBuilder.ToString();
    }

    internal void TryMoveTo(int x, int y)
    {
        if (this.Tiles[x, y] != Game.Tile.l)
        {
            this.PlayerLocation.x = x;
            this.PlayerLocation.y = y;
            if (this.Big > 0) this.Big--;
        }

        if (this.Tiles[this.PlayerLocation.x, this.PlayerLocation.y] == Game.Tile.o)
            this.Tiles[this.PlayerLocation.x, this.PlayerLocation.y] = Game.Tile._;

        if (this.Tiles[this.PlayerLocation.x, this.PlayerLocation.y] == Game.Tile.O)
        {
            this.Tiles[this.PlayerLocation.x, this.PlayerLocation.y] = Game.Tile._;
            this.Big = 8;
        }
    }

    public bool Won
    {
        get
        {
            foreach (var y in Enumerable.Range(0, Tiles.GetLength(1)))
                foreach (var x in Enumerable.Range(0, Tiles.GetLength(0)))
                    if (this.Tiles[x, y] == Tile.o || this.Tiles[x, y] == Tile.O)
                        return false;

            return true;
        }
    }
}
