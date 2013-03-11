using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    using Extensions.MatrixExtensions;

    class Game
    {
        public enum Tile
        {
            Blank,
            _ = Blank,

            Dot,
            o = Dot,

            BigDot,
            O = BigDot,

            Wall,
            l = Wall,
        }

        public Location PlayerLocation = new Location(3, 3);
        public List<Location> GhostLocations = new List<Location>(new[]{ 
                                             new Location (2,7),
                                             new Location (2,8),
                                         });

        public Matrix<Tile> Tiles = new Matrix<Tile>(new Tile[,] {
            { Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l,},
            { Tile.l, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.o, Tile.l, },
            { Tile.l, Tile.o, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.o, Tile.o, Tile.l, },
            { Tile.l, Tile.o, Tile.o, Tile.o, Tile.O, Tile.o, Tile.o, Tile.o, Tile.O, Tile.l, },
            { Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l, Tile.l,},
        });

        public int Big = 0;

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var y in Tiles.VerticalRange)
            {
                foreach (var x in Tiles.HorizontalRange)
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
                            case Tile.Blank:
                                stringBuilder.Append(" ");
                                break;

                            case Tile.Dot:
                                stringBuilder.Append(".");
                                break;

                            case Tile.BigDot:
                                stringBuilder.Append("*");
                                break;

                            case Tile.Wall:
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

        public void MoveBy(int dx, int dy)
        {
            Location newLocation = new Location(PlayerLocation.x + dx, PlayerLocation.y + dy);

            if (this.Tiles.GetAt(newLocation) != Game.Tile.Wall)
            {
                this.PlayerLocation = newLocation;
                if (this.Big > 0) this.Big--;
            }

            if (this.Tiles.GetAt(this.PlayerLocation) == Game.Tile.o)
                this.Tiles.SetAt(this.PlayerLocation, Game.Tile._);

            if (this.Tiles.GetAt(this.PlayerLocation) == Game.Tile.O)
            {
                this.Tiles.SetAt(this.PlayerLocation, Game.Tile._);
                this.Big = 8;
            }

            if (this.Big > 0)
            {
                var ghostId = GhostLocations.FindIndex(gl => PlayerLocation.x == gl.x && PlayerLocation.y == gl.y);
                if (ghostId != -1)
                    this.GhostLocations.RemoveAt(ghostId);
            }
        }

        public void MoveGhosts()
        {
            foreach (var ghostId in Enumerable.Range(0, this.GhostLocations.Count))
            {
                bool movedGhost;
                do
                {
                    switch (random.Next(4))
                    {
                        case 0:
                            movedGhost = TryMoveGhostBy(ghostId, -1, 0);
                            break;

                        case 1:
                            movedGhost = TryMoveGhostBy(ghostId, +1, 0);
                            break;

                        case 2:
                            movedGhost = TryMoveGhostBy(ghostId, 0, -1);
                            break;

                        case 3:
                            movedGhost = TryMoveGhostBy(ghostId, 0, +1);
                            break;

                        default:
                            throw new Exception();
                    }
                } while (!movedGhost);
            }

        }

        bool TryMoveGhostBy(int id, int dx, int dy)
        {
            Location newLocation = new Location(this.GhostLocations[id].x + dx, this.GhostLocations[id].y + dy);

            if (this.Tiles.GetAt(newLocation) != Game.Tile.l)
            {
                this.GhostLocations[id] = newLocation;
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
                foreach (var y in Tiles.VerticalRange)
                    foreach (var x in Tiles.HorizontalRange)
                        if (this.Tiles[x, y] == Tile.o || this.Tiles[x, y] == Tile.O)
                            return false;

                return true;
            }
        }
    }
}
