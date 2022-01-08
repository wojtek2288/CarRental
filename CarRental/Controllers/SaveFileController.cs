﻿using CarRental.Data;
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
    public class SaveFileController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private ISaveFileService _saveFileService;

        public SaveFileController(ILogger<UsersController> logger, ISaveFileService saveFileService)
        {
            _logger = logger;
            _saveFileService = saveFileService;
        }

        /// <summary>
        /// Adds an image.
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">File extension not supported or file over 2 MB</response>
        [HttpPost("image")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult PostImage()
        {
            string fname = _saveFileService.PostImage(HttpContext);
            return Ok(fname);
        }

        /// <summary>
        /// Adds a PDF document.
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">File extension not supported or file over 2 MB</response>
        [HttpPost("document")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult PostDocument()
        {
            string fname = _saveFileService.PostPDF(HttpContext);
            return Ok(fname);
        }
    }
}
