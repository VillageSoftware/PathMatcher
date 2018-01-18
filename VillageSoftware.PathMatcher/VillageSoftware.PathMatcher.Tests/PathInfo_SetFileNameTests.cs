using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;


namespace VillageSoftware.PathMatcher.Tests
{
    [TestClass]
    public class PathInfo_SetFileNameTests
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
        public void SetFileName_BlankToRemoveFileName()
        {
            var originalPath = @"C:\Files\MyFile.txt";
            var target = new PathInfo(originalPath);

            target.SetFileName("");

            var expected = @"C:\Files\";
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
            // It's now "https://files.example/User" which does not "appear" to be a directory, but a file

            // "MyImage.png" will overwrite "User"
            target.SetFileName("MyImage.png");
            
            var expected = @"https://files.example/MyImage.png";
            var actual = target.FilePath;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExampleUsageForReadme()
        {
            string myPath = @"C:\Users\Coder\Documents\Code\Project\File.cs";
            var pathInfo = new PathInfo(myPath);
            pathInfo.ConformSeparatorTo('/');

            //Remove the filename (strip it back to directory only)
            pathInfo.SetFileName("");

            pathInfo.AddChunks(true, "src", "Controllers");
            pathInfo.SetFileName("FileNameController.cs");

            Assert.AreEqual(@"C:\Users\Coder\Documents\Code\Project\src\Controllers\FileNameController.cs", pathInfo.FilePath);

            string remotePath = "https://files.example/Coder/Documents/";
            string newPath = PathMatcher.Resituate(pathInfo.FilePath, remotePath);

            string expected = "https://files.example/Coder/Documents/Code/Project/src/Controllers/FileNameController.cs";
            Assert.AreEqual(expected, newPath);
        }
    }
}
