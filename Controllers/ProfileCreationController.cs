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
using Newtonsoft.Json.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using System.Net.Mail;
using System.Drawing;
using System.Drawing.Imaging;
using Kaliido.QuickBlox;
using Kaliido.QuickBlox.Parameters;
using BLL.Masters;


namespace WebApiCampusConcierge.Controllers
{
    public class ProfileCreationController : ApiController
    {

        #region Variable Declaration
        Masters objmasters = new Masters();
        DS_MemberTables objDS_MemberTables = new DS_MemberTables();
        DS_Transtration objDs_trastration = new DS_Transtration();
        #endregion


        // GET api/profilecreation
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/profilecreation/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/profilecreation
        public void Post([FromBody]string value)
        {
        }

        // PUT api/profilecreation/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/profilecreation/5
        public void Delete(int id)
        {
        }


        #region GET Methods

        [HttpPost]
        public string GetAllSororities()
        {
            DataTable dt = new DataTable();
            string result = "";
            dt = objmasters.getAllSororities();
            if (dt != null)
            {
                result = GetJson1(dt);
            }

            return result;
        }
        [HttpGet]
        public string GetAllSororities1()
        {
            DataTable dt = new DataTable();
            string result = "";
            dt = objmasters.getAllSororities();
            if (dt != null)
            {
                result = GetJson1(dt);
            }

            return "hellooo";
        }
        //22-4-2016
        [HttpPost]
        public string getAllFraternitiesAndSororities()
        {
            DataTable dt = new DataTable();
            string result = "";
            dt = objmasters.getAllFraternitiesAndSororities();
            if (dt != null)
            {
                result = GetJson1(dt);
            }
            return result;
        }

        [HttpPost]
        public string Getsecurityquestion()
        {
            DataTable dt = new DataTable();
            string result = "";
            dt = objmasters.securityquestion();
            if (dt != null)
            {
                result = GetJson1(dt);
            }
            return result;

        }

        #endregion

        #region Save/Update Methods

        [HttpPost]
        public string saveProfileCreationData([FromBody]JObject parameter)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string[] str = new string[3];

            ProfileCreation profileData = new ProfileCreation();
            course_list obj_course_list = new course_list();
            //course_list_tutoring course_list_tutoring = new course_list_tutoring();

            if (parameter["ProfileCreation"] != null)
                profileData = parameter["ProfileCreation"].ToObject<ProfileCreation>();

            if (parameter["course_list"]["course_list_learning"].ToString() != "" || parameter["course_list"]["course_list_tutoring"].ToString() != "")
                obj_course_list = parameter["course_list"].ToObject<course_list>();


            //if (parameter["course_list_tutoring"] != null)
            //    course_list_tutoring = parameter["course_list"].ToObject<course_list_tutoring>();

