using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;


namespace VillageSoftware.PathMatcher.Tests
{
    [TestClass]
    class PathInfo_SetFileNameTests
    {
        [TestMethod]
        public void SetFileName_LocalPath()
        {
            var originalPath = @"C:\Files\MyFile.txt";
            var target = new PathInfo(originalPath);

            target.SetFileName("NewThing.png");

            var expected = @"C:\Files\NewThing.png";
            var actual = target.FilePath;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetFileName_WebPath()
        {
            var originalPath = @"https://files.example/User/Code.py";
            var target = new PathInfo(originalPath);

            target.SetFileName("Business.cs");

            var expected = @"https://files.example/User/Business.cs";
            var actual = target.FilePath;

            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void AddChunkAnd_SetFileName_LocalPath()
        {
            var originalPath = @"C:\Files\";
            var target = new PathInfo(originalPath);

            target.AddChunks(true, "User");
            target.SetFileName("MyImage.png");

            var expected = @"C:\Files\User\MyImage.png";
            var actual = target.FilePath;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddChunkAnd_SetFileName_WebPath()
        {
            var originalPath = @"https://files.example/";
            var target = new PathInfo(originalPath);

            target.AddChunks(true, "User");
            target.SetFileName("MyImage.png");

            var expected = @"https://files.example/User/MyImage.png";
            var actual = target.FilePath;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddChunkAnd_SetFileName_ForgottenSeparators_WebPath()
        {
            //"Forget" the terminating slash in originalPath:
            var originalPath = @"https://files.example";
            var target = new PathInfo(originalPath);

            //False, to "forget" to add intermediate separator
            target.AddChunks(false, "User");
            target.SetFileName("MyImage.png");

            //Result should have all relevant Seps
            var expected = @"https://files.example/User/MyImage.png";
            var actual = target.FilePath;

            Assert.AreEqual(expected, actual);
        }
    }
}
