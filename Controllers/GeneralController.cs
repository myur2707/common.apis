using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Net.Mail;
using Newtonsoft.Json.Linq;
using WebApiCampusConcierge.Models;
using System.Configuration;
using WebApiCampusConcierge.XSD;
using System.Web;
using BLL.Utilities;
using BLL.Master;
using System.Data;
using System.Web.Script.Serialization;
using System.Text;
using System.Web.SessionState;
namespace WebApiCampusConcierge.Controllers
{
    public class GeneralController : ApiController
    { 
        
        
        #region variable Declaration
        Masters objmasters = new Masters();
        DS_MemberTables objDS_MemberTables = new DS_MemberTables();
        #endregion

        // GET api/general
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/general/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/general
        public void Post([FromBody]string value)
        {
        }

        // PUT api/general/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/general/5
        public void Delete(int id)
        {
        }
       
        [HttpPost]
        public string Logout([FromBody]JObject parameter)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            DataTable dt = new DataTable();
            string result = "";
            string tok = parameter["token_id"].ToString();
            dt = objmasters.Gettokenuserinfo(tok);
            if (dt != null)
            {
                DataRow dr = dt.Rows[0];
                dr["token_id"] = '0';
                objDS_MemberTables.login_token.ImportRow(dr);
                objBLReturnObject = objmasters.logoutlink(objDS_MemberTables, true);
                  if (objBLReturnObject.ExecutionStatus == 1)
                  {
                      return "pass";
                  }
                  else {

                      return "fail";
                  }
            }
            return "fail";
          
        }


    }
}