            try
            {
                DataTable dtMemberDetail = objmasters.LoginCheckProfilecreation(profileData.email_id);
                if (dtMemberDetail != null && dtMemberDetail.Rows.Count == 1)
                {
                    #region member_master
                    //DS_MemberTables.member_masterRow memMstRow = objDS_MemberTables.member_master.Newmember_masterRow();
                    DataRow drMemDetail = dtMemberDetail.Rows[0];
                    dtMemberDetail.ImportRow(drMemDetail);

                    drMemDetail["first_name"] = profileData.first_name;
                    drMemDetail["middle_name"] = profileData.middle_name;
                    drMemDetail["nick_name"] = profileData.nick_name;
                    drMemDetail["short_bio"] = profileData.short_bio;
                    drMemDetail["last_name"] = profileData.last_name;
                    drMemDetail["phone_number1"] = profileData.phone_number1;
                    drMemDetail["birthdate"] = profileData.birthdate;
                    drMemDetail["fun_campus"] = profileData.fun_campus;
                    drMemDetail["major"] = profileData.major;

                    drMemDetail["image"] = profileData.image;
                    drMemDetail["feternity_id"] = profileData.feternity_id;
                    drMemDetail["soriety_id"] = profileData.soriety_id;
                    drMemDetail["last_modified_by"] = dtMemberDetail.Rows[0]["email_id"].ToString();
                    drMemDetail["last_modified_date"] = System.DateTime.Now;
                    drMemDetail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
                    drMemDetail["classification"] = profileData.classification;
                    drMemDetail["security_question"] = profileData.security_question;
                    drMemDetail["question_ans"] = profileData.question_ans;
                    drMemDetail["email_rand_no"] = "0";
                    drMemDetail["is_active"] = "Y";

                    //If user is only studen/tutor then default_role will be same but if he has both role the default role is TT.
                    if (profileData.role_id == "ST" || profileData.role_id == "TT")
                        drMemDetail["default_role"] = profileData.role_id;
                    else if (profileData.role_id == "BOTH")
                        drMemDetail["default_role"] = "ST";
                    //if (profileData.role_id == "TT")
                    //    drMemDetail["is_active"] = "N";
                    //if (profileData.role_id == "ST")
                    //    drMemDetail["is_active"] = "Y";
                    objDS_MemberTables.member_master.ImportRow(drMemDetail);

                    #endregion

                    #region member_role

                    if (obj_course_list != null)
                    {
                        if (obj_course_list.course_list_learning != null && obj_course_list.course_list_learning.Length > 0)
                        {
                            for (int i = 0; i < obj_course_list.course_list_learning.Length; i++)
                            {
                                DS_MemberTables.member_roleRow memRoleRow = objDS_MemberTables.member_role.Newmember_roleRow();

                                memRoleRow.sr_no = randomNo.ToString() + "__" + i;  //It is just passing the primary which will not store in db.
                                memRoleRow.university_id = dtMemberDetail.Rows[0]["university"].ToString();
                                memRoleRow.member_id = profileData.email_id;
                                memRoleRow.role_id = "ST";
                                memRoleRow.course_code = obj_course_list.course_list_learning[i];
                                memRoleRow.is_active = "Y";
                                memRoleRow.is_approved = "Y";
                                memRoleRow.created_by = profileData.email_id;
                                memRoleRow.created_date = System.DateTime.Now;
                                memRoleRow.created_host = HttpContext.Current.Request.UserHostName;

                                objDS_MemberTables.member_role.Addmember_roleRow(memRoleRow);

                            }
                        }

                        if (obj_course_list.course_list_tutoring != null && obj_course_list.course_list_tutoring.Length > 0)
                        {
                            for (int i = 0; i < obj_course_list.course_list_tutoring.Length; i++)
                            {
                                DS_MemberTables.member_roleRow memRoleRow = objDS_MemberTables.member_role.Newmember_roleRow();

                                memRoleRow.sr_no = randomNo.ToString() + "__" + i + i;  //It is just passing the primary which will not store in db.
                                memRoleRow.university_id = dtMemberDetail.Rows[0]["university"].ToString();
                                memRoleRow.member_id = profileData.email_id;
                                memRoleRow.role_id = "TT";
                                memRoleRow.course_code = obj_course_list.course_list_tutoring[i];
                                memRoleRow.is_active = "Y";
                                memRoleRow.is_approved = "N";
                                memRoleRow.created_by = profileData.email_id;
                                memRoleRow.created_date = System.DateTime.Now;
                                memRoleRow.created_host = HttpContext.Current.Request.UserHostName;

                                objDS_MemberTables.member_role.Addmember_roleRow(memRoleRow);

                            }
                        }
                    }
                    #endregion

                    #region member_tutor_transcript

                    if (profileData.transprit_file_desc != null || profileData.transprit_file_desc != string.Empty)
                    {
                        DS_MemberTables.member_tutor_transpritRow memTuTranscriptRow = objDS_MemberTables.member_tutor_transprit.Newmember_tutor_transpritRow();

                        memTuTranscriptRow.sr_no = randomNo.ToString();  //It is just passing the primary which will not store in db.
                        memTuTranscriptRow.university_id = dtMemberDetail.Rows[0]["university"].ToString();    //profileData.uni;
                        memTuTranscriptRow.member_id = profileData.email_id;
                        memTuTranscriptRow.transprit_file_desc = profileData.transprit_file_desc;
                        memTuTranscriptRow.file_path = profileData.file_path;
                        memTuTranscriptRow.is_active = "Y";
                        memTuTranscriptRow.created_by = profileData.email_id;
                        memTuTranscriptRow.created_date = System.DateTime.Now;
                        memTuTranscriptRow.created_host = HttpContext.Current.Request.UserHostName;

                        objDS_MemberTables.member_tutor_transprit.Addmember_tutor_transpritRow(memTuTranscriptRow);
                    }

                    DS_Transtration.fn_token_balanceRow token_bal_row = objDs_trastration.fn_token_balance.Newfn_token_balanceRow();
                    token_bal_row.doc_no = "1";
                    token_bal_row.member_id = profileData.email_id;
                    token_bal_row.total_debit = Convert.ToInt16(0.0);
                    token_bal_row.total_credit = Convert.ToInt16(0.0);
                    token_bal_row.balance_token = Convert.ToInt16(0.0);
                    token_bal_row.balance_amount = Convert.ToInt16(0.0);

                    token_bal_row.is_active = "Y";
                    token_bal_row.created_by = profileData.email_id;
                    token_bal_row.created_date = System.DateTime.Now;
                    token_bal_row.created_host = HttpContext.Current.Request.UserHostName;

                    objDs_trastration.fn_token_balance.Addfn_token_balanceRow(token_bal_row);
                    #endregion

                    objBLReturnObject = objmasters.saveProfileLinkData(objDS_MemberTables, objDs_trastration, false);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        if (profileData.role_id == "TT" && profileData.role_id == "BOTH")
                        {
                            bool res_mail = sendmailtutorcreate(profileData.first_name, dtMemberDetail.Rows[0]["university"].ToString());
                            if (res_mail == true) { flag = true; } else { flag = false; }
                        }
                        else { flag = true; }

                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
                ServerLog.Log(ex.Message.ToString() + Environment.NewLine + ex.StackTrace);
            }

            if (flag == true)
                return "Pass";
            else
                return "Fail";
        }
        [HttpPost]
        public string saveProfileCreationData_new([FromBody]JObject parameter)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;
            bool updateflag = false;
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string[] str = new string[3];

            ProfileCreation profileData = new ProfileCreation();
            course_list obj_course_list = new course_list();
            //course_list_tutoring course_list_tutoring = new course_list_tutoring();

            if (parameter["ProfileCreation"] != null)
                profileData = parameter["ProfileCreation"].ToObject<ProfileCreation>();

            if (parameter["course_list"]["course_list_learning"].ToString() != "" || parameter["course_list"]["course_list_tutoring"].ToString() != "")
                obj_course_list = parameter["course_list"].ToObject<course_list>();


            //if (parameter["course_list_tutoring"] != null)
            //    course_list_tutoring = parameter["course_list"].ToObject<course_list_tutoring>();

