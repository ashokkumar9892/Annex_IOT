using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;

namespace RPMCRUDAPIs.Controllers
{
    public class HomeController : Controller
    {
        IAmazonS3 S3Client { get; set; }

        public HomeController(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}