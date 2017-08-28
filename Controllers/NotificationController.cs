using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using BLL.Utilities;
using BLL.Master;
using WebApiCampusConcierge.Models;
using WebApiCampusConcierge.XSD;
using Newtonsoft.Json;
using System.Web;
using System.IO;
using System.Configuration;
using System.Net.Mail;
using Newtonsoft.Json.Linq;
using Stripe;
using Stripe.Infrastructure;
namespace WebApiCampusConcierge.Controllers
{
    public class NotificationController : ApiController
    {
        DS_MemberTables objDS_MemberTables = new DS_MemberTables();
        // GET api/notification
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/notification/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/notification
        [HttpPost]
        public void Post([FromBody]string id)
        {

        }

        // PUT api/notification/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/notification/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        public String RegisterDeviceAndroid([FromBody] JObject parameter)
        {
            //ResponseObjectInfo objResponseObjectInfo = new ResponseObjectInfo();
            BLReturnObject objBLReturnObject = new BLReturnObject();
            String AppName = parameter["AppName"].ToString();
            String RepId = parameter["RepId"].ToString();
            String DeviceId = parameter["DeviceId"].ToString();
            String TokenId = parameter["TokenId"].ToString();
            String DeviceInfo = parameter["DeviceInfo"].ToString();
            String OS = parameter["OS"].ToString();
            String IMEINo = parameter["IMEINo"].ToString();
            try
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "RegisterDeviceAndroid(" + Convert.ToString(AppName) + ", " + Convert.ToString(RepId) + ", " + Convert.ToString(DeviceId) + ", " + Convert.ToString(TokenId) + ", " + Convert.ToString(DeviceInfo) + ", " + Convert.ToString(OS) + ", " + Convert.ToString(IMEINo) + ")");
                // ServerLog.ExceptionLog("RegisterDeviceAndroid(" + Convert.ToString(AppName) + ", " + Convert.ToString(RepId) + ", " + Convert.ToString(DeviceId) + ", " + Convert.ToString(TokenId) + ", " + Convert.ToString(DeviceInfo) + ", " + Convert.ToString(OS) + ", " + Convert.ToString(IMEINo) + ")");
                if (AppName == null || AppName.Trim() == String.Empty)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Please supplied AppName.";
                    objBLReturnObject.dt_ReturnedTables = null;
                }
                else if (RepId == null || RepId.Trim() == String.Empty)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Please supplied RepId.";
                    objBLReturnObject.dt_ReturnedTables = null;
                }
                else if (DeviceId == null || DeviceId.Trim() == String.Empty)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Please supplied DeviceId.";
                    objBLReturnObject.dt_ReturnedTables = null;
                }
                else if (TokenId == null || TokenId.Trim() == String.Empty)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Please supplied TokenId.";
                    objBLReturnObject.dt_ReturnedTables = null;
                }
                else if (OS == null || OS.Trim() == String.Empty)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Please supplied OS.";
                    objBLReturnObject.dt_ReturnedTables = null;
                }
                else if (IMEINo == null || IMEINo.Trim() == String.Empty)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Please supplied IMEINo.";
                    objBLReturnObject.dt_ReturnedTables = null;
                }
                else
                {
                    String Message = String.Empty;
                    Masters objmasters = new Masters();
                    Byte result = objmasters.AddDeviceInfo(AppName, RepId, DeviceId, TokenId, DeviceInfo, OS, IMEINo, ref Message);
                    objBLReturnObject.ExecutionStatus = result;
                    objBLReturnObject.ServerMessage = Message;
                    objBLReturnObject.dt_ReturnedTables = null;
                }
            }
            catch (Exception ex)
            {
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = ex.Message;
                objBLReturnObject.dt_ReturnedTables = null;
                // ServerLog.ExceptionLog("RegisterDeviceAndroid(" + Convert.ToString(AppName) + ", " + Convert.ToString(RepId) + ", " + Convert.ToString(DeviceId) + ", " + Convert.ToString(TokenId) + ", " + Convert.ToString(DeviceInfo) + ", " + Convert.ToString(OS) + ")");
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);
            }
            return JsonConvert.SerializeObject(objBLReturnObject);
        }

        [HttpPost]
        public String unregistr([FromBody] JObject parameter)
        {

            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            String AppName = parameter["AppName"].ToString();
            String DeviceId = parameter["DeviceId"].ToString();
            BLReturnObject objBLReturnObject = new BLReturnObject();
            String Message = String.Empty;
            Masters objmasters = new Masters();
            Byte result = objmasters.RemoveDeviceInfo(AppName, DeviceId, ref Message);
            objBLReturnObject.ExecutionStatus = result;
            objBLReturnObject.ServerMessage = Message;
            objBLReturnObject.dt_ReturnedTables = null;

            return JsonConvert.SerializeObject(objBLReturnObject);
        }

        [HttpPost]
        public String UnreadCount([FromBody] JObject parameter)
        {
            String AppName = parameter["AppName"].ToString();
            String RepId = parameter["RepId"].ToString();
            String DeviceId = parameter["DeviceId"].ToString();
            int UnreadCount = Convert.ToInt16(parameter["UnreadCount"].ToString());

            //ResponseObjectInfo objResponseObjectInfo = new ResponseObjectInfo();

            BLReturnObject objBLReturnObject = new BLReturnObject();
            String Message = String.Empty;
            Masters objmasters = new Masters();
            try
            {
                //ServerLog.MgmtExceptionLog("UnreadCount(" + Convert.ToString(AppName) + ", " + Convert.ToString(RepId) + ", " + Convert.ToString(DeviceId) + ", " + Convert.ToString(UnreadCount) + ")");
                if (AppName == null || AppName.Trim() == String.Empty)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Please supplied AppName.";
                    objBLReturnObject.dt_ReturnedTables = null;
                }
                else if (RepId == null || RepId.Trim() == String.Empty)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Please supplied RepId.";
                    objBLReturnObject.dt_ReturnedTables = null;
                }
                else if (DeviceId == null || DeviceId.Trim() == String.Empty)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Please supplied DeviceId.";
                    objBLReturnObject.dt_ReturnedTables = null;
                }
                else if (UnreadCount < 0)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Please supplied Valid UnreadCount.";
                    objBLReturnObject.dt_ReturnedTables = null;
                }
                else
                {

                    Byte result = objmasters.UpdateUnreadCount(AppName, RepId, DeviceId, UnreadCount, ref Message);
                    objBLReturnObject.ExecutionStatus = result;
                    objBLReturnObject.ServerMessage = Message;
                    objBLReturnObject.dt_ReturnedTables = null;
                }
            }
            catch (Exception ex)
            {
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = ex.Message;
                objBLReturnObject.dt_ReturnedTables = null;
                //    ServerLog.MgmtExceptionLog("UnreadCount(" + Convert.ToString(AppName) + ", " + Convert.ToString(RepId) + ", " + Convert.ToString(DeviceId) + ", " + Convert.ToString(UnreadCount) + ")");
                //    ServerLog.MgmtExceptionLog(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return JsonConvert.SerializeObject(objBLReturnObject);


        }
    }
}
