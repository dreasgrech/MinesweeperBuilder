using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperBuilder
{
    class MineField
    {
        private readonly MineFieldRow[] matrix;
        private readonly char mineSymbol;

        public MineField(IList<string> grid, char mineSymbol)
        {
            this.mineSymbol = mineSymbol;
            matrix = new MineFieldRow[grid.Count];
            for (int i = 0; i < grid.Count; i++)
            {
                matrix[i] = new MineFieldRow(i,grid[i],mineSymbol);
            }
        }

        public char GetSymbolAt(int x, int y)
        {
            return matrix[y].GetSymbolAt(x);
        }

        public MineFieldRow GetRowAt(int y)
        {
            return matrix[y];
        }

        public int Rows { get { return matrix.Length; } }

        public IEnumerable<MineFieldRow> GetRows()
        {
            for (int i = 0; i < Rows; i++)
            {
                yield return GetRowAt(i);
            }
        }

        public int GetAdjacentMineCount(int gridX, int gridY)
        {
            /* Adjacent Cells:
             * 
             * X -> cell at (gridX, gridY)
             * a -> adjacent cells
             * 
             * |a|a|a|
             * |a|X|a|
             * |a|a|a|
             */

            var totalAdjacentMines = 0;

            for (int adjacentLine = -1; adjacentLine <= 1; adjacentLine++)
            {
                for (int adjacentCell = -1; adjacentCell <= 1; adjacentCell++)
                {
                    if (adjacentCell == 0 && adjacentLine == 0) // current enumerated cell is the one at (gridX, gridY) so skip it
                    {
                        continue;
                    }
                    int adjacentGridX = gridX + adjacentCell, adjacentGridY = gridY + adjacentLine; // get the absolute grid position of the current enumerated cell
                    if (adjacentGridY == -1 || adjacentGridY > matrix.Length - 1) // (x, -1) or (x, numOfLines + 1) => out of bounds
                    {
                        // stop checking cells because the current enumerated cell is out of bounds and thus its horizontally-adjacent cells will also be out of bounds
                        break;
                    }

                    if (adjacentGridX == -1 || adjacentGridX > matrix[gridY].Length - 1) // (-1, y) or (numOfCells + 1, y) => out of bounds
                    {
                        // skip checking the current enumerated cell because it's out of bounds
                        continue;
                    }

                    var adjacentCellSymbol = matrix[adjacentGridY].GetSymbolAt(adjacentGridX);
                    if (adjacentCellSymbol == mineSymbol)
                    {
                        totalAdjacentMines++;
                    }
                }
            }

            return totalAdjacentMines;
        }

        /// <summary>
        /// Returns the formatted representation of the grid
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var output = new StringBuilder();

            foreach (var row in GetRows())
            {
                for (int gridX = 0; gridX < row.Length; gridX++) // iterate over all the cells of the current row
                {
                    if (row.IsMineAt(gridX))
                    {
                        output.Append(mineSymbol);
                        continue;
                    }

                    var adjacentMineCount = GetAdjacentMineCount(gridX, row.Position);
                    output.Append(adjacentMineCount);
                }
                output.AppendLine("");
            }

            return output.ToString();
        }
    }
}
