using System;
using System.Collections.Generic;
using System.Text;

namespace DJBlogs.ComputerVision.Model
{
    public class Adult
    {
        public bool IsAdultContent { get; set; }
        public double AdultScore { get; set; }
        public bool IsRacyContent { get; set; }
        public double RacyScore { get; set; }
    }
}
