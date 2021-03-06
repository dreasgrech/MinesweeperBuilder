﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace MinesweeperBuilder
{
    class Program
    {
        /* TODO: Command line options
         * TODO: The GetMineFields method doesn't belong in this class, so it needs refactoring to somewhere else
         * TODO: https://github.com/dreasgrech/MinesweeperBuilder/issues#issue/1
         */


        private const char MINE = '*', SPACE = '.';

        static List<MineField> GetMineFields(IEnumerator<string> lineEnumerator)
        {
            lineEnumerator.MoveNext();
            MatchCollection metaData;
            Regex firstLineIdentifier = new Regex(@"^(\d)? (\d)?$"), fieldLineIdentifier;

            var grids = new List<MineField>();
            while ((metaData = firstLineIdentifier.Matches(lineEnumerator.Current)).Count > 0)
            {
                int lines = Convert.ToInt32(metaData[0].Groups[1].Value),
                    columns = Convert.ToInt32(metaData[0].Groups[2].Value);

                fieldLineIdentifier = new Regex(@"^(\" + MINE + @"|\" +SPACE + "){" + lines + "}");
                var grid = new string[lines];

                for (int i = 0; i < lines; i++)
                {
                    lineEnumerator.MoveNext();
                    if (lineEnumerator.Current.Length > columns || !fieldLineIdentifier.IsMatch(lineEnumerator.Current)) // the regex already checks for the length but if there are more characters, we can catch that with a trivial greater than operation rather than with IsMatch of the Regex, which is more expensive
                    {
                        throw new Exception("So...either the number of symbols in the line exceed the total specified by the YOU before or the line contains invalid characters (Why would you do such a thing?).  Be con·sist·ent");
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
            if (args.Length == 0)
            {
                throw new FileNotFoundException("I need some sample input in the form of a path mate!  How can I work without some input from your side?");
            }

            if (!File.Exists(args[0]))
            {
                throw new FileLoadException("This file does not exist.  Wtf?");
            }

            var lineEnumerator = new FileReader(args[0]).GetEnumerator();

            var mineFields = GetMineFields(lineEnumerator);

            if (mineFields.Count == 0)
            {
                throw new Exception("I found no mine fields in your file, and that's probably due to invalid formatting from your side.  Double check you file next time please.");
            }
            for (var i = 0; i < mineFields.Count; i++) 
            {
                Console.WriteLine(String.Format("Field #{0}", i + 1));
                Console.WriteLine(mineFields[i]);
            }

            if (Debugger.IsAttached) //Running from the IDE
            {
                Console.ReadKey();
            }
        }
    }
}
