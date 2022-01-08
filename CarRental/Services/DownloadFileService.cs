using CarRental.AzureFiles;
using CarRental.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace CarRental.Services
{
    public interface IDownloadFileService
    {
        MemoryStream Image(HttpContext context);
        MemoryStream PDF(HttpContext context);
    }

    public class DownloadFileService : IDownloadFileService
    {
        MemoryStream IDownloadFileService.Image(HttpContext context)
        {
            throw new NotImplementedException();
        }

        MemoryStream IDownloadFileService.PDF(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
