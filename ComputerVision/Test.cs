using System;
using System.Collections.Generic;
using System.Text;
using DJBlogs.ComputerVision;
namespace ComputerVision
{
    public class Test
    {
        static string subscriptionKey = Environment.GetEnvironmentVariable("Key");
        static string endpoint = Environment.GetEnvironmentVariable("Endpoint");
        private const string ANALYZE_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png";
        private const string DETECT_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample9.png";
        private const string DETECT_DOMAIN_SPECIFIC_URL = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/landmark.jpg";
        private const string READ_TEXT_URL_IMAGE = "https://intelligentkioskstore.blob.core.windows.net/visionapi/suggestedphotos/3.png";

        static void Main(string[] args)
        {
            Console.WriteLine("Azure Cognitive Services Computer Vision - .NET quickstart example");
            Console.WriteLine();
            // <snippet_client>
            // Create a client
            AnalyzeImage analyzeImage = new AnalyzeImage(endpoint, subscriptionKey);
            analyzeImage.AnalyzeImageUrl(ANALYZE_URL_IMAGE).Wait();
            var dd = analyzeImage.GetImageAnalysisCustomModel(ANALYZE_URL_IMAGE).Result;
            //analyzeImage.DetectObjectsUrl(DETECT_URL_IMAGE).Wait();
            //analyzeImage.ReadFileUrl( READ_TEXT_URL_IMAGE).Wait();
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
