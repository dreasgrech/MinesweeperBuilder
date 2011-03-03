using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MineSweeper
{
    class MineField
    {
        private string[] matrix;
        private char mineSymbol;

        public MineField(string[] matrix, char mineSymbol)
        {
            this.mineSymbol = mineSymbol;
            this.matrix = matrix;
        }

        public char GetSymbolAt(int x, int y)
        {
            return matrix[y][x];
        }

        public string GetRowAt(int y)
        {
            return matrix[y];
        }

        public int Rows { get { return matrix.Length; } }
        public int Columns { get { return matrix[0].Length; } }

        public int GetAdjacentMineCount(int gridX, int gridY)
        {
            var adjacentMines = 0;

            // Start checking adjacent cells
            for (int adjacentLine = -1; adjacentLine <= 1; adjacentLine++)
            {
                for (int adjacentCell = -1; adjacentCell <= 1; adjacentCell++)
                {
                    if (adjacentCell == 0 && adjacentLine == 0)
                    {
                        continue;
                    }
                    int adjacentGridX = gridX + adjacentCell, adjacentGridY = gridY + adjacentLine;
                    if (adjacentGridY == -1 || adjacentGridY > matrix.Length - 1) // (x, -1) or (x, numOfLines + 1) => out of bounds
                    {
                        break;
                    }
                    if (adjacentGridX == -1 || adjacentGridX > matrix[gridY].Length - 1) // (-1, y) or (numOfCells + 1, y) => out of bounds
                    {
                        continue;
                    }
                    var adjacentCellSymbol = matrix[adjacentGridY][adjacentGridX];
                    if (adjacentCellSymbol == mineSymbol)
                    {
                        adjacentMines++;
                    }
                }
            }

            return adjacentMines;
        }
    }
}
