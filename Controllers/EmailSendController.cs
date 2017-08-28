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
//using System.Web.Http.Cors;



namespace WebApiCampusConcierge.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmailSendController : ApiController
    {

        #region variable Declaration
        Masters objmasters = new Masters();
        DS_MemberTables objDS_MemberTables = new DS_MemberTables();
        DS_Transtration objDs_transtration = new DS_Transtration();
        #endregion


        // GET api/emailsend
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/emailsend/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/emailsend
        public void Post([FromBody]string value)
        {
        }

        // PUT api/emailsend/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/emailsend/5
        public void Delete(int id)
        {
        }

        #region Get Methods


        public string sendflexi(string email_id)
        {
            //StreamReader sr;
            //string str = "<body><p>You have been invited to register with my campus concierge.</p><br/><b>LINK</b></body>";

            string str = "<body>";

            //str += "<div style='border-radius: 5px; border: 2px solid black;padding: 20px;width : auto;height : auto;font-family: Muli'>";
            //str += "<img src=\"" + ConfigurationManager.AppSettings["Domain"] + "/img/favicon.png\" width=\"76px\" Height=\"80px\" style='margin-left:40%';font-family: Muli>";
            //str += "<br />";
            ////str += "<h4>Hello USER,</h4>";
            //str += "<p>Your friend  has invited you to join the fun on My Campus Concierge Inc. Click on the button below to accept this invitation and see what all the rave is about.</p>";
            //str += "<br/>";
            //str += "<button style='margin-left:36%;border-radius: 70px;font-size: 15px;font-family: Muli;background: #00529B;border-color: #00529B;color: #fff; width: 120px;height:30px'><a style='color:white;text-decoration: none;' href ='" + ConfigurationManager.AppSettings["Domain"] + "/welcome.html" + "'" + ">Join the fun!</a></button>";
            str += "'" + email_id + "'";
            str += "</body>";


            string strdata = "";
            string TITLE = "Please click following link ";

            // string LINK = "<a href ='" + ConfigurationManager.AppSettings["Domain"] + "/welcome.html" + "'" + "> " + ConfigurationManager.AppSettings["Domain"] + "/welcome.html </a>";
            //str = str.Replace("LINK", LINK);

            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Credentials = new System.Net.NetworkCredential("", "");
                //SmtpServer.Credentials = new System.Net.NetworkCredential("kunjflexi@gmail.com", "kunj@123");
                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("", "My Campus Concierge Inc", System.Text.Encoding.UTF8);
                mail.To.Add("");

                mail.Subject = "New Friend Invitation from My Campus Concierge Inc.";
                mail.Body = str;
                mail.IsBodyHtml = true;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                //mail.ReplyTo = new MailAddress(user_id);
                SmtpServer.Send(mail);
                return "pass";
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return "false";

            }

        }

        public bool sendinvitemail(string email_id)
        {
            //StreamReader sr;
            //string str = "<body><p>You have been invited to register with my campus concierge.</p><br/><b>LINK</b></body>";

            string str = "<body>";
            str += "<div style='border-radius: 5px; border: 2px solid black;padding: 20px;width : auto;height : auto;font-family: Muli;margin-bottom: 15px;padding-bottom: 50px;'>";
            str += "<img src=\"" + ConfigurationManager.AppSettings["Domain"] + "/img/favicon.png\" width=\"76px\" Height=\"80px\" style='margin-left:40%';font-family: Muli>";
            str += "<br />";
            //str += "<h4>Hello USER,</h4>";
            str += "<p>Your friend  has invited you to join the fun on My Campus Concierge Inc. Click on the button below to accept this invitation and see what all the rave is about.</p>";
            str += "<br/>";

            str += "<div >";
            str += "<button style='margin-left: 0%;border-radius: 70px;font-size: 15px;font-family: Kavoon ;background: #00529B;border-color: #00529B;color: #fff; width: 33%;height:30px;float: left;'><a style='color:white;text-decoration: none;' href ='" + ConfigurationManager.AppSettings["iphone"] + "'>App Store</a></button>";
            str += "<label style='font-size: 15px;font-family: Kavoon;color: black; width: 33%;height:30px;float: left;text-align: center;'>Join the fun!</label>";
            str += "<button style='float:right;border-radius: 70px;font-size: 15px;font-family: Kavoon ;background: #00529B;border-color: #00529B;color: #fff; width: 33%;height:30px;float: left;'><a style='color:white;text-decoration: none;' href ='" + ConfigurationManager.AppSettings["android"] + "'>Play Store</a></button>";

            str += "</div></div></body>";


            string strdata = "";
            string TITLE = "Please click following link ";

            // string LINK = "<a href ='" + ConfigurationManager.AppSettings["Domain"] + "/welcome.html" + "'" + "> " + ConfigurationManager.AppSettings["Domain"] + "/welcome.html </a>";
            //str = str.Replace("LINK", LINK);

            try
            {
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
                mail.Body = str;
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

        [HttpPost]
        public string LoginCheck([FromBody]LoginData loginData)
        {
            int flg = 0;
            BLReturnObject objBLReturnObject = new BLReturnObject();
            DataTable dtMemberDetails = new DataTable();
            string result = "";
            string encrpt_password = "";
            dtMemberDetails = objmasters.LoginCheck(loginData.userName);
            try
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + loginData.dives + " " + loginData.password + " " + loginData.userName + "status" + "pass");
                if (dtMemberDetails == null || dtMemberDetails.Rows.Count == 0)
                {
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + loginData.ToString() + "status" + "NotExists");
                    return "NotExists";
                }
                if (dtMemberDetails.Rows[0]["university"].ToString() != loginData.university_id)
                {
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + loginData.ToString() + "status" + "Wronguniversity");
                    return "Wronguniversity";
                }
                else
                {
                    if (loginData.pass_encrpted_flag != "true")
                    {

                        loginData.password = loginData.password.Trim();
                        EncryptPassword encrpt = new EncryptPassword();

                        encrpt_password = encrpt.Encrypt(loginData.password.ToString().Trim());
                    }
                    else
                    {
                        encrpt_password = loginData.password;
                    }

                    if (dtMemberDetails.Rows[0]["password"].ToString() != encrpt_password)
                    {
                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + loginData.ToString() + "status" + "WrongPass");
                        return "WrongPass";
                    }
                    if (dtMemberDetails.Rows[0]["default_role"].ToString() == "TT")
                    {
                        DataTable dtrole = objmasters.checkmemberrole(loginData.userName);
                        for (int i = 0; i < dtrole.Rows.Count; i++)
                        {

                            if (dtrole.Rows[i]["role_id"].ToString() == "ST")
                            {

                                break;


                            }
                            else if (dtrole.Rows[i]["role_id"].ToString() == "TT")
                            {
                                if (dtrole.Rows[i]["is_approved"].ToString() == "Y" && dtrole.Rows[i]["approved_by_whome"].ToString() != "" && dtrole.Rows[i]["approved_by_whome"].ToString() != null)
                                {
                                    break;
                                }
                                else
                                {
                                    flg++;

                                }
                            }


                        }

                        if (dtrole.Rows.Count == flg)
                        {
                            ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + loginData.ToString() + "status" + "notApproved");
                            return "notApproved";
                        }

                    }

                    //Random rand = new Random();
                    //int tokenId = rand.Next(1, 100000);
                    Guid tokenId = Guid.NewGuid();

                    DataRow dr = dtMemberDetails.Rows[0];
                    //dr["token_id"] = tokenId.ToString();

                    objDS_MemberTables.member_master.ImportRow(dr);
                    DS_MemberTables.login_tokenRow rowtoken = objDS_MemberTables.login_token.Newlogin_tokenRow();
                    rowtoken.member_id = loginData.userName;
                    rowtoken.device_id = loginData.dives;
                    rowtoken.token_id = tokenId.ToString();

                    if (rowtoken.device_id.Trim().Length == 0)
                    {
                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + loginData.ToString() + "status" + "Device not null");
                        return "Device not null";
                    }
                    else
                    {


                        objDS_MemberTables.login_token.Addlogin_tokenRow(rowtoken);


                        objBLReturnObject = objmasters.saveLinkData_login(objDS_MemberTables, true);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {

                            dtMemberDetails.Columns.Add("token_id");
                            dtMemberDetails.Rows[0]["token_id"] = tokenId.ToString(); ;
                            result = GetJson1(dtMemberDetails);

                        }
                        else
                        {
                            ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + loginData.ToString() + "status" + "TokenIssue");
                            return "TokenIssue";

                        }

                    }

                }
            }
            catch (Exception e)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + loginData.ToString() + "status" + e.StackTrace);
                return result;
            }
            return result;
        }


        [HttpPost]
        public string sendata([FromBody]JObject peramiter)
        {
            string data = sendflexi(peramiter["email_id"].ToString());
            return data;
        }

        [HttpPost]
        public string GetAllStudentRating()
        {
            DataTable dtuniversityDetails = new DataTable();
            string result = "";
            dtuniversityDetails = objmasters.GetAllStudentRating();
            if (dtuniversityDetails != null)
            {
                result = GetJson1(dtuniversityDetails);
            }
            return result;
        }

        [HttpPost]
        public string GetAllTutorRating()
        {
            DataTable dtuniversityDetails = new DataTable();
            string result = "";
            dtuniversityDetails = objmasters.GetAllTutorRating();
            if (dtuniversityDetails != null)
            {
                result = GetJson1(dtuniversityDetails);
            }
            return result;
        }




        //iphone
        [HttpPost]
        public string gettoken([FromBody]JObject loginData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            Random rand = new Random();
            int tokenId = rand.Next(1, 100000);
            String result = "";

            DataTable userdata = objmasters.getuserdetai_byiphone(loginData["memberid"].ToString());


            userdata.Rows[0]["token_id"] = tokenId.ToString();

            objDS_MemberTables.member_master.ImportRow(userdata.Rows[0]);

            objBLReturnObject = objmasters.saveLinkData(objDS_MemberTables, true);
            if (objBLReturnObject.ExecutionStatus == 1)
            {

                result = GetJson1(userdata);
            }
            else
            {

                result = "";
            }

            return result;
        }
        //it only for profile and mail it time create
        [HttpPost]
        public string LoginCheckprofile([FromBody]LoginData loginData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            DataTable dtMemberDetails = new DataTable();
            string result = "";
            string encrpt_password = "";
            dtMemberDetails = objmasters.LoginCheckProfilecreation(loginData.userName);
            if (dtMemberDetails == null || dtMemberDetails.Rows.Count == 0)
            {
                return "NotExists";
            }
            else
            {
                if (loginData.pass_encrpted_flag != "true")
                {
                    loginData.password = loginData.password.Trim();
                    EncryptPassword encrpt = new EncryptPassword();
                    encrpt_password = encrpt.Encrypt(loginData.password.ToString().Trim());
                }
                else
                {
                    encrpt_password = loginData.password;
                }

                if (dtMemberDetails.Rows[0]["password"].ToString() != encrpt_password)
                {
                    return "WrongPass";
                }
                else
                {
                    Random rand = new Random();
                    int tokenId = rand.Next(1, 100000);

                    DataRow dr = dtMemberDetails.Rows[0];
                    //dr["token_id"] = tokenId.ToString();

                    objDS_MemberTables.member_master.ImportRow(dr);

                    objBLReturnObject = objmasters.saveLinkData(objDS_MemberTables, true);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        //var session = HttpContext.Current.Session;
                        //if (session != null)
                        //{
                        //    if (session["token_id"] == null)
                        //    {
                        //        session["token_id"] = dtMemberDetails.Rows[0]["token_id"].ToString();
                        //    }
                        //}


                        //string str = HttpContext.Current.Session["token_id"].ToString();
                        //result = str;
                        result = GetJson1(dtMemberDetails);
                    }
                    else
                    {
                        return "TokenIssue";
                    }

                }
            }
            return result;
        }

        //22/4/16 Get list of cources from course_mst university wise
        [HttpPost]
        public string getCourses([FromBody]university_id uniId)
        {
            DataTable dtuniversityDetails = new DataTable();
            string result = "";
            dtuniversityDetails = objmasters.getCourses(uniId.uni_id);
            if (dtuniversityDetails != null)
            {
                result = GetJson1(dtuniversityDetails);
            }
            return result;
        }

        [HttpPost]
        public string GetAllUniversity()
        {
            DataTable dtuniversityDetails = new DataTable();
            string result = "";
            dtuniversityDetails = objmasters.getAllUniversity();
            if (dtuniversityDetails != null)
            {
                result = GetJson1(dtuniversityDetails);
            }
            return result;
        }

        [HttpGet]
        public string GetAllUniversity1()
        {
            DataTable dtuniversityDetails = new DataTable();
            string result = "";
            dtuniversityDetails = objmasters.getAllUniversity();
            if (dtuniversityDetails != null)
            {
                result = GetJson1(dtuniversityDetails);
            }
            return result;
        }


        [HttpPost]
        public string GetPerticularUniDetails([FromBody]university_id uni_id)
        {
            DataTable dtuniversityDetails = new DataTable();
            string result = "";
            dtuniversityDetails = objmasters.GetPerticularUniDetails(uni_id.uni_id);
            if (dtuniversityDetails != null && dtuniversityDetails.Rows.Count == 1)
            {
                result = GetJson1(dtuniversityDetails);
            }
            return result;
        }
        #endregion

        #region Save/Update Methods

        //by pooja bhadania on 16/4/16 [To save user data into member_master table after sending verification email]




        [HttpPost]
        public string sendmailforinvitefriend([FromBody]JObject parameter)
        {

            //JavaScriptSerializer js = new JavaScriptSerializer();
            //string[] persons = js.Deserialize<string[]>(parameter["dataemail"]["email_list_send"].ToString());


            emailSendFrined objsendmail = new emailSendFrined();
            string result = ""; bool resultSendEmail = false;
            bool security_flag = false;
            try
            {
                if ((parameter["tokendata"]["member_id"].ToString() != null || parameter["tokendata"]["member_id"].ToString() != "") && (parameter["tokendata"]["Token"].ToString() != null || parameter["tokendata"]["Token"].ToString() != ""))
                {
                    security_flag = objmasters.securityCheck(parameter["tokendata"]["member_id"].ToString(), parameter["tokendata"]["Token"].ToString());
                }
                if (parameter["dataemail"]["email_list_send"].ToString() != "")
                {

                    objsendmail = parameter["dataemail"].ToObject<emailSendFrined>();


                }

                if (security_flag == true)
                {
                    if (objsendmail.email_list_send != null && objsendmail.email_list_send.Length > 0)
                        for (int i = 0; i < objsendmail.email_list_send.Length; i++)
                        {
                            resultSendEmail = sendinvitemail(objsendmail.email_list_send[i]);
                        }

                }
                else
                {

                    return "securityIssue";
                }
                if (resultSendEmail == true)
                {

                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                    return "pass";
                }
                else
                {
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                    return "fail";
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);
                return "fail";
            }
            //return result;



        }
        [HttpPost]
        public string saveUserLinkData([FromBody]EmailLink emailLinkData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            string flag = "";
            bool resultSendEmail = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);


            //pooja bhadania on 18/4/16
            DataTable dtAlreadyExistsEmailId = objmasters.LoginCheck(emailLinkData.email_id);  //LoginCheck method has same parameter that we need.So directly use it but name is differ
            if (dtAlreadyExistsEmailId != null && dtAlreadyExistsEmailId.Rows.Count == 1)
            {
                flag = "EmailExists";
                return flag;
            }


            emailLinkData.password = emailLinkData.password.Trim();
            EncryptPassword encrpt = new EncryptPassword();
            string encrpt_password = encrpt.Encrypt(emailLinkData.password.ToString().Trim());

            try
            {

                DataTable dtAlreadyExistsButNotVerified = objmasters.GetMemberDataFromEmailId(emailLinkData.email_id);
                if (dtAlreadyExistsButNotVerified != null && dtAlreadyExistsButNotVerified.Rows.Count == 1)
                {
                    DataRow dr = dtAlreadyExistsButNotVerified.Rows[0];
                    dr["first_name"] = emailLinkData.userName;
                    dr["university"] = emailLinkData.university;
                    dr["password"] = encrpt_password;
                    dr["email_rand_no"] = randomNo;
                    dr["created_date"] = System.DateTime.Now;
                    dr["created_host"] = HttpContext.Current.Request.UserHostName;

                    objDS_MemberTables.member_master.ImportRow(dr);
                    objBLReturnObject = objmasters.saveLinkData(objDS_MemberTables, true); //update flag true
                }
                else
                {

                    DS_MemberTables.member_masterRow memMstRow = objDS_MemberTables.member_master.Newmember_masterRow();

                    memMstRow.member_code = randomNo.ToString();  //It is just passing the primary which will not store in db.
                    memMstRow.first_name = emailLinkData.userName;
                    memMstRow.university = emailLinkData.university;
                    memMstRow.email_id = emailLinkData.email_id;
                    memMstRow.password = encrpt_password;
                    memMstRow.email_rand_no = randomNo;  //randomly generated number
                    memMstRow.is_active = "N";
                    memMstRow.created_by = emailLinkData.email_id;
                    memMstRow.created_date = System.DateTime.Now;
                    memMstRow.created_host = HttpContext.Current.Request.UserHostName;


                    objDS_MemberTables.member_master.Addmember_masterRow(memMstRow);
                    objBLReturnObject = objmasters.saveLinkData(objDS_MemberTables, false);
                }

                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    //resultSendEmail = sendEmail(emailLinkData.email_id, emailLinkData.userName, randomNo, emailLinkData.university, emailLinkData.university_icon);

                    //try
                    //{
                    //    DS_Transtration.fn_token_balanceRow book_row = objDs_transtration.fn_token_balance.Newfn_token_balanceRow();

                    //    book_row.doc_no = "1";
                    //    book_row.member_id = emailLinkData.email_id;
                    //    book_row.total_credit = 0;
                    //    book_row.total_debit = 0;
                    //    book_row.balance_amount = 0;
                    //    book_row.is_active = "Y";
                    //    book_row.created_by = emailLinkData.email_id;
                    //    book_row.created_date = System.DateTime.Now;
                    //    book_row.created_host = HttpContext.Current.Request.UserHostName;

                    //    objDs_transtration.fn_token_balance.Addfn_token_balanceRow(book_row);

                    //    objBLReturnObject = objmasters.savetokenbooking(objDs_transtration, false);

                    //}
                    //catch (Exception e)
                    //{


                    //}

                }
                else
                {
                    flag = "Fail";
                    return flag;
                }
            }

            catch (Exception ex)
            {
                flag = "Fail";
                return flag;
            }

            if (resultSendEmail == true)
                flag = "Pass";
            else
                flag = "Fail";

            return flag;
        }

        //by pooja bhadania on 18/4/16 [To update ]
        [HttpPost]
        public string updateForEmailVerification([FromBody]UpdateForEmailVerification emailVerificationData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;
            bool alreadyVerifiedFLAG = false;
            //get already exists data
            DataTable dtMemberMst = objmasters.GetMemberMstData(emailVerificationData.QS_emailId, emailVerificationData.QS_randomNo);
            if (dtMemberMst == null || dtMemberMst.Rows.Count == 0)
            {
                flag = false;
                alreadyVerifiedFLAG = true;
            }
            else
            {
                try
                {
                    DataRow dr = dtMemberMst.Rows[0];
                    dr["email_rand_no"] = 0;
                    dr["is_active"] = "Y";

                    objDS_MemberTables.member_master.ImportRow(dr);
                    objBLReturnObject = objmasters.saveLinkData(objDS_MemberTables, true); //second parameter indicates that its a update method call
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        flag = true;
                    }
                }

                catch (Exception ex)
                {
                    flag = false;
                }
            }
            if (flag == true)
            {
                string result = GetJson1(dtMemberMst);
                return result;
            }
            else
            {
                if (alreadyVerifiedFLAG == true)
                    return "AlreadyVerified";
                else
                    return "Fail";
            }

        }


        //25-4-2016
        public bool sendEmailfogot(string email_id, string userName, string uni)
        {
            //StreamReader sr;
            //string str = "<body><p>Oops!! Seems like you have forgotten your user information please click on the link below to reset your credentials.</p><br/><b>LINK</b></body>";

            string str = "<body>";
            str += "<div style='border-radius: 5px; border: 2px solid black;padding: 20px;width : auto;height : auto;'>";
            str += "<img src=\"" + ConfigurationManager.AppSettings["Domain"] + "/img/favicon.png\" width=\"76px\" Height=\"80px\" style='margin-left:40%'>";
            str += "<br />";
            // str += "<h4>Hello USER,</h4>";
            str += "<p>Oops!! Seems like you have forgotten your user information please click on the button below to reset your credentials.</p>";
            str += "<br/>";
            str += "<button style='margin-left:36%;border-radius: 70px;font-size: 15px;background: #00529B;border-color: #00529B;color: #fff;height:30px'><a style='color:white;text-decoration: none;' href ='" + ConfigurationManager.AppSettings["Domain"] + "/Forgotnewpassword.html?University=" + uni + "&member_code=" + userName + "'>Set New Password</a></button>";
            str += "</body>";

            string strdata = "";
            string TITLE = "Please click following link to Enter New Password.";

            //string LINK = "<a href ='" + ConfigurationManager.AppSettings["Domain"] + "/Forgotnewpassword.html?University=" + uni + "&member_code=" + userName + "'> " + ConfigurationManager.AppSettings["Domain"] + "/Forgotnewpassword.html?University=" + uni + "&member_code=" + userName + " </a>";
            //str = str.Replace("LINK", LINK);
            str = str.Replace("USER", userName);

            try
            {
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
                mail.Body = str;
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

        //25-4-2016
        [HttpPost]
        public string sendmailforfotgotpassword([FromBody]JObject parameter)
        {
            DataTable dtuniversityDetails = new DataTable();
            string result = ""; bool resultSendEmail = false;
            dtuniversityDetails = objmasters.forgotpwd(parameter["email_id"].ToString(), parameter["birthdate"].ToString(), parameter["security_question"].ToString(), parameter["question_ans"].ToString());

            if (dtuniversityDetails != null)
            {
                string member_code = dtuniversityDetails.Rows[0]["member_code"].ToString();
                resultSendEmail = sendEmailfogot(parameter["email_id"].ToString(), member_code, dtuniversityDetails.Rows[0]["university"].ToString());
            }
            if (resultSendEmail == true)
            {

                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                return "pass ";
            }
            else
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                return "fail";
            }
            //return result;



        }

        //25-4-2016
        [HttpPost]
        public string update_password([FromBody] JObject parameter)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                string pwd = parameter["password"].ToString();
                pwd = pwd.Trim();
                EncryptPassword encrpt = new EncryptPassword();

                string encrpt_password = encrpt.Encrypt(pwd.ToString().Trim());

                string member_code = parameter["member_code"].ToString();
                DataTable dt = new DataTable();
                dt = objmasters.getuserinfo(member_code);

                DataRow dr = dt.Rows[0];
                dr["password"] = encrpt_password;
                objDS_MemberTables.member_master.ImportRow(dr);

                objBLReturnObject = objmasters.saveLinkData(objDS_MemberTables, true);

                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                    return "pass";
                }
                else
                {
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                    return "fail";
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);

                return "fail";
            }

        }




        #endregion


        #region Local Methods

        //by pooja bhdania on 16/4/16 [To send email for varification link of Registration] 
        //public bool sendEmail(string email_id, string userName, int randomNo, string university_id, string university_icon)
        //{
        //    StreamReader sr;

        //    string str = "<body>";
        //    str += "<div style='border-radius: 5px; border: 2px solid black;padding: 20px;width : auto;height : auto;font-size:50px'>";
        //    str += "<img src=\"" + ConfigurationManager.AppSettings["Domain"] + "/img/favicon.png\" width=\"76px\" Height=\"80px\" style='margin-left:40%'>";
        //    str += "<br />";
        //    str += "<h4>Hello USER,</h4>";
        //    str += "<p>Welcome to My Campus Concierge Inc. Your personal college life concierge.  Let’s get you started on that path towards good grades, stash of cash and loads of fun. </p>";
        //    str += "<br/>";
        //    str += "<button style='margin-left: 37%;border-radius: 70px;font-size: 15px;background: #00529B;border-color: #00529B;color: #fff;height:70px'><a style='color:white;text-decoration: none;' href='" + ConfigurationManager.AppSettings["Domain"] + "/form.html?emailId=" + email_id + "&randomNo=" + randomNo + "&University=" + university_id + "'>Confirm Your Email</a></button>";
        //    str += "</body>";


        //    //sr = new StreamReader("D:/campus concierge/WebApiCampusConcierge/WebApiCampusConcierge/EmailTemplate/EmailTemplate_VerificatonLink.html");
        //    //string str = "<body><h4>Hello USER,</h4>";
        //    //str += "<p>Welcome to My Campus Concierge. Your personal college life concierge.  Let’s get you started on that path towards good grades, stash of cash and loads of fun. </p><br/>";
        //    //str += "<b>LINK</b>";

        //    //str += "<img alt=\"Image not found.\" src=\""+ConfigurationManager.AppSettings["Domain"]+"/"+university_icon+"\" width=\"76px\" Height=\"80px\">";
        //    ////str += "<img alt=\"Image not found.\" src=\"http://localhost:2029/university_details/Houston/HoustonLogo.png\" width=\"76px\" Height=\"80px\">";

        //    //str += "</body>";

        //    string strdata = "";
        //    string TITLE = "Please click following link to complete your Registration.";

        //    //string LINK = "<a href ='" + ConfigurationManager.AppSettings["Domain"] + "/form.html?emailId=" + email_id + "&randomNo=" + randomNo + "&University=" + university_id + "'> " + ConfigurationManager.AppSettings["Domain"] + "/form.html?emailId=" + email_id + "&randomNo=" + randomNo + "&University=" + university_id + "</a>";

        //    //str = str.Replace("LINK", LINK);
        //    str = str.Replace("USER", userName);
        //    try
        //    {
        //        SmtpClient SmtpServer = new SmtpClient();

        //        //SmtpServer.Credentials = new System.Net.NetworkCredential("testaarin5889@gmail.com", "aarin@123");
        //        SmtpServer.Credentials = new System.Net.NetworkCredential("info@mycampusconcierge.com", "Logistix01");
        //        SmtpServer.Port = 587;
        //        SmtpServer.Host = "smtp.gmail.com";
        //        SmtpServer.EnableSsl = true;
        //        MailMessage mail = new MailMessage();

        //        mail.From = new MailAddress(email_id, "My Campus Concierge Account Activation", System.Text.Encoding.UTF8);
        //        mail.To.Add(email_id);

        //        mail.Subject = "My Campus Concierge Account Activation";
        //        mail.Body = str;
        //        mail.IsBodyHtml = true;
        //        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

        //        //mail.ReplyTo = new MailAddress(user_id);
        //        SmtpServer.Send(mail);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //return ex.ToString();
        //        return false;

        //    }

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

        #endregion



    }
}
