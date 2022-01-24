using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.AzureFiles
{
    public static class AzureFilesPushPull
    {
        private static string containerName = "carrentalservicecontainer";

        public static async Task<string> UploadFile(string fileName, Stream fileStream)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var config = builder.Build();

            string connectionString = config.GetConnectionString("AzureFiles");
            string baseName = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            string fname = baseName + Guid.NewGuid().ToString() + extension;

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(fname);

            await blobClient.UploadAsync(fileStream);

            return fname;
        }

        public static async Task<MemoryStream> DownloadFile(string fileName)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var config = builder.Build();

            string connectionString = config.GetConnectionString("AzureFiles");

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            MemoryStream ms = new MemoryStream();

            await blobClient.DownloadToAsync(ms);
            return ms;
        }

        public static async Task<bool> DeleteFile(string fileName)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var config = builder.Build();

            string connectionString = config.GetConnectionString("AzureFiles");

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            bool delete = await blobClient.DeleteIfExistsAsync();
            return delete;
        }
    }
}
