using CarRental.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.AzureFiles;
using System.IO;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaveFileController : Controller
    {
        private DbUtils dbUtils;
        private readonly ILogger<UsersController> _logger;
        public SaveFileController(ILogger<UsersController> logger, DatabaseContext context)
        {
            _logger = logger;
            dbUtils = new DbUtils(context);
        }

        /// <summary>
        /// Adds an image.
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">File extension not supported or file over 2 MB</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Post()
        {
            List<string> extensions = new List<string>() { ".jpg, .png, .jpeg" };
            int twoMB = 2 * 1024 * 1024;
            try
            {
                var file = Request.Form.Files[0];

                if (!extensions.Contains(Path.GetExtension(file.FileName)))
                {
                    return BadRequest("Not supported extension");
                }
                else if (file.Length < twoMB)
                {
                    return BadRequest("File too big");
                }
                else if(file.Length <= 0)
                {
                    return BadRequest();
                }
                else
                {
                    string fname = AzureFilesPushPull.UploadFile(file.FileName, file.OpenReadStream());
                    return Ok(fname);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
