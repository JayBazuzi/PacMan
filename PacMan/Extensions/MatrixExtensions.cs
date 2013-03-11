using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Extensions.MatrixExtensions
{
    static class _
    {
        public static void SetAt<T>(this Matrix<T> @this, Location location, T value)
        {
            @this[location.x, location.y] = value;
        }

        public static T GetAt<T>(this Matrix<T> @this, Location location)
        {
            return @this[location.x, location.y];
        }
    }
}
