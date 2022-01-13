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
    public class FileController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private IFileService _fileService;

        public FileController(ILogger<UsersController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        /// <summary>
        /// Adds an image.
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">File extension not supported or file over 2 MB</response>
        [HttpPost("save")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Post()
        {
            string fname = await _fileService.Post(HttpContext);
            return Ok(fname);
        }

        /// <summary>
        /// Downloads an image.
        /// </summary>
        /// <response code="200">Success</response>
        [HttpGet("download/{FileName}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Download([FromRoute]string FileName)
        {
            MemoryStream ms = await _fileService.Download(FileName);
            return File(ms.GetBuffer(), "application/octet-stream", FileName);
        }
    }
}
