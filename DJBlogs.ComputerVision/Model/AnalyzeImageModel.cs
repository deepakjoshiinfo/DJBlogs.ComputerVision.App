using System;
using System.Collections.Generic;
using System.Text;

namespace DJBlogs.ComputerVision.Model
{
    public class AnalyzeImageModel
    {
        public AnalyzeImageModel()
        {
            Summary = new List<Summary>();
            Categories = new List<Category>();
            Tags = new List<Tag>();
            Objects = new List<ImageObject>();
            Faces = new List<Face>();
            Brands = new List<Brand>();
            Celebrities = new List<Celebrity>();
            Landmarks = new List<Landmark>();
        }
        public List<Summary> Summary { get; set; }
        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }
        public List<ImageObject> Objects { get; set; }
        public List<Face> Faces { get; set; }
        public Adult Adult { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Celebrity> Celebrities { get; set; }
        public List<Landmark> Landmarks { get; set; }
        public ImageColor Color { get; set; }
        public ImageType ImageType { get; set; }
    }
}
