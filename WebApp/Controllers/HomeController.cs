using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DJBlogs.ComputerVision;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using WebApp.Models;
using WebApp.Utility;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Get a reference to a container that's available for anonymous access.
            CloudBlobContainer container = new CloudBlobContainer(new Uri(AppConstant.CloudBlobContainerURL));
            //New for core
            OperationContext context = new OperationContext();
            BlobRequestOptions options = new BlobRequestOptions();
            // List blobs in the container.
            var list = container.ListBlobsSegmentedAsync(null, true, BlobListingDetails.All, null, null, options, context);
            var _list = new List<ImageModel>();
            foreach (IListBlobItem blobItem in list.Result.Results)
            {
                _list.Add(new ImageModel()
                {
                    URL = blobItem.Uri.ToString(),
                    Name = Path.GetFileName(blobItem.Uri.ToString())
                });
                //Console.WriteLine(blobItem.Uri);
            }
            return View(_list);
        }
        public IActionResult AnalyzeImage(string url)
        {
            AnalyzeImage analyzeImage = new AnalyzeImage(AppConstant.Endpoint, AppConstant.Key);
            var json = analyzeImage.GetImageAnalysisCustomModel(url).Result;
            ImageInfo imageInfo = new ImageInfo()
            {
                ImageAnalysis = json,
                URL = url
            };
            return View(imageInfo);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult UploadFiles()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return Content("files not selected");

            foreach (var file in files)
            {
                var stream = file.OpenReadStream();

                // Create storagecredentials object by reading the values from the configuration (appsettings.json)
                //deeppawncloudcustomer - Access keys
                StorageCredentials storageCredentials = new StorageCredentials(AppConstant.CloudBlobAccountName, AppConstant.CloudBlobKey);

                // Create cloudstorage account by passing the storagecredentials
                CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
                CloudBlobContainer container = blobClient.GetContainerReference(AppConstant.CloudBlobContainer);

                // Get the reference to the block blob from the container
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);

                // Upload the file
                await blockBlob.UploadFromStreamAsync(stream);
                //var path = Path.Combine(
                //        Directory.GetCurrentDirectory(), "wwwroot",
                //        file.GetFilename());

                //using (var stream = new FileStream(path, FileMode.Create))
                //{
                //    await file.CopyToAsync(stream);
                //}
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            //return Ok(new { count = files.Count });
            return RedirectToAction("Index");
        }
    }
}
