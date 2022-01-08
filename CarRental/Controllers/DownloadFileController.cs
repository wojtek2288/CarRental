using CarRental.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.AzureFiles;
using System.IO;
using CarRental.Services;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DownloadFileController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private IDownloadFileService _downloadFileService;

        public DownloadFileController(ILogger<UsersController> logger, IDownloadFileService downloadFileService)
        {
            _logger = logger;
            _downloadFileService = downloadFileService;
        }

    }
}
