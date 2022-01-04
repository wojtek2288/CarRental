﻿using CarRental.AzureFiles;
using CarRental.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace CarRental.Services
{
    public interface ISaveFileService
    {
        string Post(HttpContext context);
    }

    public class SaveFileService : ISaveFileService
    {
        public string Post(HttpContext context)
        {
            List<string> extensions = new List<string>() { ".jpg", ".png", ".jpeg" };
            int twoMB = 2 * 1024 * 1024;

            var file = context.Request.Form.Files[0];

            if (!extensions.Contains(Path.GetExtension(file.FileName)))
            {
                throw new BadHttpRequestException("Not supported extension");
            }
            else if (file.Length > twoMB)
            {
                throw new BadHttpRequestException("File too big");
            }
            else if (file.Length <= 0)
            {
                throw new BadHttpRequestException("File error");
            }
            else
            {
                string fname = AzureFilesPushPull.UploadFile(file.FileName, file.OpenReadStream());
                return fname;
            }
        }
    }
}
