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
using BLL.Masters;
using Kaliido.QuickBlox;
using Kaliido.QuickBlox.Parameters;

namespace WebApiCampusConcierge.Controllers
{
    public class Qua_GeneralController : ApiController
    {
        #region variable
        Masters objmasters = new Masters();
        DS_General objgeneral = new DS_General();
        DS_Transtration objDs_trastration = new DS_Transtration();
        #endregion
        // GET api/qua_general
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/qua_general/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/qua_general
        public void Post([FromBody]string value)
        {
        }

        // PUT api/qua_general/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/qua_general/5
        public void Delete(int id)
        {
        }


        //registration Quaentana


        [HttpPost]
        public string save_registrationquantina([FromBody]JObject parameter)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");

            if (parameter["university"] == null || parameter["university"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "university not found");
            }
            else if (parameter["email_id"] == null || parameter["email_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "Email id not found");
            }
            else if (parameter["password"] == null || parameter["password"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "password not found");
            }
            else if (parameter["app_name"] == null || parameter["app_name"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "app name not found");
            }


            //check if exites user
            DataTable user = objmasters.ismemberexits(parameter["email_id"].ToString().ToLower());
            if (user != null && user.Rows.Count > 0)
            {
                return BLGeneralUtil.return_ajax_string("0", "this email Id is already registered ");
            }
            try
            {
                DS_General.member_masterRow objmember_maste = objgeneral.member_master.Newmember_masterRow();

                objmember_maste.member_code = "1";
                objmember_maste.app_name = parameter["app_name"].ToString();
                objmember_maste.university = parameter["university"].ToString();
                objmember_maste.first_name = parameter["name"].ToString();
                objmember_maste.email_id = parameter["email_id"].ToString().ToLower();
                EncryptPassword encrpt = new EncryptPassword();

                string encrpt_password = encrpt.Encrypt(parameter["password"].ToString());
                objmember_maste.email_rand_no = randomNo;
                objmember_maste.password = encrpt_password;
                objmember_maste.is_active = "N";
                objmember_maste.is_quantina = "Y";

                objmember_maste.created_by = parameter["email_id"].ToString();
                objmember_maste.created_date = System.DateTime.Now;
                objmember_maste.created_host = HttpContext.Current.Request.UserHostName;

                string BlobId = DoUserRegistrationQuickBlox(parameter["name"].ToString(), parameter["email_id"].ToString(), "admin@123");
                if (BlobId == null || BlobId == "")
                {
                    return BLGeneralUtil.return_ajax_string("0", "some thing went wrong");
                }
                else
                {
                    objmember_maste.BlobId = BlobId;
                    objgeneral.member_master.Addmember_masterRow(objmember_maste);
                }



                DS_Transtration.fn_token_balanceRow token_bal_row = objDs_trastration.fn_token_balance.Newfn_token_balanceRow();
                token_bal_row.doc_no = "1";
                token_bal_row.member_id = parameter["email_id"].ToString().ToLower();
                token_bal_row.total_debit = Convert.ToInt16(0.0);
                token_bal_row.total_credit = Convert.ToInt16(0.0);
                token_bal_row.balance_token = Convert.ToInt16(0.0);
                token_bal_row.balance_amount = Convert.ToInt16(0.0);

                token_bal_row.is_active = "Y";
                token_bal_row.created_by = parameter["email_id"].ToString().ToLower();
                token_bal_row.created_date = System.DateTime.Now;
                token_bal_row.created_host = HttpContext.Current.Request.UserHostName;

                objDs_trastration.fn_token_balance.Addfn_token_balanceRow(token_bal_row);



                objBLReturnObject = objmasters.save_regstration(objgeneral, objDs_trastration);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    try
                    {

                        // bool resultSendEmail = false;
                        bool resultSendEmail = profliecreationsendEmail(parameter["email_id"].ToString(), parameter["name"].ToString(), randomNo, parameter["university"].ToString(), "");
                        if (resultSendEmail == true) { flag = true; }
                    }
                    catch (Exception ex)
                    {
                        ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);
                        return BLGeneralUtil.return_ajax_string("0", ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);
                return BLGeneralUtil.return_ajax_string("0", ex.ToString());
            }
            if (flag == true)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "1");
                return BLGeneralUtil.return_ajax_string("1", "Thanks for signing up! Please click on the link sent to your email to confirm your account");
            }
            else
            {
                return BLGeneralUtil.return_ajax_string("0", "some things goes to wrong");
            }
        }


        [HttpPost]
        public string logincheck([FromBody]JObject parameter)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");


            if (parameter["email_id"] == null || parameter["email_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "Email id not found");
            }
            else if (parameter["password"] == null || parameter["password"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "password not found");
            }
            else if (parameter["app_name"] == null || parameter["app_name"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "app name not found");
            }




            try
            {
                EncryptPassword encrpt = new EncryptPassword();

                string encrpt_password = encrpt.Encrypt(parameter["password"].ToString());
                //check 
                DataTable user = objmasters.ismemberexits(parameter["email_id"].ToString().ToLower());
                if (user == null || user.Rows.Count < 0)
                {
                    return BLGeneralUtil.return_ajax_string("0", "this email Id is not registered ");
                }

                if (user.Rows[0]["email_rand_no"].ToString() != "0")
                {
                    return BLGeneralUtil.return_ajax_string("0", "This email ID has not yet been confirmed ");
                }
                else if (user.Rows[0]["is_quantina"].ToString() == null || user.Rows[0]["is_quantina"].ToString() == "")
                {
                    return BLGeneralUtil.return_ajax_string("0", "this email Id is not allowed in  this app ");
                }
                else if (user.Rows[0]["BlobId"].ToString() == null || user.Rows[0]["BlobId"].ToString() == "")
                {
                    return BLGeneralUtil.return_ajax_string("0", "Blob ID not Found ");
                }
                else if (user.Rows[0]["password"].ToString() != encrpt_password)
                {

                    return BLGeneralUtil.return_ajax_string("0", " password is wrong");
                }

                else if (user.Rows[0]["password"].ToString() == encrpt_password)
                {
                    if (user.Rows[0]["is_quantina"].ToString() == "Y")
                    {

                        return BLGeneralUtil.return_ajax_data("1", GetJson1(user));
                    }
                }
                else
                {



                }

            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);
                return BLGeneralUtil.return_ajax_string("0", ex.ToString());
            }
            if (flag == true)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "1");
                return BLGeneralUtil.return_ajax_string("1", "saved successfully");
            }
            else
            {
                return BLGeneralUtil.return_ajax_string("0", "some things goes to wrong");
            }
        }

        [HttpPost]
        public string forgotpassword([FromBody]JObject parameter)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");


            if (parameter["email_id"] == null || parameter["email_id"].ToString() == "")
            {
                return BLGeneralUtil.return_ajax_string("0", "Email id not found");
            }

            try
            {
                DataTable dt = objmasters.LoginCheck(parameter["email_id"].ToString());
                if (dt == null)
                {
                    return BLGeneralUtil.return_ajax_string("0", "No Datails Found");
                }
                else
                {
                    flag = forgotpassword(dt.Rows[0]["email_id"].ToString(), dt.Rows[0]["university"].ToString());

                }
            }
            catch (Exception ex)
            {

                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + ex.ToString() + "status" + "");
                return BLGeneralUtil.return_ajax_string("0", ex.Message);
            }
            if (flag == true)
            {
                return BLGeneralUtil.return_ajax_string("1", "please check  mail to update new password");
            }
            else
            {

                return BLGeneralUtil.return_ajax_string("0", "some thing went wrong");
            }

        }

        [HttpPost]
        public string sendmailforinvitefriend([FromBody]JObject parameter)
        {
            ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "call");
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //string[] persons = js.Deserialize<string[]>(parameter["dataemail"]["email_list_send"].ToString());


            emailSendFrined objsendmail = new emailSendFrined();
            string result = ""; bool resultSendEmail = false;
            bool security_flag = false;
            try
            {

                if (parameter["dataemail"]["email_list_send"].ToString() != "")
                {

                    objsendmail = parameter["dataemail"].ToObject<emailSendFrined>();


                }


                if (objsendmail.email_list_send != null && objsendmail.email_list_send.Length > 0)
                    for (int i = 0; i < objsendmail.email_list_send.Length; i++)
                    {
                        resultSendEmail = sendinvitemail(objsendmail.email_list_send[i]);
                    }



                if (resultSendEmail == true)
                {

                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                    return BLGeneralUtil.return_ajax_string("1", "Thanks for your referral");
                }
                else
                {
                    ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                    return BLGeneralUtil.return_ajax_string("0", "please Enter Email ID");
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log_QUA((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);
                return BLGeneralUtil.return_ajax_string("0", "some thing went wrong");
            }
            //return result;



        }
        #region QuickBlox

        public string Timestamp()
        {
            long ticks = DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000;
            return ticks.ToString();
        }

        public string DoUserRegistrationQuickBlox(string name, string email, string password)
        {
            try
            {
                //register user

                //  ResponseObjectInfo objResponseObjectInfo = new ResponseObjectInfo();

                string application_id = "49511";
                string auth_key = "YVfFMVjZryr3HC5";
                string timestamp = Timestamp();
                string nonce = new Random().Next().ToString();
                string auth_secret = "ktmQ32QKqHEMEHh";
                //string setAccountKey = "ckY9GmG3pkzKZr76RKh1";


                string result = null;
                var apiCredentials = new ApiCredential(application_id, auth_key, auth_secret);
                var adminCredentials = new UserCredential() { UserLogin = "quotes4transport", Password = "Matt2105*" };
                QuickbloxRestApi client = new QuickbloxRestApi(apiCredentials, adminCredentials);
                UserParameters UserParam = new UserParameters();


                UserParam.Email = email;
                UserParam.Password = password;
                UserParam.FullName = name;
                UserParam.Login = email;


                var users = client.RegisterAsync(UserParam);

                //objResponseObjectInfo.Status = 1;
                //objResponseObjectInfo.Message = "Success";
                //objResponseObjectInfo.dt_ReturnedTables = new dynamic[1];
                //objResponseObjectInfo.dt_ReturnedTables[0] = users;

                return users;
            }
            catch (Exception ex)
            {
                return null;

            }

        }

        [HttpPost]
        public void RegisterAndUpdateBlobIdFromConfigMaster([FromBody]JObject parameter)
        {
            //  ResponseObjectInfo objResponseObjectInfo = new ResponseObjectInfo();
            try
            {
                BLReturnObject objBLReturnObject = new BLReturnObject();
                // ServerLog.MgmtExceptionLog("RegisterAndUpdateBlobIdFromConfigMaster()");

                String Message = String.Empty, AccountTypeList = String.Empty;


                DataTable dtUserDetailsWithBlobIdNull = objmasters.get_member_id_allnull();

                if (dtUserDetailsWithBlobIdNull != null && dtUserDetailsWithBlobIdNull.Rows.Count > 0)
                {
                    DataRow[] drBlobId = dtUserDetailsWithBlobIdNull.Select("BlobId is null");


                    int bcount = 0;
                    foreach (DataRow dr in drBlobId)
                    {

                        // DataRow dr = drBlobId[0];
                        string FetchedRepId = dr["member_code"].ToString();

                        //user quickblox registration
                        string BlobId = DoUserRegistrationQuickBlox(dr["first_name"].ToString(), dr["email_id"].ToString(), "admin@123");
                        if (BlobId == null) { }
                        else
                        {
                            drBlobId[bcount]["BlobId"] = BlobId;
                            objgeneral.member_master.ImportRow(drBlobId[bcount]);
                            objBLReturnObject = objmasters.update_member(objgeneral);
                        }
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {



                        }
                        bcount++;

                    }


                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion
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
        # region registration send mail

        public bool profliecreationsendEmail(string email_id, string userName, int randomNo, string university_id, string university_icon)
        {
            StreamReader sr;
            //sr = new StreamReader(HttpContext.Current.Request.PhysicalApplicationPath + "\\EmailTemplate\\Confirmation_old.html");

            sr = new StreamReader(HttpContext.Current.Request.PhysicalApplicationPath + "\\EmailTemplate\\ConfirmEmail.html");

            //string str = "<body>";
            //str += "<div style='border-radius: 5px; border: 2px solid black;padding: 20px;width : auto;height : auto;font-size:25px:font-family: Muli'>";
            //str += "<img src=\"" + ConfigurationManager.AppSettings["Domain"] + "/img/favicon.png\" width=\"76px\" Height=\"80px\" style='margin-left:40%'>";
            //str += "<br />";
            //str += "<h4>Hello USER,</h4>";
            //str += "<p>Welcome to My Campus Concierge Inc. Your personal college life concierge.  Let’s get you started on that path towards good grades, stash of cash and loads of fun. </p>";
            //str += "<br/>";
            //str += "<button style='margin-left: 37%;border-radius: 12px;font-size: 15px;background: #00529B;border-color: #00529B;color: #fff;height:53px;font-family: Muli'><a style='color:white;text-decoration: none;' href='" + ConfigurationManager.AppSettings["Domain"] + "/form.html?emailId=" + email_id + "&randomNo=" + randomNo + "&University=" + university_id + "'>Confirm Your Email</a></button>";
            //str += "</body>";


            //sr = new StreamReader("D:/campus concierge/WebApiCampusConcierge/WebApiCampusConcierge/EmailTemplate/EmailTemplate_VerificatonLink.html");
            //string str = "<body><h4>Hello USER,</h4>";
            //str += "<p>Welcome to My Campus Concierge. Your personal college life concierge.  Let’s get you started on that path towards good grades, stash of cash and loads of fun. </p><br/>";
            //str += "<b>LINK</b>";

            //str += "<img alt=\"Image not found.\" src=\""+ConfigurationManager.AppSettings["Domain"]+"/"+university_icon+"\" width=\"76px\" Height=\"80px\">";
            ////str += "<img alt=\"Image not found.\" src=\"http://localhost:2029/university_details/Houston/HoustonLogo.png\" width=\"76px\" Height=\"80px\">";

            //str += "</body>";

            //1-feb-17
            string setlink = ConfigurationManager.AppSettings["Domain"] + "/form.html?emailId=" + email_id + "&randomNo=" + randomNo + "&University=" + university_id;
            string strdata = "";
            string TITLE = "Please click following link to complete your Registration.";
            //1-feb-17

            //string LINK = "<a href ='" + ConfigurationManager.AppSettings["Domain"] + "/form.html?emailId=" + email_id + "&randomNo=" + randomNo + "&University=" + university_id + "'> " + ConfigurationManager.AppSettings["Domain"] + "/form.html?emailId=" + email_id + "&randomNo=" + randomNo + "&University=" + university_id + "</a>";

            //str = str.Replace("LINK", LINK);
            //str = str.Replace("USER", userName);
            try
            {
                while (!sr.EndOfStream)
                {
                    strdata = sr.ReadToEnd();

                    strdata = strdata.Replace("link_confir", setlink);
                    strdata = strdata.Replace("User", userName);
                }
                SmtpClient SmtpServer = new SmtpClient();

                //SmtpServer.Credentials = new System.Net.NetworkCredential("testaarin5889@gmail.com", "aarin@123");
                SmtpServer.Credentials = new System.Net.NetworkCredential("info@mycampusconcierge.com", "Logistix01");
                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(email_id, "My Campus Concierge Account Activation", System.Text.Encoding.UTF8);
                mail.To.Add(email_id);

                mail.Subject = "My Campus Concierge Account Activation";
                mail.Body = strdata;
                //mail.Body = str;
                mail.IsBodyHtml = true;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                //mail.ReplyTo = new MailAddress(user_id);
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return false;

            }

        }

        #endregion
        #region invite friend
        public bool sendinvitemail(string email_id)
        {



            StreamReader sr;
            sr = new StreamReader(HttpContext.Current.Request.PhysicalApplicationPath + "\\EmailTemplate\\NewFriendInvitation.html");
            //string str = "<body><p>You have been invited to register with my campus concierge.</p><br/><b>LINK</b></body>";

            //string str = "<body>";
            //str += "<div style='border-radius: 5px; border: 2px solid black;padding: 20px;width : auto;height : auto;font-family: Muli'>";
            //str += "<img src=\"" + ConfigurationManager.AppSettings["Domain"] + "/img/favicon.png\" width=\"76px\" Height=\"80px\" style='margin-left:40%';font-family: Muli>";
            //str += "<br />";
            ////str += "<h4>Hello USER,</h4>";
            //str += "<p>Your friend  has invited you to join the fun on My Campus Concierge Inc. Click on the button below to accept this invitation and see what all the rave is about.</p>";
            //str += "<br/>";
            //str += "<button style='margin-left: 0%;border-radius: 70px;font-size: 15px;font-family: Kavoon ;background: #00529B;border-color: #00529B;color: #fff; width: 120px;height:30px'><a style='color:white;text-decoration: none;' href ='" + ConfigurationManager.AppSettings["iphone"] + "'>App Store</a></button>";
            //str += "<label style='margin-left: 25%;font-size: 15px;font-family: Kavoon ;background: #00529B;border-color: #00529B;color: #fff; width: 120px;height:30px'>Join the fun!</label>";
            //str += "<button style='float:right;border-radius: 70px;font-size: 15px;font-family: Kavoon ;background: #00529B;border-color: #00529B;color: #fff; width: 120px;height:30px'><a style='color:white;text-decoration: none;' href ='" + ConfigurationManager.AppSettings["android"] + "'>Play Store</a></button>";

            //str += "</body>";


            string strdata = "";
            string TITLE = "Please click following link ";

            // string LINK = "<a href ='" + ConfigurationManager.AppSettings["Domain"] + "/welcome.html" + "'" + "> " + ConfigurationManager.AppSettings["Domain"] + "/welcome.html </a>";
            //str = str.Replace("LINK", LINK);

            try
            {
                while (!sr.EndOfStream)
                {
                    strdata = sr.ReadToEnd();
                    strdata = strdata.Replace("app_iphoneDLT", ConfigurationManager.AppSettings["iphone"]);
                    strdata = strdata.Replace("app_anroidDLT", ConfigurationManager.AppSettings["android"]);
                    strdata = strdata.Replace("app_iphoneQUA", ConfigurationManager.AppSettings["QUA_Iphone"]);
                    strdata = strdata.Replace("app_anroidQUA", ConfigurationManager.AppSettings["QUA_Android"]);
                }
                sr.Close();
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Credentials = new System.Net.NetworkCredential("info@mycampusconcierge.com", "Logistix01");
                //SmtpServer.Credentials = new System.Net.NetworkCredential("kunjflexi@gmail.com", "kunj@123");
                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(email_id, "My Campus Concierge Inc", System.Text.Encoding.UTF8);
                mail.To.Add(email_id);

                mail.Subject = "New Friend Invitation from My Campus Concierge Inc.";
                mail.Body = strdata;
                mail.IsBodyHtml = true;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                //mail.ReplyTo = new MailAddress(user_id);
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return false;

            }

        }

        #endregion
        public bool forgotpassword(string email_id, string university_id)
        {
            StreamReader sr;
            sr = new StreamReader(HttpContext.Current.Request.PhysicalApplicationPath + "\\EmailTemplate\\ForgotPassword.html");
            //string str = "<body>";
            //str += "<div style='border-radius: 5px; border: 2px solid black;padding: 20px;width : auto;height : auto;font-size:25px:font-family: Muli'>";
            //str += "<img src=\"" + ConfigurationManager.AppSettings["Domain"] + "/img/favicon.png\" width=\"76px\" Height=\"80px\" style='margin-left:40%'>";
            //str += "<br />";
            //str += "<h4>Hello USER,</h4>";
            //str += "<p>we've recevied a request to reset your password. if you didn't make request,just ignore this mail.otherwise you can reset your password using this link: </p>";
            //str += "<br/>";
            //str += "<button style='margin-left: 37%;border-radius: 12px;font-size: 15px;background: #00529B;border-color: #00529B;color: #fff;height:53px;font-family: Muli'><a style='color:white;text-decoration: none;' href='" + ConfigurationManager.AppSettings["Domain"] + "/forgotpasswordQUA.html?member_code=" + email_id + "&University=" + university_id + "'>Confirm Your Email</a></button>";
            //str += "</body>";


            //sr = new StreamReader("D:/campus concierge/WebApiCampusConcierge/WebApiCampusConcierge/EmailTemplate/EmailTemplate_VerificatonLink.html");
            //string str = "<body><h4>Hello USER,</h4>";
            //str += "<p>Welcome to My Campus Concierge. Your personal college life concierge.  Let’s get you started on that path towards good grades, stash of cash and loads of fun. </p><br/>";
            //str += "<b>LINK</b>";

            //str += "<img alt=\"Image not found.\" src=\""+ConfigurationManager.AppSettings["Domain"]+"/"+university_icon+"\" width=\"76px\" Height=\"80px\">";
            ////str += "<img alt=\"Image not found.\" src=\"http://localhost:2029/university_details/Houston/HoustonLogo.png\" width=\"76px\" Height=\"80px\">";

            //str += "</body>";
            string setlink = ConfigurationManager.AppSettings["Domain"] + "/forgotpasswordQUA.html?member_code=" + email_id + "&University=" + university_id;

            string strdata = "";
            string TITLE = "My Campus Concierge Forgot Password";

            //string LINK = "<a href ='" + ConfigurationManager.AppSettings["Domain"] + "/form.html?emailId=" + email_id + "&randomNo=" + randomNo + "&University=" + university_id + "'> " + ConfigurationManager.AppSettings["Domain"] + "/form.html?emailId=" + email_id + "&randomNo=" + randomNo + "&University=" + university_id + "</a>";

            //str = str.Replace("LINK", LINK);
            // str = str.Replace("USER", userName);
            try
            {
                while (!sr.EndOfStream)
                {
                    strdata = sr.ReadToEnd();

                    strdata = strdata.Replace("link_path", setlink);
                    //strdata = strdata.Replace("User", userName);
                }
                SmtpClient SmtpServer = new SmtpClient();

                //SmtpServer.Credentials = new System.Net.NetworkCredential("testaarin5889@gmail.com", "aarin@123");
                SmtpServer.Credentials = new System.Net.NetworkCredential("info@mycampusconcierge.com", "Logistix01");
                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(email_id, "My Campus Concierge Forgot Password", System.Text.Encoding.UTF8);
                mail.To.Add(email_id);

                mail.Subject = "My Campus Concierge Forgot Password";
                mail.Body = strdata;
                mail.IsBodyHtml = true;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                //mail.ReplyTo = new MailAddress(user_id);
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return false;

            }

        }

        //[HttpPost]
        // public byte[] ReadImageFile()
        // {
        //     string imageLocation = HttpContext.Current.Server.MapPath("~/Images/post_Thumbnail/1485929086963image3.jpg");
        //     byte[] imageData = null;
        //     FileInfo fileInfo = new FileInfo(imageLocation);
        //     long imageFileLength = fileInfo.Length;
        //     FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
        //     BinaryReader br = new BinaryReader(fs);
        //     imageData = br.ReadBytes((int)imageFileLength);
        //    // return imageData;
        //     //string s = System.Text.Encoding.UTF8.GetString(imageData, 0, imageData.Length);
        //     return imageData;
        // }





    }
}
