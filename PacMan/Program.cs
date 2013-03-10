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

                if (game.Lost)
                {
                    Console.Write("You lose!");
                    break;
                }

            } while (true);
        }
    }
}

class Location
{
    public readonly int x;
    public readonly int y;

    public Location(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

class Game
{
    public enum Tile
    {
        _,
        o,
        O,
        l
    }

    public Location PlayerLocation = new Location(3, 3);
    public List<Location> GhostLocations = new List<Location>(new[]{ 
                                             new Location (2,7),
                                             new Location (2,8),
                                         });

    public Tile[,] Tiles = new Tile[,] {
        { Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l,},
        { Tile.l, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.l, },
        { Tile.l, Tile.o, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.o, Tile.o, Tile.l, },
        { Tile.l, Tile.o, Tile.o, Tile.o, Tile.O, Tile.o, Tile.o, Tile.o, Tile.O, Tile.l, },
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
                    if ((PlayerLocation.x + PlayerLocation.y) % 2 == 0)
                        stringBuilder.Append(Big > 0 ? "C" : "c");
                    else
                        stringBuilder.Append(Big > 0 ? "O" : "o");

                }
                else if (GhostLocations.Any(gl => gl.x == x && gl.y == y))
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

    public void TryMoveTo(int x, int y)
    {
        if (this.Tiles[x, y] != Game.Tile.l)
        {
            this.PlayerLocation = new Location(x, y);
            if (this.Big > 0) this.Big--;
        }

        if (this.Tiles[this.PlayerLocation.x, this.PlayerLocation.y] == Game.Tile.o)
            this.Tiles[this.PlayerLocation.x, this.PlayerLocation.y] = Game.Tile._;

        if (this.Tiles[this.PlayerLocation.x, this.PlayerLocation.y] == Game.Tile.O)
        {
            this.Tiles[this.PlayerLocation.x, this.PlayerLocation.y] = Game.Tile._;
            this.Big = 8;
        }

        foreach (var ghostId in Enumerable.Range(0, this.GhostLocations.Count))
        {
            bool movedGhost;
            do
            {
                switch (random.Next(4))
                {
                    case 0:
                        movedGhost = TryMoveGhost(ghostId, GhostLocations[ghostId].x - 1, GhostLocations[ghostId].y);
                        break;

                    case 1:
                        movedGhost = TryMoveGhost(ghostId, GhostLocations[ghostId].x + 1, GhostLocations[ghostId].y);
                        break;

                    case 2:
                        movedGhost = TryMoveGhost(ghostId, GhostLocations[ghostId].x, GhostLocations[ghostId].y - 1);
                        break;

                    case 3:
                        movedGhost = TryMoveGhost(ghostId, GhostLocations[ghostId].x, GhostLocations[ghostId].y + 1);
                        break;

                    default:
                        throw new Exception();
                }
            } while (!movedGhost);
        }

        if (this.Big > 0)
        {
            var ghostId = GhostLocations.FindIndex(gl => PlayerLocation.x == gl.x && PlayerLocation.y == gl.y);
            if (ghostId != -1)
                this.GhostLocations.RemoveAt(ghostId);
        }
    }

    bool TryMoveGhost(int id, int x, int y)
    {
        if (this.Tiles[x, y] != Game.Tile.l)
        {
            this.GhostLocations[id] = new Location(x, y);
            return true;
        }
        else return false;
    }

    Random random = new Random(0);

    public bool Lost
    {
        get
        {
            return GhostLocations.Any(gl => PlayerLocation.x == gl.x && PlayerLocation.y == gl.y);
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
