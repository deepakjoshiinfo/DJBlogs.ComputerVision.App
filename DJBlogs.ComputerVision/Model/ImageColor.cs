using System;
using System.Collections.Generic;
using System.Text;

namespace DJBlogs.ComputerVision.Model
{
    public class ImageColor
    {
        public bool IsBWImg { get; set; }
        public string AccentColor { get; set; }
        public string DominantColorBackground { get; set; }
        public string DominantColorForeground { get; set; }
        public string DominantColors { get; set; }
    }
}
