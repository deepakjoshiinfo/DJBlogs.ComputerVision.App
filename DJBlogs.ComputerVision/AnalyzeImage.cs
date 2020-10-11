using DJBlogs.ComputerVision.Model;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Category = DJBlogs.ComputerVision.Model.Category;

namespace DJBlogs.ComputerVision
{
    public class AnalyzeImage
    {
        private ComputerVisionClient _computerVisionClient;
        public AnalyzeImage(string endpoint, string key)
        {
            _computerVisionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            { Endpoint = endpoint };
        }
        // <snippet_visualfeatures>
        /* 
         * ANALYZE IMAGE - URL IMAGE
         * Analyze URL image. Extracts captions, categories, tags, objects, faces, racy/adult content,
         * brands, celebrities, landmarks, color scheme, and image types.
         */
        public async Task AnalyzeImageUrl(string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - URL");
            Console.WriteLine();

            // Creating a list that defines the features to be extracted from the image. 

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };
            // </snippet_visualfeatures>

            // <snippet_analyze_call>
            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
            // Analyze the URL image 
            ImageAnalysis results = await _computerVisionClient.AnalyzeImageAsync(imageUrl, features);
            // </snippet_analyze_call>

            // <snippet_describe>
            // Sunmarizes the image content.
            Console.WriteLine("Summary:");
            foreach (var caption in results.Description.Captions)
            {
                Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
            }
            Console.WriteLine();
            // </snippet_describe>

            // <snippet_categorize>
            // Display categories the image is divided into.
            Console.WriteLine("Categories:");
            foreach (var category in results.Categories)
            {
                Console.WriteLine($"{category.Name} with confidence {category.Score}");
            }
            Console.WriteLine();
            // </snippet_categorize>

