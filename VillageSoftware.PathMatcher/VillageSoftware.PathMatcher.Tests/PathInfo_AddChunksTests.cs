using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace VillageSoftware.PathMatcher.Tests
{
    [TestClass]
    public class PathInfo_AddChunksTests
    {
        [TestMethod]
        public void AddChunks_AddTwoLocalChunks()
        {
            var path = @"C:\Users\Alice\";
            var pathInfo = new PathInfo(path);

            pathInfo.AddChunks(true, "Documents", "Work");

            var expected = @"C:\Users\Alice\Documents\Work\";
            var actual = pathInfo.FilePath;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddChunks_AddTwoWebChunks()
        {
            var path = @"https://files.example/Alice";
            var pathInfo = new PathInfo(path);

            pathInfo.AddChunks(true, "Documents", "Work");

            var expected = @"https://files.example/Alice/Documents/Work/";
            var actual = pathInfo.FilePath;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddChunks_AddTwoLocalChunks_NoTerminatingSlash()
        {
            var path = @"C:\Users\Alice\";
            var pathInfo = new PathInfo(path);

            pathInfo.AddChunks(false, "Documents", "Work");

            var expected = @"C:\Users\Alice\Documents\Work";
            var actual = pathInfo.FilePath;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddChunks_AddTwoWebChunks_NoTerminatingSlash()
        {
            var path = @"https://files.example/Alice";
            var pathInfo = new PathInfo(path);

            pathInfo.AddChunks(false, "Documents", "Work");

            var expected = @"https://files.example/Alice/Documents/Work";
            var actual = pathInfo.FilePath;

            Assert.AreEqual(expected, actual);
        }


    }
}
