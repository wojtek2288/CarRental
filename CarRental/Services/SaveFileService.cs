using CarRental.AzureFiles;
using CarRental.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace CarRental.Services
{
    public class SaveFileService
    {
        private SaveFileController saveFileController;

        public SaveFileService(SaveFileController saveFileController)
        {
            this.saveFileController = saveFileController;
        }

        public ActionResult Post()
        {
            List<string> extensions = new List<string>() { ".jpg, .png, .jpeg" };
            int twoMB = 2 * 1024 * 1024;
            try
            {
                var file = saveFileController.Request.Form.Files[0];

                if (!extensions.Contains(Path.GetExtension(file.FileName)))
                {
                    return saveFileController.BadRequest("Not supported extension");
                }
                else if (file.Length < twoMB)
                {
                    return saveFileController.BadRequest("File too big");
                }
                else if (file.Length <= 0)
                {
                    return saveFileController.BadRequest();
                }
                else
                {
                    string fname = AzureFilesPushPull.UploadFile(file.FileName, file.OpenReadStream());
                    return saveFileController.Ok(fname);
                }
            }
            catch (Exception ex)
            {
                return saveFileController.StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
