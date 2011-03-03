using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MineSweeper
{
    class FileReader:IEnumerable<string>
    {
        private readonly string filePath;
        public FileReader(string filePath)
        {
            this.filePath = filePath;
        }

        public IEnumerable<string> Read()
        {
            using (var sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Read().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
