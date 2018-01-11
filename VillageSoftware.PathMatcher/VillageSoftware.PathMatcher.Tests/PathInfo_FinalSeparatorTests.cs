using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace VillageSoftware.PathMatcher.Tests
{
    [TestClass]
    public class PathInfo_FinalSeparatorTests
    {
        [TestMethod]
        public void GetPathWithFinalSeparatorOnOff_NotPresent_WantIt()
        {
            var path = @"C:\files";
            var pathInfo = new PathInfo(path);

            var expected = @"C:\files\";
            var actual = pathInfo.GetPathWithFinalSeparatorOnOff(path, true);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPathWithFinalSeparatorOnOff_NotPresent_DontWantIt()
        {
            var path = @"C:\files";
            var pathInfo = new PathInfo(path);

            var expected = path;
            var actual = pathInfo.GetPathWithFinalSeparatorOnOff(path, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPathWithFinalSeparatorOnOff_Present_WantIt()
        {
            var path = @"C:\files\";
            var pathInfo = new PathInfo(path);

            var expected = path;
            var actual = pathInfo.GetPathWithFinalSeparatorOnOff(path, true);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPathWithFinalSeparatorOnOff_Present_DontWantIt()
        {
            var path = @"C:\files\";
            var pathInfo = new PathInfo(path);

            var expected = @"C:\files";
            var actual = pathInfo.GetPathWithFinalSeparatorOnOff(path, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPathWithFinalSeparatorOnOff_AlwaysUsePathInfoSeparator()
        {
            //This test uses a different separator to make sure that GetPathWithFinalSeparatorOnOff
            // always adds the prevalent separator and not a default one

            var path = @"C:/files";
            var pathInfo = new PathInfo(path);

            var expected = @"C:/files/";
            var actual = pathInfo.GetPathWithFinalSeparatorOnOff(path, true);

            Assert.AreEqual(expected, actual);
        }
    }
}