            try
            {
                if ((obj_course_list.course_list_learning.Count() > 0) && obj_course_list.course_list_tutoring.Count() > 0)
                {
                    if (obj_course_list.course_list_learning.Intersect(obj_course_list.course_list_tutoring).Any() == true) { return "coursesame"; }
                }
                DataTable dtAlreadyExistsEmailId = objmasters.logincheck_new(profileData.email_id.ToLower());  //LoginCheck method has same parameter that we need.So directly use it but name is differ
                if (dtAlreadyExistsEmailId != null && dtAlreadyExistsEmailId.Rows.Count == 1)
                {
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "EmailExists");
                    //flag = "EmailExists";

                    if (dtAlreadyExistsEmailId.Rows[0]["is_dlt"].ToString() == "" || dtAlreadyExistsEmailId.Rows[0]["is_dlt"].ToString() == null)
                    {
                        updateflag = true;

                    }
                    else
                    {
                        return "EmailExists";
                    }
                }

                if ((obj_course_list.course_list_learning.Count() < 0) || obj_course_list.course_list_tutoring.Count() < 0)
                {
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                    return "Fail";

                }

                //DataTable dtMemberDetail = objmasters.LoginCheckProfilecreation(profileData.email_id);


                if (parameter["ProfileCreation"] != null)
                {
                    if (updateflag == false)
                    {

                        #region member_master
                        DS_MemberTables.member_masterRow memMstRow = objDS_MemberTables.member_master.Newmember_masterRow();
                        //DataRow drMemDetail = dtMemberDetail.Rows[0];
                        //dtMemberDetail.ImportRow(drMemDetail);
                        memMstRow.member_code = "1";
                        memMstRow.university = profileData.university;
                        memMstRow.first_name = profileData.first_name;
                        memMstRow.middle_name = profileData.middle_name;
                        memMstRow.nick_name = profileData.nick_name;
                        memMstRow.short_bio = profileData.short_bio;
                        memMstRow.last_name = profileData.last_name;
                        memMstRow.phone_number1 = profileData.phone_number1;
                        memMstRow.birthdate = Convert.ToDateTime(profileData.birthdate);
                        memMstRow.fun_campus = profileData.fun_campus;
                        memMstRow.major = profileData.major;
                        EncryptPassword encrpt = new EncryptPassword();

                        string encrpt_password = encrpt.Encrypt(profileData.password.ToString().Trim());
                        memMstRow.password = encrpt_password;
                        memMstRow.image = profileData.image;
                        memMstRow.feternity_id = profileData.feternity_id;
                        //drMemDetail["soriety_id"] = profileData.soriety_id;
                        memMstRow.email_id = profileData.email_id.ToLower();
                        memMstRow.created_date = System.DateTime.Now;
                        memMstRow.created_host = HttpContext.Current.Request.UserHostName;
                        memMstRow.classification = profileData.classification;
                        memMstRow.security_question = Convert.ToInt32(profileData.security_question);
                        memMstRow.question_ans = profileData.question_ans;
                        memMstRow.email_rand_no = randomNo;
                        memMstRow.app_name = "DLT";
                        memMstRow.is_dlt = "Y";
                        memMstRow.is_quantina = "Y";
                        memMstRow.is_envy = "Y";
                        memMstRow.is_active = "N";
                        memMstRow.availability = "N";
                        memMstRow.created_by = profileData.email_id;
                        //If user is only studen/tutor then default_role will be same but if he has both role the default role is TT.
                        if (profileData.role_id == "ST" || profileData.role_id == "TT")
                            memMstRow.default_role = profileData.role_id;
                        else if (profileData.role_id == "BOTH")
                            memMstRow.default_role = "ST";
                        //if (profileData.role_id == "TT")
                        //    drMemDetail["is_active"] = "N";
                        //if (profileData.role_id == "ST")
                        //    drMemDetail["is_active"] = "Y";
                        objDS_MemberTables.member_master.Addmember_masterRow(memMstRow);

                        #endregion
                    }
                    else
                    {


                        dtAlreadyExistsEmailId.Rows[0]["first_name"] = profileData.first_name;
                        dtAlreadyExistsEmailId.Rows[0]["middle_name"] = profileData.middle_name;
                        dtAlreadyExistsEmailId.Rows[0]["nick_name"] = profileData.nick_name;
                        dtAlreadyExistsEmailId.Rows[0]["short_bio"] = profileData.short_bio;
                        dtAlreadyExistsEmailId.Rows[0]["last_name"] = profileData.last_name;
                        dtAlreadyExistsEmailId.Rows[0]["phone_number1"] = profileData.phone_number1;
                        dtAlreadyExistsEmailId.Rows[0]["birthdate"] = Convert.ToDateTime(profileData.birthdate);
                        dtAlreadyExistsEmailId.Rows[0]["fun_campus"] = profileData.fun_campus;
                        dtAlreadyExistsEmailId.Rows[0]["major"] = profileData.major;
                        //EncryptPassword encrpt = new EncryptPassword();

                        //string encrpt_password = encrpt.Encrypt(profileData.password.ToString().Trim());
                        //dtAlreadyExistsEmailId.Rows[0]["password"] = encrpt_password;
                        dtAlreadyExistsEmailId.Rows[0]["image"] = profileData.image;
                        dtAlreadyExistsEmailId.Rows[0]["feternity_id"] = profileData.feternity_id;
                        //drMemDetail["soriety_id"] = profileData.soriety_id;
                        //memMstRow.email_id = profileData.email_id.ToLower();
                        // memMstRow.created_date = System.DateTime.Now;
                        // memMstRow.created_host = HttpContext.Current.Request.UserHostName;
                        dtAlreadyExistsEmailId.Rows[0]["classification"] = profileData.classification;
                        dtAlreadyExistsEmailId.Rows[0]["security_question"] = Convert.ToInt32(profileData.security_question);
                        dtAlreadyExistsEmailId.Rows[0]["question_ans"] = profileData.question_ans;
                        dtAlreadyExistsEmailId.Rows[0]["email_rand_no"] = randomNo;
                        dtAlreadyExistsEmailId.Rows[0]["app_name"] = "DLT";
                        dtAlreadyExistsEmailId.Rows[0]["is_dlt"] = "Y";
                        dtAlreadyExistsEmailId.Rows[0]["is_quantina"] = "Y";
                        dtAlreadyExistsEmailId.Rows[0]["is_envy"] = "Y";
                        dtAlreadyExistsEmailId.Rows[0]["is_active"] = "N";
                        dtAlreadyExistsEmailId.Rows[0]["availability"] = "N";
                        dtAlreadyExistsEmailId.Rows[0]["created_by"] = profileData.email_id;
                        //If user is only studen/tutor then default_role will be same but if he has both role the default role is TT.
                        if (profileData.role_id == "ST" || profileData.role_id == "TT")
                            dtAlreadyExistsEmailId.Rows[0]["default_role"] = profileData.role_id;
                        else if (profileData.role_id == "BOTH")
                            dtAlreadyExistsEmailId.Rows[0]["default_role"] = "ST";
                        //if (profileData.role_id == "TT")
                        //    drMemDetail["is_active"] = "N";
                        //if (profileData.role_id == "ST")
                        //    drMemDetail["is_active"] = "Y";
                        objDS_MemberTables.member_master.ImportRow(dtAlreadyExistsEmailId.Rows[0]);
                    }
                    #region member_role

                    if (obj_course_list != null)
                    {
                        if (obj_course_list.course_list_learning != null && obj_course_list.course_list_learning.Length > 0)
                        {
                            for (int i = 0; i < obj_course_list.course_list_learning.Length; i++)
                            {
                                DS_MemberTables.member_roleRow memRoleRow = objDS_MemberTables.member_role.Newmember_roleRow();

                                memRoleRow.sr_no = randomNo.ToString() + "__" + i;  //It is just passing the primary which will not store in db.
                                memRoleRow.university_id = profileData.university;
                                memRoleRow.member_id = profileData.email_id;
                                memRoleRow.role_id = "ST";
                                memRoleRow.course_code = obj_course_list.course_list_learning[i];
                                memRoleRow.is_active = "Y";
                                memRoleRow.is_approved = "Y";
                                memRoleRow.created_by = profileData.email_id;
                                memRoleRow.created_date = System.DateTime.Now;
                                memRoleRow.created_host = HttpContext.Current.Request.UserHostName;

                                objDS_MemberTables.member_role.Addmember_roleRow(memRoleRow);

                            }
                        }

                        if (obj_course_list.course_list_tutoring != null && obj_course_list.course_list_tutoring.Length > 0)
                        {
                            for (int i = 0; i < obj_course_list.course_list_tutoring.Length; i++)
                            {
                                DS_MemberTables.member_roleRow memRoleRow = objDS_MemberTables.member_role.Newmember_roleRow();

                                memRoleRow.sr_no = randomNo.ToString() + "__" + i + i;  //It is just passing the primary which will not store in db.
                                memRoleRow.university_id = profileData.university;
                                memRoleRow.member_id = profileData.email_id;
                                memRoleRow.role_id = "TT";
                                memRoleRow.course_code = obj_course_list.course_list_tutoring[i];
                                memRoleRow.is_active = "Y";
                                memRoleRow.is_approved = "N";
                                memRoleRow.created_by = profileData.email_id;
                                memRoleRow.created_date = System.DateTime.Now;
                                memRoleRow.created_host = HttpContext.Current.Request.UserHostName;

                                objDS_MemberTables.member_role.Addmember_roleRow(memRoleRow);

                            }
                        }
                    }
                    #endregion

                    #region member_tutor_transcript

                    if (profileData.transprit_file_desc != null && profileData.transprit_file_desc != string.Empty)
                    {
                        DS_MemberTables.member_tutor_transpritRow memTuTranscriptRow = objDS_MemberTables.member_tutor_transprit.Newmember_tutor_transpritRow();

                        memTuTranscriptRow.sr_no = randomNo.ToString();  //It is just passing the primary which will not store in db.
                        memTuTranscriptRow.university_id = profileData.university;    //profileData.uni;
                        memTuTranscriptRow.member_id = profileData.email_id;
                        memTuTranscriptRow.transprit_file_desc = profileData.transprit_file_desc;
                        memTuTranscriptRow.file_path = profileData.file_path;
                        memTuTranscriptRow.is_active = "Y";
                        memTuTranscriptRow.created_by = profileData.email_id;
                        memTuTranscriptRow.created_date = System.DateTime.Now;
                        memTuTranscriptRow.created_host = HttpContext.Current.Request.UserHostName;

                        objDS_MemberTables.member_tutor_transprit.Addmember_tutor_transpritRow(memTuTranscriptRow);
                    }
                    if (updateflag == false)
                    {
                        DS_Transtration.fn_token_balanceRow token_bal_row = objDs_trastration.fn_token_balance.Newfn_token_balanceRow();
                        token_bal_row.doc_no = "1";
                        token_bal_row.member_id = profileData.email_id;
                        token_bal_row.total_debit = Convert.ToInt16(0.0);
                        token_bal_row.total_credit = Convert.ToInt16(0.0);
                        token_bal_row.balance_token = Convert.ToInt16(0.0);
                        token_bal_row.balance_amount = Convert.ToInt16(0.0);

                        token_bal_row.is_active = "Y";
                        token_bal_row.created_by = profileData.email_id;
                        token_bal_row.created_date = System.DateTime.Now;
                        token_bal_row.created_host = HttpContext.Current.Request.UserHostName;

                        objDs_trastration.fn_token_balance.Addfn_token_balanceRow(token_bal_row);
                    }
                    #endregion
                    #region Quickblox
                    if (updateflag == false)
                    {
                        string BlobId = DoUserRegistrationQuickBlox(profileData.first_name, profileData.email_id, "admin@123");
                        if (BlobId == null || BlobId == "")
                        {
                            return BLGeneralUtil.return_ajax_string("0", "some thing went wrong");
                        }
                        else
                        {
                            objDS_MemberTables.member_master.Rows[0]["BlobId"] = BlobId;


                        }

                    }
                    #endregion

                    objBLReturnObject = objmasters.saveProfileLinkData(objDS_MemberTables, objDs_trastration, updateflag);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        bool resultSendEmail = sendEmail(profileData.email_id, profileData.first_name, randomNo, profileData.university, profileData.university_icon);

                        if (profileData.role_id == "TT" || profileData.role_id == "BOTH")
                        {
                            bool res_mail = sendmailtutorcreate(profileData.first_name, profileData.university);
                            if (res_mail == true) { flag = true; } else { flag = false; }
                        }
                        else { flag = true; }

                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);
            }

