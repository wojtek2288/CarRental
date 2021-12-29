using Azure.Storage.Blobs;
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
        private static string connectionString = "DefaultEndpointsProtocol=https;AccountName=carrentalservicestorage;AccountKey=hSRbSQtX9aqi4PBLT48zJNCV51Yl0FFujRwtqKPVDKczyoKkn61UTS0XfKVWuSbBzmIndqbznrrK9E0JYj/4Iw==;EndpointSuffix=core.windows.net";

        public static string UploadFile(string fileName, Stream fileStream)
        {
            string baseName = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            string fname = baseName + Guid.NewGuid().ToString() + extension;

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(fname);

            blobClient.Upload(fileStream);

            return fname;
        }

        public static MemoryStream DownloadFile(string fileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            MemoryStream ms = new MemoryStream();

            blobClient.DownloadTo(ms);
            return ms;
        }
    }
}
