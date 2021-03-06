﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VillageSoftware.PathMatcher
{
    public class PathInfo
    {
        public string FilePath { get; private set; }
        public IEnumerable<string> Chunks { get; private set; }
        public char Separator { get; private set; }
        public string FileDirectoryOnly { get; private set; }
        public string FileNameOnly { get; private set; }
        public bool IsUrl { get; private set; }
        public bool HasTerminatingSeparator { get; private set; }

        private Uri _url;

        public PathInfo(string path)
        {
            RebuildUsing(path);
        }

        private void RebuildUsing(string path)
        {
            //Set the basics
            FilePath = path;
            Separator = MostCommonSeparatorIn(path);
            HasTerminatingSeparator = HasFinalSeparator(FilePath, Separator);
            Chunks = Chunk(path);

            //Decide whether it's a URL or local
            IsUrl = false;
            if (FilePath.Contains("//"))
            {
                try
                {
                    _url = new Uri(FilePath);
                    IsUrl = true;
                }
                catch (Exception)
                {
                    IsUrl = false;
                }
            }

            //Stop further processing if path is empty
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            //Set the DirectoryOnly and FileNameOnly bits
            if (IsUrl)
            {
                //If the fragment contains directory separators, we should consider it part of the path
                var totalUrlPath = _url.Fragment.Contains(Separator.ToString())
                    ? _url.AbsolutePath + _url.Fragment
                    : _url.AbsolutePath;

                var urlDirectory = Path.GetDirectoryName(totalUrlPath);
                urlDirectory = ConformAllSeparatorsTo(urlDirectory, Separator);
                FileDirectoryOnly = UrlRoot + urlDirectory;
                if (_url.IsFile)
                {
                    FileNameOnly = Path.GetFileName(_url.LocalPath);
                }
                else
                {
                    FileNameOnly = "";
                }
            }
            else
            {
                FileDirectoryOnly = Path.GetDirectoryName(FilePath);
                FileNameOnly = Path.GetFileName(FilePath);
            }
        }

        private string UrlRoot
        {
            get
            {
                return _url.Scheme + "://" + _url.Authority;
            }
        }

        private static bool HasFinalSeparator(string path, char separator)
        {
            return path.EndsWith(separator.ToString());
        }

        /// <summary>
        /// Ensure that the passed path either has or does not have a trailing separator, using the default Separator for this instance of PathInfo
        /// </summary>
        /// <param name="path">The path to work on</param>
        /// <param name="showFinalSeparator">True to add the trailing separator if not present, False to remove the trailing separator if present. Otherwise method does leaves original path as-is.</param>
        /// <returns>The corrected path string</returns>
        public string GetPathWithFinalSeparatorOnOff(string path, bool showFinalSeparator)
        {
            if (HasFinalSeparator(path, Separator))
            {
                return showFinalSeparator
                    ? path
                    : path.Substring(0, path.Length - 1);
            }
            else
            {
                return showFinalSeparator
                    ? path + Separator
                    : path;
            }
        }

        /// <summary>
        /// Ensure that the passed path either has or does not have a trailing separator, using the passed separator
        /// </summary>
        /// <param name="path">The path to work on</param>
        /// <param name="showFinalSeparator">True to add the trailing separator if not present, False to remove the trailing separator if present. Otherwise method does leaves original path as-is.</param>
        /// <param name="separator">Specifies the separator character to use.</param>
        /// <returns>The corrected path string</returns>
        public static string GetPathWithFinalSeparatorOnOff(string path, bool showFinalSeparator, char separator = '\\')
        {
            if (HasFinalSeparator(path, separator))
            {
                return showFinalSeparator
                    ? path
                    : path.Substring(0, path.Length - 1);
            }
            else
            {
                return showFinalSeparator
                    ? path + separator
                    : path;
            }
        }

        private List<string> Chunk(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new List<string>();
            }

            var theChunks = path.Split(Separator).ToList();
            return theChunks;
        }

        private char MostCommonSeparatorIn(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Path.DirectorySeparatorChar;
            }

            var pathCharArray = path.ToCharArray();
            var countOfDefaultSeparator = pathCharArray.Count(c => c == Path.DirectorySeparatorChar);
            var countOfAltSeparator = pathCharArray.Count(c => c == Path.AltDirectorySeparatorChar);

            if (countOfDefaultSeparator >= countOfAltSeparator)
            {
                return Path.DirectorySeparatorChar;
            }
            return Path.AltDirectorySeparatorChar;
        }

        /// <summary>
        /// Return the section of this path which appears after the specified chunk
        /// </summary>
        /// <param name="chunk">A string segment which appears between two directory separators in this path</param>
        /// <returns>The resulting remainder of the path</returns>
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

        /// <summary>
        /// Return the section of this path which appears before the specified chunk
        /// </summary>
        /// <param name="chunk">A string segment which appears between two directory separators in this path</param>
        /// <returns>The resulting section of path</returns>
        public string PathBeforeChunk(string chunk)
        {
            var startIndex = FilePath.IndexOf(chunk);
            if (startIndex == -1)
            {
                return FilePath;
            }
            return FilePath.Substring(0, startIndex);
        }

        /// <summary>
        /// Change the separators on this PathInfo object to the specified separator and rebuild all of the PathInfo fields
        /// </summary>
        /// <param name="separator">The new separator to use</param>
        public void ConformSeparatorTo(char separator)
        {
            var newPath = FilePath.Replace(Separator, separator);
            RebuildUsing(newPath);
        }

        private string ConformAllSeparatorsTo(string path, char newSeparator)
        {
            if (string.IsNullOrEmpty(path))
            {
                return "";
            }

            var commonSep = MostCommonSeparatorIn(path);
            path = path.Replace(commonSep, newSeparator);
            return path;
        }

        /// <summary>
        /// Safely add the specified segments to the path of this PathInfo instance, using the correct separator and an optional terminating separator
        /// </summary>
        /// <param name="addTerminatingSeparator">True to include a terminating (trailing) separator</param>
        /// <param name="chunks">The path segments which you want to add to this PathInfo</param>
        public void AddChunks(bool addTerminatingSeparator, params string[] chunks)
        {
            var basis = GetPathWithFinalSeparatorOnOff(FilePath, false);
            foreach (var chunk in chunks)
            {
                basis += Separator + chunk;
            }

            if (addTerminatingSeparator)
            {
                basis += Separator;
            }

            RebuildUsing(basis);
        }

        /// <summary>
        /// Set the file name of this PathInfo to the filename found in the supplied parameter
        /// </summary>
        /// <param name="fileName">A filename or a path containing a filename to use to modify this PathInfo</param>
        public void SetFileName(string fileName)
        {
            var newPath = GetPathWithFinalSeparatorOnOff(FileDirectoryOnly, true);
            var newFileNameInfo = new PathInfo(fileName);
            newPath += newFileNameInfo.FileNameOnly;
            RebuildUsing(newPath);
        }

    }
}
