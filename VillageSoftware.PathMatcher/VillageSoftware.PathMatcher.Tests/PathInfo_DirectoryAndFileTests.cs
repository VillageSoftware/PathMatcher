using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace VillageSoftware.PathMatcher.Tests
{
    [TestClass]
    public class PathInfo_DirectoryAndFileTests
    {
        [TestMethod]
        public void FileDirectoryOnly_LocalFile()
        {
            var path = @"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md";
            var pathInfo = new PathInfo(path);

            var expected = @"C:\Users\Alice\Documents\Codes\OpenSource\Project";
            var actual = pathInfo.FileDirectoryOnly;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FileDirectoryOnly_LocalDirectory()
        {
            var path = @"C:\Users\Alice\Documents\Codes\OpenSource\Project\";
            var pathInfo = new PathInfo(path);

            var expected = @"C:\Users\Alice\Documents\Codes\OpenSource\Project";
            var actual = pathInfo.FileDirectoryOnly;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FileDirectoryOnly_UrlFile_CrunchBang()
        {
            var path = @"https://filestore.cloud.example/#!/Alice/Codes/Script.py";
            var pathInfo = new PathInfo(path);

            var expected = @"https://filestore.cloud.example/#!/Alice/Codes";
            var actual = pathInfo.FileDirectoryOnly;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FileDirectoryOnly_UrlFile_Normal()
        {
            var path = @"https://filestore.cloud.example/Alice/Codes/Script.py";
            var pathInfo = new PathInfo(path);

            var expected = @"https://filestore.cloud.example/Alice/Codes";
            var actual = pathInfo.FileDirectoryOnly;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FileDirectoryOnly_UrlDirectory_Normal()
        {
            var path = @"https://filestore.cloud.example/Alice/Codes/";
            var pathInfo = new PathInfo(path);

            var expected = @"https://filestore.cloud.example/Alice/Codes";
            var actual = pathInfo.FileDirectoryOnly;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FileDirectoryOnly_UrlDirectory_CrunchBang()
        {
            var path = @"https://filestore.cloud.example/#!/Alice/Codes/";
            var pathInfo = new PathInfo(path);

            var expected = @"https://filestore.cloud.example/#!/Alice/Codes";
            var actual = pathInfo.FileDirectoryOnly;

            Assert.AreEqual(expected, actual);
        }

    }
}
