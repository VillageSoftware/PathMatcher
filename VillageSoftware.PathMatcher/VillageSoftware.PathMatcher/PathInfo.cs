using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VillageSoftware.PathMatcher
{
    public class PathInfo
    {
        public string FilePath { get; set; }
        public IEnumerable<string> Chunks { get; set; }
        public char Separator { get; set; }

        public PathInfo(string path)
        {
            RebuildUsing(path);
        }

        private void RebuildUsing(string path)
        {
            FilePath = path;
            Separator = MostCommonSeparatorIn(path);
            Chunks = Chunk(path);
        }

        private List<string> Chunk(string path)
        {
            var theChunks = path.Split(Separator).ToList();
            return theChunks;
        }

        private char MostCommonSeparatorIn(string path)
        {
            var pathCharArray = path.ToCharArray();
            var countOfDefaultSeparator = pathCharArray.Count(c => c == Path.DirectorySeparatorChar);
            var countOfAltSeparator = pathCharArray.Count(c => c == Path.AltDirectorySeparatorChar);

            if (countOfDefaultSeparator >= countOfAltSeparator)
            {
                return Path.DirectorySeparatorChar;
            }
            return Path.AltDirectorySeparatorChar;
        }

        public string PathAfterChunk(string chunk)
        {
            var startIndex = FilePath.IndexOf(chunk);
            if (startIndex == -1)
            {
                return FilePath;
            }
            var endIndex = startIndex + chunk.Length;
            return FilePath.Substring(endIndex);
        }

        public string PathBeforeChunk(string chunk)
        {
            var startIndex = FilePath.IndexOf(chunk);
            if (startIndex == -1)
            {
                return FilePath;
            }
            return FilePath.Substring(0, startIndex);
        }

        public void ConformSeparatorTo(char separator)
        {
            var newPath = FilePath.Replace(Separator, separator);
            RebuildUsing(newPath);
        }
    }
}