            if (flag == true)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                return "Pass";
            }
            else
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                return "Fail";
            }
        }


        [HttpPost]
        public string updateProfileCreationData([FromBody]JObject parameter)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string[] str = new string[3];

            ProfileCreation profileData = new ProfileCreation();
            course_list obj_course_list = new course_list();

            bool security_flag = objmasters.securityCheck(parameter["ProfileCreation"]["email_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                if (parameter["ProfileCreation"] != null)
                    profileData = parameter["ProfileCreation"].ToObject<ProfileCreation>();

                if (parameter["course_list"]["course_list_learning"].ToString() != "" || parameter["course_list"]["course_list_tutoring"].ToString() != "")
                    obj_course_list = parameter["course_list"].ToObject<course_list>();

                if ((obj_course_list.course_list_learning.Count() <= 0) && obj_course_list.course_list_tutoring.Count() <= 0)
                {
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "course not empty");
                    return "course not empty";

                }

                if ((obj_course_list.course_list_learning.Count() > 0) && obj_course_list.course_list_tutoring.Count() > 0)
                {
                    if (obj_course_list.course_list_learning.Intersect(obj_course_list.course_list_tutoring).Any() == true) { return "coursesame"; }
                }
                //if (parameter["course_list_tutoring"] != null)
                //    course_list_tutoring = parameter["course_list"].ToObject<course_list_tutoring>();

                try
                {
                    DataTable dtMemberDetail = objmasters.LoginCheck(profileData.email_id);
                    if (dtMemberDetail != null && dtMemberDetail.Rows.Count == 1)
                    {
                        #region member_master
                        //DS_MemberTables.member_masterRow memMstRow = objDS_MemberTables.member_master.Newmember_masterRow();
                        DataRow drMemDetail = dtMemberDetail.Rows[0];
                        dtMemberDetail.ImportRow(drMemDetail);

                        drMemDetail["first_name"] = profileData.first_name;
                        drMemDetail["middle_name"] = profileData.middle_name;
                        drMemDetail["nick_name"] = profileData.nick_name;
                        drMemDetail["short_bio"] = profileData.short_bio;
                        drMemDetail["last_name"] = profileData.last_name;
                        drMemDetail["phone_number1"] = profileData.phone_number1;
                        drMemDetail["birthdate"] = profileData.birthdate;
                        drMemDetail["fun_campus"] = profileData.fun_campus;
                        drMemDetail["major"] = profileData.major;

                        drMemDetail["image"] = profileData.image;
                        drMemDetail["feternity_id"] = profileData.feternity_id;
                        drMemDetail["soriety_id"] = profileData.soriety_id;
                        drMemDetail["last_modified_by"] = dtMemberDetail.Rows[0]["email_id"].ToString();
                        drMemDetail["last_modified_date"] = System.DateTime.Now;
                        drMemDetail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
                        drMemDetail["classification"] = profileData.classification;
                        drMemDetail["security_question"] = profileData.security_question;
                        drMemDetail["question_ans"] = profileData.question_ans;


                        //If user is only studen/tutor then default_role will be same but if he has both role the default role is TT.
                        if (profileData.role_id == "ST" || profileData.role_id == "TT")
                            drMemDetail["default_role"] = profileData.role_id;
                        else if (profileData.role_id == "BOTH")
                            drMemDetail["default_role"] = "ST";

                        objDS_MemberTables.member_master.ImportRow(drMemDetail);

                        #endregion

                        #region member_role
                        var list = string.Join(",", obj_course_list.course_list_tutoring);
                        //string list = .ToString();
                        if (list.ToString() != null && list.ToString() != "")
                            objBLReturnObject = objmasters.delete_role_course(list, profileData.email_id);
                        if (objBLReturnObject.ExecutionStatus == 2)
                        {
                            ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "fail");
                            return "fail";
                        }
                        DataTable dt_tutor_old = objmasters.tutor_courselist(profileData.email_id);



                        if (obj_course_list != null)
                        {
                            if (obj_course_list.course_list_learning != null && obj_course_list.course_list_learning.Length > 0)
                            {



                                for (int i = 0; i < obj_course_list.course_list_learning.Length; i++)
                                {
                                    DS_MemberTables.member_roleRow memRoleRow = objDS_MemberTables.member_role.Newmember_roleRow();

                                    memRoleRow.sr_no = randomNo.ToString() + "__" + i;  //It is just passing the primary which will not store in db.
                                    memRoleRow.university_id = dtMemberDetail.Rows[0]["university"].ToString();
                                    memRoleRow.member_id = profileData.email_id;
                                    memRoleRow.role_id = "ST";
                                    memRoleRow.course_code = obj_course_list.course_list_learning[i];
                                    memRoleRow.is_active = "Y";
                                    memRoleRow.created_by = profileData.email_id;
                                    memRoleRow.created_date = System.DateTime.Now;
                                    memRoleRow.created_host = HttpContext.Current.Request.UserHostName;

                                    objDS_MemberTables.member_role.Addmember_roleRow(memRoleRow);

                                }
                            }

                            if (obj_course_list.course_list_tutoring != null && obj_course_list.course_list_tutoring.Length > 0)
                            {
                                for (int j = 0; j < obj_course_list.course_list_tutoring.Length; j++)
                                {

                                    if (dt_tutor_old != null && dt_tutor_old.Select("course_code = '" + obj_course_list.course_list_tutoring[j].ToString() + "'").Length > 0)
                                    {
                                    }
                                    else
                                    {

                                        DS_MemberTables.member_roleRow memRoleRow = objDS_MemberTables.member_role.Newmember_roleRow();

                                        memRoleRow.sr_no = randomNo.ToString() + "__" + j + j;  //It is just passing the primary which will not store in db.
                                        memRoleRow.university_id = dtMemberDetail.Rows[0]["university"].ToString();
                                        memRoleRow.member_id = profileData.email_id;
                                        memRoleRow.role_id = "TT";
                                        memRoleRow.course_code = obj_course_list.course_list_tutoring[j];
                                        memRoleRow.is_approved = "N";

                                        memRoleRow.is_active = "Y";
                                        memRoleRow.created_by = profileData.email_id;
                                        memRoleRow.created_date = System.DateTime.Now;
                                        memRoleRow.created_host = HttpContext.Current.Request.UserHostName;

                                        objDS_MemberTables.member_role.Addmember_roleRow(memRoleRow);
                                    }

                                }


                                //for (int i = 0; i < obj_course_list.course_list_tutoring.Length; i++)
                                //{
                                //    DS_MemberTables.member_roleRow memRoleRow = objDS_MemberTables.member_role.Newmember_roleRow();

                                //    memRoleRow.sr_no = randomNo.ToString() + "__" + i + i;  //It is just passing the primary which will not store in db.
                                //    memRoleRow.university_id = dtMemberDetail.Rows[0]["university"].ToString();
                                //    memRoleRow.member_id = profileData.email_id;
                                //    memRoleRow.role_id = "TT";
                                //    memRoleRow.course_code = obj_course_list.course_list_tutoring[i];
                                //    memRoleRow.is_active = "Y";
                                //    memRoleRow.created_by = profileData.email_id;
                                //    memRoleRow.created_date = System.DateTime.Now;
                                //    memRoleRow.created_host = HttpContext.Current.Request.UserHostName;

                                //    objDS_MemberTables.member_role.Addmember_roleRow(memRoleRow);

                                //}
                            }
                        }
                        #endregion

                        #region member_tutor_transcript
                        DataTable dtTranscript = objmasters.GetMemberTrspritData(profileData.email_id);
                        if (profileData.transprit_file_desc != null && profileData.transprit_file_desc != string.Empty)
                        {
                            DS_MemberTables.member_tutor_transpritRow memTuTranscriptRow = objDS_MemberTables.member_tutor_transprit.Newmember_tutor_transpritRow();
                            if (dtTranscript == null || dtTranscript.Rows.Count == 0)
                            {
                                memTuTranscriptRow.sr_no = randomNo.ToString();  //It is just passing the primary which will not store in db.
                                memTuTranscriptRow.university_id = dtMemberDetail.Rows[0]["university"].ToString();    //profileData.uni;
                                memTuTranscriptRow.member_id = profileData.email_id;
                                memTuTranscriptRow.transprit_file_desc = profileData.transprit_file_desc;
                                memTuTranscriptRow.file_path = profileData.file_path;
                                memTuTranscriptRow.is_active = "Y";
                                memTuTranscriptRow.created_by = profileData.email_id;
                                memTuTranscriptRow.created_date = System.DateTime.Now;
                                memTuTranscriptRow.created_host = HttpContext.Current.Request.UserHostName;

                                //objDS_MemberTables.member_tutor_transprit.Addmember_tutor_transpritRow(memTuTranscriptRow);
                            }
                            else
                            {
                                memTuTranscriptRow.sr_no = dtTranscript.Rows[0]["sr_no"].ToString();  //It is just passing the primary which will not store in db.
                                memTuTranscriptRow.university_id = dtTranscript.Rows[0]["university_id"].ToString();    //profileData.uni;
                                memTuTranscriptRow.member_id = dtTranscript.Rows[0]["member_id"].ToString();
                                memTuTranscriptRow.transprit_file_desc = profileData.transprit_file_desc;
                                memTuTranscriptRow.file_path = profileData.file_path;
                                memTuTranscriptRow.is_active = "Y";
                                memTuTranscriptRow.created_by = dtTranscript.Rows[0]["member_id"].ToString();
                                memTuTranscriptRow.created_date = System.DateTime.Now;
                                memTuTranscriptRow.created_host = HttpContext.Current.Request.UserHostName;
                            }
                            objDS_MemberTables.member_tutor_transprit.Addmember_tutor_transpritRow(memTuTranscriptRow);
                        }

                        #endregion

                        objBLReturnObject = objmasters.updateProfileLinkData(objDS_MemberTables);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {
                            flag = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);
                }

                if (flag == true)
                {
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                    return "Pass";
                }
                else
                {
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                    return "Fail";
                }
            }

            else
            {
                return "securityIssue";
            }

        }

        //it is transcript upload

        [HttpPost]
        public HttpResponseMessage upload_transript()
        {
            HttpResponseMessage result = null;
            try
            {

                string sPath = "";
                //sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/locker/");

                //  ServerLog.Log("callmethod");
                string imagepath = "";
                var httpRequest = HttpContext.Current.Request;

                Dictionary<string, string> array1 = new Dictionary<string, string>();
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

                if (httpRequest.Files.Count > 0)
                {

                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        if (postedFile.FileName.Trim() != "")
                        {
                            //var path = "D:\\jaimin data\\CampusConcierge\\CampusConcierge\\member_uploaded_detail\\member_photo\\";
                            var path = ConfigurationManager.AppSettings["TranscriptPath"].ToString();
                            var imagename = postedFile.FileName;

                            //var filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/../CampusConcierge/CampusConcierge/member_uploaded_detail/member_photo/" + imagename);
                            // var filePath = HttpContext.Current.Server.MapPath("~/Images/profile/" + postedFile.FileName);

                            var filePath = path + imagename;

                            postedFile.SaveAs(filePath);

                            docfiles.Add(filePath);
                            //array1[file] = "~/imageFolder/DrvPhoto/" + postedFile.FileName;
                            imagepath = "member_uploaded_detail/member_transcript/" + imagename;

                        }
                    }
                    if (httpRequest["member_id"] == "" && httpRequest["member_id"] == null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.Created, "member_id not found");
                    }
                    if (httpRequest["university_id"] == "" && httpRequest["member_id"] == null)
                    {
                        result = Request.CreateResponse(HttpStatusCode.Created, "university not found");
                    }
                    if (httpRequest["member_id"] != "" && httpRequest["university_id"] != "")
                    {
                        String str = updateprofileimage(httpRequest["member_id"], httpRequest["university_id"], imagepath);

                        ServerLog.Log("str" + (System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + httpRequest["member_id"] + "status" + str);

                        if (str == "1")
                        {
                            ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + httpRequest["member_id"] + "status" + "Pass");
                            result = Request.CreateResponse(HttpStatusCode.Created, "pass");
                        }
                        else
                        {
                            result = Request.CreateResponse(HttpStatusCode.Created, "fail");
                        }
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

                return result;
            }
            return result;
        }

        //profile image updload
        [HttpPost]
        public HttpResponseMessage PostFile()
        {
            HttpResponseMessage result = null;
            try
            {
                string sPath = "";
                //sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/locker/");

                //  ServerLog.Log("callmethod");
                string imagepath = "";
                var httpRequest = HttpContext.Current.Request;
                Dictionary<string, string> array1 = new Dictionary<string, string>();
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

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
                            //array1[file] = "~/imageFolder/DrvPhoto/" + postedFile.FileName;
                            imagepath = "member_uploaded_detail/member_photo/" + imagename;
                            //Test(imagepath);
                            //imageresize();

                        }
                    }

                    // String str = updateprofileimage(httpRequest["member_id"], imagepath);

                    result = Request.CreateResponse(HttpStatusCode.Created, imagepath);
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







        //it not used
        [HttpGet]
        public string updateprofileimage(string member_id, string universtiy, string imagepath)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            // updateProfileLinkData

            // getuserdetai
            try
            {
                DS_MemberTables.member_tutor_transpritRow memTuTranscriptRow = objDS_MemberTables.member_tutor_transprit.Newmember_tutor_transpritRow();

                memTuTranscriptRow.sr_no = "1";  //It is just passing the primary which will not store in db.
                memTuTranscriptRow.university_id = universtiy;    //profileData.uni;
                memTuTranscriptRow.member_id = member_id;
                memTuTranscriptRow.transprit_file_desc = imagepath;
                memTuTranscriptRow.file_path = imagepath;
                memTuTranscriptRow.is_active = "Y";
                memTuTranscriptRow.created_by = member_id;
                memTuTranscriptRow.created_date = System.DateTime.Now;
                memTuTranscriptRow.created_host = HttpContext.Current.Request.UserHostName;
                objDS_MemberTables.member_tutor_transprit.Addmember_tutor_transpritRow(memTuTranscriptRow);

                objBLReturnObject = objmasters.updateTrans(objDS_MemberTables);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return "0";

            }
            return "0";
        }
        #endregion
        public bool sendEmail(string email_id, string userName, int randomNo, string university_id, string university_icon)
        {
            StreamReader sr;

            string str = "<body>";
            str += "<div style='border-radius: 5px; border: 2px solid black;padding: 20px;width : auto;height : auto;font-size:25px:font-family: Muli'>";
            str += "<img src=\"" + ConfigurationManager.AppSettings["Domain"] + "/img/favicon.png\" width=\"76px\" Height=\"80px\" style='margin-left:40%'>";
            str += "<br />";
            str += "<h4>Hello USER,</h4>";
            str += "<p>Welcome to My Campus Concierge Inc. Your personal college life concierge.  Let’s get you started on that path towards good grades, stash of cash and loads of fun. </p>";
            str += "<br/>";
            str += "<button style='margin-left: 37%;border-radius: 12px;font-size: 15px;background: #00529B;border-color: #00529B;color: #fff;height:53px;font-family: Muli'><a style='color:white;text-decoration: none;' href='" + ConfigurationManager.AppSettings["Domain"] + "/form.html?emailId=" + email_id + "&randomNo=" + randomNo + "&University=" + university_id + "'>Confirm Your Email</a></button>";
            str += "</body>";


            //sr = new StreamReader("D:/campus concierge/WebApiCampusConcierge/WebApiCampusConcierge/EmailTemplate/EmailTemplate_VerificatonLink.html");
            //string str = "<body><h4>Hello USER,</h4>";
            //str += "<p>Welcome to My Campus Concierge. Your personal college life concierge.  Let’s get you started on that path towards good grades, stash of cash and loads of fun. </p><br/>";
            //str += "<b>LINK</b>";

            //str += "<img alt=\"Image not found.\" src=\""+ConfigurationManager.AppSettings["Domain"]+"/"+university_icon+"\" width=\"76px\" Height=\"80px\">";
            ////str += "<img alt=\"Image not found.\" src=\"http://localhost:2029/university_details/Houston/HoustonLogo.png\" width=\"76px\" Height=\"80px\">";

            //str += "</body>";

            string strdata = "";
            string TITLE = "Please click following link to complete your Registration.";

            //string LINK = "<a href ='" + ConfigurationManager.AppSettings["Domain"] + "/form.html?emailId=" + email_id + "&randomNo=" + randomNo + "&University=" + university_id + "'> " + ConfigurationManager.AppSettings["Domain"] + "/form.html?emailId=" + email_id + "&randomNo=" + randomNo + "&University=" + university_id + "</a>";

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

                mail.From = new MailAddress(email_id, "My Campus Concierge Account Activation", System.Text.Encoding.UTF8);
                mail.To.Add(email_id);

                mail.Subject = "My Campus Concierge Account Activation";
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


        // QuickBox Registration 
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

        //[HttpPost]
        //public void RegisterAndUpdateBlobIdFromConfigMaster([FromBody]JObject parameter)
        //{
        //    //  ResponseObjectInfo objResponseObjectInfo = new ResponseObjectInfo();
        //    try
        //    {
        //        BLReturnObject objBLReturnObject = new BLReturnObject();
        //        // ServerLog.MgmtExceptionLog("RegisterAndUpdateBlobIdFromConfigMaster()");

        //        String Message = String.Empty, AccountTypeList = String.Empty;


        //        DataTable dtUserDetailsWithBlobIdNull = objmasters.get_member_id_allnull();

        //        if (dtUserDetailsWithBlobIdNull != null && dtUserDetailsWithBlobIdNull.Rows.Count > 0)
        //        {
        //            DataRow[] drBlobId = dtUserDetailsWithBlobIdNull.Select("BlobId is null");


        //            int bcount = 0;
        //            foreach (DataRow dr in drBlobId)
        //            {

        //                // DataRow dr = drBlobId[0];
        //                string FetchedRepId = dr["member_code"].ToString();

        //                //user quickblox registration
        //                string BlobId = DoUserRegistrationQuickBlox(dr["first_name"].ToString(), dr["email_id"].ToString(), "admin@123");
        //                if (BlobId == null) { }
        //                else
        //                {
        //                    drBlobId[bcount]["BlobId"] = BlobId;
        //                    objgeneral.member_master.ImportRow(drBlobId[bcount]);
        //                    objBLReturnObject = objmasters.update_member(objgeneral);
        //                }
        //                if (objBLReturnObject.ExecutionStatus == 1)
        //                {



        //                }
        //                bcount++;

        //            }


        //        }
        //        else
        //        {

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
        //End registration

        #region Local Methods

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


        public bool sendmailtutorcreate(string tutor_name, string university)
        {
            //StreamReader sr;
            //string str = "<body><p>Oops!! Seems like you have forgotten your user information please click on the link below to reset your credentials.</p><br/><b>LINK</b></body>";

            string str = "<body>";
            str += "<div style='border-radius: 5px; border: 2px solid black;padding: 20px;width : auto;height : auto;'>";



            str += "<p> " + tutor_name + "has applied to be a tutor. </p>";
            str += "<br/>";

            str += "</body>";

            string strdata = "";
            string TITLE = "New tutor apply";

            //string LINK = "<a href ='" + ConfigurationManager.AppSettings["Domain"] + "/Forgotnewpassword.html?University=" + uni + "&member_code=" + userName + "'> " + ConfigurationManager.AppSettings["Domain"] + "/Forgotnewpassword.html?University=" + uni + "&member_code=" + userName + " </a>";
            //str = str.Replace("LINK", LINK);
            //str = str.Replace("USER", userName);

            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Credentials = new System.Net.NetworkCredential("testaarin5889@gmail.com", "aarin@123");
                // SmtpServer.Credentials = new System.Net.NetworkCredential("info@mycampusconcierge.com", "Logistix01");

                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("info@mycampusconceniger.com", "New Tutor", System.Text.Encoding.UTF8);
                mail.To.Add("info@mycampusconceniger.com");

                mail.Subject = "New tutor apply";
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
        #endregion

        #endregion



    }
}
