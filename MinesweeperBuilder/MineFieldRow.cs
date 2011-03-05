using System;
using System.Collections;
using System.Collections.Generic;

namespace MinesweeperBuilder
{
    class MineFieldRow:IEnumerable<char>
    {
        private string row;
        private readonly char mineSymbol;

        public int Length { get { return row.Length; } }

        public int Position { get; private set; }
        public MineFieldRow(int position, string row, char mineSymbol)
        {
            Position = position;
            this.row = row;
            this.mineSymbol = mineSymbol;
        }

        public char GetSymbolAt(int cell)
        {
            if (cell < 0 || cell >= Length)
            {
                throw new IndexOutOfRangeException(String.Format("The specified position, {0}, is out of bounds.  The range of the current cell is 0..{1}", cell, Length - 1));
            }

            return row[cell];
        }

        /// <summary>
        /// Returns true if the cell is a mine
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool IsMineAt(int cell)
        {
            return GetSymbolAt(cell) == mineSymbol;
        }

        public IEnumerator<char> GetEnumerator()
        {
            return row.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
