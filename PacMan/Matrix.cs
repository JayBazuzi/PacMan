using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Matrix<T>
    {
        public readonly int Width;
        public readonly int Height;

        List<List<T>> _values;

        public Matrix(T[,] values)
        {
            this._values = new List<List<T>>();
            this.Width = values.GetLength(0);
            this.Height = values.GetLength(1);

            foreach (var x in HorizontalRange)
            {
                this._values.Add(new List<T>());

                foreach (var y in VerticalRange)
                {
                    this._values[x].Add(values[x, y]);
                }
            }
        }

        public T this[int i, int j]
        {
            get { return this._values[i][j]; }
            set { this._values[i][j] = value; }
        }

        public IEnumerable<int> HorizontalRange { get { return Enumerable.Range(0, Width); } }
        public IEnumerable<int> VerticalRange { get { return Enumerable.Range(0, Height); } }
    }
}