            // <snippet_tags>
            // Image tags and their confidence score
            Console.WriteLine("Tags:");
            foreach (var tag in results.Tags)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence}");
            }
            Console.WriteLine();
            // </snippet_tags>

            // <snippet_objects>
            // Objects
            Console.WriteLine("Objects:");
            foreach (var obj in results.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                  $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
            }
            Console.WriteLine();
            // </snippet_objects>

            // <snippet_faces>
            // Faces
            Console.WriteLine("Faces:");
            foreach (var face in results.Faces)
            {
                Console.WriteLine($"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Left}, " +
                  $"{face.FaceRectangle.Left}, {face.FaceRectangle.Top + face.FaceRectangle.Width}, " +
                  $"{face.FaceRectangle.Top + face.FaceRectangle.Height}");
            }
            Console.WriteLine();
            // </snippet_faces>

            // <snippet_adult>
            // Adult or racy content, if any.
            Console.WriteLine("Adult:");
            Console.WriteLine($"Has adult content: {results.Adult.IsAdultContent} with confidence {results.Adult.AdultScore}");
            Console.WriteLine($"Has racy content: {results.Adult.IsRacyContent} with confidence {results.Adult.RacyScore}");
            Console.WriteLine();
            // </snippet_adult>

            // <snippet_brands>
            // Well-known (or custom, if set) brands.
            Console.WriteLine("Brands:");
            foreach (var brand in results.Brands)
            {
                Console.WriteLine($"Logo of {brand.Name} with confidence {brand.Confidence} at location {brand.Rectangle.X}, " +
                  $"{brand.Rectangle.X + brand.Rectangle.W}, {brand.Rectangle.Y}, {brand.Rectangle.Y + brand.Rectangle.H}");
            }
            Console.WriteLine();
            // </snippet_brands>

            // <snippet_celebs>
            // Celebrities in image, if any.
            Console.WriteLine("Celebrities:");
            foreach (var category in results.Categories)
            {
                if (category.Detail?.Celebrities != null)
                {
                    foreach (var celeb in category.Detail.Celebrities)
                    {
                        Console.WriteLine($"{celeb.Name} with confidence {celeb.Confidence} at location {celeb.FaceRectangle.Left}, " +
                          $"{celeb.FaceRectangle.Top}, {celeb.FaceRectangle.Height}, {celeb.FaceRectangle.Width}");
                    }
                }
            }
            Console.WriteLine();
            // </snippet_celebs>


            // <snippet_landmarks>
            // Popular landmarks in image, if any.
            Console.WriteLine("Landmarks:");
            foreach (var category in results.Categories)
            {
                if (category.Detail?.Landmarks != null)
                {
                    foreach (var landmark in category.Detail.Landmarks)
                    {
                        Console.WriteLine($"{landmark.Name} with confidence {landmark.Confidence}");
                    }
                }
            }
            Console.WriteLine();
            // </snippet_landmarks>

            // <snippet_color>
            // Identifies the color scheme.
            Console.WriteLine("Color Scheme:");
            Console.WriteLine("Is black and white?: " + results.Color.IsBWImg);
            Console.WriteLine("Accent color: " + results.Color.AccentColor);
            Console.WriteLine("Dominant background color: " + results.Color.DominantColorBackground);
            Console.WriteLine("Dominant foreground color: " + results.Color.DominantColorForeground);
            Console.WriteLine("Dominant colors: " + string.Join(",", results.Color.DominantColors));
            Console.WriteLine();
            // </snippet_color>

            // <snippet_type>
            // Detects the image types.
            Console.WriteLine("Image Type:");
            Console.WriteLine("Clip Art Type: " + results.ImageType.ClipArtType);
            Console.WriteLine("Line Drawing Type: " + results.ImageType.LineDrawingType);
            Console.WriteLine();
            // </snippet_type>
        }

        public async Task<ImageAnalysis> GetImageAnalysis(string imageUrl)
        {
            var analyzeImageModel = new AnalyzeImageModel();
            // Creating a list that defines the features to be extracted from the image. 

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };
            ImageAnalysis results = await _computerVisionClient.AnalyzeImageAsync(imageUrl, features);
            return results;
        }
        public async Task<AnalyzeImageModel> GetImageAnalysisCustomModel(string imageUrl)
        {
           var analyzeImageModel = new AnalyzeImageModel();
            // Creating a list that defines the features to be extracted from the image. 

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };
            ImageAnalysis results = await _computerVisionClient.AnalyzeImageAsync(imageUrl, features);
            // </snippet_analyze_call>

            // <snippet_describe>
            // Sunmarizes the image content.
            foreach (var caption in results.Description.Captions)
            {
                var item = new Summary()
                {
                    Name = caption.Text,
                    Confidence = caption.Confidence
                };
                analyzeImageModel.Summary.Add(item);
            }
            // </snippet_describe>

            // <snippet_categorize>
            // Display categories the image is divided into.
            foreach (var category in results.Categories)
            {
                var item = new Category()
                {
                    Name = category.Name,
                    Confidence = category.Score
                };
                analyzeImageModel.Categories.Add(item);
            }
            // </snippet_categorize>

            // <snippet_tags>
            // Image tags and their confidence score
            foreach (var tag in results.Tags)
            {
                var item = new Tag()
                {
                    Name = tag.Name,
                    Confidence = tag.Confidence
                };
                analyzeImageModel.Tags.Add(item);
            }
            // </snippet_tags>

            // <snippet_objects>
            // Objects
            foreach (var obj in results.Objects)
            {
                var item = new ImageObject()
                {
                    Name = obj.ObjectProperty,
                    Confidence = obj.Confidence,
                    LocationX1= obj.Rectangle.X,
                    LocationX2= obj.Rectangle.X + obj.Rectangle.W,
                    LocationY1= obj.Rectangle.Y,
                    LocationY2= obj.Rectangle.Y + obj.Rectangle.H
                };
                analyzeImageModel.Objects.Add(item);
            }
            // </snippet_objects>

            // <snippet_faces>
            // Faces
            foreach (var face in results.Faces)
            {
                var item = new Face()
                {
                    Gender = face.Gender.HasValue ? face.Gender.Value.ToString() : string.Empty,
                    Age = face.Age,
                    LocationX1 = face.FaceRectangle.Left,
                    LocationX2 = face.FaceRectangle.Left,
                    LocationY1 = face.FaceRectangle.Top + face.FaceRectangle.Width,
                    LocationY2 = face.FaceRectangle.Top + face.FaceRectangle.Height
                };
                analyzeImageModel.Faces.Add(item);
            }
            // </snippet_faces>

            // <snippet_adult>
            // Adult or racy content, if any.
            Console.WriteLine("Adult:");
            if(results.Adult!=null)
            {
                var adult = new Adult()
                {
                    IsAdultContent = results.Adult.IsAdultContent,
                    AdultScore = results.Adult.AdultScore,
                    IsRacyContent = results.Adult.IsRacyContent,
                    RacyScore = results.Adult.RacyScore
                };
                analyzeImageModel.Adult = adult;
            }
            // </snippet_adult>

            // <snippet_brands>
            // Well-known (or custom, if set) brands.
            foreach (var brand in results.Brands)
            {
                var item = new Brand()
                {
                    Name = brand.Name,
                    Confidence = brand.Confidence,
                    LocationX1 = brand.Rectangle.X,
                    LocationX2 = brand.Rectangle.X + brand.Rectangle.W,
                    LocationY1 = brand.Rectangle.Y,
                    LocationY2 = brand.Rectangle.Y + brand.Rectangle.H
                };
                analyzeImageModel.Brands.Add(item);
            }
            // </snippet_brands>

            // <snippet_celebs>
            // Celebrities in image, if any.
            foreach (var category in results.Categories)
            {
                if (category.Detail?.Celebrities != null)
                {
                    foreach (var celeb in category.Detail.Celebrities)
                    {
                        var item = new Celebrity()
                        {
                            Name = celeb.Name,
                            Confidence = celeb.Confidence,
                            LocationX1 = celeb.FaceRectangle.Left,
                            LocationX2 = celeb.FaceRectangle.Top,
                            LocationY1 = celeb.FaceRectangle.Height,
                            LocationY2 = celeb.FaceRectangle.Width
                        };
                        analyzeImageModel.Celebrities.Add(item);
                    }
                }
            }
            // </snippet_celebs>


            // <snippet_landmarks>
            // Popular landmarks in image, if any.
            foreach (var category in results.Categories)
            {
                if (category.Detail?.Landmarks != null)
                {
                    foreach (var landmark in category.Detail.Landmarks)
                    {
                        var item = new Landmark()
                        {
                            Name = landmark.Name,
                            Confidence = landmark.Confidence,
                        };
                        analyzeImageModel.Landmarks.Add(item);
                    }
                }
            }
            // </snippet_landmarks>

            // <snippet_color>
            // Identifies the color scheme.
            if (results.Color != null)
            {
                var color = new ImageColor()
                {
                    IsBWImg = results.Color.IsBWImg,
                    AccentColor = results.Color.AccentColor,
                    DominantColorBackground = results.Color.DominantColorBackground,
                    DominantColorForeground = results.Color.DominantColorForeground,
                    DominantColors = string.Join(",", results.Color.DominantColors)
                };
                analyzeImageModel.Color = color;
            }
            // </snippet_color>

            // <snippet_type>
            // Detects the image types.
            if (results.ImageType != null)
            {
                var imagetype = new Model.ImageType()
                {
                    ClipArtType = results.ImageType.ClipArtType,
                    LineDrawingType = results.ImageType.LineDrawingType
                };
                analyzeImageModel.ImageType = imagetype;
            }
            // </snippet_type>
            return analyzeImageModel;
        }
        /*
         * END - ANALYZE IMAGE - URL IMAGE
         */

        /*
       * ANALYZE IMAGE - LOCAL IMAGE
	     * Analyze local image. Extracts captions, categories, tags, objects, faces, racy/adult content,
	     * brands, celebrities, landmarks, color scheme, and image types.
       */
        public async Task AnalyzeImageLocal(string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - LOCAL IMAGE");
            Console.WriteLine();

            // Creating a list that defines the features to be extracted from the image. 
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
        {
          VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
          VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
          VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
          VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
          VisualFeatureTypes.Objects
        };

            Console.WriteLine($"Analyzing the local image {Path.GetFileName(localImage)}...");
            Console.WriteLine();

            using (Stream analyzeImageStream = File.OpenRead(localImage))
            {
                // Analyze the local image.
                ImageAnalysis results = await _computerVisionClient.AnalyzeImageInStreamAsync(analyzeImageStream);

                // Sunmarizes the image content.
                Console.WriteLine("Summary:");
                foreach (var caption in results.Description.Captions)
                {
                    Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
                }
                Console.WriteLine();

                // Display categories the image is divided into.
                Console.WriteLine("Categories:");
                foreach (var category in results.Categories)
                {
                    Console.WriteLine($"{category.Name} with confidence {category.Score}");
                }
                Console.WriteLine();

                // Image tags and their confidence score
                Console.WriteLine("Tags:");
                foreach (var tag in results.Tags)
                {
                    Console.WriteLine($"{tag.Name} {tag.Confidence}");
                }
                Console.WriteLine();

                // Objects
                Console.WriteLine("Objects:");
                foreach (var obj in results.Objects)
                {
                    Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                      $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
                }
                Console.WriteLine();

                // Detected faces, if any.
                Console.WriteLine("Faces:");
                foreach (var face in results.Faces)
                {
                    Console.WriteLine($"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Left}, {face.FaceRectangle.Top}, " +
                      $"{face.FaceRectangle.Left + face.FaceRectangle.Width}, {face.FaceRectangle.Top + face.FaceRectangle.Height}");
                }
                Console.WriteLine();

                // Adult or racy content, if any.
                Console.WriteLine("Adult:");
                Console.WriteLine($"Has adult content: {results.Adult.IsAdultContent} with confidence {results.Adult.AdultScore}");
                Console.WriteLine($"Has racy content: {results.Adult.IsRacyContent} with confidence {results.Adult.RacyScore}");
                Console.WriteLine();

                // Well-known brands, if any.
                Console.WriteLine("Brands:");
                foreach (var brand in results.Brands)
                {
                    Console.WriteLine($"Logo of {brand.Name} with confidence {brand.Confidence} at location {brand.Rectangle.X}, " +
                      $"{brand.Rectangle.X + brand.Rectangle.W}, {brand.Rectangle.Y}, {brand.Rectangle.Y + brand.Rectangle.H}");
                }
                Console.WriteLine();

                // Celebrities in image, if any.
                Console.WriteLine("Celebrities:");
                foreach (var category in results.Categories)
                {
                    if (category.Detail?.Celebrities != null)
                    {
                        foreach (var celeb in category.Detail.Celebrities)
                        {
                            Console.WriteLine($"{celeb.Name} with confidence {celeb.Confidence} at location {celeb.FaceRectangle.Left}, " +
                              $"{celeb.FaceRectangle.Top},{celeb.FaceRectangle.Height},{celeb.FaceRectangle.Width}");
                        }
                    }
                }
                Console.WriteLine();

                // Popular landmarks in image, if any.
                Console.WriteLine("Landmarks:");
                foreach (var category in results.Categories)
                {
                    if (category.Detail?.Landmarks != null)
                    {
                        foreach (var landmark in category.Detail.Landmarks)
                        {
                            Console.WriteLine($"{landmark.Name} with confidence {landmark.Confidence}");
                        }
                    }
                }
                Console.WriteLine();

                // Identifies the color scheme.
                Console.WriteLine("Color Scheme:");
                Console.WriteLine("Is black and white?: " + results.Color.IsBWImg);
                Console.WriteLine("Accent color: " + results.Color.AccentColor);
                Console.WriteLine("Dominant background color: " + results.Color.DominantColorBackground);
                Console.WriteLine("Dominant foreground color: " + results.Color.DominantColorForeground);
                Console.WriteLine("Dominant colors: " + string.Join(",", results.Color.DominantColors));
                Console.WriteLine();

                // Detects the image types.
                Console.WriteLine("Image Type:");
                Console.WriteLine("Clip Art Type: " + results.ImageType.ClipArtType);
                Console.WriteLine("Line Drawing Type: " + results.ImageType.LineDrawingType);
                Console.WriteLine();
            }
        }
        /*
         * END - ANALYZE IMAGE - LOCAL IMAGE
         */

        /* 
       * DETECT OBJECTS - URL IMAGE
       */
        public async Task DetectObjectsUrl( string urlImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("DETECT OBJECTS - URL IMAGE");
            Console.WriteLine();

            Console.WriteLine($"Detecting objects in URL image {Path.GetFileName(urlImage)}...");
            Console.WriteLine();
            // Detect the objects
            DetectResult detectObjectAnalysis = await _computerVisionClient.DetectObjectsAsync(urlImage);

            // For each detected object in the picture, print out the bounding object detected, confidence of that detection and bounding box within the image
            Console.WriteLine("Detected objects:");
            foreach (var obj in detectObjectAnalysis.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                  $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
            }
            Console.WriteLine();
        }
        /*
         * END - DETECT OBJECTS - URL IMAGE
         */

        /*
         * DETECT OBJECTS - LOCAL IMAGE
         * This is an alternative way to detect objects, instead of doing so through AnalyzeImage.
         */
        public  async Task DetectObjectsLocal( string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("DETECT OBJECTS - LOCAL IMAGE");
            Console.WriteLine();

            using (Stream stream = File.OpenRead(localImage))
            {
                // Make a call to the Computer Vision service using the local file
                DetectResult results = await _computerVisionClient.DetectObjectsInStreamAsync(stream);

                Console.WriteLine($"Detecting objects in local image {Path.GetFileName(localImage)}...");
                Console.WriteLine();

                // For each detected object in the picture, print out the bounding object detected, confidence of that detection and bounding box within the image
                Console.WriteLine("Detected objects:");
                foreach (var obj in results.Objects)
                {
                    Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                      $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
                }
                Console.WriteLine();
            }
        }
        /*
         * END - DETECT OBJECTS - LOCAL IMAGE
         */

        /*
         * DETECT DOMAIN-SPECIFIC CONTENT
         * Recognizes landmarks or celebrities in an image.
         */
        public async Task DetectDomainSpecific( string urlImage, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("DETECT DOMAIN-SPECIFIC CONTENT - URL & LOCAL IMAGE");
            Console.WriteLine();

            // Detect the domain-specific content in a URL image.
            DomainModelResults resultsUrl = await _computerVisionClient.AnalyzeImageByDomainAsync("landmarks", urlImage);
            // Display results.
            Console.WriteLine($"Detecting landmarks in the URL image {Path.GetFileName(urlImage)}...");

            var jsonUrl = JsonConvert.SerializeObject(resultsUrl.Result);
            JObject resultJsonUrl = JObject.Parse(jsonUrl);
            Console.WriteLine($"Landmark detected: {resultJsonUrl["landmarks"][0]["name"]} " +
              $"with confidence {resultJsonUrl["landmarks"][0]["confidence"]}.");
            Console.WriteLine();

            // Detect the domain-specific content in a local image.
            using (Stream imageStream = File.OpenRead(localImage))
            {
                // Change "celebrities" to "landmarks" if that is the domain you are interested in.
                DomainModelResults resultsLocal = await _computerVisionClient.AnalyzeImageByDomainInStreamAsync("celebrities", imageStream);
                Console.WriteLine($"Detecting celebrities in the local image {Path.GetFileName(localImage)}...");
                // Display results.
                var jsonLocal = JsonConvert.SerializeObject(resultsLocal.Result);
                JObject resultJsonLocal = JObject.Parse(jsonLocal);
                Console.WriteLine($"Celebrity detected: {resultJsonLocal["celebrities"][2]["name"]} " +
                  $"with confidence {resultJsonLocal["celebrities"][2]["confidence"]}");

                Console.WriteLine(resultJsonLocal);
            }
            Console.WriteLine();
        }
        /*
         * END - DETECT DOMAIN-SPECIFIC CONTENT
         */

        /*
         * GENERATE THUMBNAIL
         * Taking in a URL and local image, this example will generate a thumbnail image with specified width/height (pixels).
         * The thumbnail will be saved locally.
         */
        public  async Task GenerateThumbnail(string urlImage, string localImage)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("GENERATE THUMBNAIL - URL & LOCAL IMAGE");
            Console.WriteLine();

            // Thumbnails will be saved locally in your bin\Debug\netcoreappx.x\ folder of this project.
            string localSavePath = @".";

            // URL
            Console.WriteLine("Generating thumbnail with URL image...");
            // Setting smartCropping to true enables the image to adjust its aspect ratio 
            // to center on the area of interest in the image. Change the width/height, if desired.
            Stream thumbnailUrl = await _computerVisionClient.GenerateThumbnailAsync(60, 60, urlImage, true);

            string imageNameUrl = Path.GetFileName(urlImage);
            string thumbnailFilePathUrl = Path.Combine(localSavePath, imageNameUrl.Insert(imageNameUrl.Length - 4, "_thumb"));

            Console.WriteLine("Saving thumbnail from URL image to " + thumbnailFilePathUrl);
            using (Stream file = File.Create(thumbnailFilePathUrl)) { thumbnailUrl.CopyTo(file); }

            Console.WriteLine();

            // LOCAL
            Console.WriteLine("Generating thumbnail with local image...");

            using (Stream imageStream = File.OpenRead(localImage))
            {
                Stream thumbnailLocal = await _computerVisionClient.GenerateThumbnailInStreamAsync(100, 100, imageStream, smartCropping: true);

                string imageNameLocal = Path.GetFileName(localImage);
                string thumbnailFilePathLocal = Path.Combine(localSavePath,
                        imageNameLocal.Insert(imageNameLocal.Length - 4, "_thumb"));
                // Save to file
                Console.WriteLine("Saving thumbnail from local image to " + thumbnailFilePathLocal);
                using (Stream file = File.Create(thumbnailFilePathLocal)) { thumbnailLocal.CopyTo(file); }
            }
            Console.WriteLine();
        }
        /*
         * END - GENERATE THUMBNAIL
         */

        // <snippet_read_url>
        /*
         * READ FILE - URL 
         * Extracts text. 
         */
        public  async Task ReadFileUrl(string urlFile)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("READ FILE FROM URL");
            Console.WriteLine();

            // Read text from URL
            var textHeaders = await _computerVisionClient.ReadAsync(urlFile, language: "en");
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);
            // </snippet_extract_call>

            // <snippet_read_response>
            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            Console.WriteLine($"Extracting text from URL file {Path.GetFileName(urlFile)}...");
            Console.WriteLine();
            do
            {
                results = await _computerVisionClient.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));
            // </snippet_read_response>

            // <snippet_read_display>
            // Display the found text.
            Console.WriteLine();
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    Console.WriteLine(line.Text);
                }
            }
            // </snippet_read_display>
            Console.WriteLine();
        }


        // </snippet_read_url>
        /*
         * END - READ FILE - URL
         */


        // <snippet_read_local>
        /*
         * READ FILE - LOCAL
         */

        public  async Task ReadFileLocal(string localFile)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("READ FILE FROM LOCAL");
            Console.WriteLine();

            // Read text from URL
            var textHeaders = await _computerVisionClient.ReadInStreamAsync(File.OpenRead(localFile), language: "en");
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);
            // </snippet_extract_call>

            // <snippet_extract_response>
            // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            Console.WriteLine($"Reading text from local file {Path.GetFileName(localFile)}...");
            Console.WriteLine();
            do
            {
                results = await _computerVisionClient.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));
            // </snippet_extract_response>

            // <snippet_extract_display>
            // Display the found text.
            Console.WriteLine();
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    Console.WriteLine(line.Text);
                }
            }
            Console.WriteLine();
        }
        /*
         * END - READ FILE - LOCAL
         */
    }
}
