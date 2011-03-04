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
            return row[cell];
        }

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
