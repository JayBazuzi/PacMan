using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication1
{
    public partial class Form1 : Form
    {
        Game game = new Game();
        readonly Graphics graphics;
        public Form1()
        {
            graphics = this.CreateGraphics();
            InitializeComponent();
            game.OnGameOver += GameOver;
            this.timer1.Start();
        }

        private void GameOver(object sender, bool won)
        {
            this.Invalidate();
            this.timer1.Stop();
            if (won) MessageBox.Show("You win!");
            else MessageBox.Show("You lose!");


            Application.Exit();
        }

        const int scale = 14;
        readonly Brush WallBrush = new SolidBrush(Color.DarkBlue);
        readonly Brush PacManBrush = new SolidBrush(Color.Orange);
        readonly Brush BigPacManBrush = new SolidBrush(Color.Brown);
        readonly Brush DotBrush = new SolidBrush(Color.Gray);
        readonly Brush BigDotBrush = new SolidBrush(Color.White);
        readonly Brush BlankBrush = new SolidBrush(Color.Black);
        readonly Brush[] GhostBrushes = new[] {
            new SolidBrush(Color.Red),
            new SolidBrush(Color.Green),
            new SolidBrush(Color.Pink),
            new SolidBrush(Color.Blue),
        };

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var y in game.Tiles.VerticalRange)
            {
                foreach (var x in game.Tiles.HorizontalRange)
                {
                    Rectangle rectangle = new Rectangle(x * scale, (game.Tiles.Height - y) * scale, scale, scale);

                    if (game.GhostLocations.Any(gl => gl.x == x && gl.y == y))
                    {
                        // TODO: mix colors
                        graphics.FillRectangle(this.GhostBrushes[0], rectangle);
                    }
                    else if (game.PlayerLocation.x == x && game.PlayerLocation.y == y)
                    {
                        graphics.FillRectangle(game.Big > 0 ? this.BigPacManBrush : this.PacManBrush, rectangle);

                    }
                    else
                    {
                        switch (game.Tiles[x, y])
                        {
                            case Game.Tile.Blank:
                                graphics.FillRectangle(this.BlankBrush, rectangle);
                                break;

                            case Game.Tile.Dot:
                                graphics.FillRectangle(this.DotBrush, rectangle);
                                break;

                            case Game.Tile.BigDot:
                                graphics.FillRectangle(this.BigDotBrush, rectangle);
                                break;

                            case Game.Tile.Wall:
                                graphics.FillRectangle(this.WallBrush, rectangle);
                                break;

                            default:
                                throw new Exception();
                        }
                    }
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    game.MoveBy(0, +1);
                    break;

                case Keys.Down:
                    game.MoveBy(0, -1);
                    break;

                case Keys.Left:
                    game.MoveBy(-1, 0);
                    break;

                case Keys.Right:
                    game.MoveBy(+1, 0);
                    break;
            }

            this.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            game.MoveGhosts();
            this.Invalidate();
        }
    }
}
