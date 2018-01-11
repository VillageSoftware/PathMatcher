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
        public void LastCommonChunk_NoMatch()
        {
            var pathA = new PathInfo(@"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md");
            var pathB = new PathInfo(@"https://filestore.cloud.example/#!/AJones/Files/");

            string expected = "";
            string actual = PathMatcher.LastCommonChunk(pathA, pathB);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Resituate_OneMatchingChunk_Normal()
        {
            string filePathA = @"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md";
            string filePathB = @"https://filestore.cloud.example/Alice/";

            var actual = PathMatcher.Resituate(filePathA, filePathB);
            var expected = @"https://filestore.cloud.example/Alice/Documents/Codes/OpenSource/Project/README.md";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Resituate_OneMatchingChunk_CrunchBang()
        {
            string filePathA = @"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md";
            string filePathB = @"https://filestore.cloud.example/#!/Alice/";

            var actual = PathMatcher.Resituate(filePathA, filePathB);
            var expected = @"https://filestore.cloud.example/#!/Alice/Documents/Codes/OpenSource/Project/README.md";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Resituate_UseLastOfMultipleMatches()
        {
            string filePathA = @"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md";
            string filePathB = @"https://filestore.cloud.example/#!/Alice/OpenSource/";

            var actual = PathMatcher.Resituate(filePathA, filePathB);
            var expected = @"https://filestore.cloud.example/#!/Alice/OpenSource/Project/README.md";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Resituate_MatchesInSkewedOrder()
        {
            string filePathA = @"C:\Users\Alice\Documents\OpenSource\Codes\Project\README.md";
            string filePathB = @"https://filestore.cloud.example/Alice/Codes/OpenSource/";

            //The correct behaviour is, in fact, to match on the 'OpenSource' chunk
            // and then copy the directory tree after that from the source
            var expected = @"https://filestore.cloud.example/Alice/Codes/OpenSource/Codes/Project/README.md";
            var actual = PathMatcher.Resituate(filePathA, filePathB);
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Resituate_NoMatchesJustPutSourceFileInDestDir()
        {
            string filePathA = @"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md";
            string filePathB = @"https://filestore.cloud.example/#!/AJones/Files/";

            var actual = PathMatcher.Resituate(filePathA, filePathB);
            var expected = @"https://filestore.cloud.example/#!/AJones/Files/README.md";

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

        [TestMethod]
        public void PathInfo_DetectUrlWithHttpsScheme()
        {
            string filePath = @"https://files.com/myfile.txt";
            var path = new PathInfo(filePath);
            
            var expected = true;
            var actual = path.IsUrl;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PathInfo_DetectUrlWithFtpScheme()
        {
            string filePath = @"ftp://files.com/myfile.txt";
            var path = new PathInfo(filePath);

            var expected = true;
            var actual = path.IsUrl;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PathInfo_DetectUrlWithoutScheme()
        {
            string filePath = @"//files.com/myfile.txt";
            var path = new PathInfo(filePath);

            var expected = true;
            var actual = path.IsUrl;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PathInfo_DetectNonUrlWithDrive()
        {
            string filePath = @"C:\Files\Docs\thing.png";
            var path = new PathInfo(filePath);

            var expected = false;
            var actual = path.IsUrl;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PathInfo_DetectNonUrlWithoutDrive()
        {
            string filePath = @"\Files\Docs\thing.png";
            var path = new PathInfo(filePath);

            var expected = false;
            var actual = path.IsUrl;

            Assert.AreEqual(expected, actual);
        }
    }
}
