using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Assembler.UnitTests
{
    [TestClass]
    public class FileManagerTest
    {
        [TestInitialize]
        public void TestInit()
        {
        }

        [TestMethod]
        public void FileManagerTester_WriteFile_Success()
        {
            var textLines = new string[] { "Hello", "World" };
            var resultPath = FileManager.Instance.ToWriteFile("mock.txt", textLines);
            Assert.IsTrue(!string.IsNullOrEmpty(resultPath));
        }

        [TestMethod]
        public void FileManagerTester_WriteFile_Error()
        {
            var resultPath = FileManager.Instance.ToWriteFile("mock.txt", null);
            Assert.IsFalse(!string.IsNullOrEmpty(resultPath));
        }

        [TestMethod]
        public void FileManagerTester_ReadFile_Success()
        {
            string nameOfFile = "mock.txt";
            var readLines = FileManager.Instance.ToReadFile(nameOfFile);

            Assert.IsTrue(readLines != null);
        }

        [TestMethod]
        public void FileManagerTester_ReadFile_Error()
        {
            string nameOfFile = null;
            var readLines = FileManager.Instance.ToReadFile(nameOfFile);

            Assert.IsFalse(readLines != null);
        }
        [TestMethod]
        public void FileManagerTester_LoggerFile_Success()
        {
            string nameOfFile = "log.txt";
            var logResult = FileManager.Instance.LoggerFile(nameOfFile,"Mock Text");

            Assert.IsTrue(logResult);
        }
        [TestMethod]
        public void FileManagerTester_LoggerFile_Error()
        {
            string nameOfFile = null;
            var logResult = FileManager.Instance.LoggerFile(nameOfFile, "Mock Text");

            Assert.IsFalse(logResult);
        }

    }
}