using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiCampusConcierge.Models
{
    public class UpdateForEmailVerification
    {
        public string QS_emailId { get; set; }
        public string QS_randomNo { get; set; }
    }
}