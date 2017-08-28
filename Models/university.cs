using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiCampusConcierge.Models
{
    public class university
    {
        public string university_id { get; set; }
        public string university_name { get; set; }
        public string country_name { get; set; }
        public string university_icon { get; set; }
        public string background_image { get; set; }
        public string theme_colour { get; set; }
        public string location { get; set; }
        public string website_link { get; set; }
        public string is_active { get; set; }
        

    }

    public class university_id
    {
        public string uni_id { get; set; }
        public string member_id { get; set; }
        
    }
}