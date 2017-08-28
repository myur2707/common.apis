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
    public class ADMINCCController : ApiController
    {
        Masters objmasters = new Masters();
        DS_Admin obj_DSAdmin = new DS_Admin();
        DS_MemberTables obj_Dsmember = new DS_MemberTables();
        DS_Transtration objDs_trastration = new DS_Transtration();
        DS_MemberTables objDS_MemberTables = new DS_MemberTables(); 
        // GET api/admin
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/admin/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/admin
        public void Post([FromBody]string value)
        {
        }

        // PUT api/admin/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/admin/5
        public void Delete(int id)
        {
        }

        #region Get Method



        [HttpPost]
        public string getNewUsers([FromBody]JObject parameter)
        {
            DataTable dtstudent_rate = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {

                dtstudent_rate = objmasters.GetNewUsers(parameter["dates"].ToString());
                if (dtstudent_rate != null)
                {
                    result = GetJson1(dtstudent_rate);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }

        [HttpPost]
        public string getUniversity_userlist([FromBody]JObject parameter)
        {
            DataTable dtstudent_rate = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {

                dtstudent_rate = objmasters.getuserlist(parameter["university"].ToString());
                if (dtstudent_rate != null)
                {
                    result = GetJson1(dtstudent_rate);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }

        [HttpPost]
        public string getstudentleger_admin([FromBody]JObject parameter)
        {
            DataTable dtstudent_rate = new DataTable();
            string result = "";
            bool security_flag = true;
            if (security_flag == true)
            {

                dtstudent_rate = objmasters.getlegerforstudent(parameter["student_id"].ToString());
                if (dtstudent_rate != null)
                {
                    result = GetJson1(dtstudent_rate);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }


        [HttpPost]
        public string GetCCEarnigs([FromBody]JObject parameter)
        {
            DataTable dtstudent_rate = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["Token"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {

                dtstudent_rate = objmasters.GetCCEarnigs(parameter["member_id"].ToString());
                if (dtstudent_rate != null)
                {
                    result = GetJson1(dtstudent_rate);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }



        [HttpPost]
        public string getcourserateid([FromBody]JObject parameter)
        {
            DataTable dtstudent_rate = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                dtstudent_rate = objmasters.studentcourserate();
                if (dtstudent_rate != null)
                {
                    result = GetJson1(dtstudent_rate);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }

        [HttpPost]
        public string getCoursemasterdetail([FromBody]JObject parameter)
        {
            DataTable dtAllFarSoro = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                dtAllFarSoro = objmasters.getcoursemaster();
                if (dtAllFarSoro != null)
                {
                    result = GetJson1(dtAllFarSoro);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }


        [HttpPost]
        public string getstudentdetail([FromBody]JObject parameter)
        {
            DataTable dtstudent_rate = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                // it will be change naisargi
                // dtstudent_rate = objmasters.studentrate_id();
                dtstudent_rate = objmasters.getAlluserdetai();
                if (dtstudent_rate != null)
                {
                    result = GetJson1(dtstudent_rate);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }


        [HttpPost]
        public string gettutorlocation([FromBody]JObject parameter)
        {
            DataTable dtstudent_rate = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {

                dtstudent_rate = objmasters.gettutorlocation();
                if (dtstudent_rate != null)
                {
                    result = GetJson1(dtstudent_rate);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }

        [HttpPost]
        public string getstudent_rate_id([FromBody]JObject parameter)
        {
            DataTable dtstudent_rate = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                dtstudent_rate = objmasters.studentrate_id();
                if (dtstudent_rate != null)
                {
                    result = GetJson1(dtstudent_rate);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }


        [HttpPost]
        public string getCourse_type_mst([FromBody]JObject parameter)
        {
            DataTable dtAllFarSoro = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                dtAllFarSoro = objmasters.getcourse_type();
                if (dtAllFarSoro != null)
                {
                    result = GetJson1(dtAllFarSoro);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }

        //nisaragini
        [HttpPost]
        public string getAllFarSoro([FromBody]JObject parameter)
        {
            DataTable dtAllFarSoro = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtAllFarSoro = objmasters.getAllFarSoro();
                if (dtAllFarSoro != null)
                {
                    result = GetJson1(dtAllFarSoro);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }
        //dhanunjay
        [HttpPost]
        public string getcourse_type_detail([FromBody]JObject parameter)
        {
            DataTable dtUniversityList = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                dtUniversityList = objmasters.getcourse_type_mster();
                if (dtUniversityList != null)
                {
                    result = GetJson1(dtUniversityList);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }


        // mounika
        [HttpPost]
        public string getpeak_rate_master_detail([FromBody]JObject parameter)
        {
            DataTable dtUniversityList = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                dtUniversityList = objmasters.peak_rate_master_detail();
                if (dtUniversityList != null)
                {
                    result = GetJson1(dtUniversityList);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }



        //4/6/16
        [HttpPost]
        public string getUniversitywiseCourseType([FromBody]JObject parameter)
        {
            DataTable dtUniversityList = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                dtUniversityList = objmasters.getcourse_type_new(parameter["university"].ToString());
                if (dtUniversityList != null)
                {
                    result = GetJson1(dtUniversityList);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }


        //4/6/16
        [HttpPost]
        public string getStandardRateList([FromBody]JObject parameter)
        {
            DataTable dtUniversityList = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                dtUniversityList = objmasters.getStandardRateList();
                if (dtUniversityList != null)
                {
                    result = GetJson1(dtUniversityList);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }

        //6-6-2016
        [HttpPost]
        public string getUniversityDeatil([FromBody]JObject parameter)
        {
            DataTable dtUniversityList = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                dtUniversityList = objmasters.getuniversitymaster(parameter["id"].ToString());
                if (dtUniversityList != null)
                {
                    result = GetJson1(dtUniversityList);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }


        //4/6/16
        [HttpPost]
        public string getPerticularStandardRate([FromBody]JObject parameter)
        {
            DataTable dtUniversityList = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                dtUniversityList = objmasters.getPerticularStandardRate(parameter["rate_sr_no"].ToString());
                if (dtUniversityList != null)
                {
                    result = GetJson1(dtUniversityList);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }

        //6-6-2016
        [HttpPost]
        public string getuniversitydetail([FromBody]JObject parameter)
        {
            DataTable dtUniversityList = new DataTable();
            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                dtUniversityList = objmasters.getuniversitymaster();
                if (dtUniversityList != null)
                {
                    result = GetJson1(dtUniversityList);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }




        [HttpPost]
        public string GetReedeemRequest([FromBody]JObject parameter)
        {
            DataTable dtreedeemreq = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtreedeemreq = objmasters.reedeemrequest();
                if (dtreedeemreq != null)
                {
                    result = GetJson1(dtreedeemreq);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }
        #endregion

        #region Save Method

        //4/6/16
        [HttpPost]
        public string Save_StandardRate([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;

            try
            {
                bool security_flag = true;
                //bool security_flag = objmasters.securityCheck(parameter["tokendata"]["member_id"].ToString(), parameter["tokendata"]["Token"].ToString());
                if (security_flag == true)
                {
                    #region tutoring_rate_mst
                    DS_Admin.tutoring_rate_mstRow tutor_rate_row = obj_DSAdmin.tutoring_rate_mst.Newtutoring_rate_mstRow();

                    tutor_rate_row.sr_no = "1";
                    tutor_rate_row.university_id = parameter["stdandard_rate_data"]["university"].ToString();
                    tutor_rate_row.course_type = parameter["stdandard_rate_data"]["course_type"].ToString();
                    tutor_rate_row.session_type = parameter["stdandard_rate_data"]["session_type"].ToString();
                    tutor_rate_row.from_date = Convert.ToDateTime(parameter["stdandard_rate_data"]["from_date"].ToString());

                    if (parameter["stdandard_rate_data"]["to_date"].ToString() != "")
                        tutor_rate_row.to_date = Convert.ToDateTime(parameter["stdandard_rate_data"]["to_date"].ToString());

                    tutor_rate_row.amount = parameter["stdandard_rate_data"]["amount"].ToString();

                    //if (parameter["stdandard_rate_data"]["tutor_id"] != null && parameter["stdandard_rate_data"]["tutor_id"].ToString() != "")
                    //    tutor_rate_row.tutor_id = parameter["stdandard_rate_data"]["tutor_id"].ToString();

                    tutor_rate_row.is_active = "Y";
                    //tutor_rate_row.created_by = parameter["tokendata"]["member_id"].ToString(); ;
                    tutor_rate_row.created_by = "admin";
                    tutor_rate_row.created_date = System.DateTime.UtcNow;
                    tutor_rate_row.created_host = HttpContext.Current.Request.UserHostName;

                    obj_DSAdmin.tutoring_rate_mst.Addtutoring_rate_mstRow(tutor_rate_row);


                    #endregion


                    objBLReturnObject = objmasters.Save_Standard_Rate(obj_DSAdmin);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        //return "Pass";
                        result = "Pass";
                    }
                }
                else
                {
                    result = "securityIssue";
                    //return "securityIssue";
                }
            }
            catch (Exception e)
            {
                //return "Fail";
                result = "Fail";
            }

            return result;


        }


        //6-6-2016

        public string Save_University([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;

            try
            {
                //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["Token"].ToString());
                bool security_flag = true;
                if (security_flag == true)
                {
                    if (parameter["isupdate"].ToString() == "N")
                    {
                        #region tutoring_rate_mst
                        DS_Admin.university_masterRow Universuty_row = obj_DSAdmin.university_master.Newuniversity_masterRow();

                        Universuty_row.university_id = "1";
                        Universuty_row.university_name = parameter["university_name"].ToString();
                        if (parameter["Image"].ToString() != "")
                        {
                            Universuty_row.background_image = parameter["Image"].ToString();
                        }
                        if (parameter["theme_color"].ToString() != "")
                        {
                            Universuty_row.theme_colour = parameter["theme_color"].ToString();
                        }
                        if (parameter["btn_color"].ToString() != "")
                        {
                            Universuty_row.btn_color = parameter["btn_color"].ToString();
                        }
                        Universuty_row.timezone = parameter["timezone"].ToString();

                        Universuty_row.is_active = "Y";
                        Universuty_row.prefix_email = parameter["prefix_email"].ToString();
                        Universuty_row.created_by = "Admin";
                        Universuty_row.created_date = System.DateTime.UtcNow;
                        Universuty_row.created_host = HttpContext.Current.Request.UserHostName;

                        obj_DSAdmin.university_master.Adduniversity_masterRow(Universuty_row);


                        #endregion


                        objBLReturnObject = objmasters.Save_University_Mst(obj_DSAdmin);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {
                            //return "Pass";
                            result = "Pass";
                        }
                    }
                    else
                    {

                        #region update university



                        if (parameter["isupdate"].ToString() == "Y")
                        {


                            DataTable dtsession = objmasters.getuniversitymaster(parameter["uni_id"].ToString());
                            DataRow dr_session_detail = dtsession.Rows[0];
                            dtsession.ImportRow(dr_session_detail);

                            dr_session_detail["university_name"] = parameter["university_name"].ToString();
                            dr_session_detail["background_image"] = parameter["Image"].ToString();

                            dr_session_detail["theme_colour"] = parameter["theme_color"].ToString();

                            dr_session_detail["btn_color"] = parameter["btn_color"].ToString();
                            dr_session_detail["prefix_email"] = parameter["prefix_email"].ToString();
                            dr_session_detail["timezone"] = parameter["timezone"].ToString();
                            dr_session_detail["last_modified_by"] = "Admin";
                            dr_session_detail["last_modified_date"] = System.DateTime.Now;
                            dr_session_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                            obj_DSAdmin.university_master.ImportRow(dr_session_detail);



                            objBLReturnObject = objmasters.update_University_Mst(obj_DSAdmin);
                            if (objBLReturnObject.ExecutionStatus == 1)
                            {
                                return "Pass";
                            }

                        }




                        #endregion
                    }

                }
                else
                {
                    result = "securityIssue";
                    //return "securityIssue";
                }
            }
            catch (Exception e)
            {
                //return "Fail";
                result = "Fail";
            }

            return result;


        }


        //6-7-2016
        //add fraternity_sorority_mst update 


        //[HttpPost]
        //public string Set_tutor_approved_rejectbyadmin([FromBody]JObject parameter)
        //{

        //    BLReturnObject objBLReturnObject = new BLReturnObject();

        //    Random rand = new Random();
        //    int randomNo = rand.Next(1, 1000000);

        //    string result = "";
        //    bool flag = false;

        //    try
        //    {
        //        bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
        //        if (security_flag == true)
        //        {
        //            DataTable dtrole = objmasters.tutordetail(parameter["srno"].ToString());
        //            DataTable dt_member = objmasters.GetMemberDataFromEmailId(dtrole.Rows[0]["member_id"].ToString());
        //            if (dtrole != null && dtrole.Rows.Count > 0 && dt_member != null && dt_member.Rows.Count > 0)
        //            {

        //                DataRow dr_role_detail = dtrole.Rows[0];
        //                dtrole.ImportRow(dr_role_detail);
        //                if (parameter["status"].ToString() != null)
        //                {

        //                    dr_role_detail["is_active"] = parameter["status"].ToString();

        //                    dr_role_detail["is_approved"] = parameter["status"].ToString();

        //                    dr_role_detail["last_modified_by"] = parameter["member_id"].ToString();
        //                    dr_role_detail["approved_by_whome"] = "admin";
        //                    dr_role_detail["last_modified_date"] = System.DateTime.Now;
        //                    dr_role_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
        //                }
        //                obj_Dsmember.member_role.ImportRow(dr_role_detail);

        //                // MEMBER_MASTER UPDATE

        //                DataRow dr_member = dt_member.Rows[0];
        //                dt_member.ImportRow(dr_member);
        //                if (parameter["status"].ToString() != null)
        //                {

        //                    dr_member["default_role"] = "TT";

        //                    // dr_member["is_approved"] = parameter["status"].ToString();

        //                    dr_member["last_modified_by"] = parameter["member_id"].ToString();
        //                    //dr_member["approved_by_whome"] = "admin";
        //                    dr_member["last_modified_date"] = System.DateTime.Now;
        //                    dr_member["last_modified_host"] = HttpContext.Current.Request.UserHostName;
        //                }
        //                obj_Dsmember.member_master.ImportRow(dr_member);
        //                objBLReturnObject = objmasters.updatetutorbyadmin(obj_Dsmember);
        //                if (objBLReturnObject.ExecutionStatus == 1)
        //                {
        //                    return "Pass";
        //                }
        //                else
        //                {
        //                    return "Fail";
        //                }
        //            }
        //            else
        //            {
        //                return "Fail";
        //            }
        //        }
        //        else
        //        {
        //            return "securityIssue";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return "Fail";
        //    }


        //}


        //6-7-2016
        public string Save_fraternity_sorority_mst([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["Token"].ToString());
                if (security_flag == true)
                {
                    if (parameter["isupdate"].ToString() == "N")
                    {
                        #region tutoring_rate_mst
                        DS_Admin.fraternity_sorority_mstRow fratinitysoroty = obj_DSAdmin.fraternity_sorority_mst.Newfraternity_sorority_mstRow();

                        fratinitysoroty.fra_soro_id = "1";
                        fratinitysoroty.Description = parameter["Description"].ToString();

                        fratinitysoroty.is_active = "Y";
                        fratinitysoroty.created_by = "Admin";
                        fratinitysoroty.created_date = System.DateTime.UtcNow;
                        fratinitysoroty.created_host = HttpContext.Current.Request.UserHostName;

                        obj_DSAdmin.fraternity_sorority_mst.Addfraternity_sorority_mstRow(fratinitysoroty);


                        #endregion


                        objBLReturnObject = objmasters.Save_fratenity_soroty(obj_DSAdmin);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {
                            //return "Pass";
                            result = "Pass";
                        }
                    }
                    else
                    {

                        #region update fratinatio and sorotiy



                        if (parameter["isupdate"].ToString() == "Y")
                        {


                            DataTable dtfarsoromst = objmasters.getAllFarSoro(parameter["fra_Soro_id"].ToString());
                            DataRow dr_farsoro_detail = dtfarsoromst.Rows[0];
                            dtfarsoromst.ImportRow(dr_farsoro_detail);


                            dr_farsoro_detail["is_active"] = parameter["isactive"].ToString();

                            dr_farsoro_detail["last_modified_by"] = "Admin";
                            dr_farsoro_detail["last_modified_date"] = System.DateTime.Now;
                            dr_farsoro_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                            obj_DSAdmin.fraternity_sorority_mst.ImportRow(dr_farsoro_detail);



                            objBLReturnObject = objmasters.update_fratenity_soroty(obj_DSAdmin);
                            if (objBLReturnObject.ExecutionStatus == 1)
                            {
                                return "Pass";
                            }

                        }




                        #endregion
                    }

                }
                else
                {
                    result = "securityIssue";
                    //return "securityIssue";
                }
            }
            catch (Exception e)
            {
                //return "Fail";
                result = "Fail";
            }

            return result;


        }



        // peek rete master update ,save 

        public string Save_peekreate([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;

            try
            {
                //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["Token"].ToString());
                bool security_flag = true;

                if (security_flag == true)
                {
                    if (parameter["isupdate"].ToString() == "N")
                    {
                        #region tutoring_rate_mst

                        DS_Admin.peak_rate_masterRow peek_rate = obj_DSAdmin.peak_rate_master.Newpeak_rate_masterRow();
                        peek_rate.sr_no = "1";
                        peek_rate.university_id = parameter["university_id"].ToString();
                        peek_rate.course_type = parameter["course_type"].ToString();
                        peek_rate.peak_period_start_date = Convert.ToDateTime(parameter["start_date"].ToString());
                        peek_rate.peak_period_end_date = Convert.ToDateTime(parameter["end_date"].ToString());
                        peek_rate.peak_rate_percentage = Convert.ToDecimal(parameter["rate"].ToString());
                        peek_rate.is_active = "Y";
                        peek_rate.created_by = "Admin";
                        peek_rate.created_date = System.DateTime.UtcNow;
                        peek_rate.created_host = HttpContext.Current.Request.UserHostName;

                        obj_DSAdmin.peak_rate_master.Addpeak_rate_masterRow(peek_rate);


                        #endregion

                        #region alert message
                        DataTable dt_student = objmasters.getalluserofuniversity(parameter["university_id"].ToString());
                        if (dt_student != null && dt_student.Rows.Count > 0)
                        {

                            for (int i = 0; i < dt_student.Rows.Count; i++)
                            {

                                DS_Admin.alertmessageRow alert_row = obj_DSAdmin.alertmessage.NewalertmessageRow();
                                alert_row.Doc_no = i + 1.ToString();
                                alert_row.university_id = dt_student.Rows[i]["university"].ToString();
                                alert_row.to_member = dt_student.Rows[i]["email_id"].ToString();
                                alert_row.to_member_role = "TT";
                                alert_row.notification_date = System.DateTime.Now;
                                alert_row._event = "PeekRate";
                                alert_row.template = "Tutor rates are up!! Turn on your availability flag to capitalize on these great rates";
                                alert_row.is_read = "N";
                                alert_row.is_active = "Y";
                                alert_row.senddate = System.DateTime.Now;
                                alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                                alert_row.created_by = dt_student.Rows[i]["email_id"].ToString();
                                alert_row.created_date = System.DateTime.Now;
                                alert_row.created_host = HttpContext.Current.Request.UserHostName;

                                obj_DSAdmin.alertmessage.AddalertmessageRow(alert_row);

                            }
                        }


                        #endregion
                        objBLReturnObject = objmasters.Save_peek_rate_mst(obj_DSAdmin);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {
                            //return "Pass";
                            result = "Pass";
                        }
                    }
                    else
                    {

                        #region update peek rate master


                        if (parameter["isupdate"].ToString() == "Y")
                        {


                            DataTable dt_peek_rate = objmasters.getpeekreatedetail(parameter["id"].ToString());
                            DataRow dr = dt_peek_rate.Rows[0];
                            dt_peek_rate.ImportRow(dr);


                            dr["is_active"] = parameter["is_active"].ToString();

                            dr["last_modified_by"] = "Admin";
                            dr["last_modified_date"] = System.DateTime.Now;
                            dr["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                            obj_DSAdmin.peak_rate_master.ImportRow(dr);

                            #region alert message
                            DataTable dt_student = objmasters.getalluserofuniversity(parameter["university_id"].ToString());
                            if (dt_student != null && dt_student.Rows.Count > 0)
                            {

                                for (int i = 0; i < dt_student.Rows.Count; i++)
                                {

                                    DS_Admin.alertmessageRow alert_row = obj_DSAdmin.alertmessage.NewalertmessageRow();
                                    alert_row.Doc_no = i + 1.ToString();
                                    alert_row.university_id = dt_student.Rows[i]["university"].ToString();
                                    alert_row.to_member = dt_student.Rows[i]["email_id"].ToString();
                                    alert_row.to_member_role = "TT";
                                    alert_row.notification_date = System.DateTime.Now;
                                    alert_row._event = "PeekRate";
                                    alert_row.template = "Tutor rates are up!! Turn on your availability flag to capitalize on these great rates";
                                    alert_row.is_read = "N";
                                    alert_row.is_active = "Y";
                                    alert_row.senddate = System.DateTime.Now;
                                    alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                                    alert_row.created_by = dt_student.Rows[i]["email_id"].ToString();
                                    alert_row.created_date = System.DateTime.Now;
                                    alert_row.created_host = HttpContext.Current.Request.UserHostName;

                                    obj_DSAdmin.alertmessage.AddalertmessageRow(alert_row);

                                }
                            }


                            #endregion

                            objBLReturnObject = objmasters.update_peek_rate_mst(obj_DSAdmin);
                            if (objBLReturnObject.ExecutionStatus == 1)
                            {
                                return "Pass";
                            }

                        }




                        #endregion
                    }

                }
                else
                {
                    result = "securityIssue";
                    //return "securityIssue";
                }
            }
            catch (Exception e)
            {
                //return "Fail";
                result = "Fail";
            }

            return result;


        }
        // course type master update,save


        public string Save_course_mst([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;

            try
            {
                //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["Token"].ToString());
                bool security_flag = true;
                if (security_flag == true)
                {
                    if (parameter["isupdate"].ToString() == "N")
                    {
                        #region tutoring_rate_mst

                        DS_Admin.course_type_mstRow course_type_row = obj_DSAdmin.course_type_mst.Newcourse_type_mstRow();

                        course_type_row.doc_id = "1";
                        course_type_row.University_id = parameter["uni"].ToString();
                        course_type_row.Description = parameter["desc"].ToString();
                        course_type_row.is_active = "Y";
                        course_type_row.created_by = "Admin";
                        course_type_row.created_date = System.DateTime.UtcNow;
                        course_type_row.created_host = HttpContext.Current.Request.UserHostName;

                        obj_DSAdmin.course_type_mst.Addcourse_type_mstRow(course_type_row);


                        #endregion


                        objBLReturnObject = objmasters.Save_course_type_mst(obj_DSAdmin);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {
                            //return "Pass";
                            result = "Pass";
                        }
                    }
                    else
                    {

                        #region update university



                        if (parameter["isupdate"].ToString() == "Y")
                        {


                            DataTable dtfarsoromst = objmasters.getcoursetypemst(parameter["id"].ToString());
                            DataRow dr_coursetype_detail = dtfarsoromst.Rows[0];
                            dtfarsoromst.ImportRow(dr_coursetype_detail);


                            dr_coursetype_detail["is_active"] = parameter["isactive"].ToString();

                            dr_coursetype_detail["last_modified_by"] = "Admin";
                            dr_coursetype_detail["last_modified_date"] = System.DateTime.Now;
                            dr_coursetype_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                            obj_DSAdmin.course_type_mst.ImportRow(dr_coursetype_detail);



                            objBLReturnObject = objmasters.update_course_type_mst(obj_DSAdmin);
                            if (objBLReturnObject.ExecutionStatus == 1)
                            {
                                return "Pass";
                            }

                        }







                        #endregion
                    }

                }
                else
                {
                    result = "securityIssue";
                    //return "securityIssue";
                }
            }
            catch (Exception e)
            {
                //return "Fail";
                result = "Fail";
            }

            return result;


        }


        //discount rate management

        public string Save_discount_rate([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;

            try
            {
                //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["Token"].ToString());
                bool security_flag = true;
                if (security_flag == true)
                {
                    if (parameter["isupdate"].ToString() == "N")
                    {
                        #region tutoring_rate_mst

                        DS_Admin.discount_rate_mstRow discount_row = obj_DSAdmin.discount_rate_mst.Newdiscount_rate_mstRow();

                        discount_row.university = parameter["university"].ToString();
                        discount_row.student_rating_id = parameter["student_rating_id"].ToString();
                        discount_row.discount_percentage = Convert.ToDecimal(parameter["discount_percentage"].ToString());
                        // discount_row.is_standard_rate = parameter["student_rating_id"].ToString();
                        // discount_row.is_peak_rate_applicable = parameter["student_rating_id"].ToString();



                        discount_row.is_active = "Y";
                        discount_row.created_by = "Admin";
                        discount_row.created_date = System.DateTime.UtcNow;
                        discount_row.created_host = HttpContext.Current.Request.UserHostName;

                        obj_DSAdmin.discount_rate_mst.Adddiscount_rate_mstRow(discount_row);


                        #endregion


                        objBLReturnObject = objmasters.Save_discount_rate(obj_DSAdmin);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {
                            //return "Pass";
                            result = "Pass";
                        }
                    }
                    else
                    {

                        #region update university



                        if (parameter["isupdate"].ToString() == "Y")
                        {


                            DataTable dt = objmasters.getdisocuntrate(parameter["rateid"].ToString(), parameter["universityid"].ToString());
                            DataRow dr_discount_detail = dt.Rows[0];
                            dt.ImportRow(dr_discount_detail);

                            dr_discount_detail["is_active"] = parameter["isactive"].ToString();

                            dr_discount_detail["last_modified_by"] = "Admin";
                            dr_discount_detail["last_modified_date"] = System.DateTime.Now;
                            dr_discount_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                            obj_DSAdmin.discount_rate_mst.ImportRow(dr_discount_detail);



                            objBLReturnObject = objmasters.Save_discount_rate(obj_DSAdmin);
                            if (objBLReturnObject.ExecutionStatus == 1)
                            {
                                return "Pass";
                            }

                        }







                        #endregion
                    }

                }
                else
                {
                    result = "securityIssue";
                    //return "securityIssue";
                }
            }
            catch (Exception e)
            {
                //return "Fail";
                result = "Fail";
            }

            return result;


        }


        //course master


        public string Save_course_master([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;

            try
            {
                bool security_flag = true;
                //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["Token"].ToString());
                if (security_flag == true)
                {
                    if (parameter["isupdate"].ToString() == "N")
                    {
                        #region tutoring_rate_mst

                        DS_Admin.course_mstRow course_mst = obj_DSAdmin.course_mst.Newcourse_mstRow();

                        course_mst.course_id = "1";
                        course_mst.university_id = parameter["university_id"].ToString();
                        course_mst.course_name = parameter["course_name"].ToString();
                        course_mst.course_type = parameter["course_type"].ToString();




                        course_mst.is_active = "Y";
                        course_mst.created_by = "Admin";
                        course_mst.created_date = System.DateTime.UtcNow;
                        course_mst.created_host = HttpContext.Current.Request.UserHostName;

                        obj_DSAdmin.course_mst.Addcourse_mstRow(course_mst);


                        #endregion


                        objBLReturnObject = objmasters.Save_course(obj_DSAdmin);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {
                            //return "Pass";
                            result = "Pass";
                        }
                    }
                    else
                    {

                        #region update university



                        if (parameter["isupdate"].ToString() == "Y")
                        {


                            DataTable dt = objmasters.getcoursemaster(parameter["id"].ToString());
                            DataRow dr_discount_detail = dt.Rows[0];
                            dt.ImportRow(dr_discount_detail);

                            dr_discount_detail["is_active"] = parameter["isactive"].ToString();

                            dr_discount_detail["last_modified_by"] = "Admin";
                            dr_discount_detail["last_modified_date"] = System.DateTime.Now;
                            dr_discount_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                            obj_DSAdmin.course_mst.ImportRow(dr_discount_detail);



                            objBLReturnObject = objmasters.update_course(obj_DSAdmin);
                            if (objBLReturnObject.ExecutionStatus == 1)
                            {
                                return "Pass";
                            }

                        }







                        #endregion
                    }

                }
                else
                {
                    result = "securityIssue";
                    //return "securityIssue";
                }
            }
            catch (Exception e)
            {
                //return "Fail";
                result = "Fail";
            }

            return result;


        }


        public string Update_User([FromBody]JObject parameter)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            string result = "";
            bool flag = false;
            try
            {
                //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["Token"].ToString());
                bool security_flag = true;
                if (security_flag == true)
                {
                    if (parameter["isupdate"].ToString() == "N")
                    {
                        #region tutoring_rate_mst

                        DS_Admin.course_mstRow course_mst = obj_DSAdmin.course_mst.Newcourse_mstRow();

                        course_mst.course_id = "1";
                        course_mst.university_id = parameter["university_id"].ToString();
                        course_mst.course_name = parameter["course_name"].ToString();
                        course_mst.course_type = parameter["course_type"].ToString();




                        course_mst.is_active = "Y";
                        course_mst.created_by = "Admin";
                        course_mst.created_date = System.DateTime.UtcNow;
                        course_mst.created_host = HttpContext.Current.Request.UserHostName;

                        obj_DSAdmin.course_mst.Addcourse_mstRow(course_mst);


                        #endregion


                        objBLReturnObject = objmasters.Save_course(obj_DSAdmin);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {
                            //return "Pass";
                            result = "Pass";
                        }
                    }
                    else
                    {

                        #region update university



                        if (parameter["isupdate"].ToString() == "Y")
                        {
                            DataTable dt = objmasters.getuserinfo(parameter["id"].ToString());
                            DataRow dr_discount_detail = dt.Rows[0];
                            dt.ImportRow(dr_discount_detail);
                            dr_discount_detail["is_active"] = parameter["isactive"].ToString();
                            dr_discount_detail["last_modified_by"] = "Admin";
                            dr_discount_detail["last_modified_date"] = System.DateTime.Now;
                            dr_discount_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
                            obj_DSAdmin.member_master.ImportRow(dr_discount_detail);

                            objBLReturnObject = objmasters.dsupdate_User(obj_DSAdmin);
                            if (objBLReturnObject.ExecutionStatus == 1)
                            {
                                return "Pass";
                            }
                        }


                        #endregion
                    }

                }
                else
                {
                    result = "securityIssue";
                    //return "securityIssue";
                }
            }
            catch (Exception e)
            {
                //return "Fail";
                result = "Fail";
            }

            return result;


        }

        public string save_tutor_location([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;
            #region tutoring location

            try
            {


                DS_Admin.tutor_locationRow tutor_row = obj_DSAdmin.tutor_location.Newtutor_locationRow();
                tutor_row.doc_no = "1";
                tutor_row.university_id = parameter["university"].ToString();
                tutor_row.name = parameter["location_name"].ToString();

                tutor_row.is_active = "Y";
                tutor_row.created_by = "Admin";
                tutor_row.created_date = System.DateTime.UtcNow;
                tutor_row.created_host = HttpContext.Current.Request.UserHostName;

                obj_DSAdmin.tutor_location.Addtutor_locationRow(tutor_row);

                objBLReturnObject = objmasters.Save_tutoring_location(obj_DSAdmin);
                if (objBLReturnObject.ExecutionStatus == 1)
                {
                    //return "Pass";
                    result = "Pass";
                }
            }
            catch (Exception ex)
            {

            }
            #endregion



            return result;

        }

        public string custome_alert([FromBody]JObject parameter)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            string uni = "";
            string student_achive = "";
            string tutor_achive = "";
            string mem_role = "";
            try
            {
                if (parameter["university"].ToString() != null)
                {

                    uni = parameter["university"].ToString();
                }
                if (parameter["Student_achive"].ToString() != null)
                {

                    student_achive = parameter["Student_achive"].ToString();
                }
                if (parameter["tutor"].ToString() != null)
                {
                    tutor_achive = parameter["tutor"].ToString();
                }
                if (parameter["message"].ToString() != null)
                {

                }
                if (parameter["role"].ToString() != null) { mem_role = parameter["role"].ToString(); }
                #region alert message
                DataTable dt_student = objmasters.custonme_alert(uni, student_achive, tutor_achive, mem_role);
                if (dt_student != null && dt_student.Rows.Count > 0)
                {

                    for (int i = 0; i < dt_student.Rows.Count; i++)
                    {

                        DS_Admin.alertmessageRow alert_row = obj_DSAdmin.alertmessage.NewalertmessageRow();
                        alert_row.Doc_no = i + 1.ToString();
                        alert_row.university_id = dt_student.Rows[i]["university"].ToString();
                        alert_row.to_member = dt_student.Rows[i]["email_id"].ToString();
                        alert_row.to_member_role = dt_student.Rows[i]["default_role"].ToString();
                        alert_row.notification_date = System.DateTime.Now;
                        alert_row._event = "cutsome";
                        alert_row.template = parameter["message"].ToString();
                        alert_row.is_read = "N";
                        alert_row.is_active = "Y";
                        alert_row.senddate = System.DateTime.Now;
                        alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                        alert_row.created_by = "admin";
                        alert_row.created_date = System.DateTime.Now;
                        alert_row.created_host = HttpContext.Current.Request.UserHostName;

                        obj_DSAdmin.alertmessage.AddalertmessageRow(alert_row);

                    }
                    objBLReturnObject = objmasters.alert_custome(obj_DSAdmin);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        return "Pass";
                        //result = "Pass";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

                #endregion

            return "fail";
        }
        //redeam transtration
        [HttpPost]
        public string update_redeam_request([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["Token"].ToString());
                if (security_flag == true)
                {
                    if (parameter["id"].ToString() != "" && parameter["id"].ToString() != null)
                    {
                        DataTable dt_redeam = objmasters.getredeam_user(parameter["id"].ToString());

                        DataTable dt_token_bal = objmasters.gettokenbal(dt_redeam.Rows[0]["is_transtion_member_id"].ToString());

                        #region redeam update
                        dt_redeam.Rows[0]["payment_status"] = 'Y';
                        dt_redeam.Rows[0]["last_modified_by"] = parameter["member_id"].ToString();
                        dt_redeam.Rows[0]["last_modified_date"] = System.DateTime.Now;

                        objDs_trastration.fn_token_reedemption.ImportRow(dt_redeam.Rows[0]);
                        #endregion
                        //#region token balance update
                        //dt_token_bal.Rows[0]["total_debit"] = (Convert.ToDecimal(dt_token_bal.Rows[0]["total_debit"].ToString()) + (Convert.ToDecimal(dt_redeam.Rows[0]["amount"].ToString())));
                        //dt_token_bal.Rows[0]["balance_token"] = (Convert.ToDecimal(dt_token_bal.Rows[0]["balance_token"].ToString()) - (Convert.ToDecimal(dt_redeam.Rows[0]["amount"].ToString())));
                        //objDs_trastration.fn_token_balance.ImportRow(dt_token_bal.Rows[0]);
                        //#endregion

                        // #region transtration new 

                        //    DS_Transtration.fn_token_transtionRow token_transtration_row = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                        //    token_transtration_row.doc_no = "1";
                        //    token_transtration_row.doc_date = System.DateTime.Now;
                        //    token_transtration_row.ref_transtion_type = "Redeam";
                        //    token_transtration_row.ref_transtion_doc_date = System.DateTime.Now;
                        //    token_transtration_row.ref_transtion_doc_no = dt_redeam.Rows[0]["doc_no"].ToString();
                        //    token_transtration_row.amount = (Convert.ToDecimal(dt_redeam.Rows[0]["amount"].ToString()));
                        //    token_transtration_row.balance_after_transtion = Convert.ToDecimal(Convert.ToDecimal(dt_token_bal.Rows[0]["balance_token"].ToString()));
                        //    token_transtration_row.type_credit_debit = "debit";
                        //    token_transtration_row.member_id = dt_redeam.Rows[0]["is_transtion_member_id"].ToString();
                        //    token_transtration_row.created_by = parameter["member_id"].ToString();
                        //    token_transtration_row.created_date = System.DateTime.Now;
                        //    token_transtration_row.created_host = HttpContext.Current.Request.UserHostName;
                        //    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row);


                        //    #endregion
                    }

                    objBLReturnObject = objmasters.update_redeam(objDs_trastration);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        //return "Pass";
                        result = "Pass";
                    }
                }
                else
                {
                    result = "securityIssue";
                    //return "securityIssue";
                }
            }
            catch (Exception e)
            {
                //return "Fail";
                result = "Fail";
            }

            return result;


        }
        #endregion


        #region Local Method
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

        //email send
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

        //dashboard
        [HttpPost]
        public string getapplytobetutorlist([FromBody]JObject parameter)
        {
            DataTable dtMemberList = new DataTable();
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                string result = "";
                dtMemberList = objmasters.applytobetutorlist();
                if (dtMemberList != null)
                {
                    result = GetJson1(dtMemberList);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }

        [HttpPost]
        public string gettutorlistbyadmin([FromBody]JObject parameter)
        {
            DataTable dtMemberList = new DataTable();
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                string result = "";
                dtMemberList = objmasters.tutorlistbyadmin(parameter["tutor_id"].ToString());
                if (dtMemberList != null)
                {
                    result = GetJson1(dtMemberList);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }

        [HttpPost]
        public string getTrnascript([FromBody]JObject parameter)
        {
            DataTable dtMemberList = new DataTable();
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            bool security_flag = true;
            if (security_flag == true)
            {
                string result = "";
                dtMemberList = objmasters.GetTranscript(parameter["tutor_id"].ToString());
                if (dtMemberList != null)
                {
                    result = GetJson1(dtMemberList);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }


        [HttpPost]
        public string Set_tutor_approved_rejectbyadmin([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;

            try
            {
                //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                bool security_flag = true;
                if (security_flag == true)
                {
                    DataTable dtrole = objmasters.tutordetail(parameter["srno"].ToString());
                    DataTable dt_member = objmasters.GetMemberDataFromEmailId(dtrole.Rows[0]["member_id"].ToString());
                    if (dtrole != null && dtrole.Rows.Count > 0 && dt_member != null && dt_member.Rows.Count > 0)
                    {

                        DataRow dr_role_detail = dtrole.Rows[0];
                        dtrole.ImportRow(dr_role_detail);
                        if (parameter["status"].ToString() != null)
                        {

                            dr_role_detail["is_active"] = parameter["status"].ToString();

                            dr_role_detail["is_approved"] = parameter["status"].ToString();

                            dr_role_detail["last_modified_by"] = parameter["member_id"].ToString();
                            dr_role_detail["approved_by_whome"] = "admin";
                            dr_role_detail["last_modified_date"] = System.DateTime.Now;
                            dr_role_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
                        }
                        objDS_MemberTables.member_role.ImportRow(dr_role_detail);

                        // MEMBER_MASTER UPDATE

                        DataRow dr_member = dt_member.Rows[0];
                        dt_member.ImportRow(dr_member);
                        if (parameter["status"].ToString() != null)
                        {

                            dr_member["default_role"] = "TT";

                            // dr_member["is_approved"] = parameter["status"].ToString();

                            dr_member["last_modified_by"] = parameter["member_id"].ToString();
                            //dr_member["approved_by_whome"] = "admin";
                            dr_member["last_modified_date"] = System.DateTime.Now;
                            dr_member["last_modified_host"] = HttpContext.Current.Request.UserHostName;
                        }
                        objDS_MemberTables.member_master.ImportRow(dr_member);
                        objBLReturnObject = objmasters.updatetutorbyadmin(objDS_MemberTables);
                        if (objBLReturnObject.ExecutionStatus == 1)
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
                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                        return "Fail";
                    }
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + e.StackTrace);
                return "Fail";
            }


        }

        [HttpPost]
        public string get_total_user_count()
        {
            DataTable dtstudent_rate = new DataTable();
            string result = "";
            bool security_flag = true;
            if (security_flag == true)
            {

                dtstudent_rate = objmasters.get_total_user_count();
                if (dtstudent_rate != null)
                {
                    result = GetJson1(dtstudent_rate);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }

 
    }
}
