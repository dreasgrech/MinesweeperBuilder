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

        static List<MineField> GetMineFields(IEnumerator<string> lineEnumerator)
        {
            MatchCollection metaData;
            Regex firstLineIdentifier = new Regex(@"^(\d)? (\d)?$"), fieldLineIdentifier;

            var grids = new List<MineField>();
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
                grids.Add(new MineField(grid, MINE));
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
                for (int gridY = 0; gridY < grid.Rows; gridY++)
                {
                    var line = grid.GetRowAt(gridY);
                    for (int gridX = 0; gridX < line.Length; gridX++)
                    {
                        var currentSymbol = grid.GetSymbolAt(gridX, gridY);
                        if (currentSymbol == MINE)
                        {
                            output.Append(MINE);
                            continue;
                        }

                        var adjacentMines = grid.GetAdjacentMineCount(gridX, gridY);
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
    }
}
