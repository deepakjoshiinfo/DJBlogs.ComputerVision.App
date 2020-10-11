using System;
using System.Collections.Generic;
using System.Text;

namespace DJBlogs.ComputerVision.Model
{
    public class Celebrity
    {
        public string Name { get; set; }
        public double Confidence { get; set; }
        public int LocationX1 { get; set; }
        public int LocationX2 { get; set; }
        public int LocationY1 { get; set; }
        public int LocationY2 { get; set; }
    }
}
