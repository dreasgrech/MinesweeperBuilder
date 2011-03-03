using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MineSweeper
{
    class MineField
    {
        private string[] matrix;

        public MineField(string[] matrix)
        {
            this.matrix = matrix;
        }

        public char GetSymbolAt(int x, int y)
        {
            return matrix[y][x];
        }
    }
}
