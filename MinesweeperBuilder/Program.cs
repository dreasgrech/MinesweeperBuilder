using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MineSweeper
{
    class Program
    {
        private const char MINE = '*', SPACE = '.';

        static List<string[]> GetMineFields(IEnumerator<string> lineEnumerator)
        {
            MatchCollection metaData;
            Regex firstLineIdentifier = new Regex(@"^(\d)? (\d)?$"), fieldLineIdentifier;

            var grids = new List<string[]>();
            while ((metaData = firstLineIdentifier.Matches(lineEnumerator.Current)).Count > 0)
            {
                int lines = Convert.ToInt32(metaData[0].Groups[1].Value),
                    columns = Convert.ToInt32(metaData[0].Groups[2].Value);

                fieldLineIdentifier = new Regex(@"^(\*|\.){" + lines + "}");
                var grid = new string[lines];

                for (int i = 0; i < lines; i++)
                {
                    lineEnumerator.MoveNext();
                    if (lineEnumerator.Current.Length > columns || !fieldLineIdentifier.IsMatch(lineEnumerator.Current)) // the regex already checks for the length but if there are more characters, we can catch that with a trivial greater than operation rather than with IsMatch of the Regex, which is more expensive
                    {
                        throw new Exception("Either the number of symbols in the line exceed the total specified by the fields or the line contains invalid characters");
                    }
                    grid[i] = lineEnumerator.Current;
                }
                grids.Add(grid);
                lineEnumerator.MoveNext();
            }

            return grids;
        }

        static void Main(string[] args)
        {
            var output = new StringBuilder();

            var fileReader = new FileReader(args[0]);
            var lineEnumerator = fileReader.GetEnumerator();

            lineEnumerator.MoveNext();

            var grids = GetMineFields(lineEnumerator);

            for (var i = 0; i < grids.Count; i++)
            {
                var grid = grids[i];
                output.AppendLine(String.Format("Field #{0}", i + 1));
                for (int gridY = 0; gridY < grid.Length; gridY++)
                {
                    var line = grid[gridY];
                    for (int gridX = 0; gridX < line.Length; gridX++)
                    {
                        var currentSymbol = line[gridX];
                        if (currentSymbol == MINE)
                        {
                            output.Append('*');
                            continue;
                        }

                        var adjacentMines = GetAdjacentMineCount(gridX, gridY, grid);
                        output.Append(adjacentMines);
                    }
                    output.AppendLine("");
                }
            }

            Console.WriteLine(output.ToString());

            if (Debugger.IsAttached) //Running from the IDE
            {
                Console.ReadKey();
            }
        }

        static int GetAdjacentMineCount(int gridX, int gridY, string[] grid)
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
                    if (adjacentGridY == -1 || adjacentGridY > grid.Length - 1) // (x, -1) or (x, numOfLines + 1) => out of bounds
                    {
                        break;
                    }
                    if (adjacentGridX == -1 || adjacentGridX > grid[gridY].Length - 1) // (-1, y) or (numOfCells + 1, y) => out of bounds
                    {
                        continue;
                    }
                    var adjacentCellSymbol = grid[adjacentGridY][adjacentGridX];
                    if (adjacentCellSymbol == MINE)
                    {
                        adjacentMines++;
                    }
                }
            }

            return adjacentMines;
        }
    }
}
