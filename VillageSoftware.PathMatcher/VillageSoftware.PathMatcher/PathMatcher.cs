using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VillageSoftware.PathMatcher
{
    public static class PathMatcher
    {
        public static string Resituate(string sourcefilePath, string destinationPath)
        {
            var source = new PathInfo(sourcefilePath);
            var dest = new PathInfo(destinationPath);
            source.ConformSeparatorTo(dest.Separator);

            var lastCommonChunk = LastCommonChunk(source, dest);

            var endOfA = source.PathAfterChunk(lastCommonChunk);
            var rootOfB = dest.PathBeforeChunk(lastCommonChunk);

            var newPath = rootOfB + lastCommonChunk + endOfA;

            return newPath;
        }
        
        public static string LastCommonChunk(PathInfo pathA, PathInfo pathB)
        {
            var reversedChunksA = pathA.Chunks.Reverse();
            var reversedChunksB = pathB.Chunks.Reverse();
            foreach (var aChunk in reversedChunksA)
            {
                foreach (var bChunk in reversedChunksB)
                {
                    if (aChunk == bChunk)
                    {
                        return aChunk;
                    }
                }
            }
            return "";
        }

    }
}
