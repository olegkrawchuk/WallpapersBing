using System;
using System.Collections.Generic;
using System.Text;

namespace WallpapersBing.Model
{
    public class Image
    {
        public string startdate { get; set; }
        public string fullstartdate { get; set; }
        public string enddate { get; set; }
        public string url { get; set; }
        public string urlBase { get; set; }
        public string copyright { get; set; }
        public string copyrightlink { get; set; }
        public int drk { get; set; }
        public int top { get; set; }
        public int bot { get; set; }
    }
}
