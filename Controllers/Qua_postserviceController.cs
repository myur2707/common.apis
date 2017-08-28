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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WebApiCampusConcierge.Controllers
{
    public class Qua_postserviceController : ApiController
    {
        #region variable
        Masters objmasters = new Masters();
        DS_General objgeneral = new DS_General();
        // public static double post_charge = 0.10; // doller
        //public static double seller_charge = 1.5; // doller
        //public static double buy_charge = 1.5; // doller
        public static string mycc_id = "mycc@gmail.com";
        //public static double barter_charge = 0.15;
        DS_Admin objDS_Admin = new DS_Admin();
        DS_Transtration objDs_trastration = new DS_Transtration();
        #endregion
        // GET api/qua_postservice
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/qua_postservice/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/qua_postservice
        public void Post([FromBody]string value)
        {
        }

        // PUT api/qua_postservice/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/qua_postservice/5
        public void Delete(int id)
        {
        }

        #region get method

        [HttpPost]
        public string get_itemwise_barter([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";
            try
            {
                if (parameter["member_id"].ToString() == "" || parameter["member_id"].ToString() == null)
                {
                    return BLGeneralUtil.return_ajax_string("0", "Order id not found");
                }
                if (parameter["item_id"].ToString() == "" || parameter["item_id"].ToString() == null)
                {
                    return BLGeneralUtil.return_ajax_string("0", "Item id not found");
                }
                dt = objmasters.getbarter_wise(parameter["item_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "No data found");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", "some thing went wrong");
            }

            return BLGeneralUtil.return_ajax_data("1", result);
        }

        [HttpPost]
        public string get_itemwise_order([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";
            try
            {
                if (parameter["member_id"].ToString() == "" || parameter["member_id"].ToString() == null)
                {
                    return BLGeneralUtil.return_ajax_string("0", "Order id not found");
                }
                if (parameter["item_id"].ToString() == "" || parameter["item_id"].ToString() == null)
                {
                    return BLGeneralUtil.return_ajax_string("0", "Item id not found");
                }
                dt = objmasters.getorder_wise(parameter["item_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "No data found");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", "some thing went wrong");
            }

            return BLGeneralUtil.return_ajax_data("1", result);
        }

        [HttpPost]
        public string get_my_barter([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";
            try
            {
                if (parameter["member_id"].ToString() == "" || parameter["member_id"].ToString() == null)
                {
                    return BLGeneralUtil.return_ajax_string("0", "order id not found");
                }
                dt = objmasters.getbarter(parameter["member_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "You have not made any barter offers yet");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", "some thing went wrong");
            }

            return BLGeneralUtil.return_ajax_data("1", result);
        }

        [HttpPost]
        public string get_myorder([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";
            try
            {
                if (parameter["member_id"].ToString() == "" || parameter["member_id"].ToString() == null)
                {
                    return BLGeneralUtil.return_ajax_string("0", "order id not found");
                }
                dt = objmasters.my_order_list(parameter["member_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "You have not made any orders yet");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }

            return BLGeneralUtil.return_ajax_data("1", result);
        }

        [HttpPost]
        public string get_buy_info([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";
            try
            {
                if (parameter["order_id"].ToString() == "" || parameter["order_id"].ToString() == null)
                {
                    return BLGeneralUtil.return_ajax_string("0", "order id not found");
                }
                dt = objmasters.getbuyerinfo(parameter["order_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "no data found");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }

            return BLGeneralUtil.return_ajax_data("1", result);
        }

        [HttpPost]
        public string get_mypost([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";
            try
            {
                dt = objmasters.mypost(parameter["member_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "  You have not posted any listings yet ");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }

            return BLGeneralUtil.return_ajax_data("1", result);
        }


        [HttpPost]
        public string Get_Graph([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";
            try
            {
                dt = objmasters.Gharph_QUA(parameter["member_id"].ToString(), parameter["year"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "no data found");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }

            return BLGeneralUtil.return_ajax_data("1", result);
        }


        [HttpPost]
        public string get_message([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";
            if (parameter["member_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member id not  found");
            }
            try
            {
                dt = objmasters.message(parameter["member_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "You have no new notifications");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }

            return BLGeneralUtil.return_ajax_data("1", result);
        }


        [HttpPost]
        public string get_my_order([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";
            if (parameter["member_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member id not  found");
            }
            try
            {
                dt = objmasters.myorder(parameter["member_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "no data found");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }

            return BLGeneralUtil.return_ajax_data("1", result);
        }


        [HttpPost]
        public string get_my_purchase([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";
            if (parameter["member_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member id not  found");
            }
            try
            {
                dt = objmasters.My_purchase(parameter["member_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "You have not made any purchases");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }

            return BLGeneralUtil.return_ajax_data("1", result);
        }


        [HttpPost]
        public string Get_categories()
        {
            DataTable dt = new DataTable();
            string result = "";
            dt = objmasters.getcategories();
            if (dt != null)
            {
                result = GetJson1(dt);
            }
            return BLGeneralUtil.return_ajax_data("1", result);
        }


        [HttpPost]
        public string get_itemlist([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";
            try
            {
                if (parameter["categories_id"].ToString() == "")
                {

                    return BLGeneralUtil.return_ajax_string("0", "categories  data found");
                }
                if (parameter["Sr_no"].ToString() == "")
                {
                    return BLGeneralUtil.return_ajax_string("0", "sr no data found");

                }
                if (parameter["member_id"].ToString() == "")
                {
                    return BLGeneralUtil.return_ajax_string("0", "member found");


                }
                DataTable dt_user = objmasters.getuserinfo(parameter["member_id"].ToString());
                dt = objmasters.getitem_list(parameter["categories_id"].ToString(), parameter["Sr_no"].ToString(), parameter["member_id"].ToString(), dt_user.Rows[0]["university"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "No item available in this Category");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }

            return BLGeneralUtil.return_ajax_data("1", result);
        }


        [HttpPost]
        public string get_wishlist([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member  id not found");
            }
            try
            {
                dt = objmasters.getwishlist(parameter["member_id"].ToString());
                if (dt != null)
                {




                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "Your wishlist is empty ");

                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            return BLGeneralUtil.return_ajax_data("1", result);
        }


        [HttpPost]
        public string getfeedbackcount_rating([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member  id not found");
            }
            try
            {
                dt = objmasters.feedback_count_rating(parameter["member_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            return BLGeneralUtil.return_ajax_data("1", result);
        }


        [HttpPost]
        public string getfeedback([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member  id not found");
            }
            try
            {
                dt = objmasters.feedback(parameter["member_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "No reviews available");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            return BLGeneralUtil.return_ajax_data("1", result);
        }

        [HttpPost]
        public string get_sales_list([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member  id not found");
            }
            try
            {
                dt = objmasters.my_sales(parameter["member_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", " You have not made any sales ");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            return BLGeneralUtil.return_ajax_data("1", result);
        }

        [HttpPost]
        public string Get_AdvanceSearchNameList([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member  id not found");
            }
            try
            {
                DataTable dt_userinfo = objmasters.Getuniversityid(parameter["member_id"].ToString());
                if (dt_userinfo == null)
                {
                    return BLGeneralUtil.return_ajax_string("0", "member not found");
                }
                dt = objmasters.advanceSearch(parameter["text"].ToString(), dt_userinfo.Rows[0]["university"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "Sorry No Search Results Found");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            return BLGeneralUtil.return_ajax_data("1", result);
        }


        [HttpPost]
        public string Get_AdvanceSearchNamelist_data([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";
            DataTable dtuser = new DataTable();
            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member  id not found");
            }
            if (parameter["text"].ToString() == null || parameter["text"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member  id not found");
            }

            try
            {
                dtuser = objmasters.Getuniversityid(parameter["member_id"].ToString());
                if (dtuser == null)
                {
                    return BLGeneralUtil.return_ajax_string("0", "member not found");
                }
                dt = objmasters.advanceSearch_data_list(parameter["text"].ToString(), parameter["categories_id"].ToString(), parameter["sr_no"].ToString(), parameter["member_id"].ToString(), dtuser.Rows[0]["university"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "Sorry No Search Results Found");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            return BLGeneralUtil.return_ajax_data("1", result);
        }

        [HttpPost]
        public string GetMultipleImage([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member  id not found");
            }
            try
            {
                dt = objmasters.getmultipleImage(parameter["item_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "Data not found");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            return BLGeneralUtil.return_ajax_data("1", result);
        }

        [HttpPost]
        public string getproductDetail([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member  id not found");
            }
            try
            {
                dt = objmasters.getitemdetailforchat(parameter["member_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "Data not found");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            return BLGeneralUtil.return_ajax_data("1", result);
        }

        [HttpPost]
        public string getproducnameAndImage([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dt = new DataTable();
            string result = "";

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member  id not found");
            }
            if (parameter["product_id"].ToString() == null || parameter["product_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "member  id not found");
            }
            try
            {
                dt = objmasters.getUserImageAndproductName(parameter["member_id"].ToString(), parameter["product_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "Data not found");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            return BLGeneralUtil.return_ajax_data("1", result);
        }


        #endregion

        #region post mathod save


        public string save_poststuff(string name, string desc, string cat_id, string price, string memberid, string smallpath, string bigpath)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;
            double post_charge = 0.0;
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + name + "" + desc + "status" + "call");

            try
            {

                DataTable DT_UserInfo = objmasters.getuserdetai(memberid);
                DataTable DT_Price = objmasters.GetQUAPriceMaster();
                if (DT_Price.Rows.Count > 0)
                {
                    if (DT_Price.Rows[0]["Code"].ToString() == "post_charge")
                    {
                        post_charge = Convert.ToDouble(DT_Price.Rows[0]["Value"].ToString());
                    }
                }
                if (DT_UserInfo.Rows.Count > 0)
                {

                    DS_General.QUA_posting_masterRow row_post = objgeneral.QUA_posting_master.NewQUA_posting_masterRow();

                    row_post.item_id = "1";
                    row_post.Name_title = name;
                    row_post.Description = desc;
                    row_post.categories_id = cat_id;
                    row_post.Price = price;
                    row_post.Small_image_path = objgeneral.QUA_image_parameter.Rows[0]["original_image_path"].ToString();
                    row_post.Big_name_path = objgeneral.QUA_image_parameter.Rows[0]["thumbnail_image_path"].ToString();
                    row_post.University_id = DT_UserInfo.Rows[0]["university"].ToString();
                    row_post.Member_id = memberid;
                    row_post.Date_of_post = System.DateTime.Now;
                    row_post.status = "A";  //status is P and S
                    row_post.is_active = "Y";
                    row_post.created_by = memberid;
                    row_post.created_date = System.DateTime.Now;
                    row_post.created_host = HttpContext.Current.Request.UserHostName;

                    objgeneral.QUA_posting_master.AddQUA_posting_masterRow(row_post);

                }
                else
                {
                    return "0";
                }
                //transation
                /*
                 below code will have stop to some time system will not deduct any 
                 posting charge. 
                 * so i put some if condition will no calculte post charge
                
                 */
                #region post charge 0.10

                DataTable Dt_PerameterMaster = objmasters.ParameterMaster();

                if (Dt_PerameterMaster.Rows[1]["Value"].ToString() == "Y")
                {
                    DataTable dt_member = objmasters.gettokenbal(memberid);
                    DataTable dt_mycc = objmasters.gettokenbal(mycc_id);
                    if (dt_member == null || dt_member.Rows.Count < 0)
                    {
                        return "0";
                    }
                    if (Convert.ToDecimal(dt_member.Rows[0]["balance_token"].ToString()) > (Convert.ToDecimal(post_charge)))
                    {


                    }
                    else
                    {
                        return "3";
                    }
                    //deducted member acount
                    #region member acount
                    #region student excharge
                    dt_member.Rows[0]["total_debit"] = (Convert.ToDecimal(dt_member.Rows[0]["total_debit"].ToString()) + (Convert.ToDecimal(post_charge)));
                    dt_member.Rows[0]["balance_token"] = (Convert.ToDecimal(dt_member.Rows[0]["balance_token"].ToString()) - (Convert.ToDecimal(post_charge)));

                    objDs_trastration.fn_token_balance.ImportRow(dt_member.Rows[0]);

                    DS_Transtration.fn_token_transtionRow token_transtration_row4 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                    token_transtration_row4.doc_no = "1";
                    token_transtration_row4.doc_date = System.DateTime.Now;
                    token_transtration_row4.ref_transtion_type = "post_charge";
                    token_transtration_row4.ref_transtion_doc_date = System.DateTime.Now;
                    token_transtration_row4.ref_transtion_doc_no = "1";
                    token_transtration_row4.type_credit_debit = "debit";
                    token_transtration_row4.amount = (Convert.ToDecimal(post_charge));
                    token_transtration_row4.balance_after_transtion = Convert.ToDecimal(dt_member.Rows[0]["balance_token"].ToString());
                    token_transtration_row4.member_id = memberid;
                    token_transtration_row4.created_by = "system";
                    token_transtration_row4.created_date = System.DateTime.Now;
                    token_transtration_row4.created_host = HttpContext.Current.Request.UserHostName;

                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row4);

                    #endregion

                    #endregion
                    #region mycc acount
                    dt_mycc.Rows[0]["total_credit"] = (Convert.ToDecimal(dt_mycc.Rows[0]["total_credit"].ToString()) + (Convert.ToDecimal(post_charge)));
                    dt_mycc.Rows[0]["balance_token"] = (Convert.ToDecimal(dt_mycc.Rows[0]["balance_token"].ToString()) + (Convert.ToDecimal(post_charge)));

                    objDs_trastration.fn_token_balance.ImportRow(dt_mycc.Rows[0]);

                    DS_Transtration.fn_token_transtionRow token_transtration_row5 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                    token_transtration_row5.doc_no = "2";
                    token_transtration_row5.doc_date = System.DateTime.Now;
                    token_transtration_row5.ref_transtion_type = "post_charge";
                    token_transtration_row5.ref_transtion_doc_date = System.DateTime.Now;
                    token_transtration_row5.ref_transtion_doc_no = "1";
                    token_transtration_row5.type_credit_debit = "Credit";
                    token_transtration_row5.amount = (Convert.ToDecimal(post_charge));
                    token_transtration_row5.balance_after_transtion = Convert.ToDecimal(dt_mycc.Rows[0]["balance_token"].ToString());
                    token_transtration_row5.member_id = mycc_id;
                    token_transtration_row5.created_by = "system";
                    token_transtration_row5.created_date = System.DateTime.Now;
                    token_transtration_row5.created_host = HttpContext.Current.Request.UserHostName;

                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row5);

                    #endregion
                }

                #endregion

                objBLReturnObject = objmasters.save_poststaff(objgeneral, objDs_trastration);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    flag = true;

                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not " + "status" + ex.StackTrace);
                return "0";
            }
            if (flag == true)
            {

                return "1";
            }
            else
            {
                return "0";
            }
        }


        public string save_barter(string name, string desc, string item_id, string buy_id, string memberid, string smallpath, string bigpath)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + name + "" + desc + "status" + "call");
            try
            {
                #region bartter
                DataTable dt_item = objmasters.getitembyid(item_id);
                DS_General.QUA_barter_masterRow row_post = objgeneral.QUA_barter_master.NewQUA_barter_masterRow();

                row_post.Barter_id = "1";
                row_post.Item_id = item_id;
                row_post.title = name;
                row_post.Description = desc;
                row_post.Buyer_Member_id = buy_id;

                row_post.smalI_image_path = smallpath;
                row_post.Big_image_path = bigpath;


                row_post.is_accepted = "P";  //status is P and S  p is peending
                row_post.is_active = "Y";
                row_post.created_by = memberid;
                row_post.created_date = System.DateTime.Now;
                row_post.created_host = HttpContext.Current.Request.UserHostName;

                objgeneral.QUA_barter_master.AddQUA_barter_masterRow(row_post);

                #endregion

                #region notification

                #region create notification

                DS_General.QUA_notificationRow new_row = objgeneral.QUA_notification.NewQUA_notificationRow();
                new_row.doc_no = "1";
                new_row.from_member = memberid;
                new_row.to_member = dt_item.Rows[0]["member_id"].ToString();
                new_row.notification_date = System.DateTime.Now;
                new_row._event = "Barter";
                new_row.template = "You have a new barter request  for " + dt_item.Rows[0]["Name_Title"].ToString();
                new_row.item_id = item_id;
                new_row.is_read = "N";
                new_row.is_accept = "N";
                new_row.is_active = "Y";
                new_row.created_by = memberid;
                new_row.created_date = System.DateTime.Now;
                new_row.created_host = HttpContext.Current.Request.UserHostName;

                objgeneral.QUA_notification.AddQUA_notificationRow(new_row);

                #endregion end notification
                #endregion
                #region alert message
                DS_General.alertmessageRow alert_row1 = objgeneral.alertmessage.NewalertmessageRow();

                alert_row1.Doc_no = "101";


                alert_row1.to_member = dt_item.Rows[0]["member_id"].ToString();
                //   alert_row.to_member_role = "ST";
                alert_row1._event = "Request";
                alert_row1.template = " You have a new barter request for " + dt_item.Rows[0]["Name_Title"].ToString();


                alert_row1.notification_date = System.DateTime.Now;

                alert_row1.is_read = "N";
                alert_row1.is_active = "Y";
                alert_row1.AppName = "QUA";

                alert_row1.senddate = System.DateTime.Now;
                alert_row1.utc_time = System.DateTime.UtcNow.TimeOfDay;
                alert_row1.created_by = "system";
                alert_row1.created_date = System.DateTime.Now;
                alert_row1.created_host = HttpContext.Current.Request.UserHostName;

                objgeneral.alertmessage.AddalertmessageRow(alert_row1);
                #endregion alert message

                #region item master

                DataTable item_master = objmasters.getitemlist(item_id);
                if (item_master.Rows.Count > 0)
                {
                    if (item_master.Rows[0]["type"] == "sales")
                    {
                        item_master.Rows[0]["status"] = "B";

                    }
                    else
                    {

                        item_master.Rows[0]["status"] = "A";
                    }
                    objgeneral.QUA_posting_master.ImportRow(item_master.Rows[0]);
                }
                #endregion
                objBLReturnObject = objmasters.save_barter(objgeneral);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    flag = true;

                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not " + "status" + ex.StackTrace);
                return "0";
            }
            if (flag == true)
            {

                return "1";
            }
            else
            {
                return "0";
            }
        }




        //jaimin email templet
        #region email template
        [HttpPost]

        public string email_template([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);


            if (parameter["email_id"].ToString() == null || parameter["email_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "member id not found"); }
            try
            {


                DS_General.email_templateRow row = objgeneral.email_template.Newemail_templateRow();

                row.sr_no = "1";
                row.Email = parameter["email_id"].ToString();
                objgeneral.email_template.Addemail_templateRow(row);


                objBLReturnObject = objmasters.email_template(objgeneral, flag);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    flag = true;

                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not " + "status" + ex.StackTrace);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            if (flag == true)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not" + "status" + "1");
                return BLGeneralUtil.return_ajax_string("1", "save");
            }
            else
            {
                return BLGeneralUtil.return_ajax_string("0", "try again");
            }
        }
        //end 
        #endregion


        [HttpPost]
        public string Add_wishlist([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            if (parameter["item_id"].ToString() == null || parameter["item_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "item id not found"); }

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "member id not found"); }
            try
            {

                DataTable dt_list = objmasters.getcheckwishtlist(parameter["member_id"].ToString(), parameter["item_id"].ToString());
                if (parameter["flag"].ToString() == "false")
                {
                    if (dt_list != null && dt_list.Rows.Count > 0) { return BLGeneralUtil.return_ajax_string("0", "Item already added"); }
                    DS_General.QUA_wish_listRow row_post = objgeneral.QUA_wish_list.NewQUA_wish_listRow();


                    row_post.Doc_id = "1";
                    row_post.Item_id = parameter["item_id"].ToString();
                    row_post.Member_id = parameter["member_id"].ToString();
                    row_post.is_active = "Y";
                    row_post.created_by = parameter["member_id"].ToString(); ;
                    row_post.created_date = System.DateTime.Now;
                    row_post.created_host = HttpContext.Current.Request.UserHostName;

                    objgeneral.QUA_wish_list.AddQUA_wish_listRow(row_post);

                    flag = false;
                }
                else
                {

                    dt_list.Rows[0]["is_active"] = "N";
                    objgeneral.QUA_wish_list.ImportRow(dt_list.Rows[0]);
                    flag = true;
                }

                objBLReturnObject = objmasters.add_list(objgeneral, flag);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    flag = true;

                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not " + "status" + ex.StackTrace);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            if (flag == true)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not" + "status" + "1");
                if (parameter["flag"].ToString() == "false")
                {
                    return BLGeneralUtil.return_ajax_string("1", "This item has been successfully added to your wishlist");
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("1", "Item has been removed successfully");
                }
            }
            else
            {
                return BLGeneralUtil.return_ajax_string("0", "some thing went wrong");
            }
        }

        [HttpPost]
        public string buy_product([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            if (parameter["item_id"].ToString() == null || parameter["item_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "item id not found"); }

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "member id not found"); }
            try
            {

                DataTable dt_list = objmasters.getitemlist(parameter["item_id"].ToString());


                if (dt_list != null)
                {
                    if (dt_list.Rows[0]["status"] == "S")
                    {
                        return BLGeneralUtil.return_ajax_string("1", "Item Already Sold ");
                    }

                }
                if (dt_list != null)
                {
                    //first time buy that time flag is Reqested 
                    if (dt_list.Rows[0]["type"] == "sales")
                    {
                        dt_list.Rows[0]["status"] = "R";
                        dt_list.Rows[0]["last_modified_by"] = parameter["member_id"].ToString();
                        dt_list.Rows[0]["last_modified_date"] = System.DateTime.Now;
                        objgeneral.QUA_posting_master.ImportRow(dt_list.Rows[0]);
                    }
                    else
                    {

                        dt_list.Rows[0]["status"] = "R"; //service request
                        dt_list.Rows[0]["last_modified_by"] = parameter["member_id"].ToString();
                        dt_list.Rows[0]["last_modified_date"] = System.DateTime.Now;
                        objgeneral.QUA_posting_master.ImportRow(dt_list.Rows[0]);

                    }
                }

                // set the item   notification
                #region create notification
                DS_General.QUA_notificationRow new_row = objgeneral.QUA_notification.NewQUA_notificationRow();
                new_row.doc_no = "1";
                new_row.from_member = parameter["member_id"].ToString();
                new_row.to_member = dt_list.Rows[0]["Member_id"].ToString();
                new_row.notification_date = System.DateTime.Now;
                new_row._event = "BUY";
                new_row.template = "Hooray! You have a new buy request for" + "  " + dt_list.Rows[0]["Name_title"].ToString();
                new_row.item_id = parameter["item_id"].ToString();
                new_row.is_read = "N";
                new_row.is_accept = "N";
                new_row.is_active = "Y";
                new_row.created_by = parameter["member_id"].ToString();
                new_row.created_date = System.DateTime.Now;
                new_row.created_host = HttpContext.Current.Request.UserHostName;

                objgeneral.QUA_notification.AddQUA_notificationRow(new_row);

                #endregion end notification
                #region alert message
                DS_General.alertmessageRow alert_row1 = objgeneral.alertmessage.NewalertmessageRow();

                alert_row1.Doc_no = "101";



                alert_row1.to_member = dt_list.Rows[0]["Member_id"].ToString();
                //   alert_row.to_member_role = "ST";
                alert_row1._event = "Request";
                alert_row1.template = "Hooray! You have a new buy request for" + "  " + dt_list.Rows[0]["Name_title"].ToString();


                alert_row1.notification_date = System.DateTime.Now;

                alert_row1.is_read = "N";
                alert_row1.is_active = "Y";
                alert_row1.AppName = "QUA";

                alert_row1.senddate = System.DateTime.Now;
                alert_row1.utc_time = System.DateTime.UtcNow.TimeOfDay;
                alert_row1.created_by = "system";
                alert_row1.created_date = System.DateTime.Now;
                alert_row1.created_host = HttpContext.Current.Request.UserHostName;

                objgeneral.alertmessage.AddalertmessageRow(alert_row1);
                #endregion alert message
                #region create order
                DS_General.QUA_order_masterRow now_order = objgeneral.QUA_order_master.NewQUA_order_masterRow();
                now_order.Order_id = "1";
                now_order.Member_id = parameter["member_id"].ToString();
                now_order.Item_id = parameter["item_id"].ToString();
                now_order.Order_date = System.DateTime.Now;
                now_order.Order_status = "R"; // request status
                now_order.is_active = "Y";
                now_order.created_by = parameter["member_id"].ToString();
                now_order.created_date = System.DateTime.Now;
                now_order.created_host = HttpContext.Current.Request.UserHostName;

                objgeneral.QUA_order_master.AddQUA_order_masterRow(now_order);

                #endregion



                objBLReturnObject = objmasters.Buy_data(objgeneral);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    flag = true;

                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not " + "status" + ex.StackTrace);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            if (flag == true)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not" + "status" + "1");
                return BLGeneralUtil.return_ajax_string("1", "Your request has been sent to the seller");
            }
            else
            {
                return BLGeneralUtil.return_ajax_string("0", "some thing went wrong");
            }
        }

        [HttpPost]
        public string update_product([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            if (parameter["item_id"].ToString() == null || parameter["item_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "item id not found"); }

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "member id not found"); }

            if (parameter["flag"].ToString() == null || parameter["flag"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "flag is not found ");
            }
            try
            {

                DataTable dt_list = objmasters.getitemlist(parameter["item_id"].ToString());
                if (dt_list != null)
                {
                    //first time buy that time flag is Reqested 

                    dt_list.Rows[0]["status"] = "D"; // this status is  Deactive item
                    dt_list.Rows[0]["is_active"] = "N";
                    dt_list.Rows[0]["last_modified_by"] = parameter["member_id"].ToString();
                    dt_list.Rows[0]["last_modified_date"] = System.DateTime.Now;
                    objgeneral.QUA_posting_master.ImportRow(dt_list.Rows[0]);
                }

                // set the item   notification



                objBLReturnObject = objmasters.Buy_data(objgeneral);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    flag = true;

                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not " + "status" + ex.StackTrace);
                return BLGeneralUtil.return_ajax_string("0", "some thing went wrong");
            }
            if (flag == true)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not" + "status" + "1");
                return BLGeneralUtil.return_ajax_string("1", "data saved successful");
            }
            else
            {
                return BLGeneralUtil.return_ajax_string("0", "some thing went wrong");
            }
        }

        //order update request accept/reject
        [HttpPost]
        public string update_Request_order([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;
            Decimal sellect_amount;
            double seller_charge = 0.0; // doller  this come form databae price master
            double buy_charge = 0.0;  //doller  this come form databae price master
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            if (parameter["item_id"].ToString() == null || parameter["item_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "item id not found"); }

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "member id not found"); }

            if (parameter["flag"].ToString() == null || parameter["flag"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "flag is not found ");
            }
            if (parameter["order_id"].ToString() == null || parameter["order_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "order id is not found ");
            }
            try
            {


                DataTable dt_order = objmasters.order_data(parameter["order_id"].ToString());
                DataTable dt_list = objmasters.getitembyid(parameter["item_id"].ToString());
                if (dt_list != null)
                {
                    if (dt_order.Rows[0]["Order_status"].ToString() == "S")
                    {

                        return BLGeneralUtil.return_ajax_string("0", "Order Already Completed ");
                    }
                }
                else
                {

                    return BLGeneralUtil.return_ajax_string("0", "Order Not Found ");

                }
                if (dt_list != null)
                {
                    if (dt_list.Rows[0]["status"] == "S")
                    {
                        return BLGeneralUtil.return_ajax_string("0", "Item Already Sell ");
                    }

                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "Item Not Found ");

                }
                if (parameter["flag"].ToString() == "A")
                {

                    DataTable DTCheckbal = objmasters.checkbalance(dt_order.Rows[0]["Member_id"].ToString(), dt_list.Rows[0]["price"].ToString());
                    if (DTCheckbal == null || DTCheckbal.Rows.Count == 0)
                    {

                        return BLGeneralUtil.return_ajax_string("0", "Buyer Has Insufficient Balance. Either contact buyer  or reject request ");

                    }

                }

                if (dt_list != null)
                {

                    if (parameter["flag"].ToString() == "A") // accept order request
                    {
                        if (dt_list.Rows[0]["type"].ToString() == "sales")
                        {
                            dt_list.Rows[0]["status"] = "S";
                            dt_list.Rows[0]["req_type"] = "S";
                        }
                        else
                        {

                            dt_list.Rows[0]["status"] = "A";
                            dt_list.Rows[0]["req_type"] = "";
                        }
                    }

                    if (parameter["flag"].ToString() == "R") // Reject order request
                    {
                        dt_list.Rows[0]["status"] = "A";

                    }

                    dt_list.Rows[0]["last_modified_by"] = parameter["member_id"].ToString();
                    dt_list.Rows[0]["last_modified_date"] = System.DateTime.Now;
                    objgeneral.QUA_posting_master.ImportRow(dt_list.Rows[0]);
                }


                if (parameter["flag"].ToString() == "A")
                {

                    DS_General.QUA_feedback_participantsRow fb_part_row = objgeneral.QUA_feedback_participants.NewQUA_feedback_participantsRow();
                    fb_part_row.Item_id = parameter["item_id"].ToString();
                    fb_part_row.member_id = dt_order.Rows[0]["Member_id"].ToString();
                    fb_part_row.is_survey_done = "N";
                    fb_part_row.OrderID = parameter["order_id"].ToString();
                    objgeneral.QUA_feedback_participants.AddQUA_feedback_participantsRow(fb_part_row);

                    DS_General.QUA_feedback_participantsRow fb_part_row1 = objgeneral.QUA_feedback_participants.NewQUA_feedback_participantsRow();
                    fb_part_row1.Item_id = parameter["item_id"].ToString();
                    fb_part_row1.member_id = parameter["member_id"].ToString();
                    fb_part_row1.OrderID = parameter["order_id"].ToString();
                    fb_part_row1.is_survey_done = "N";
                    objgeneral.QUA_feedback_participants.AddQUA_feedback_participantsRow(fb_part_row1);
                    if (dt_list.Rows[0]["type"].ToString() == "sales")
                    {
                        bool fl = rejectbartoffer(parameter["item_id"].ToString(), parameter["member_id"].ToString(), dt_list.Rows[0]["name_title"].ToString());

                        if (fl == false)
                        {

                            return BLGeneralUtil.return_ajax_string("0", "try again");

                        }
                    }
                }
                // set the item   notification
                #region create notification
                DS_General.QUA_notificationRow new_row = objgeneral.QUA_notification.NewQUA_notificationRow();
                new_row.doc_no = "1";
                new_row.from_member = dt_list.Rows[0]["Member_id"].ToString();
                new_row.to_member = dt_order.Rows[0]["Member_id"].ToString();
                new_row.notification_date = System.DateTime.Now;
                new_row._event = "BUY";

                if (parameter["flag"].ToString() == "A")
                    new_row.template = " Cha ching! your request has been accepted  for  " + dt_list.Rows[0]["name_title"].ToString() + "  Please do not forget to rate your seller in the order history page";
                if (parameter["flag"].ToString() == "R")
                    new_row.template = "Your Buy Offer for   " + dt_list.Rows[0]["name_title"].ToString() + "  has been rejected. ";

                new_row.item_id = parameter["item_id"].ToString();
                new_row.is_read = "N";
                new_row.is_accept = "N";
                new_row.is_active = "Y";
                new_row.created_by = parameter["member_id"].ToString();
                new_row.created_date = System.DateTime.Now;
                new_row.created_host = HttpContext.Current.Request.UserHostName;

                objgeneral.QUA_notification.AddQUA_notificationRow(new_row);

                #endregion end notification
                #region Push Notification

                DS_General.alertmessageRow alert_row = objgeneral.alertmessage.NewalertmessageRow();
                alert_row.Doc_no = "1";

                if (parameter["flag"].ToString() == "A")
                {

                    alert_row.to_member = dt_order.Rows[0]["Member_id"].ToString(); ;
                    //  alert_row.to_member_role = "TT";
                    alert_row._event = "Request";
                    alert_row.template = "Cha ching! your request has been accepted for " + dt_list.Rows[0]["Name_title"].ToString() + "Please do not forget to rate your seller/buye in the order history page";
                }
                if (parameter["flag"].ToString() == "R")
                {
                    alert_row.to_member = dt_order.Rows[0]["Member_id"].ToString();
                    //   alert_row.to_member_role = "ST";
                    alert_row._event = "Request";
                    alert_row.template = "sorry your request was rejected for " + dt_list.Rows[0]["Name_title"].ToString();
                }

                alert_row.notification_date = System.DateTime.Now;

                alert_row.is_read = "N";
                alert_row.is_active = "Y";
                alert_row.AppName = "QUA";

                alert_row.senddate = System.DateTime.Now;
                alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                alert_row.created_by = "system";
                alert_row.created_date = System.DateTime.Now;
                alert_row.created_host = HttpContext.Current.Request.UserHostName;

                objgeneral.alertmessage.AddalertmessageRow(alert_row);


                #endregion


                #region create order





                if (parameter["flag"].ToString() == "A")
                {
                    dt_order.Rows[0]["order_status"] = "S";
                    dt_order.Rows[0]["last_modified_date"] = System.DateTime.Now;
                }

                if (parameter["flag"].ToString() == "R")
                {
                    dt_order.Rows[0]["order_status"] = "C";
                }
                objgeneral.QUA_order_master.ImportRow(dt_order.Rows[0]);
                //DS_General.QUA_order_masterRow now_order = objgeneral.QUA_order_master.NewQUA_order_masterRow();
                //now_order.Order_id = "1";
                //now_order.Member_id = parameter["member_id"].ToString();
                //now_order.Item_id = parameter["item_id"].ToString();
                //now_order.Order_date = System.DateTime.Now;
                //if (parameter["flag"].ToString() == "A")
                //    now_order.Order_status = "S"; // buy complect in by table


                //if (parameter["flag"].ToString() == "R")
                //    now_order.Order_status = "C"; // order cancle
                //now_order.is_active = "Y";
                //now_order.created_by = parameter["member_id"].ToString();
                //now_order.created_date = System.DateTime.Now;
                //now_order.created_host = HttpContext.Current.Request.UserHostName;

                //objgeneral.QUA_order_master.AddQUA_order_masterRow(now_order);

                #endregion
                if (parameter["flag"].ToString() == "A")
                {
                    DataTable DT_Price = objmasters.GetQUAPriceMaster();
                    if (DT_Price.Rows.Count > 0)
                    {
                        if (DT_Price.Rows[1]["Code"].ToString() == "seller_charge")
                        {
                            seller_charge = Convert.ToDouble(DT_Price.Rows[1]["Value"].ToString());
                        }
                        if (DT_Price.Rows[2]["Code"].ToString() == "buy_charge")
                        {
                            buy_charge = Convert.ToDouble(DT_Price.Rows[2]["Value"].ToString());
                        }
                    }
                    #region post charge 0.10

                    DataTable dt_member_seller = objmasters.gettokenbal(parameter["member_id"].ToString());
                    DataTable dt_mycc = objmasters.gettokenbal(mycc_id);
                    DataTable dt_member_buyer = objmasters.gettokenbal(dt_order.Rows[0]["Member_id"].ToString());

                    if (dt_member_seller == null)
                    {

                        return BLGeneralUtil.return_ajax_string("0", "member_account detail not found");
                    }
                    if (dt_mycc == null)
                    {
                        return BLGeneralUtil.return_ajax_string("0", "member_account detail not found");
                    }
                    if (dt_member_buyer == null)
                    {
                        return BLGeneralUtil.return_ajax_string("0", "member_account detail not found");
                    }
                    //deducted member acount
                    #region member acount
                    #region seller excharge

                    dt_member_buyer.Rows[0]["total_debit"] = (Convert.ToDecimal(dt_member_buyer.Rows[0]["total_debit"].ToString()) + ((Convert.ToDecimal(buy_charge)) + (Convert.ToDecimal(dt_list.Rows[0]["price"].ToString()))));
                    dt_member_buyer.Rows[0]["balance_token"] = (Convert.ToDecimal(dt_member_buyer.Rows[0]["balance_token"].ToString()) - ((Convert.ToDecimal(buy_charge)) + (Convert.ToDecimal(dt_list.Rows[0]["price"].ToString()))));
                    objDs_trastration.fn_token_balance.ImportRow(dt_member_buyer.Rows[0]);

                    DS_Transtration.fn_token_transtionRow token_transtration_row4 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                    token_transtration_row4.doc_no = "1";
                    token_transtration_row4.doc_date = System.DateTime.Now;
                    token_transtration_row4.ref_transtion_type = "purchase";
                    token_transtration_row4.ref_transtion_doc_date = System.DateTime.Now;
                    token_transtration_row4.ref_transtion_doc_no = parameter["order_id"].ToString();
                    token_transtration_row4.type_credit_debit = "debit";
                    token_transtration_row4.amount = (Convert.ToDecimal(buy_charge)) + (Convert.ToDecimal(dt_list.Rows[0]["price"].ToString()));
                    token_transtration_row4.balance_after_transtion = Convert.ToDecimal(dt_member_buyer.Rows[0]["balance_token"].ToString());
                    token_transtration_row4.member_id = dt_order.Rows[0]["Member_id"].ToString();
                    token_transtration_row4.created_by = "system";
                    token_transtration_row4.created_date = System.DateTime.Now;
                    token_transtration_row4.created_host = HttpContext.Current.Request.UserHostName;

                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row4);

                    #endregion

                    #endregion
                    #region mycc acount
                    dt_mycc.Rows[0]["total_credit"] = (Convert.ToDecimal(dt_mycc.Rows[0]["total_credit"].ToString()) + (Convert.ToDecimal(buy_charge)));
                    dt_mycc.Rows[0]["balance_token"] = (Convert.ToDecimal(dt_mycc.Rows[0]["balance_token"].ToString()) + (Convert.ToDecimal(buy_charge)));

                    //objDs_trastration.fn_token_balance.ImportRow(dt_mycc.Rows[0]);

                    DS_Transtration.fn_token_transtionRow token_transtration_row6 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                    token_transtration_row6.doc_no = "3";
                    token_transtration_row6.doc_date = System.DateTime.Now;
                    token_transtration_row6.ref_transtion_type = "Buy_charge";
                    token_transtration_row6.ref_transtion_doc_date = System.DateTime.Now;
                    token_transtration_row6.ref_transtion_doc_no = parameter["order_id"].ToString();
                    token_transtration_row6.type_credit_debit = "Credit";
                    token_transtration_row6.amount = (Convert.ToDecimal(buy_charge));
                    token_transtration_row6.balance_after_transtion = Convert.ToDecimal(dt_mycc.Rows[0]["balance_token"].ToString());
                    token_transtration_row6.member_id = mycc_id;
                    token_transtration_row6.created_by = "system";
                    token_transtration_row6.created_date = System.DateTime.Now;
                    token_transtration_row6.created_host = HttpContext.Current.Request.UserHostName;

                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row6);

                    #endregion

                    #region seller

                    //sellect_amount = Convert.ToDecimal(seller_charge) * Convert.ToDecimal(dt_list.Rows[0]["price"].ToString());

                    sellect_amount = Convert.ToDecimal(seller_charge);

                    dt_member_seller.Rows[0]["total_credit"] = (Convert.ToDecimal(dt_member_seller.Rows[0]["total_credit"].ToString()) + (Convert.ToDecimal(dt_list.Rows[0]["price"].ToString()) - (sellect_amount)));
                    dt_member_seller.Rows[0]["balance_token"] = (Convert.ToDecimal(dt_member_seller.Rows[0]["balance_token"].ToString()) + (Convert.ToDecimal(dt_list.Rows[0]["price"].ToString()) - (sellect_amount)));

                    objDs_trastration.fn_token_balance.ImportRow(dt_member_seller.Rows[0]);

                    DS_Transtration.fn_token_transtionRow token_transtration_row5 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                    token_transtration_row5.doc_no = "2";
                    token_transtration_row5.doc_date = System.DateTime.Now;
                    token_transtration_row5.ref_transtion_type = "Sales";
                    token_transtration_row5.ref_transtion_doc_date = System.DateTime.Now;
                    token_transtration_row5.ref_transtion_doc_no = parameter["order_id"].ToString();
                    token_transtration_row5.type_credit_debit = "Credit";
                    token_transtration_row5.amount = (Convert.ToDecimal(dt_list.Rows[0]["price"].ToString())) - (Convert.ToDecimal(sellect_amount));
                    token_transtration_row5.balance_after_transtion = Convert.ToDecimal(dt_member_buyer.Rows[0]["balance_token"].ToString());
                    token_transtration_row5.member_id = parameter["member_id"].ToString();
                    token_transtration_row5.created_by = "system";
                    token_transtration_row5.created_date = System.DateTime.Now;
                    token_transtration_row5.created_host = HttpContext.Current.Request.UserHostName;

                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row5);
                    #endregion

                    #region seller charge

                    dt_mycc.Rows[0]["total_credit"] = (Convert.ToDecimal(dt_mycc.Rows[0]["total_credit"].ToString()) + (sellect_amount));
                    dt_mycc.Rows[0]["balance_token"] = (Convert.ToDecimal(dt_mycc.Rows[0]["balance_token"].ToString()) + (sellect_amount));

                    objDs_trastration.fn_token_balance.ImportRow(dt_mycc.Rows[0]);

                    DS_Transtration.fn_token_transtionRow token_transtration_row7 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                    token_transtration_row7.doc_no = "4";
                    token_transtration_row7.doc_date = System.DateTime.Now;
                    token_transtration_row7.ref_transtion_type = "sell_charge";
                    token_transtration_row7.ref_transtion_doc_date = System.DateTime.Now;
                    token_transtration_row7.ref_transtion_doc_no = parameter["order_id"].ToString();
                    token_transtration_row7.type_credit_debit = "Credit";
                    token_transtration_row7.amount = (sellect_amount);
                    token_transtration_row7.balance_after_transtion = Convert.ToDecimal(dt_mycc.Rows[0]["balance_token"].ToString());
                    token_transtration_row7.member_id = mycc_id;
                    token_transtration_row7.created_by = "system";
                    token_transtration_row7.created_date = System.DateTime.Now;
                    token_transtration_row7.created_host = HttpContext.Current.Request.UserHostName;

                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row7);

                    #endregion
                    #endregion
        #endregion
                }
                objBLReturnObject = objmasters.Buy_data_order_complated(objgeneral, objDs_trastration);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    flag = true;

                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not " + "status" + ex.StackTrace);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            if (flag == true)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not" + "status" + "1");
                if (parameter["flag"].ToString() == "A")
                {
                    return BLGeneralUtil.return_ajax_string("1", "Congrats! your listing has been bought Please do not forget to rate your customer in the order history page.");
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("1", "Your Reject request has been sent.");
                }
            }
            else
            {
                return BLGeneralUtil.return_ajax_string("0", "try again");
            }
        }

        //update barter offer

        [HttpPost]
        public string barter_offer_update([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag =
                false;
            Decimal sellect_amount;
            string buy_member_id = "";
            Random rand = new Random();
            double barter_charge = 0.0;
            int randomNo = rand.Next(1, 1000000);

            if (parameter["item_id"].ToString() == null || parameter["item_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "item id not found"); }

            if (parameter["member_id"].ToString() == null || parameter["member_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "member id not found"); }

            if (parameter["flag"].ToString() == null || parameter["flag"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "flag is not found ");
            }
            if (parameter["barter_id"].ToString() == null || parameter["barter_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "barter id is not found ");
            }
            if (parameter["buyer_id"].ToString() == null || parameter["buyer_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "buyer id is not found ");
            }
            try
            {
                DataTable dt_list_check = objmasters.getitembyid(parameter["item_id"].ToString());
                DataTable dt_posting = objmasters.getitembyid(parameter["item_id"].ToString());
                DataTable dt_barter = objmasters.getdetailbarter(parameter["barter_id"].ToString());

                if (dt_barter.Rows[0]["is_accepted"].ToString() == "S")
                {

                    return BLGeneralUtil.return_ajax_string("0", "Already Accept Request  ");

                }
                if (dt_list_check != null)
                {
                    if (dt_list_check.Rows[0]["status"].ToString() == "S")
                    {
                        return BLGeneralUtil.return_ajax_string("0", "Item Already Barter ");
                    }

                }
                //DataTable dt_barter = objmasters.getdetailbarter(parameter["barter_id"].ToString());
                if (dt_barter != null)
                {

                    if (parameter["flag"].ToString() == "A")// accept order request
                    {
                        dt_barter.Rows[0]["is_accepted"] = "S";
                        buy_member_id = dt_barter.Rows[0]["buyer_member_id"].ToString();
                    }

                    if (parameter["flag"].ToString() == "R")// accept order request
                    { dt_barter.Rows[0]["is_accepted"] = "C"; }

                    dt_barter.Rows[0]["last_modified_by"] = parameter["member_id"].ToString();
                    dt_barter.Rows[0]["last_modified_date"] = System.DateTime.Now;
                    objgeneral.QUA_barter_master.ImportRow(dt_barter.Rows[0]);


                    if (parameter["flag"].ToString() == "A")
                    {
                        DS_General.QUA_feedback_participantsRow fb_part_row = objgeneral.QUA_feedback_participants.NewQUA_feedback_participantsRow();
                        fb_part_row.Item_id = parameter["item_id"].ToString();
                        fb_part_row.member_id = dt_barter.Rows[0]["buyer_member_id"].ToString();
                        fb_part_row.is_survey_done = "N";
                        fb_part_row.OrderID = parameter["barter_id"].ToString();
                        objgeneral.QUA_feedback_participants.AddQUA_feedback_participantsRow(fb_part_row);

                        DS_General.QUA_feedback_participantsRow fb_part_row1 = objgeneral.QUA_feedback_participants.NewQUA_feedback_participantsRow();
                        fb_part_row1.Item_id = parameter["item_id"].ToString();
                        fb_part_row1.member_id = parameter["member_id"].ToString();
                        fb_part_row1.OrderID = parameter["barter_id"].ToString();
                        fb_part_row1.is_survey_done = "N";
                        objgeneral.QUA_feedback_participants.AddQUA_feedback_participantsRow(fb_part_row1);

                    }
                    #region create notification

                    DS_General.QUA_notificationRow new_row = objgeneral.QUA_notification.NewQUA_notificationRow();
                    new_row.doc_no = "1";
                    new_row.from_member = "System";
                    new_row.to_member = dt_barter.Rows[0]["buyer_member_id"].ToString();
                    new_row.notification_date = System.DateTime.Now;
                    new_row._event = "Barter";

                    if (parameter["flag"].ToString() == "A")
                        new_row.template = "Cha ching! your barter request has been accepted  for  " + dt_list_check.Rows[0]["Name_title"].ToString() + "  Please do not forget to rate your seller/buyer in the order history page";
                    if (parameter["flag"].ToString() == "R")
                        new_row.template = " sorry your barter request was rejected for " + dt_list_check.Rows[0]["Name_title"].ToString();

                    new_row.item_id = parameter["barter_id"].ToString();
                    new_row.is_read = "N";
                    new_row.is_accept = "N";
                    new_row.is_active = "Y";
                    new_row.created_by = "System";
                    new_row.created_date = System.DateTime.Now;
                    new_row.created_host = HttpContext.Current.Request.UserHostName;

                    objgeneral.QUA_notification.AddQUA_notificationRow(new_row);

                    #endregion end notification

                }
                #region Push Notification

                DS_General.alertmessageRow alert_row = objgeneral.alertmessage.NewalertmessageRow();
                alert_row.Doc_no = "1";

                if (parameter["flag"].ToString() == "A")
                {

                    alert_row.to_member = dt_barter.Rows[0]["buyer_member_id"].ToString();
                    //  alert_row.to_member_role = "TT";
                    alert_row._event = "Request";
                    alert_row.template = "Cha ching! your barter request has been accepted " + dt_list_check.Rows[0]["Name_title"].ToString() + "Please do not forget to rate your seller/buyer in the order history page";
                }
                if (parameter["flag"].ToString() == "R")
                {
                    alert_row.to_member = dt_barter.Rows[0]["buyer_member_id"].ToString();
                    //   alert_row.to_member_role = "ST";
                    alert_row._event = "Request";
                    alert_row.template = " sorry your barter request was rejected for " + dt_list_check.Rows[0]["Name_title"].ToString();
                }

                alert_row.notification_date = System.DateTime.Now;

                alert_row.is_read = "N";
                alert_row.is_active = "Y";
                alert_row.AppName = "QUA";

                alert_row.senddate = System.DateTime.Now;
                alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                alert_row.created_by = "system";
                alert_row.created_date = System.DateTime.Now;
                alert_row.created_host = HttpContext.Current.Request.UserHostName;

                objgeneral.alertmessage.AddalertmessageRow(alert_row);


                #endregion


                if (parameter["flag"].ToString() == "A")
                {


                    DataTable dt_all = objmasters.getbarter_not(parameter["barter_id"].ToString(), parameter["item_id"].ToString());
                    if (dt_posting.Rows[0]["type"].ToString() == "sales")
                    {
                        if (dt_all != null && dt_all.Rows.Count > 0)
                        {
                            for (var i = 0; i < dt_all.Rows.Count; i++)
                            {


                                dt_all.Rows[i]["is_accepted"] = "C";

                                dt_all.Rows[i]["last_modified_by"] = parameter["member_id"].ToString();
                                dt_all.Rows[i]["last_modified_date"] = System.DateTime.Now;

                                objgeneral.QUA_barter_master.ImportRow(dt_all.Rows[i]);



                                DS_General.QUA_notificationRow new_row1 = objgeneral.QUA_notification.NewQUA_notificationRow();
                                new_row1.doc_no = "1" + i;
                                new_row1.from_member = "System";
                                new_row1.to_member = dt_all.Rows[i]["buyer_member_id"].ToString();
                                new_row1.notification_date = System.DateTime.Now;
                                new_row1._event = "Barter";



                                new_row1.template = " sorry your barter request was rejected for " + dt_list_check.Rows[0]["Name_title"].ToString();

                                new_row1.item_id = parameter["item_id"].ToString();
                                new_row1.is_read = "N";
                                new_row1.is_accept = "N";
                                new_row1.is_active = "Y";
                                new_row1.created_by = "System";
                                new_row1.created_date = System.DateTime.Now;
                                new_row1.created_host = HttpContext.Current.Request.UserHostName;

                                objgeneral.QUA_notification.AddQUA_notificationRow(new_row1);



                                #region Push Notification

                                DS_General.alertmessageRow alert_row1 = objgeneral.alertmessage.NewalertmessageRow();
                                alert_row1.Doc_no = "11" + i;



                                alert_row1.to_member = dt_all.Rows[i]["buyer_member_id"].ToString();
                                //   alert_row.to_member_role = "ST";
                                alert_row1._event = "Request";
                                alert_row1.template = " sorry your barter request was rejected  for" + dt_list_check.Rows[0]["Name_title"].ToString();


                                alert_row1.notification_date = System.DateTime.Now;

                                alert_row1.is_read = "N";
                                alert_row1.is_active = "Y";
                                alert_row1.AppName = "QUA";

                                alert_row1.senddate = System.DateTime.Now;
                                alert_row1.utc_time = System.DateTime.UtcNow.TimeOfDay;
                                alert_row1.created_by = "system";
                                alert_row1.created_date = System.DateTime.Now;
                                alert_row1.created_host = HttpContext.Current.Request.UserHostName;

                                objgeneral.alertmessage.AddalertmessageRow(alert_row1);


                                #endregion
                            }
                        }
                    }
                    if (dt_posting.Rows[0]["type"].ToString() == "sales")
                    {
                        DataTable dt_order = objmasters.my_orderitemwise(parameter["item_id"].ToString());
                        if (dt_order != null && dt_order.Rows.Count > 0)
                        {

                            dt_order.Rows[0]["order_status"] = "C"; //reject

                            objgeneral.QUA_order_master.ImportRow(dt_order.Rows[0]);



                            DS_General.QUA_notificationRow new_row12 = objgeneral.QUA_notification.NewQUA_notificationRow();
                            new_row12.doc_no = "1235";
                            new_row12.from_member = "System";
                            new_row12.to_member = dt_order.Rows[0]["Member_id"].ToString();
                            new_row12.notification_date = System.DateTime.Now;
                            new_row12._event = "BUY";



                            new_row12.template = "sorry your request was rejected for  " + dt_list_check.Rows[0]["Name_title"].ToString();

                            new_row12.item_id = parameter["item_id"].ToString();
                            new_row12.is_read = "N";
                            new_row12.is_accept = "N";
                            new_row12.is_active = "Y";
                            new_row12.created_by = "System";
                            new_row12.created_date = System.DateTime.Now;
                            new_row12.created_host = HttpContext.Current.Request.UserHostName;

                            objgeneral.QUA_notification.AddQUA_notificationRow(new_row12);




                            DS_General.alertmessageRow alert_row12 = objgeneral.alertmessage.NewalertmessageRow();
                            alert_row12.Doc_no = "121";



                            alert_row12.to_member = dt_order.Rows[0]["Member_id"].ToString();
                            //   alert_row.to_member_role = "ST";
                            alert_row12._event = "Request";
                            alert_row12.template = "sorry your request was rejected for " + dt_list_check.Rows[0]["Name_title"].ToString();


                            alert_row12.notification_date = System.DateTime.Now;

                            alert_row12.is_read = "N";
                            alert_row12.is_active = "Y";
                            alert_row12.AppName = "QUA";

                            alert_row12.senddate = System.DateTime.Now;
                            alert_row12.utc_time = System.DateTime.UtcNow.TimeOfDay;
                            alert_row12.created_by = "system";
                            alert_row12.created_date = System.DateTime.Now;
                            alert_row12.created_host = HttpContext.Current.Request.UserHostName;

                            objgeneral.alertmessage.AddalertmessageRow(alert_row12);
                        }
                    }


                    if (dt_posting.Rows[0]["type"].ToString() == "sales")
                    {
                        dt_posting.Rows[0]["status"] = "S";
                        dt_posting.Rows[0]["req_type"] = "B";
                    }
                    else
                    {
                        dt_posting.Rows[0]["status"] = "A";
                        dt_posting.Rows[0]["req_type"] = "";
                    }
                    dt_posting.Rows[0]["last_modified_by"] = parameter["member_id"].ToString();
                    dt_posting.Rows[0]["last_modified_date"] = System.DateTime.Now;

                    objgeneral.QUA_posting_master.ImportRow(dt_posting.Rows[0]);

                }


                // set the item   notification

                //#region create notification
                //DS_General.QUA_notificationRow new_row = objgeneral.QUA_notification.NewQUA_notificationRow();
                //new_row.doc_no = "1";
                //new_row.from_member = dt_list.Rows[0]["Member_id"].ToString();
                //new_row.to_member = parameter["member_id"].ToString();
                //new_row.notification_date = System.DateTime.Now;
                //new_row._event = "BUY";

                //if (parameter["flag"].ToString() == "A")
                //    new_row.template = "Buy Item Request Accepting ";
                //if (parameter["flag"].ToString() == "R")
                //    new_row.template = "Buy Item Request Rejecting ";

                //new_row.item_id = parameter["item_id"].ToString();
                //new_row.is_read = "N";
                //new_row.is_accept = "N";
                //new_row.is_active = "Y";
                //new_row.created_by = parameter["member_id"].ToString();
                //new_row.created_date = System.DateTime.Now;
                //new_row.created_host = HttpContext.Current.Request.UserHostName;

                //objgeneral.QUA_notification.AddQUA_notificationRow(new_row);

                //#endregion end notification

                //#region create order
                //DS_General.QUA_order_masterRow now_order = objgeneral.QUA_order_master.NewQUA_order_masterRow();
                //now_order.Order_id = "1";
                //now_order.Member_id = parameter["member_id"].ToString();
                //now_order.Item_id = parameter["item_id"].ToString();
                //now_order.Order_date = System.DateTime.Now;
                //if (parameter["flag"].ToString() == "A")
                //    now_order.Order_status = "S"; // buy complect in by table
                //if (parameter["flag"].ToString() == "R")
                //    now_order.Order_status = "C"; // order cancle
                //now_order.is_active = "Y";
                //now_order.created_by = parameter["member_id"].ToString();
                //now_order.created_date = System.DateTime.Now;
                //now_order.created_host = HttpContext.Current.Request.UserHostName;

                //objgeneral.QUA_order_master.AddQUA_order_masterRow(now_order);

                //#endregion
                if (parameter["flag"].ToString() == "A")
                {
                    #region post charge

                    DataTable DT_Price = objmasters.GetQUAPriceMaster();
                    if (DT_Price.Rows.Count > 0)
                    {
                        if (DT_Price.Rows[3]["Code"].ToString() == "barter_charge")
                        {
                            barter_charge = Convert.ToDouble(DT_Price.Rows[3]["Value"].ToString());
                        }
                    }

                    DataTable dt_member_seller = objmasters.gettokenbal(parameter["member_id"].ToString());
                    DataTable dt_mycc = objmasters.gettokenbal(mycc_id);
                    DataTable dt_member_buyer = objmasters.gettokenbal(buy_member_id);

                    if (dt_member_seller == null)
                    {

                        return BLGeneralUtil.return_ajax_string("0", "member_account detail not found");
                    }
                    if (dt_mycc == null)
                    {
                        return BLGeneralUtil.return_ajax_string("0", "member_account detail not found");
                    }
                    if (dt_member_buyer == null)
                    {
                        return BLGeneralUtil.return_ajax_string("0", "member_account detail not found");
                    }
                    //deducted member acount
                    #region member acount
                    #region seller excharge

                    dt_member_buyer.Rows[0]["total_debit"] = (Convert.ToDecimal(dt_member_buyer.Rows[0]["total_debit"].ToString()) + (Convert.ToDecimal(barter_charge)));
                    dt_member_buyer.Rows[0]["balance_token"] = (Convert.ToDecimal(dt_member_buyer.Rows[0]["balance_token"].ToString()) - (Convert.ToDecimal(barter_charge)));
                    // objDs_trastration.fn_token_balance.ImportRow(dt_member_buyer.Rows[0]);

                    DS_Transtration.fn_token_transtionRow token_transtration_row4 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                    token_transtration_row4.doc_no = "1";
                    token_transtration_row4.doc_date = System.DateTime.Now;
                    token_transtration_row4.ref_transtion_type = "Barter";
                    token_transtration_row4.ref_transtion_doc_date = System.DateTime.Now;
                    token_transtration_row4.ref_transtion_doc_no = parameter["barter_id"].ToString();
                    token_transtration_row4.type_credit_debit = "debit";
                    token_transtration_row4.amount = (Convert.ToDecimal(barter_charge));
                    token_transtration_row4.balance_after_transtion = Convert.ToDecimal(dt_member_buyer.Rows[0]["balance_token"].ToString());
                    token_transtration_row4.member_id = parameter["member_id"].ToString();
                    token_transtration_row4.created_by = "system";
                    token_transtration_row4.created_date = System.DateTime.Now;
                    token_transtration_row4.created_host = HttpContext.Current.Request.UserHostName;

                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row4);

                    #endregion

                    #endregion
                    #region mycc acount
                    dt_mycc.Rows[0]["total_credit"] = (Convert.ToDecimal(dt_mycc.Rows[0]["total_credit"].ToString()) + (Convert.ToDecimal(barter_charge)));
                    dt_mycc.Rows[0]["balance_token"] = (Convert.ToDecimal(dt_mycc.Rows[0]["balance_token"].ToString()) + (Convert.ToDecimal(barter_charge)));

                    //objDs_trastration.fn_token_balance.ImportRow(dt_mycc.Rows[0]);

                    DS_Transtration.fn_token_transtionRow token_transtration_row6 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                    token_transtration_row6.doc_no = "3";
                    token_transtration_row6.doc_date = System.DateTime.Now;
                    token_transtration_row6.ref_transtion_type = "Barter_charge";
                    token_transtration_row6.ref_transtion_doc_date = System.DateTime.Now;
                    token_transtration_row6.ref_transtion_doc_no = parameter["barter_id"].ToString();
                    token_transtration_row6.type_credit_debit = "Credit";
                    token_transtration_row6.amount = (Convert.ToDecimal(barter_charge));
                    token_transtration_row6.balance_after_transtion = Convert.ToDecimal(dt_mycc.Rows[0]["balance_token"].ToString());
                    token_transtration_row6.member_id = mycc_id;
                    token_transtration_row6.created_by = "system";
                    token_transtration_row6.created_date = System.DateTime.Now;
                    token_transtration_row6.created_host = HttpContext.Current.Request.UserHostName;

                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row6);

                    #endregion

                    #region seller

                    //sellect_amount = Convert.ToDecimal(seller_charge) * Convert.ToDecimal(dt_list.Rows[0]["price"].ToString());

                    dt_member_seller.Rows[0]["total_debit"] = (Convert.ToDecimal(dt_member_seller.Rows[0]["total_debit"].ToString()) + (Convert.ToDecimal(barter_charge)));
                    dt_member_seller.Rows[0]["balance_token"] = (Convert.ToDecimal(dt_member_seller.Rows[0]["balance_token"].ToString()) - (Convert.ToDecimal(barter_charge)));
                    objDs_trastration.fn_token_balance.ImportRow(dt_member_buyer.Rows[0]);

                    objDs_trastration.fn_token_balance.ImportRow(dt_member_seller.Rows[0]);

                    DS_Transtration.fn_token_transtionRow token_transtration_row5 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                    token_transtration_row5.doc_no = "2";
                    token_transtration_row5.doc_date = System.DateTime.Now;
                    token_transtration_row5.ref_transtion_type = "Barter";
                    token_transtration_row5.ref_transtion_doc_date = System.DateTime.Now;
                    token_transtration_row5.ref_transtion_doc_no = parameter["barter_id"].ToString();
                    token_transtration_row5.type_credit_debit = "debit";
                    token_transtration_row5.amount = (Convert.ToDecimal(barter_charge));
                    token_transtration_row5.balance_after_transtion = Convert.ToDecimal(dt_member_seller.Rows[0]["balance_token"].ToString());
                    token_transtration_row5.member_id = buy_member_id;
                    token_transtration_row5.created_by = "system";
                    token_transtration_row5.created_date = System.DateTime.Now;
                    token_transtration_row5.created_host = HttpContext.Current.Request.UserHostName;

                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row5);
                    #endregion

                    #region seller charge

                    dt_mycc.Rows[0]["total_credit"] = (Convert.ToDecimal(dt_mycc.Rows[0]["total_credit"].ToString()) + (Convert.ToDecimal(barter_charge)));
                    dt_mycc.Rows[0]["balance_token"] = (Convert.ToDecimal(dt_mycc.Rows[0]["balance_token"].ToString()) + (Convert.ToDecimal(barter_charge)));

                    objDs_trastration.fn_token_balance.ImportRow(dt_mycc.Rows[0]);

                    DS_Transtration.fn_token_transtionRow token_transtration_row7 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                    token_transtration_row7.doc_no = "4";
                    token_transtration_row7.doc_date = System.DateTime.Now;
                    token_transtration_row7.ref_transtion_type = "Barter_charge";
                    token_transtration_row7.ref_transtion_doc_date = System.DateTime.Now;
                    token_transtration_row7.ref_transtion_doc_no = parameter["barter_id"].ToString();
                    token_transtration_row7.type_credit_debit = "Credit";
                    token_transtration_row7.amount = (Convert.ToDecimal(barter_charge));
                    token_transtration_row7.balance_after_transtion = Convert.ToDecimal(dt_mycc.Rows[0]["balance_token"].ToString());
                    token_transtration_row7.member_id = mycc_id;
                    token_transtration_row7.created_by = "system";
                    token_transtration_row7.created_date = System.DateTime.Now;
                    token_transtration_row7.created_host = HttpContext.Current.Request.UserHostName;

                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row7);

                    #endregion
                    #endregion

                }
                objBLReturnObject = objmasters.barter_data_order_complated(objgeneral, objDs_trastration);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    flag = true;

                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not " + "status" + ex.StackTrace);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            if (flag == true)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not" + "status" + "1");
                if (parameter["flag"].ToString() == "A")
                {
                    return BLGeneralUtil.return_ajax_string("1", "Congratulations your item has been sold. Please do not forget to rate your buyer on the order history page");
                }
                else
                {
                    return BLGeneralUtil.return_ajax_string("1", "Offer Rejected");
                }
            }
            else
            {
                return BLGeneralUtil.return_ajax_string("0", "try again");
            }
        }

        [HttpPost]
        public HttpResponseMessage PostStuff_old()
        {
            HttpResponseMessage result = null;
            try
            {
                string sPath = "";


                string imagepath = "";
                var httpRequest = HttpContext.Current.Request;
                Dictionary<string, string> array1 = new Dictionary<string, string>();
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

                ServerLog.Log_QUA(httpRequest.Params["Title"].ToString());
                ServerLog.Log_QUA(httpRequest.Params["Desc"].ToString());
                ServerLog.Log_QUA(httpRequest.Params["Categories_id"].ToString());
                ServerLog.Log_QUA(httpRequest.Params["Price"].ToString());
                ServerLog.Log_QUA(httpRequest.Params["Member_id"].ToString());

                if (httpRequest.Params["Title"] == "" || httpRequest.Params["Title"] == null)
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + httpRequest.Params["Title"]);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, 7);

                }

                else if (httpRequest.Params["Desc"] == "" || httpRequest.Params["Desc"] == null)
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "Desc not found");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, 2);
                }

                else if (httpRequest.Params["Categories_id"] == "" || httpRequest.Params["Categories_id"] == null)
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "Categories_id not found");

                    return Request.CreateResponse(HttpStatusCode.BadRequest, 3);
                }
                else if (httpRequest.Params["Price"] == "" || httpRequest.Params["Price"] == null)
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "Price not found");

                    return Request.CreateResponse(HttpStatusCode.BadRequest, 4);
                }
                else if (httpRequest.Params["Member_id"] == "" || httpRequest.Params["Member_id"] == null)
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "Member_id not found");

                    return Request.CreateResponse(HttpStatusCode.BadRequest, 5);
                }
                if (httpRequest.Files.Count > 0)
                {
                    //ServerLog.Log("if in");
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        ServerLog.Log("test" + postedFile);
                        if (postedFile.FileName.Trim() != "")
                        {
                            //var path = "D:\\jaimindata\\CampusConcierge\\CampusConcierge\\member_uploaded_detail\\member_photo\\";
                            //  var path = ConfigurationManager.AppSettings["ProfileImagePath"].ToString();
                            var imagename = System.DateTime.Now.Ticks + "_" + postedFile.FileName;

                            var filePath = System.Web.Hosting.HostingEnvironment.MapPath("~//Images//post_orignal//" + imagename);
                            var filePathThume = System.Web.Hosting.HostingEnvironment.MapPath("~//Images//post_Thumbnail//" + imagename);

                            // var filePath = HttpContext.Current.Server.MapPath("~/Images/profile/" + postedFile.FileName);

                            var filePath_data = "Images//post_orignal//" + imagename;
                            var filePathThume_date = "Images//post_Thumbnail//" + imagename;
                            postedFile.SaveAs(filePath);

                            docfiles.Add(filePath);

                            //Test(imagepath);
                            //imageresize();

                            //this working
                            //Image image = Image.FromFile(filePath);

                            //Image thumb = image.GetThumbnailImage(120, 120, () => false, IntPtr.Zero);
                            ////thumb.Save(Path.ChangeExtension(, "thumb"));
                            //thumb.Save(Path.ChangeExtension(filePathThume, "thumb"));
                            //end working

                            // Load image.
                            Image image = Image.FromFile(filePath);
                            // // Bitmap bt_image = Bitmap.FromFile(filePath);
                            //   //Compute thumbnail size.
                            Size thumbnailSize = GetThumbnailSize(image);
                            // BitmapData thumbnailSize = ;
                            //   //Get thumbnail.
                            Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width,
                                thumbnailSize.Height, null, IntPtr.Zero);

                            //   //Save thumbnail.
                            thumbnail.Save(filePathThume);
                            //  ResizeImage(100, 100, filePath, filePathThume);
                            //string I = ResizeImage(11, 11, filePath, filePathThume);
                            //postedFile.SaveAs(filePathThume+I);
                            // yourImage = resizeImage(image, new Size(50, 50));
                            //docfiles.Add(filePathThume + I);
                            try
                            {
                                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + filePath_data + "" + filePathThume_date);

                                string re_result = save_poststuff(httpRequest.Params["Title"], httpRequest.Params["Desc"],
                                    httpRequest.Params["Categories_id"], httpRequest.Params["Price"], httpRequest.Params["Member_id"], filePathThume_date, filePath_data);
                                if (re_result == "0")
                                {
                                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + 6);

                                    result = Request.CreateResponse(HttpStatusCode.BadRequest, 0);
                                }
                                else if (re_result == "3")
                                {

                                    result = Request.CreateResponse(HttpStatusCode.Accepted, 6);
                                }
                                else
                                {
                                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + 7);

                                    result = Request.CreateResponse(HttpStatusCode.Accepted, 1);

                                }
                            }
                            catch (Exception ex)
                            {
                                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + ex.ToString());

                                result = Request.CreateResponse(HttpStatusCode.BadRequest, 0);
                            }
                        }
                    }

                    // String str = updateprofileimage(httpRequest["member_id"], imagepath);

                    //  result = Request.CreateResponse(HttpStatusCode.Created, imagepath);
                }
                else
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + 9);

                    result = Request.CreateResponse(HttpStatusCode.BadRequest, 0);
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + ex.ToString());
                result = Request.CreateResponse(HttpStatusCode.BadRequest, 0);

                return null;
            }
            return result;
        }

        public bool ThumbnailCallback()
        {
            return false;
        }
        [HttpPost]
        public string user_feedback([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            if (parameter["item_id"].ToString() == null || parameter["item_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "item id not found"); }

            if (parameter["current_member_id"].ToString() == null || parameter["current_member_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "member id not found"); }

            if (parameter["feedback_member_id"].ToString() == null || parameter["feedback_member_id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "feedback id not found"); }

            if (parameter["value_rating"].ToString() == null || parameter["value_rating"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "values  not found"); }

            if (parameter["comments"].ToString() == null || parameter["comments"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "text   not found"); }

            if (parameter["id"].ToString() == null || parameter["id"].ToString() == "")
            { return BLGeneralUtil.return_ajax_string("0", "order id not found"); }



            try
            {
                DataTable dt_feedbackcheck = objmasters.checkfeedback(parameter["current_member_id"].ToString(), parameter["item_id"].ToString(), parameter["id"].ToString());
                if (dt_feedbackcheck == null || dt_feedbackcheck.Rows.Count == 0)
                {

                    return BLGeneralUtil.return_ajax_string("1", "feedback already provided");

                }

                DS_General.QUA_feedbackRow new_row = objgeneral.QUA_feedback.NewQUA_feedbackRow();

                new_row.sr_no = "1";
                new_row.current_Member_id = parameter["current_member_id"].ToString();
                new_row.feedback_member_id = parameter["feedback_member_id"].ToString();
                new_row.values_rating = Convert.ToDecimal(parameter["value_rating"].ToString());
                new_row.comments = parameter["comments"].ToString();
                new_row.item_id = parameter["item_id"].ToString();
                new_row.created_by = "System";
                new_row.created_date = System.DateTime.Now;
                new_row.created_host = HttpContext.Current.Request.UserHostName;

                objgeneral.QUA_feedback.AddQUA_feedbackRow(new_row);

                DataTable dt_feedback = objmasters.getpartication(parameter["current_member_id"].ToString(), parameter["item_id"].ToString(), parameter["id"].ToString());
                if (dt_feedback != null)
                {

                    dt_feedback.Rows[0]["is_survey_done"] = 'Y';

                    objgeneral.QUA_feedback_participants.ImportRow(dt_feedback.Rows[0]);
                }

                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "try again");
                }

                objBLReturnObject = objmasters.save_feedback(objgeneral);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    flag = true;

                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not " + "status" + ex.StackTrace);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            if (flag == true)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not" + "status" + "1");
                return BLGeneralUtil.return_ajax_string("1", "Thanks for your Feedback ");
            }
            else
            {
                return BLGeneralUtil.return_ajax_string("0", "try again");
            }
        }

        //update profile 


        //update Notification
        [HttpPost]
        public string update_notification([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;
            Decimal sellect_amount;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            try
            {

                DataTable dt = objmasters.get_notification(parameter["doc_no"].ToString());
                if (dt != null)
                {


                    dt.Rows[0]["is_read"] = "Y";
                    dt.Rows[0]["is_accept"] = "Y";
                    dt.Rows[0]["last_modified_by"] = parameter["Member_id"];
                    dt.Rows[0]["last_modified_date"] = System.DateTime.Now;
                    objgeneral.QUA_notification.ImportRow(dt.Rows[0]);
                }



                // set the item   notification


                objBLReturnObject = objmasters.update_notification(objgeneral);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    flag = true;

                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not " + "status" + ex.StackTrace);
                // return BLGeneralUtil.return_ajax_string("0", ex.Message);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            if (flag == true)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not" + "status" + "1");

                return BLGeneralUtil.return_ajax_string("1", "Read successful");
                //return BLGeneralUtil.return_ajax_string("1", "data save successful");
            }
            else
            {
                return BLGeneralUtil.return_ajax_string("0", "try again");
                //return BLGeneralUtil.return_ajax_string("0", "try again");
            }
        }


        //05-12-2016
        [HttpPost]
        public string get_totaltoken_debit_credit_QUA([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            DataTable dtMemberDetail = new DataTable();
            string result = "";
            try
            {
                //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());

                dtMemberDetail = objmasters.token_bal(parameter["member_id"].ToString());
                if (dtMemberDetail != null)
                {
                    result = GetJson1(dtMemberDetail);
                }
                else
                {

                    return BLGeneralUtil.return_ajax_string("0", "no Data Found");
                }

                return BLGeneralUtil.return_ajax_data("1", result);


            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }

        }

        [HttpPost]
        public string gettokentrationdatauser_QUA([FromBody] JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            string result = "";
            //  bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            DataTable dt = objmasters.tokentrationdata(parameter["member_id"].ToString());
            try
            {
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                else
                {

                    return BLGeneralUtil.return_ajax_string("0", "no Data Found");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + 0);

                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            return BLGeneralUtil.return_ajax_data("1", result);


        }
        [HttpPost]
        public string SetUserAccount_QUA([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            try
            {
                bool security_flag = true; //objmasters.securityCheck(parameter["member_id"].ToString(), parameter["Token"].ToString());
                if (security_flag == true)
                {
                    DataTable dtuser = objmasters.getreedemcheck(parameter["member_id"].ToString(), parameter["amount"].ToString());
                    if (dtuser == null || dtuser.Rows.Count < 0)
                    {
                        return BLGeneralUtil.return_ajax_string("0", "insufficient balance");
                        //  return "";
                    }
                    if (dtuser != null && dtuser.Rows.Count > 0)
                    {
                        #region user bank details
                        DS_Admin.member_masterRow objNew = objDS_Admin.member_master.Newmember_masterRow();

                        dtuser.Rows[0]["bank_account_name"] = parameter["bank_account_name"].ToString();
                        dtuser.Rows[0]["ifsc_code"] = parameter["ifsc_code"].ToString();
                        dtuser.Rows[0]["account_number"] = parameter["account_number"].ToString();

                        objDS_Admin.member_master.ImportRow(dtuser.Rows[0]);
                        #endregion


                        #region reedam transtration

                        DS_Transtration.fn_token_reedemptionRow objredeeamrow = objDs_trastration.fn_token_reedemption.Newfn_token_reedemptionRow();
                        objredeeamrow.doc_date = System.DateTime.Now;
                        objredeeamrow.doc_no = "1";
                        objredeeamrow.token_request_doc_no = "11";
                        objredeeamrow.is_transtion_member_id = parameter["member_id"].ToString();
                        objredeeamrow.amount = Convert.ToDecimal(parameter["amount"].ToString());
                        objredeeamrow.payment_status = "N";
                        objredeeamrow.request_date_time = System.DateTime.Now;

                        objredeeamrow.is_active = "Y";
                        objredeeamrow.created_by = parameter["member_id"].ToString();
                        objredeeamrow.created_date = System.DateTime.Now;
                        objredeeamrow.created_host = HttpContext.Current.Request.UserHostName; ;

                        objDs_trastration.fn_token_reedemption.Addfn_token_reedemptionRow(objredeeamrow);


                        #endregion
                        //   DataTable dt_redeam = objmasters.getredeam_user(parameter["id"].ToString());

                        DataTable dt_token_bal = objmasters.gettokenbal(parameter["member_id"].ToString());

                        #region token balance update
                        dt_token_bal.Rows[0]["total_debit"] = (Convert.ToDecimal(dt_token_bal.Rows[0]["total_debit"].ToString()) + (Convert.ToDecimal(parameter["amount"].ToString())));
                        dt_token_bal.Rows[0]["balance_token"] = (Convert.ToDecimal(dt_token_bal.Rows[0]["balance_token"].ToString()) - (Convert.ToDecimal(parameter["amount"].ToString())));
                        objDs_trastration.fn_token_balance.ImportRow(dt_token_bal.Rows[0]);
                        #endregion

                        #region transtration new

                        DS_Transtration.fn_token_transtionRow token_transtration_row = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                        token_transtration_row.doc_no = "1";
                        token_transtration_row.doc_date = System.DateTime.Now;
                        token_transtration_row.ref_transtion_type = "redeem";
                        token_transtration_row.ref_transtion_doc_date = System.DateTime.Now;
                        token_transtration_row.ref_transtion_doc_no = "11";
                        token_transtration_row.amount = (Convert.ToDecimal(parameter["amount"].ToString()));
                        token_transtration_row.balance_after_transtion = Convert.ToDecimal(Convert.ToDecimal(dt_token_bal.Rows[0]["balance_token"].ToString()));
                        token_transtration_row.type_credit_debit = "debit";
                        token_transtration_row.member_id = parameter["member_id"].ToString();
                        token_transtration_row.created_by = parameter["member_id"].ToString();
                        token_transtration_row.created_date = System.DateTime.Now;
                        token_transtration_row.created_host = HttpContext.Current.Request.UserHostName;
                        objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row);


                        #endregion
                    }

                    objBLReturnObject = objmasters.updateuseraccount(objDS_Admin, objDs_trastration);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                        return BLGeneralUtil.return_ajax_string("1", "Your request has been submitted. Your bank account will be credited within 24Hours");
                        // return "Pass";
                    }
                    else
                    {
                        ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                        return BLGeneralUtil.return_ajax_string("0", "Please try again");
                    }
                }

                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "Please try again");
                    // return "securityIssue";
                }
            }
            catch (Exception e)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + e.StackTrace);
                return BLGeneralUtil.return_ajax_string("0", "Please try again");
            }

        }
        [HttpPost]
        public string savetokentran_QUA([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();

            try
            {
                bool security_flag = true;
                //= objmasters.securityCheck(parameter["memberid"].ToString(), parameter["token_id"].ToString());
                if (security_flag == true)
                {
                    DataTable dtMemberDetail = new DataTable();

                    dtMemberDetail = objmasters.token_bal(parameter["memberid"].ToString());

                    if (Convert.ToDecimal(dtMemberDetail.Rows[0]["balance_token"]) >= Convert.ToDecimal(parameter["amount"]))
                    {
                        #region reedam transtration

                        DS_Transtration.fn_token_transferRow transfer_row = objDs_trastration.fn_token_transfer.Newfn_token_transferRow();
                        transfer_row.doc_no = "1";
                        transfer_row.doc_date = System.DateTime.Now;
                        transfer_row.from_meber_id = parameter["memberid"].ToString();
                        transfer_row.to_meber_id = parameter["to_member"].ToString();
                        transfer_row.amount = Convert.ToDecimal(parameter["amount"].ToString());
                        transfer_row.is_active = "Y";
                        transfer_row.created_by = parameter["memberid"].ToString();
                        transfer_row.created_date = System.DateTime.Now;
                        objDs_trastration.fn_token_transfer.Addfn_token_transferRow(transfer_row);


                        #endregion

                        #region token balance update for from member
                        DataTable dtform_member = objmasters.gettokenbal(parameter["memberid"].ToString());

                        dtform_member.Rows[0]["total_debit"] = (Convert.ToDecimal(dtform_member.Rows[0]["total_debit"].ToString()) + Convert.ToDecimal(parameter["amount"].ToString()));
                        dtform_member.Rows[0]["balance_token"] = (Convert.ToDecimal(dtform_member.Rows[0]["balance_token"].ToString()) - Convert.ToDecimal(parameter["amount"].ToString()));
                        objDs_trastration.fn_token_balance.ImportRow(dtform_member.Rows[0]);
                        #endregion

                        #region token balance update for to member
                        DataTable dtto_member = objmasters.gettokenbal(parameter["to_member"].ToString());

                        dtto_member.Rows[0]["total_credit"] = (Convert.ToDecimal(dtto_member.Rows[0]["total_debit"].ToString()) + Convert.ToDecimal(parameter["amount"].ToString()));
                        dtto_member.Rows[0]["balance_token"] = (Convert.ToDecimal(dtto_member.Rows[0]["balance_token"].ToString()) + Convert.ToDecimal(parameter["amount"].ToString()));
                        objDs_trastration.fn_token_balance.ImportRow(dtto_member.Rows[0]);
                        #endregion


                        #region transatrtion

                        DS_Transtration.fn_token_transtionRow token_transtration_row = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                        token_transtration_row.doc_no = "1";
                        token_transtration_row.doc_date = System.DateTime.Now;
                        token_transtration_row.ref_transtion_type = "transfer";
                        token_transtration_row.ref_transtion_doc_date = System.DateTime.Now;
                        token_transtration_row.ref_transtion_doc_no = "12";
                        token_transtration_row.amount = (Convert.ToDecimal(objDs_trastration.fn_token_transfer.Rows[0]["amount"].ToString()));
                        token_transtration_row.balance_after_transtion = Convert.ToDecimal(dtform_member.Rows[0]["balance_token"].ToString());
                        token_transtration_row.type_credit_debit = "debit";
                        token_transtration_row.member_id = parameter["memberid"].ToString();
                        token_transtration_row.created_by = parameter["memberid"].ToString();
                        token_transtration_row.created_date = System.DateTime.Now;
                        token_transtration_row.created_host = HttpContext.Current.Request.UserHostName;
                        objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row);


                        DS_Transtration.fn_token_transtionRow token_transtration_row2 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                        token_transtration_row2.doc_no = "2";
                        token_transtration_row2.doc_date = System.DateTime.Now;
                        token_transtration_row2.ref_transtion_type = "transfer";
                        token_transtration_row2.ref_transtion_doc_date = System.DateTime.Now;
                        token_transtration_row2.ref_transtion_doc_no = "12";
                        token_transtration_row2.amount = (Convert.ToDecimal(objDs_trastration.fn_token_transfer.Rows[0]["amount"].ToString()));
                        token_transtration_row2.type_credit_debit = "Credit";
                        token_transtration_row2.member_id = parameter["to_member"].ToString();
                        token_transtration_row2.balance_after_transtion = (Convert.ToDecimal(dtto_member.Rows[0]["balance_token"].ToString()));

                        token_transtration_row2.created_by = parameter["to_member"].ToString();
                        token_transtration_row2.created_date = System.DateTime.Now;
                        token_transtration_row2.created_host = HttpContext.Current.Request.UserHostName;
                        objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row2);




                        #endregion


                        objBLReturnObject = objmasters.updatetokentranfer(objDs_trastration);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {
                            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                            return BLGeneralUtil.return_ajax_string("1", "Token(s) transferred successfully");
                        }
                        else
                        {
                            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "fail");
                            return BLGeneralUtil.return_ajax_string("0", "try again ");
                        }

                    }
                    else
                    {
                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "ExceedAmmount");
                        return BLGeneralUtil.return_ajax_string("0", "Total Balance Exceeded ");
                        //return "ExceedAmmount";
                    }
                }

                else
                {
                    return BLGeneralUtil.return_ajax_string("0", "try again ");
                }
            }
            catch (Exception e)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + e.StackTrace);
                return BLGeneralUtil.return_ajax_string("0", "try again ");
            }

        }
        [HttpPost]
        public HttpResponseMessage profileimage()
        {
            HttpResponseMessage result = null;
            try
            {
                string sPath = "";
                //sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/locker/");

                //  ServerLog.Log("callmethod");
                string imagepath = "";
                string re_status = "";
                var httpRequest = HttpContext.Current.Request;
                Dictionary<string, string> array1 = new Dictionary<string, string>();
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                if (httpRequest.Params["email_id"].ToString() == "")
                {

                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                if (httpRequest.Files.Count > 0)
                {
                    //ServerLog.Log("if in");
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        // ServerLog.Log("test" + postedFile);
                        if (postedFile.FileName.Trim() != "")
                        {
                            //var path = "D:\\jaimin data\\CampusConcierge\\CampusConcierge\\member_uploaded_detail\\member_photo\\";
                            var path = ConfigurationManager.AppSettings["ProfileImagePath"].ToString();
                            var imagename = postedFile.FileName;

                            //var filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/../CampusConcierge/CampusConcierge/member_uploaded_detail/member_photo/" + imagename);
                            // var filePath = HttpContext.Current.Server.MapPath("~/Images/profile/" + postedFile.FileName);

                            var filePath = path + imagename;

                            postedFile.SaveAs(filePath);

                            docfiles.Add(filePath);

                            imagepath = "member_uploaded_detail/member_photo/" + imagename;

                            re_status = update_profile(httpRequest.Params["email_id"].ToString(), imagepath);
                        }
                    }


                    if (re_status == "1")
                    {
                        result = Request.CreateResponse(HttpStatusCode.Created, imagepath);
                    }
                    else
                    {
                        result = Request.CreateResponse(HttpStatusCode.NotAcceptable, re_status);

                    }

                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log(ex.Message.ToString() + Environment.NewLine + ex.StackTrace);

                return null;
            }
            return result;
        }

        public string update_profile(string email, string path)
        {
            //ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;
            Decimal sellect_amount;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            try
            {

                DataTable dt = objmasters.get_member_detail(email);
                if (dt != null)
                {


                    dt.Rows[0]["image"] = path;
                    dt.Rows[0]["last_modified_by"] = email;
                    dt.Rows[0]["last_modified_date"] = System.DateTime.Now;
                    objgeneral.member_master.ImportRow(dt.Rows[0]);
                }



                // set the item   notification


                objBLReturnObject = objmasters.updatemember(objgeneral);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    flag = true;

                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not " + "status" + ex.StackTrace);
                // return BLGeneralUtil.return_ajax_string("0", ex.Message);
                return "0";
            }
            if (flag == true)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not" + "status" + "1");

                return "1";
                //return BLGeneralUtil.return_ajax_string("1", "data save successful");
            }
            else
            {
                return "0";
                //return BLGeneralUtil.return_ajax_string("0", "try again");
            }
        }

        static Size GetThumbnailSize(Image original)
        {

            const int maxPixels = 200;

            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }
            else
            {
                factor = (double)maxPixels / originalHeight;
            }


            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));





        }

        [HttpPost]
        public HttpResponseMessage barterpost()
        {
            HttpResponseMessage result = null;
            try
            {
                string sPath = "";


                string imagepath = "";
                var httpRequest = HttpContext.Current.Request;
                Dictionary<string, string> array1 = new Dictionary<string, string>();
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

                //ServerLog.Log_QUA(httpRequest.Params["Title"].ToString());
                //ServerLog.Log_QUA(httpRequest.Params["Desc"].ToString());
                //ServerLog.Log_QUA(httpRequest.Params["Categories_id"].ToString());
                //ServerLog.Log_QUA(httpRequest.Params["Price"].ToString());
                //ServerLog.Log_QUA(httpRequest.Params["Member_id"].ToString());

                if (httpRequest.Params["Title"] == "" || httpRequest.Params["Title"] == null)
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + httpRequest.Params["Title"]);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, 7);

                }

                else if (httpRequest.Params["Desc"] == "" || httpRequest.Params["Desc"] == null)
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "Desc not found");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, 2);
                }

                else if (httpRequest.Params["Item_id"] == "" || httpRequest.Params["Item_id"] == null)
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "Categories_id not found");

                    return Request.CreateResponse(HttpStatusCode.BadRequest, 3);
                }
                else if (httpRequest.Params["buy_member_id"] == "" || httpRequest.Params["buy_member_id"] == null)
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "Price not found");

                    return Request.CreateResponse(HttpStatusCode.BadRequest, 4);
                }
                else if (httpRequest.Params["Member_id"] == "" || httpRequest.Params["Member_id"] == null)
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "Member_id not found");

                    return Request.CreateResponse(HttpStatusCode.BadRequest, 5);
                }
                if (httpRequest.Files.Count > 0)
                {
                    //ServerLog.Log("if in");
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        ServerLog.Log("test" + postedFile);
                        if (postedFile.FileName.Trim() != "")
                        {
                            //var path = "D:\\jaimindata\\CampusConcierge\\CampusConcierge\\member_uploaded_detail\\member_photo\\";
                            //  var path = ConfigurationManager.AppSettings["ProfileImagePath"].ToString();
                            var imagename = System.DateTime.Now.Ticks + "_" + postedFile.FileName;

                            var filePath = System.Web.Hosting.HostingEnvironment.MapPath("~//Images//post_orignal//" + imagename);
                            var filePathThume = System.Web.Hosting.HostingEnvironment.MapPath("~//Images//post_Thumbnail//" + imagename);

                            // var filePath = HttpContext.Current.Server.MapPath("~/Images/profile/" + postedFile.FileName);

                            var filePath_data = "Images//post_orignal//" + imagename;
                            var filePathThume_date = "Images//post_Thumbnail//" + imagename;
                            postedFile.SaveAs(filePath);

                            docfiles.Add(filePath);
                            //array1[file] = "~/imageFolder/DrvPhoto/" + postedFile.FileName;
                            imagepath = "member_uploaded_detail/member_photo/" + imagename;

                            //Test(imagepath);
                            //imageresize();

                            //this working
                            //Image image = Image.FromFile(filePath);

                            //Image thumb = image.GetThumbnailImage(120, 120, () => false, IntPtr.Zero);
                            ////thumb.Save(Path.ChangeExtension(, "thumb"));
                            //thumb.Save(Path.ChangeExtension(filePathThume, "thumb"));
                            //end working

                            // Load image.
                            /// Image image = Image.FromFile(filePath);

                            // Compute thumbnail size.
                            ///Size thumbnailSize = GetThumbnailSize(image);

                            // Get thumbnail.
                            //Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width,
                            //    thumbnailSize.Height, null, IntPtr.Zero);

                            // Save thumbnail.
                            //thumbnail.Save(filePathThume);


                            try
                            {
                                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + filePath_data + "" + filePathThume_date);

                                string re_result = save_barter(httpRequest.Params["Title"], httpRequest.Params["Desc"],
                                    httpRequest.Params["Item_id"], httpRequest.Params["buy_member_id"], httpRequest.Params["Member_id"], filePath_data, filePathThume_date);

                                if (re_result == "0")
                                {
                                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + 6);

                                    result = Request.CreateResponse(HttpStatusCode.BadRequest, 0);
                                }
                                else if (re_result == "3")
                                {

                                    result = Request.CreateResponse(HttpStatusCode.Accepted, 6);
                                }
                                else
                                {
                                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + 7);

                                    result = Request.CreateResponse(HttpStatusCode.Accepted, 1);

                                }
                            }
                            catch (Exception ex)
                            {
                                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + ex.ToString());

                                result = Request.CreateResponse(HttpStatusCode.BadRequest, 0);
                            }
                        }
                    }

                    // String str = updateprofileimage(httpRequest["member_id"], imagepath);

                    //  result = Request.CreateResponse(HttpStatusCode.Created, imagepath);
                }
                else
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + 9);

                    result = Request.CreateResponse(HttpStatusCode.BadRequest, 0);
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + ex.ToString());
                result = Request.CreateResponse(HttpStatusCode.BadRequest, 0);

                return null;
            }
            return result;
        }

        [HttpGet]
        public string emailmail()
        {
            DataTable dt = objmasters.getmail();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                emailsend(dt.Rows[i]["email"].ToString());

            }
            return "send";
        }

        public string emailsend(string email_id)
        {


            var a = "http://viid.me/q0cKos";

            //StreamReader sr;
            //string str = "<body><p>You have been invited to register with my campus concierge.</p><br/><b>LINK</b></body>";

            string str = "<body>";
            // str += "<div style='border-radius: 5px; border: 2px solid black;padding: 20px;width : auto;height : auto;font-family: Muli'>";
            //// str += "<img src=\"" + ConfigurationManager.AppSettings["Domain"] + "/img/favicon.png\" width=\"76px\" Height=\"80px\" style='margin-left:40%';font-family: Muli>";
            // str += "<br />";
            // //str += "<h4>Hello USER,</h4>";
            // str += "<p>Your friend  has invited you to join the fun on My Campus Concierge Inc. Click on the button below to accept this invitation and see what all the rave is about.</p>";
            // str += "<br/>";
            // str += "<button style='margin-left: 0%;border-radius: 70px;font-size: 15px;font-family: Kavoon ;background: #00529B;border-color: #00529B;color: #fff; width: 120px;height:30px'><a style='color:white;text-decoration: none;' href ='" + ConfigurationManager.AppSettings["iphone"] + "'>App Store</a></button>";
            // str += "<label style='margin-left: 25%;font-size: 15px;font-family: Kavoon ;background: #00529B;border-color: #00529B;color: #fff; width: 120px;height:30px'>Join the fun!</label>";
            // str += "<button style='float:right;border-radius: 70px;font-size: 15px;font-family: Kavoon ;background: #00529B;border-color: #00529B;color: #fff; width: 120px;height:30px'><a style='color:white;text-decoration: none;' href ='" + ConfigurationManager.AppSettings["android"] + "'>Play Store</a></button>";
            str += "<button style='float:right;border-radius: 70px;font-size: 15px;font-family: Kavoon ;background: #00529B;border-color: #00529B;color: #fff; width: 120px;height:30px'><a style='color:white;text-decoration: none;' href ='" + a + "'>Play Store</a></button>";
            str += "</body>";


            string strdata = "";
            string TITLE = "Please click following link ";

            // string LINK = "<a href ='" + ConfigurationManager.AppSettings["Domain"] + "/welcome.html" + "'" + "> " + ConfigurationManager.AppSettings["Domain"] + "/welcome.html </a>";
            //str = str.Replace("LINK", LINK);

            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                ///SmtpServer.Credentials = new System.Net.NetworkCredential("info@mycampusconcierge.com", "Logistix01");
                SmtpServer.Credentials = new System.Net.NetworkCredential("noreplay042", "jaimin14493");
                // SmtpServer.Credentials = new System.Net.NetworkCredential("jaiminpatel520@gmail.com", "jaimin****14493");
                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(email_id, "New Product", System.Text.Encoding.UTF8);
                mail.To.Add(email_id);

                mail.Subject = "New product";
                mail.Body = str;
                mail.IsBodyHtml = true;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                //mail.ReplyTo = new MailAddress(user_id);
                SmtpServer.Send(mail);
                return "send";
            }
            catch (Exception ex)
            {
                return ex.ToString();
                //                return false;

            }

        }

        //[HttpPost]
        //public string gettokentrationdatauser_DLT([FromBody] JObject parameter)
        //{

        //    string result = "";
        //    //  bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());

        //    DataTable dt = objmasters.tokentrationdata(parameter["member_id"].ToString());
        //    if (dt != null)
        //    {
        //        result = GetJson1(dt);
        //    }
        //    return BLGeneralUtil.return_ajax_data("1", result); ;


        //}

        #region Json from Datatable

        public string GetJson1(DataTable dt)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Dictionary<string, string>> dataRows = new List<Dictionary<string, string>>(); // will contain datarows as dictionary objects
            if (dt == null)
                return ser.Serialize(dataRows);
            //Convert DataTable to List<Dictionary<string, string>> data structure
            foreach (DataRow VDataRow in dt.Rows)
            {
                var Row = new Dictionary<string, string>(); // DataRow as key-value pairs where key=columnName and value=fieldValue 
                foreach (DataColumn Column in dt.Columns)
                {

                    Row.Add(Column.ColumnName, VDataRow[Column].ToString());

                }
                dataRows.Add(Row);
            }
            return ser.Serialize(dataRows); // convert list to JSON string 

        }



        #endregion

        #region update barter offer reject system order accept

        private bool rejectbartoffer(string item, string id, string name)
        {

            bool flage = false;

            try
            {
                DataTable DTBarterOffer = objmasters.itemwisebarter_data(item);
                if (DTBarterOffer != null && DTBarterOffer.Rows.Count > 0)
                {
                    for (var i = 0; i < DTBarterOffer.Rows.Count; i++)
                    {


                        DTBarterOffer.Rows[i]["is_accepted"] = "C"; // Reject Other Barter offer
                        objgeneral.QUA_barter_master.ImportRow(DTBarterOffer.Rows[i]);


                        #region create notification
                        DS_General.QUA_notificationRow new_row_rej = objgeneral.QUA_notification.NewQUA_notificationRow();
                        new_row_rej.doc_no = "1" + i;
                        new_row_rej.from_member = id;
                        new_row_rej.to_member = DTBarterOffer.Rows[i]["Buyer_Member_id"].ToString();
                        new_row_rej.notification_date = System.DateTime.Now;
                        new_row_rej._event = "BUY";
                        new_row_rej.template = "sorry your barter request was rejected  for  " + name;

                        new_row_rej.item_id = item;
                        new_row_rej.is_read = "N";
                        new_row_rej.is_accept = "N";
                        new_row_rej.is_active = "Y";
                        new_row_rej.created_by = id;
                        new_row_rej.created_date = System.DateTime.Now;
                        new_row_rej.created_host = HttpContext.Current.Request.UserHostName;

                        objgeneral.QUA_notification.AddQUA_notificationRow(new_row_rej);

                        #endregion end notification

                        #region alert message
                        DS_General.alertmessageRow alert_row1 = objgeneral.alertmessage.NewalertmessageRow();

                        alert_row1.Doc_no = "101" + i;



                        alert_row1.to_member = DTBarterOffer.Rows[i]["Buyer_Member_id"].ToString();
                        //   alert_row.to_member_role = "ST";
                        alert_row1._event = "Request";
                        alert_row1.template = " sorry your barter request was rejected for  " + name;


                        alert_row1.notification_date = System.DateTime.Now;

                        alert_row1.is_read = "N";
                        alert_row1.is_active = "Y";
                        alert_row1.AppName = "QUA";

                        alert_row1.senddate = System.DateTime.Now;
                        alert_row1.utc_time = System.DateTime.UtcNow.TimeOfDay;
                        alert_row1.created_by = "system";
                        alert_row1.created_date = System.DateTime.Now;
                        alert_row1.created_host = HttpContext.Current.Request.UserHostName;

                        objgeneral.alertmessage.AddalertmessageRow(alert_row1);
                        #endregion alert message
                    }

                    flage = true;

                }
                else
                {

                    flage = true;
                }

            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "not" + "error" + ex.Message);
                flage = false;
            }



            return flage;

        }
        #endregion



        public Image resizeImage(int newWidth, int newHeight, string stPhotoPath)
        {
            Image imgPhoto = Image.FromFile(stPhotoPath);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                          PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                         imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }

        [HttpPost]
        public HttpResponseMessage PostStuff()
        {

            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                int count = 0;
                var docfiles = new List<string>();
                try
                {
                    if (httpRequest.Params["Title"] == "" || httpRequest.Params["Title"] == null)
                    {
                        ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + httpRequest.Params["Title"]);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, 7);

                    }

                    else if (httpRequest.Params["Desc"] == "" || httpRequest.Params["Desc"] == null)
                    {
                        ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "Desc not found");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, 2);
                    }

                    else if (httpRequest.Params["Categories_id"] == "" || httpRequest.Params["Categories_id"] == null)
                    {
                        ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "Categories_id not found");

                        return Request.CreateResponse(HttpStatusCode.BadRequest, 3);
                    }
                    else if (httpRequest.Params["Price"] == "" || httpRequest.Params["Price"] == null)
                    {
                        ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "Price not found");

                        return Request.CreateResponse(HttpStatusCode.BadRequest, 4);
                    }
                    else if (httpRequest.Params["Member_id"] == "" || httpRequest.Params["Member_id"] == null)
                    {
                        ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "Member_id not found");

                        return Request.CreateResponse(HttpStatusCode.BadRequest, 5);
                    }
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var filePath1 = HttpContext.Current.Server.MapPath("~/Images/" + postedFile.FileName);

                        var filePath = System.Web.Hosting.HostingEnvironment.MapPath("~//Images//post_orignal//" + postedFile.FileName);
                        var filePathThume = System.Web.Hosting.HostingEnvironment.MapPath("~//Images//post_Thumbnail//" + postedFile.FileName);

                        postedFile.SaveAs(filePath);

                        docfiles.Add(filePath);
                        Stream strm = postedFile.InputStream;

                       // Compressimage(strm, filePathThume, postedFile.FileName);
                        GenerateThumbnails(0.3, strm, filePathThume);
                        //dt.Rows[count]["original"] = "//Images//post_orignal//" + postedFile.FileName;
                        //dt.Rows[count]["small_image"] = "//Images//post_Thumbnail//" + postedFile.FileName;
                        DS_General.QUA_image_parameterRow img_row = objgeneral.QUA_image_parameter.NewQUA_image_parameterRow();
                        img_row.SrNo = Convert.ToString(count + "11");
                        img_row.Item_id = Convert.ToString(count + "1");
                        img_row.original_image_path = "//Images//post_orignal//" + postedFile.FileName;
                        img_row.thumbnail_image_path = "//Images//post_Thumbnail//" + postedFile.FileName;
                        objgeneral.QUA_image_parameter.AddQUA_image_parameterRow(img_row);
                        count++;
                    }
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + "" + "" + "");

                    string re_result = save_poststuff(httpRequest.Params["Title"], httpRequest.Params["Desc"],
                        httpRequest.Params["Categories_id"], httpRequest.Params["Price"], httpRequest.Params["Member_id"], "", "");
                    if (re_result == "0")
                    {
                        ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + 6);

                        response = Request.CreateResponse(HttpStatusCode.BadRequest, 0);
                    }
                    else if (re_result == "3")
                    {

                        response = Request.CreateResponse(HttpStatusCode.Accepted, 6);
                    }
                    else
                    {
                        ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + "status" + "" + 7);

                        response = Request.CreateResponse(HttpStatusCode.Accepted, 1);

                    }
                }
                catch (Exception ex)
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, 0);

                }
                // response = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return response;
        }
        public static void Compressimage(Stream sourcePath, string targetPath, String filename)
        {


            try
            {
                using (var image = Image.FromStream(sourcePath))
                {
                    float maxHeight = 20.0f;
                    float maxWidth = 450.0f;
                    int newWidth;
                    int newHeight;
                    string extension;
                    Bitmap originalBMP = new Bitmap(sourcePath);
                    int originalWidth = originalBMP.Width;
                    int originalHeight = originalBMP.Height;

                    if (originalWidth > maxWidth || originalHeight > maxHeight)
                    {

                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;



                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else
                    {
                        newWidth = (int)originalWidth;
                        newHeight = (int)originalHeight;

                    }
                    Bitmap bitMAP1 = new Bitmap(originalBMP, 350, 350);
                    Graphics imgGraph = Graphics.FromImage(bitMAP1);
                    extension = Path.GetExtension(targetPath);
                    if (extension == ".png" || extension == ".gif")
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);


                        bitMAP1.Save(targetPath, image.RawFormat);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();
                    }
                    else if (extension == ".jpg")
                    {

                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;

                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        bitMAP1.Save(targetPath, jpgEncoder, myEncoderParameters);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();

                    }


                }

            }
            catch (Exception)
            {
                throw;

            }
        }
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = Image.FromStream(sourcePath))
            {
                // can given width of image as we want
                var newWidth = (int)(image.Width * scaleFactor);
                // can given height of image as we want
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }
    }
}
