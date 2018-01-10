using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace VillageSoftware.PathMatcher.Tests
{
    [TestClass]
    public class PathMatcherTests
    {
        [TestMethod]
        public void PathAfterChunk_ValidMiddleChunk()
        {
            var samplePath = new PathInfo(@"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md");

            string expected = @"\Project\README.md";
            string actual = samplePath.PathAfterChunk("OpenSource");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PathAfterChunk_ChunkNotFound()
        {
            var samplePath = new PathInfo(@"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md");

            string expected = samplePath.FilePath;
            string actual = samplePath.PathAfterChunk("Mystery");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PathBeforeChunk_ValidMiddleChunk()
        {
            var samplePath = new PathInfo(@"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md");

            string expected = @"C:\Users\Alice\Documents\Codes\";
            string actual = samplePath.PathBeforeChunk("OpenSource");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PathBeforeChunk_ChunkNotFound()
        {
            var samplePath = new PathInfo(@"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md");

            string expected = samplePath.FilePath;
            string actual = samplePath.PathBeforeChunk("Mystery");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LastCommonChunk_OnlyMatchingChunk()
        {
            var pathA = new PathInfo(@"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md");
            var pathB = new PathInfo(@"https://filestore.cloud.example/#!/Alice/");

            string expected = "Alice";
            string actual = PathMatcher.LastCommonChunk(pathA, pathB);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LastCommonChunk_PicksLastOutOfMultiple()
        {
            var pathA = new PathInfo(@"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md");
            var pathB = new PathInfo(@"https://filestore.cloud.example/#!/Alice/Codes/");

            string expected = "Codes";
            string actual = PathMatcher.LastCommonChunk(pathA, pathB);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ResituationTest()
        {
            string filePathA = @"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md";
            string filePathB = @"https://filestore.cloud.example/#!/Alice/";
            //var pathA = new PathInfo(filePathA);
            //var pathB = new PathInfo(filePathB);

            var actual = PathMatcher.Resituate(filePathA, filePathB);
            var expected = @"https://filestore.cloud.example/#!/Alice/Documents/Codes/OpenSource/Project/README.md";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConformSeparator_ChangeAllBackToForward()
        {
            string filePath = @"C:\Files\stuff.txt";
            var path = new PathInfo(filePath);

            path.ConformSeparatorTo('/');
            
            var expected = @"C:/Files/stuff.txt";
            var actual = path.FilePath;
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConformSeparator_ChangeVariousToBack()
        {
            string filePath = @"C:\Files/Docs/thing.png";
            var path = new PathInfo(filePath);

            path.ConformSeparatorTo('\\');

            var expected = @"C:\Files\Docs\thing.png";
            var actual = path.FilePath;

            Assert.AreEqual(expected, actual);
        }
    }
}
