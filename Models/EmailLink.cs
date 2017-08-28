using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiCampusConcierge.Models
{
    public class EmailLink
    {
        public string userName { get; set; }
        public string email_id { get; set; }
        public string password { get; set; }
        public string university { get; set; }
        public string university_icon { get; set; }
    }

    public class LoginData
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string pass_encrpted_flag { get; set; }
        public string university_id { get; set; }
        public string dives { get; set; }
    }
}