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
            //Destination should be a directory path, not a file
            //destinationPath = Path.GetDirectoryName(destinationPath);

            var source = new PathInfo(sourcefilePath);
            var dest = new PathInfo(destinationPath);
            source.ConformSeparatorTo(dest.Separator);

            var lastCommonChunk = LastCommonChunk(source, dest);

            string newPath = "";
            if (string.IsNullOrEmpty(lastCommonChunk))
            {
                //Simply put the file from A into the directory at B
                var sourceFile = Path.GetFileName(sourcefilePath);
                var destinationDirectory = dest.GetPathWithFinalSeparatorOnOff(dest.FileDirectoryOnly, true);
                newPath = destinationDirectory + source.FileNameOnly;
            }
            else
            {
                //Integrate both paths based on the last matching chunk
                var endOfA = source.PathAfterChunk(lastCommonChunk);
                var rootOfB = dest.PathBeforeChunk(lastCommonChunk);
                newPath = rootOfB + lastCommonChunk + endOfA;
            }

            return newPath;
        }
        
        public static string LastCommonChunk(PathInfo pathA, PathInfo pathB)
        {
            var reversedChunksA = pathA.Chunks.Reverse();
            var reversedChunksB = pathB.Chunks.Reverse();

            //Prioritise B-Chunks because the destination is more important
            // See Unit Test: Resituate_MatchesInSkewedOrder
            foreach (var bChunk in reversedChunksB)
            {
                foreach (var aChunk in reversedChunksA)
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
