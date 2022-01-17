using CarRental.AzureFiles;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalTests
{
    [TestFixture]
    internal class AzureFilesPushPullTests
    {
        private string file;

        [SetUp]
        public void Setup()
        {
            file = "";
        }

        [Test]
        public async Task UploadDownloadFileTest()
        {
            byte[] data = new byte[] { 48, 49, 50, 51, 52, 53, 54, 55 };
            MemoryStream memoryStream = new(data);
            file = await AzureFilesPushPull.UploadFile("test_file.txt", memoryStream);

            memoryStream = await AzureFilesPushPull.DownloadFile(file);

            byte[] buffer = memoryStream.GetBuffer();

            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(buffer[i], data[i]);
            }
        }

        [TearDown]
        public async Task Teardown()
        {
            await AzureFilesPushPull.DeleteFile(file);
        }
    }
}
