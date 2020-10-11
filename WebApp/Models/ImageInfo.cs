using DJBlogs.ComputerVision.Model;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ImageInfo
    {
        public AnalyzeImageModel ImageAnalysis { get; set; }
        public string URL { get; set; }
    }
}
