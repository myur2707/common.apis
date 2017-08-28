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
using WebApiCampusConcierge.Controllers;



namespace WebApiCampusConcierge.Controllers
{
    public class DashBoardController : ApiController
    {
        #region Variable declaration
        public static int mycc_charge = 15;
        public static string mycc_id = "mycc@gmail.com";
        public static int cancell_tutor_charge = 10;
        Masters objmasters = new Masters();
        DS_MemberTables objDS_MemberTables = new DS_MemberTables();
        DS_ScheduleAppointment objDS_ScheduleAppointment = new DS_ScheduleAppointment();
        DS_Transtration objDs_trastration = new DS_Transtration();
        DS_Admin objDS_Admin = new DS_Admin();
        Class1 objcls = new Class1();
        #endregion

        // GET api/dashboard
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/dashboard/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dashboard
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dashboard/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dashboard/5
        public void Delete(int id)
        {
        }

        #region Get Method
        [HttpGet]
        //public void timedate() {
        //    var time = objmasters.timeconevert("ja");
        //}

        [HttpPost]
        public string GetAlertmessage([FromBody]JObject parameter)
        {

            DataTable dtSessionDetails = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtSessionDetails = objmasters.GetAlertmessage(parameter["member_id"].ToString(), parameter["role_id"].ToString(), parameter["current_time"].ToString());
                    if (dtSessionDetails != null)
                    {
                        result = GetJson1(dtSessionDetails);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {
                ServerLog.Log("GetAlertmessage" + " " + parameter.ToString() + "Error" + e.StackTrace);
                return result;
            }

        }


        [HttpPost]
        public string getRoleWiseMember([FromBody]JObject parameter)
        {

            DataTable dtMemberList = new DataTable();
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());

                if (security_flag == true)
                {
                    string result = "";
                    dtMemberList = objmasters.getRoleWiseMember(parameter["member_id"].ToString(), parameter["role_id"].ToString(), parameter["university"].ToString());
                    if (dtMemberList != null)
                    {
                        result = GetJson1(dtMemberList);
                    }

                    return result;
                }
                else
                {
                    //ServerLog.Log("GetAlertmessage" + " " + parameter.ToString() + "Error" + e.StackTrace);
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + e.StackTrace);
                return "";
            }
        }

        [HttpPost]
        public string gettutorname([FromBody]university_id uniId)
        {
            DataTable dtuniversityDetails = new DataTable();
            string result = "";
            dtuniversityDetails = objmasters.tutorname(uniId.uni_id, uniId.member_id);
            if (dtuniversityDetails != null)
            {
                result = GetJson1(dtuniversityDetails);
            }
            return result;
        }

        [HttpPost]
        public string getMostRecentSessions([FromBody] JObject parameter)
        {

            DataTable dtMostRecentSession = new DataTable();
            //Dashboard dashboard = new Dashboard();
            //if (parameter["Dashboard"] != null)
            //    dashboard = parameter["Dashboard"].ToObject<Dashboard>();
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());


            if (security_flag == true)
            {
                string result = "";
                if (parameter["member_role"].ToString() == "ST")
                {
                    var cid = "";
                    dtMostRecentSession = objmasters.getMostRecentSessions(parameter["member_id"].ToString(), parameter["member_role"].ToString(), cid);
                }
                else { dtMostRecentSession = objmasters.getMostRecentSessions(parameter["member_id"].ToString(), parameter["member_role"].ToString(), parameter["course_id"].ToString()); }
                if (dtMostRecentSession != null)
                {
                    result = GetJson1(dtMostRecentSession);
                }


                return result;
            }
            else
            {

                return "securityIssue";
            }

        }

        [HttpPost]
        public string getStudentLearningCources([FromBody] JObject parameter)
        {


            DataTable dtStuLearningCourses = new DataTable();

            string result = "";

            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtStuLearningCourses = objmasters.getStudentLearningCources(parameter["member_id"].ToString(), parameter["member_role"].ToString());
                if (dtStuLearningCourses != null)
                {
                    result = GetJson1(dtStuLearningCourses);
                }

                return result;
            }
            else
            {

                return "securityIssue";
            }


        }

        [HttpPost]
        public string getCourseWiseTutor([FromBody] JObject parameter)
        {
            DataTable dtTutorList = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtTutorList = objmasters.getCourseWiseTutor(parameter["course"].ToString(), parameter["university"].ToString());
                if (dtTutorList != null)
                {
                    result = GetJson1(dtTutorList);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }

        [HttpPost]
        public string getfeedbackpeending([FromBody] JObject parameter)
        {
            DataTable dtTutorList = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtTutorList = objmasters.feedback_peending(parameter["member_id"].ToString(), parameter["role"].ToString());
                if (dtTutorList != null)
                {
                    result = GetJson1(dtTutorList);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }
        }
        //30-4-2016
        [HttpPost]
        public string GetSearchdata([FromBody]JObject parameter)
        {
            DataTable dtserach = new DataTable();
            string result = ""; string course_id = ""; string rate = ""; string name = ""; string uni = "";
            bool security_flag = false;

            if ((parameter["member_id"].ToString() != null || parameter["member_id"].ToString() != "") && (parameter["Token"].ToString() != null || parameter["Token"].ToString() != ""))
            {
                security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["Token"].ToString());
            }
            if (security_flag == true)
            {

                if (parameter["tutorname"].ToString() != null || parameter["tutorname"].ToString() != "")
                {

                    name = parameter["tutorname"].ToString();
                }
                if (parameter["Courser_id"].ToString() != null || parameter["Courser_id"].ToString() != "")
                {
                    course_id = parameter["Courser_id"].ToString();
                }
                if (parameter["hourse"].ToString() != null || parameter["hourse"].ToString() != "")
                {
                    rate = parameter["hourse"].ToString();
                }
                if (parameter["university"].ToString() != null || parameter["university"].ToString() != "")
                {
                    uni = parameter["university"].ToString();

                }
                dtserach = objmasters.Getsearch(course_id, rate, name, uni, parameter["member_id"].ToString(), parameter["member_role"].ToString());


            }
            else
            {
                result = "securityIssue";
            }



            try
            {
                if (dtserach != null)
                {
                    result = GetJson1(dtserach);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return result;
        }
        [HttpPost]
        public string GetTutorScedule([FromBody]JObject parameter)
        {
            DataTable dtUniversityList = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtUniversityList = objmasters.GetTutorScedule(parameter["tutor_email"].ToString());
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
        public string GetTutorScedule_iphone([FromBody]JObject parameter)
        {
            DataTable dtUniversityList = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtUniversityList = objmasters.GetTutorScedule_datewish(parameter["tutor_email"].ToString(), parameter["date"].ToString());
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
        public string GetPieChartData([FromBody]JObject parameter)
        {

            DataTable dtpiechart = new DataTable();
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                string result = "";
                dtpiechart = objmasters.PieChartData(parameter["member_id"].ToString());
                if (dtpiechart != null)
                {
                    result = GetJson1(dtpiechart);
                }

                return result;

            }
            else
            {

                return "securityIssue";
            }

        }
        [HttpPost]
        public string GetSessionToGo([FromBody]JObject parameter)
        {

            DataTable dtTutorList = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());



            if (security_flag == true)
            {
                dtTutorList = objmasters.getSessionToGo(parameter["member_id"].ToString(), parameter["university"].ToString(), parameter["role"].ToString());
                if (dtTutorList != null)
                {
                    result = GetJson1(dtTutorList);
                }

                return result;
            }
            else
            {

                return "securityIssue";
            }

        }

        [HttpPost]
        public string GetPerticularMemberDetail([FromBody]JObject parameter)
        {

            // ServerLog.Log("if in");
            DataTable dtMemberDetail = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtMemberDetail = objmasters.getPerticularMemberDetail(parameter["member_id"].ToString(), parameter["university"].ToString());
                if (dtMemberDetail != null)
                {
                    result = GetJson1(dtMemberDetail);
                }

                return result;
            }
            else
            {

                return "securityIssue";
            }

        }

        [HttpPost]
        public string GETuserdataforeditprofile([FromBody] JObject parameter)
        {
            DataTable dtTutorList = new DataTable();

            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            //if (security_flag == true)
            //{
            dtTutorList = objmasters.getuserdetai(parameter["email"].ToString());
            if (dtTutorList != null)
            {
                result = GetJson1(dtTutorList);
            }
            return result;
            //}
            //else
            //{
            //    return "securityIssue";
            //}
        }

        [HttpPost]
        public string GETuserdataforeditprofilecourse([FromBody] JObject parameter)
        {
            DataTable dtTutorList = new DataTable();

            string result = "";
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            //if (security_flag == true)
            //{
            dtTutorList = objmasters.getCoursesUser(parameter["email"].ToString());
            if (dtTutorList != null)
            {
                result = GetJson1(dtTutorList);
            }
            return result;
            //}
            //else
            //{
            //    return "securityIssue";
            //}
        }

        [HttpPost]
        public string GetSchedulememberdetail([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtMemberDetail = objmasters.Schedulememberdetail(parameter["maildata"].ToString());
                if (dtMemberDetail != null)
                {
                    result = GetJson1(dtMemberDetail);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }

        }

        [HttpPost]
        public string gettutorLocationuniversitywish([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtMemberDetail = objmasters.gettutorlocation(parameter["university"].ToString());
                if (dtMemberDetail != null)
                {
                    result = GetJson1(dtMemberDetail);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }

        }

        //iphone
        [HttpPost]
        public string GetSchedulememberdetail_withcourse([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtMemberDetail = objmasters.Schedulememberdetail_course(parameter["maildata"].ToString(), parameter["course_code"].ToString());
                if (dtMemberDetail != null)
                {
                    result = GetJson1(dtMemberDetail);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }

        }

        [HttpPost]
        public string Getcourseandid([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtMemberDetail = objmasters.Coursenameandid(parameter["name"].ToString());
                    if (dtMemberDetail != null)
                    {
                        result = GetJson1(dtMemberDetail);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }


        [HttpPost]
        public string Getalertnote([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtMemberDetail = objmasters.getpushalert(parameter["member_id"].ToString(), parameter["member_role"].ToString(), parameter["current_time"].ToString(), parameter["end_time"].ToString());
                    if (dtMemberDetail != null)
                    {
                        result = GetJson1(dtMemberDetail);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }


        //[HttpPost]
        //public string getSessionManagmentDetailsWHENST([FromBody]JObject parameter)
        //{

        //    DataTable dtSessionDetails = new DataTable();
        //    string result = "";
        //    try
        //    {
        //        bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
        //        if (security_flag == true)
        //        {
        //            dtSessionDetails = objmasters.getSessionManagmentDetailsWHENST(parameter["role_id"].ToString(), parameter["member_id"].ToString(), parameter["current_member_role"].ToString());
        //            if (dtSessionDetails != null)
        //            {
        //                result = GetJson1(dtSessionDetails);
        //            }
        //            return result;
        //        }
        //        else
        //        {
        //            return "securityIssue";
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        return result;
        //    }
        //}

        [HttpPost]
        public string getSessionManagmentDetails([FromBody]JObject parameter)
        {

            DataTable dtSessionDetails = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtSessionDetails = objmasters.getSessionManagmentDetails(parameter["member_id"].ToString(), parameter["current_member_role"].ToString(), parameter["university"].ToString());
                    if (dtSessionDetails != null)
                    {
                        result = GetJson1(dtSessionDetails);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }

        [HttpPost]
        public string getPastSessionManagmentDetails([FromBody]JObject parameter)
        {

            DataTable dtSessionDetails = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtSessionDetails = objmasters.getPastSessionManagmentDetails(parameter["university"].ToString(), parameter["member_id"].ToString(), parameter["current_member_role"].ToString());
                    if (dtSessionDetails != null)
                    {
                        result = GetJson1(dtSessionDetails);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }

        [HttpPost]
        public string getParticipantFromsessionId([FromBody]JObject parameter)
        {

            DataTable dtSessionDetails = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtSessionDetails = objmasters.getParticipantFromsessionId(parameter["session_id"].ToString());
                    if (dtSessionDetails != null)
                    {
                        result = GetJson1(dtSessionDetails);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }

        //17-5-2016
        [HttpPost]
        public string GetFeedBack([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtMemberDetail = objmasters.feedback(parameter["find_email"].ToString(), parameter["course_id"].ToString());
                    if (dtMemberDetail != null)
                    {
                        result = GetJson1(dtMemberDetail);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        [HttpPost]
        public string get_totaltoken_debit_credit([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtMemberDetail = objmasters.token_bal(parameter["member_id"].ToString());
                    if (dtMemberDetail != null)
                    {
                        result = GetJson1(dtMemberDetail);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception ex)
            {
                return "";
            }

        }


        [HttpPost]
        public string GetStudentfeedback([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtMemberDetail = objmasters.studnetfeedback(parameter["find_email"].ToString(), parameter["course_id"].ToString(), parameter["session_id"].ToString());
                    if (dtMemberDetail != null)
                    {
                        result = GetJson1(dtMemberDetail);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        [HttpPost]
        public string getMemberListCourseRoleWise([FromBody]JObject parameter)
        {

            DataTable dtSessionDetails = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtSessionDetails = objmasters.getMemberListCourseRoleWise(parameter["role_id"].ToString(), parameter["course_id"].ToString(), parameter["university"].ToString(), parameter["member_id"].ToString());
                    if (dtSessionDetails != null)
                    {
                        result = GetJson1(dtSessionDetails);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }


        [HttpPost]
        public string getSession_Requests([FromBody]JObject parameter)
        {

            DataTable dtSessionDetails = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtSessionDetails = objmasters.getSession_Requests(parameter["role_id"].ToString(), parameter["member_id"].ToString(), parameter["university"].ToString());
                    if (dtSessionDetails != null)
                    {
                        result = GetJson1(dtSessionDetails);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }

        [HttpPost]
        public string getUniversityWiseNotifications([FromBody]JObject parameter)
        {

            DataTable dtSessionDetails = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtSessionDetails = objmasters.getUniversityWiseNotifications(parameter["university"].ToString(), parameter["member_id"].ToString(), parameter["role_id"].ToString());
                    if (dtSessionDetails != null)
                    {
                        result = GetJson1(dtSessionDetails);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }


        [HttpPost]
        public string getSessionDetail([FromBody]JObject parameter)
        {

            DataTable dtSessionDetails = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtSessionDetails = objmasters.getSessionDetail(parameter["role_id"].ToString(), parameter["university"].ToString(), parameter["member_id"].ToString(), parameter["session_id"].ToString());
                    if (dtSessionDetails != null)
                    {
                        result = GetJson1(dtSessionDetails);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }

        [HttpPost]
        public string GetViewprofilebytutor([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtMemberDetail = objmasters.profiledatastudent(parameter["session_id"].ToString());
                    if (dtMemberDetail != null)
                    {
                        result = GetJson1(dtMemberDetail);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }

        [HttpPost]
        public string checkforconfirmation([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtMemberDetail = objmasters.checkforconfirmation(parameter["doc_no"].ToString(), parameter["notification_type"].ToString());
                    if (dtMemberDetail != null)
                    {
                        result = GetJson1(dtMemberDetail);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }

        [HttpPost]
        public string Getsessionfriendlist([FromBody]JObject parameter)
        {
            //27-5-2016
            DataTable dtMemberDetail = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtMemberDetail = objmasters.session_friendlist(parameter["session_id"].ToString(), parameter["member_id"].ToString());
                    if (dtMemberDetail != null)
                    {
                        result = GetJson1(dtMemberDetail);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }

        [HttpPost]
        public string GetCourselist([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtMemberDetail = objmasters.courslist(parameter["university_id"].ToString());
                    if (dtMemberDetail != null)
                    {
                        result = GetJson1(dtMemberDetail);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {

                return result;
            }
        }

        [HttpPost]
        public string getsessiondata([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtMemberDetail = objmasters.sessiondata(parameter["session_id"].ToString());
                if (dtMemberDetail != null)
                {
                    result = GetJson1(dtMemberDetail);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }



        }

        [HttpPost]
        public string getDataToRateTutor([FromBody]JObject parameter)
        {

            DataTable dtDetail = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtDetail = objmasters.getDataToRateTutor(parameter["member_id"].ToString(), parameter["member_role"].ToString(), parameter["university"].ToString());
                if (dtDetail != null)
                {
                    result = GetJson1(dtDetail);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }



        }

        [HttpPost]
        public string getStudentAvgRating([FromBody]JObject parameter)
        {

            DataTable dtDetail = new DataTable();
            string result = "";

            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtDetail = objmasters.getAvgRateSummaryForStudent(parameter["member_id"].ToString(), parameter["university"].ToString());
                if (dtDetail != null)
                {
                    result = GetJson1(dtDetail);
                }

                return result;
            }
            else
            {

                return "securityIssue";
            }



        }
        [HttpPost]
        public string GetNotConfirmedSessionDetails([FromBody]JObject parameter)
        {

            DataTable dtMemberDetail = new DataTable();
            string result = "";
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    dtMemberDetail = objmasters.NotConfirmedSessionDetails(parameter["current_member_role"].ToString(), parameter["member_id"].ToString());
                    if (dtMemberDetail != null)
                    {
                        result = GetJson1(dtMemberDetail);
                    }
                    return result;
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status " + e.StackTrace);
                return result;
            }
        }
        [HttpPost]
        public string getTutorAvgRating([FromBody]JObject parameter)
        {

            DataTable dtDetail = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtDetail = objmasters.getAvgRateSummaryForTutor(parameter["member_id"].ToString(), parameter["university"].ToString());
                if (dtDetail != null)
                {
                    result = GetJson1(dtDetail);
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
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
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
        public string Getchatingdata([FromBody]JObject parameter)
        {
            //31-5-2016
            DataTable dtMemberList = new DataTable();
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                string result = "";
                dtMemberList = objmasters.chatdata(parameter["to_member_id"].ToString(), parameter["from_member_id"].ToString(), parameter["session_id"].ToString());
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
        public string Getutorchatlist([FromBody]JObject parameter)
        {
            //31-5-2016
            DataTable dtMemberDetail = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtMemberDetail = objmasters.chatwithtutorlist(parameter["member_id"].ToString());
                if (dtMemberDetail != null)
                {
                    result = GetJson1(dtMemberDetail);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }

        }

        [HttpPost]
        public string getSessionRate([FromBody]JObject parameter)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();
            DataTable dtrate = new DataTable();
            DataTable dt_checkamount = new DataTable();
            Document objDocument = new Document();
            Document Doc = new Document();
            String message = "";
            String DocNo = "";

            DataTable dtRateDetail = new DataTable();
            string result = "";
            bool flag = false;
            decimal base_amount = 0;
            decimal peak_rate = 0;
            decimal discount_rate = 0;
            decimal temp_peak = 0;
            decimal temp_discount = 0;
            try
            {
                bool security_flag = objmasters.securityCheck(parameter["tokendata"]["member_id"].ToString(), parameter["tokendata"]["Token"].ToString());
                if (security_flag == true)
                {


                    if (parameter["member"] != null)
                    {

                        Memberid = parameter["member"].ToObject<member>();

                    }
                    sessiondata = parameter["sessiontime"].ToObject<sessiontime>();



                    // check rate is avalible/or not
                    int count_no_student = (Memberid.member_list.Length - 1);

                    dtrate = objmasters.getSessionRate(parameter["sessiontime"]["session_date"].ToString(), parameter["sessiontime"]["session_type"].ToString(), parameter["sessiontime"]["course_type"].ToString(), parameter["tutor"]["tutor_id"].ToString(), parameter["sessiontime"]["university_id"].ToString(), parameter["sessiontime"]["student_rate_id"].ToString());
                    if (dtrate != null)
                    {
                        //double timediff = Convert.ToDateTime(parameter["sessiontime"]["session_end_time"]).Subtract(Convert.ToDateTime(parameter["sessiontime"]["session_start_time"])).TotalHours;
                        base_amount = ((Convert.ToDecimal(dtrate.Rows[0]["amount"].ToString()) * (Convert.ToDecimal(parameter["sessiontime"]["duration"].ToString())) / 60) + (count_no_student * 5));
                        //calculate peak rate  amount of peak rate and discount will take on $ not per
                        if (dtrate.Rows[0]["peak_rate_percentage"].ToString() != null && dtrate.Rows[0]["peak_rate_percentage"].ToString() != "")
                        {
                            temp_peak = Convert.ToDecimal(dtrate.Rows[0]["peak_rate_percentage"].ToString());
                            //temp_peak = base_amount * (Convert.ToDecimal(dtrate.Rows[0]["peak_rate_percentage"].ToString())) / 100;
                            peak_rate = temp_peak + base_amount;
                        }
                        else
                        {
                            peak_rate = base_amount;
                        }
                        //calculate discount $ will take  to change
                        if (dtrate.Rows[0]["discount_percentage"].ToString() != null && dtrate.Rows[0]["discount_percentage"].ToString() != "")
                        {
                            temp_discount = Convert.ToDecimal(dtrate.Rows[0]["discount_percentage"].ToString());
                            // temp_discount = peak_rate * (Convert.ToDecimal(dtrate.Rows[0]["discount_percentage"].ToString())) / 100;
                            discount_rate = peak_rate - temp_discount;
                        }
                        else
                        {
                            discount_rate = peak_rate;
                        }
                        rate session_rate = new rate();
                        session_rate.amount = base_amount.ToString();
                        session_rate.peekrate = temp_peak.ToString();
                        session_rate.discount = temp_discount.ToString();
                        string json = JsonConvert.SerializeObject(session_rate);
                        // result = GetJson1(session_rate);


                        return json;

                    }//end calculation of amount

                    //check student amounte
                    else
                    {

                        return "tutor amount";

                    }

                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception e)
            {
                return "";
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
        public string getapplytobetutorlist([FromBody]JObject parameter)
        {
            DataTable dtMemberList = new DataTable();
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
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
        public string getsessiondetails([FromBody] JObject parameter)
        {
            DataTable dtsessiondata = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                if (parameter["session_id"] != null && parameter["session_id"].ToString() != "")
                {
                    dtsessiondata = objmasters.getsessiondetails(parameter["session_id"].ToString());
                    if (dtsessiondata != null)
                    {
                        result = GetJson1(dtsessiondata);
                    }
                    return result;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "securityIssue";
            }
        }

        [HttpPost]
        public string gettokentrationdatauser([FromBody] JObject parameter)
        {

            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                DataTable dt = objmasters.tokentrationdata(parameter["member_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                return result;

            }
            else
            {
                return "securityIssue";
            }
        }

        [HttpPost]
        public string getcoursetutor_student([FromBody] JObject parameter)
        {

            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                DataTable dt = objmasters.tutorandstudentcourselist(parameter["member_id"].ToString(), parameter["role"].ToString(), parameter["uni_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                return result;

            }
            else
            {
                return "securityIssue";
            }
        }
        //web bar graphdata
        [HttpPost]
        public List<object> GetBarGraphData([FromBody]JObject parameter)
        {
            List<object> iData = new List<object>();
            DataTable dtbardata = new DataTable();
            bool security_flag = true;
            //bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtbardata = objmasters.BarGraphData(parameter["member_id"].ToString());
                List<string> labels = new List<string>();
                List<decimal> lst_dataItem_1 = new List<decimal>();
                if (dtbardata != null)
                {
                    foreach (DataRow row in dtbardata.Rows)
                    {
                        // lst_dataItem_1.Add(25);
                        // labels.Add("03/07/2016-09/07/2016");
                        decimal Yaxis = Convert.ToDecimal(row["Yaxis"].ToString());
                        string WeekDates = row["WeekDates"].ToString();
                        labels.Add(WeekDates);
                        lst_dataItem_1.Add(Yaxis);
                    }
                    iData.Add(labels);
                    iData.Add(lst_dataItem_1);
                }
                else
                {
                    decimal Yaxis = 0;
                    string WeekDates = DateTime.Now.ToShortDateString();
                    labels.Add(WeekDates);
                    lst_dataItem_1.Add(Yaxis);

                    iData.Add(labels);
                    iData.Add(lst_dataItem_1);
                }

            }
            return iData;
        }


        //Iphone and android graphdata
        [HttpPost]
        public string GetBarGraphDataphone([FromBody]JObject parameter)
        {

            DataTable dt = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dt = objmasters.BarGraphData(parameter["member_id"].ToString());
                if (dt != null)
                {
                    result = GetJson1(dt);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }

        }

        [HttpPost]
        public string GetTotalIncomeGenerated([FromBody]JObject parameter)
        {

            DataTable dtTotalIncome = new DataTable();
            string result = "";
            bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {
                dtTotalIncome = objmasters.total_income_tutor(parameter["member_id"].ToString());
                if (dtTotalIncome != null)
                {
                    result = GetJson1(dtTotalIncome);
                }
                return result;
            }
            else
            {
                return "securityIssue";
            }

        }
        [HttpPost]
        public string getstripecharge([FromBody]JObject parameter)
        {

            DataTable dtTotalIncome = new DataTable();
            int noOfToken = Convert.ToInt16(parameter["amount"].ToString());
            string result = "";
            bool security_flag = true;// objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
            if (security_flag == true)
            {

                var total_amt = Math.Round((Convert.ToDouble(noOfToken) + 0.30) / (1 - 0.029), 2);   //calculation for stripe charge as per stripe doc
                var stripeCharge = Math.Round((Convert.ToDouble(total_amt) - Convert.ToDouble(noOfToken)), 2);


                return "{\"total_amt\":\"" + total_amt + "\",\"stripeCharge\":\"" + stripeCharge + "\"}"; ;
            }
            else
            {
                return "securityIssue";
            }

        }


        #endregion

        #region Save Method

        [HttpPost]
        public string updateDatatoBeTutor([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string[] str = new string[3];

            ProfileCreation profileData = new ProfileCreation();
            course_list obj_course_list = new course_list();

            bool security_flag = objmasters.securityCheck(parameter["ProfileCreation"]["member_id"].ToString(), parameter["ProfileCreation"]["loginToken"].ToString());
            if (security_flag == true)
            {

                if (parameter["ProfileCreation"] != null)
                    profileData = parameter["ProfileCreation"].ToObject<ProfileCreation>();

                if (parameter["course_list"]["course_list_tutoring"].ToString() != "")
                    obj_course_list = parameter["course_list"].ToObject<course_list>();


                //if (parameter["course_list_tutoring"] != null)
                //    course_list_tutoring = parameter["course_list"].ToObject<course_list_tutoring>();

                try
                {
                    DataTable dtMemberRoleData = objmasters.getCoursesUser(profileData.member_id);
                    if (dtMemberRoleData != null && dtMemberRoleData.Rows.Count > 0)
                    {
                        #region member_master
                        ////DS_MemberTables.member_masterRow memMstRow = objDS_MemberTables.member_master.Newmember_masterRow();
                        //DataRow drMemDetail = dtMemberDetail.Rows[0];
                        //dtMemberDetail.ImportRow(drMemDetail);

                        //drMemDetail["first_name"] = profileData.first_name;
                        //drMemDetail["middle_name"] = profileData.middle_name;
                        //drMemDetail["nick_name"] = profileData.nick_name;
                        //drMemDetail["short_bio"] = profileData.short_bio;
                        //drMemDetail["last_name"] = profileData.last_name;
                        //drMemDetail["phone_number1"] = profileData.phone_number1;

                        //drMemDetail["fun_campus"] = profileData.fun_campus;
                        //drMemDetail["major"] = profileData.major;

                        //drMemDetail["image"] = profileData.image;
                        //drMemDetail["feternity_id"] = profileData.feternity_id;
                        //drMemDetail["soriety_id"] = profileData.soriety_id;
                        //drMemDetail["last_modified_by"] = dtMemberDetail.Rows[0]["email_id"].ToString();
                        //drMemDetail["last_modified_date"] = System.DateTime.Now;
                        //drMemDetail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
                        //drMemDetail["classification"] = profileData.classification;

                        ////If user is only studen/tutor then default_role will be same but if he has both role the default role is TT.
                        //if (profileData.role_id == "ST" || profileData.role_id == "TT")
                        //    drMemDetail["default_role"] = profileData.role_id;
                        //else if (profileData.role_id == "BOTH")
                        //    drMemDetail["default_role"] = "TT";

                        //objDS_MemberTables.member_master.ImportRow(drMemDetail);

                        #endregion

                        #region member_role

                        if (obj_course_list != null)
                        {
                            if (obj_course_list.course_list_tutoring.Length > 0)
                            {
                                for (int i = 0; i < obj_course_list.course_list_tutoring.Length; i++)
                                {
                                    DS_MemberTables.member_roleRow memRoleRow = objDS_MemberTables.member_role.Newmember_roleRow();

                                    memRoleRow.sr_no = randomNo.ToString() + "__" + i;  //It is just passing the primary which will not store in db.
                                    memRoleRow.university_id = dtMemberRoleData.Rows[0]["university_id"].ToString();
                                    memRoleRow.member_id = dtMemberRoleData.Rows[0]["member_id"].ToString();
                                    memRoleRow.role_id = "TT";
                                    memRoleRow.course_code = obj_course_list.course_list_tutoring[i];
                                    memRoleRow.is_active = "Y";
                                    memRoleRow.is_approved = "N";
                                    memRoleRow.created_by = dtMemberRoleData.Rows[0]["member_id"].ToString();
                                    memRoleRow.created_date = System.DateTime.Now;
                                    memRoleRow.created_host = HttpContext.Current.Request.UserHostName;

                                    objDS_MemberTables.member_role.Addmember_roleRow(memRoleRow);

                                }
                            }
                        }
                        #endregion

                        #region member_tutor_transcript
                        DataTable dtTranscript = objmasters.GetMemberTrspritData(profileData.member_id);
                        if (profileData.transprit_file_desc != null && profileData.transprit_file_desc != string.Empty)
                        {
                            DS_MemberTables.member_tutor_transpritRow memTuTranscriptRow = objDS_MemberTables.member_tutor_transprit.Newmember_tutor_transpritRow();
                            if (dtTranscript == null || dtTranscript.Rows.Count == 0)
                            {
                                memTuTranscriptRow.sr_no = randomNo.ToString();  //It is just passing the primary which will not store in db.
                                memTuTranscriptRow.university_id = dtMemberRoleData.Rows[0]["university_id"].ToString();    //profileData.uni;
                                memTuTranscriptRow.member_id = dtMemberRoleData.Rows[0]["member_id"].ToString();
                                memTuTranscriptRow.transprit_file_desc = profileData.transprit_file_desc;
                                memTuTranscriptRow.file_path = profileData.file_path;
                                memTuTranscriptRow.is_active = "Y";
                                memTuTranscriptRow.created_by = dtMemberRoleData.Rows[0]["member_id"].ToString();
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

                        objBLReturnObject = objmasters.updateDatatoBetutor(objDS_MemberTables);
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

        [HttpPost]
        public string updateDatatoBeStudent([FromBody]JObject parameter)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            bool flag = false;

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string[] str = new string[3];

            ProfileCreation profileData = new ProfileCreation();
            course_list obj_course_list = new course_list();

            bool security_flag = objmasters.securityCheck(parameter["ProfileCreation"]["member_id"].ToString(), parameter["ProfileCreation"]["loginToken"].ToString());
            if (security_flag == true)
            {

                if (parameter["ProfileCreation"] != null)
                    profileData = parameter["ProfileCreation"].ToObject<ProfileCreation>();

                if (parameter["course_list"]["course_list_learning"].ToString() != "")
                    obj_course_list = parameter["course_list"].ToObject<course_list>();


                //if (parameter["course_list_tutoring"] != null)
                //    course_list_tutoring = parameter["course_list"].ToObject<course_list_tutoring>();

                try
                {
                    DataTable dtMemberRoleData = objmasters.getCoursesUser(profileData.member_id);
                    if (dtMemberRoleData != null && dtMemberRoleData.Rows.Count > 0)
                    {
                        #region member_master
                        ////DS_MemberTables.member_masterRow memMstRow = objDS_MemberTables.member_master.Newmember_masterRow();
                        //DataRow drMemDetail = dtMemberDetail.Rows[0];
                        //dtMemberDetail.ImportRow(drMemDetail);

                        //drMemDetail["first_name"] = profileData.first_name;
                        //drMemDetail["middle_name"] = profileData.middle_name;
                        //drMemDetail["nick_name"] = profileData.nick_name;
                        //drMemDetail["short_bio"] = profileData.short_bio;
                        //drMemDetail["last_name"] = profileData.last_name;
                        //drMemDetail["phone_number1"] = profileData.phone_number1;

                        //drMemDetail["fun_campus"] = profileData.fun_campus;
                        //drMemDetail["major"] = profileData.major;

                        //drMemDetail["image"] = profileData.image;
                        //drMemDetail["feternity_id"] = profileData.feternity_id;
                        //drMemDetail["soriety_id"] = profileData.soriety_id;
                        //drMemDetail["last_modified_by"] = dtMemberDetail.Rows[0]["email_id"].ToString();
                        //drMemDetail["last_modified_date"] = System.DateTime.Now;
                        //drMemDetail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
                        //drMemDetail["classification"] = profileData.classification;

                        ////If user is only studen/tutor then default_role will be same but if he has both role the default role is TT.
                        //if (profileData.role_id == "ST" || profileData.role_id == "TT")
                        //    drMemDetail["default_role"] = profileData.role_id;
                        //else if (profileData.role_id == "BOTH")
                        //    drMemDetail["default_role"] = "TT";

                        //objDS_MemberTables.member_master.ImportRow(drMemDetail);

                        #endregion

                        #region member_role

                        if (obj_course_list != null)
                        {
                            if (obj_course_list.course_list_learning.Length > 0)
                            {
                                for (int i = 0; i < obj_course_list.course_list_learning.Length; i++)
                                {
                                    DS_MemberTables.member_roleRow memRoleRow = objDS_MemberTables.member_role.Newmember_roleRow();

                                    memRoleRow.sr_no = randomNo.ToString() + "__" + i;  //It is just passing the primary which will not store in db.
                                    memRoleRow.university_id = dtMemberRoleData.Rows[0]["university_id"].ToString();
                                    memRoleRow.member_id = dtMemberRoleData.Rows[0]["member_id"].ToString();
                                    memRoleRow.role_id = "ST";
                                    memRoleRow.course_code = obj_course_list.course_list_learning[i];
                                    memRoleRow.is_approved = "Y";
                                    memRoleRow.is_active = "Y";
                                    memRoleRow.created_by = dtMemberRoleData.Rows[0]["member_id"].ToString();
                                    memRoleRow.created_date = System.DateTime.Now;
                                    memRoleRow.created_host = HttpContext.Current.Request.UserHostName;

                                    objDS_MemberTables.member_role.Addmember_roleRow(memRoleRow);

                                }
                            }
                        }
                        #endregion

                        objBLReturnObject = objmasters.updateDatatoBetutor(objDS_MemberTables);
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

        [HttpPost]
        public string SaveScheduleAppointment([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();
            DataTable dtrate = new DataTable();
            DataTable dt_checkamount = new DataTable();
            Document objDocument = new Document();
            Document Doc = new Document();
            String message = "";
            String DocNo = "";

            string result = "";
            bool flag = false;
            decimal base_amount = 0;
            decimal peak_rate = 0;
            decimal discount_rate = 0;
            decimal temp_peak = 0;
            decimal temp_discount = 0;
            try
            {

                bool security_flag = objmasters.securityCheck(parameter["tokendata"]["member_id"].ToString(), parameter["tokendata"]["Token"].ToString());
                if (security_flag == true)
                {
                    if (parameter["member"] != null)
                    {

                        Memberid = parameter["member"].ToObject<member>();

                    }
                    if (parameter["sessiontime"]["session_end_time"].ToString() == "" && parameter["sessiontime"]["session_end_time"].ToString() == null) { return "missing"; }
                    sessiondata = parameter["sessiontime"].ToObject<sessiontime>();


                    if (Memberid.member_list.Length <= 0)
                    {
                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "member list not empty");
                        return "member list not empty ";

                    }

                    Boolean checkforSessions = objmasters.CheckConfirmedSessionRequest(parameter["sessiontime"]["session_date"].ToString(), parameter["sessiontime"]["session_start_time"].ToString(), parameter["sessiontime"]["session_end_time"].ToString(), parameter["tutor"]["tutor_id"].ToString());

                    if (checkforSessions == true)
                    // if (true)
                    {

                        if (parameter["sessiontime"]["isupdate"].ToString() == "false")
                        {

                            // check rate is avalible/or not
                            int count_no_student = (Memberid.member_list.Length - 1);

                            dtrate = objmasters.getSessionRate(parameter["sessiontime"]["session_date"].ToString(), parameter["sessiontime"]["session_type"].ToString(), parameter["sessiontime"]["course_type"].ToString(), parameter["tutor"]["tutor_id"].ToString(), parameter["sessiontime"]["university_id"].ToString(), parameter["sessiontime"]["student_rate_id"].ToString());
                            if (dtrate != null)
                            {

                                //   double timediff = Convert.ToDateTime(parameter["sessiontime"]["session_end_time"]).Subtract(Convert.ToDateTime(parameter["sessiontime"]["session_start_time"])).TotalHours;
                                base_amount = ((Convert.ToDecimal(dtrate.Rows[0]["amount"].ToString()) * Convert.ToDecimal(parameter["sessiontime"]["duration"].ToString()) / 60) + (count_no_student * 5));
                                //calculate peak rate  amount of peak rate and discount will take on $ not per
                                if (dtrate.Rows[0]["peak_rate_percentage"].ToString() != null && dtrate.Rows[0]["peak_rate_percentage"].ToString() != "")
                                {
                                    temp_peak = Convert.ToDecimal(dtrate.Rows[0]["peak_rate_percentage"].ToString());
                                    //temp_peak = base_amount * (Convert.ToDecimal(dtrate.Rows[0]["peak_rate_percentage"].ToString())) / 100;
                                    peak_rate = temp_peak + base_amount;
                                }
                                else
                                {
                                    peak_rate = base_amount;
                                }
                                //calculate discount $ will take  to change
                                if (dtrate.Rows[0]["discount_percentage"].ToString() != null && dtrate.Rows[0]["discount_percentage"].ToString() != "")
                                {
                                    temp_discount = Convert.ToDecimal(dtrate.Rows[0]["discount_percentage"].ToString());
                                    // temp_discount = peak_rate * (Convert.ToDecimal(dtrate.Rows[0]["discount_percentage"].ToString())) / 100;
                                    discount_rate = peak_rate - temp_discount;
                                }
                                else
                                {
                                    discount_rate = peak_rate;
                                }
                            }//end calculation of amount

                            //check student amounte
                            else
                            {
                                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "tutor amount");
                                return "tutor amount";

                            }
                        }
                        else
                        {

                        }

                        dt_checkamount = objmasters.checkuseramount_for_session(discount_rate, parameter["tokendata"]["member_id"].ToString());

                        if (dt_checkamount != null)
                        {

                            #region save session time

                            if (parameter["sessiontime"]["isupdate"].ToString() == "false")
                            {
                                #region save fresh data /new

                                if (parameter["sessiontime"] != null)
                                {
                                    DS_ScheduleAppointment.session_timingRow seesionttimeRow = objDS_ScheduleAppointment.session_timing.Newsession_timingRow();
                                    seesionttimeRow.session_id = "1";
                                    seesionttimeRow.university_id = sessiondata.university_id;
                                    seesionttimeRow.duration = sessiondata.duration;
                                    seesionttimeRow.session_type = sessiondata.session_type;
                                    seesionttimeRow.course_id = sessiondata.course_id;
                                    seesionttimeRow.session_date = Convert.ToDateTime(sessiondata.session_date);
                                    seesionttimeRow.session_start_time = sessiondata.session_start_time;
                                    seesionttimeRow.university_id = sessiondata.university_id;
                                    seesionttimeRow.session_type = sessiondata.session_type;
                                    seesionttimeRow.session_end_time = sessiondata.session_end_time;
                                    seesionttimeRow.booked_by = sessiondata.Bookby;
                                    seesionttimeRow.session_venue = sessiondata.session_venue;

                                    //added on 30/5/16
                                    if (parameter["tutor"]["tutor_id"] != null)
                                        seesionttimeRow.tutor_id = parameter["tutor"]["tutor_id"].ToString();

                                    seesionttimeRow.appointment_confirmed = "N"; // It is just temparory set. 17/5/16
                                    seesionttimeRow.is_active = "Y";
                                    seesionttimeRow.created_by = parameter["tokendata"]["member_id"].ToString();
                                    seesionttimeRow.created_date = System.DateTime.Now;
                                    seesionttimeRow.created_host = HttpContext.Current.Request.UserHostName;

                                    objDS_ScheduleAppointment.session_timing.Addsession_timingRow(seesionttimeRow);



                                    #region notification : table
                                    DS_ScheduleAppointment.notificationRow notifyRow = objDS_ScheduleAppointment.notification.NewnotificationRow();

                                    notifyRow.doc_no = "1";
                                    notifyRow.placeholder = "1";
                                    notifyRow.university_id = sessiondata.university_id;
                                    notifyRow.from_member = parameter["tokendata"]["member_id"].ToString();
                                    notifyRow.from_member_role = "ST";
                                    notifyRow.to_member = parameter["tutor"]["tutor_id"].ToString();
                                    notifyRow.to_member_role = "TT";
                                    notifyRow.notification_date = System.DateTime.Now;
                                    notifyRow.template = "Sharpen your pen.. a new dojo session awaits you!";
                                    notifyRow._event = "SessionScheduleReq";
                                    notifyRow.is_read = "N";
                                    notifyRow.is_accept = "N";
                                    notifyRow.is_active = "Y";
                                    notifyRow.created_by = parameter["tokendata"]["member_id"].ToString();
                                    notifyRow.created_date = System.DateTime.Now;
                                    notifyRow.created_host = HttpContext.Current.Request.UserHostName;

                                    objDS_ScheduleAppointment.notification.AddnotificationRow(notifyRow);


                                    #endregion

                                    #region push alert
                                    DS_ScheduleAppointment.alertmessageRow alert_new_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                    alert_new_row.Doc_no = "1";
                                    alert_new_row.university_id = parameter["sessiontime"]["university_id"].ToString();
                                    alert_new_row.to_member = parameter["tutor"]["tutor_id"].ToString();
                                    alert_new_row.to_member_role = "TT";
                                    alert_new_row.notification_date = System.DateTime.Now;
                                    alert_new_row._event = "Message";
                                    alert_new_row.template = "Sharpen your pen.. a new dojo session awaits you!";
                                    alert_new_row.placeholder = "";
                                    alert_new_row.is_read = "N";
                                    alert_new_row.is_active = "Y";
                                    alert_new_row.senddate = System.DateTime.UtcNow;
                                    alert_new_row.utc_time = TimeSpan.Parse(System.DateTime.UtcNow.ToString("HH:mm:ss"));
                                    alert_new_row.created_by = "system";
                                    alert_new_row.created_date = System.DateTime.Now;
                                    alert_new_row.created_host = HttpContext.Current.Request.UserHostName;

                                    objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_new_row);


                                    #endregion
                                }

                                //insert session_particiption TT
                                if (parameter["tutor"]["tutor_id"] != null)
                                {
                                    DS_ScheduleAppointment.session_participantsRow sessionparticipationRow = objDS_ScheduleAppointment.session_participants.Newsession_participantsRow();


                                    sessionparticipationRow.university_id = sessiondata.university_id;
                                    sessionparticipationRow.session_id = randomNo.ToString();
                                    sessionparticipationRow.member_id = parameter["tutor"]["tutor_id"].ToString();
                                    sessionparticipationRow.role_id = "TT";
                                    sessionparticipationRow.course_id = sessiondata.course_id;
                                    sessionparticipationRow.is_active = "Y";
                                    sessionparticipationRow.created_by = parameter["tokendata"]["member_id"].ToString();
                                    sessionparticipationRow.created_date = System.DateTime.Now;
                                    sessionparticipationRow.created_host = HttpContext.Current.Request.UserHostName;

                                    objDS_ScheduleAppointment.session_participants.Addsession_participantsRow(sessionparticipationRow);


                                }
                                //ST 

                                if (parameter["member"] != null)
                                {
                                    if (Memberid.member_list != null && Memberid.member_list.Length > 0)
                                        for (int i = 0; i < Memberid.member_list.Length; i++)
                                        {
                                            DS_ScheduleAppointment.session_participantsRow sessionparticipationRow = objDS_ScheduleAppointment.session_participants.Newsession_participantsRow();


                                            sessionparticipationRow.university_id = sessiondata.university_id;
                                            sessionparticipationRow.session_id = randomNo.ToString();
                                            sessionparticipationRow.member_id = Memberid.member_list[i];
                                            sessionparticipationRow.role_id = "ST";
                                            sessionparticipationRow.course_id = sessiondata.course_id;
                                            sessionparticipationRow.is_active = "Y";
                                            sessionparticipationRow.created_by = parameter["tokendata"]["member_id"].ToString();
                                            sessionparticipationRow.created_date = System.DateTime.Now;
                                            sessionparticipationRow.created_host = HttpContext.Current.Request.UserHostName;

                                            objDS_ScheduleAppointment.session_participants.Addsession_participantsRow(sessionparticipationRow);


                                        }

                                }

                                //objBLReturnObject = objmasters.SaveSessiontimedata(objDS_ScheduleAppointment);
                                //if (objBLReturnObject.ExecutionStatus == 1)
                                //{
                                #region sessionbookin
                                DS_Transtration.fn_session_bookingRow fn_book_row = objDs_trastration.fn_session_booking.Newfn_session_bookingRow();
                                fn_book_row.doc_no = "1";
                                fn_book_row.session_id = "12";
                                fn_book_row.session_tutor = parameter["tutor"]["tutor_id"].ToString();
                                fn_book_row.session_main_rfe_date = Convert.ToDateTime(sessiondata.session_date);
                                fn_book_row.total_session_charge = peak_rate;
                                if (dtrate.Rows[0]["discount_percentage"].ToString() != "" && dtrate.Rows[0]["discount_percentage"].ToString() != null)
                                {
                                    fn_book_row.is_disscount_applicable = "Y";
                                    fn_book_row.disscount_amount = temp_discount;
                                }
                                else
                                {
                                    fn_book_row.is_disscount_applicable = "N";
                                    fn_book_row.disscount_amount = 0;
                                }
                                fn_book_row.session_mycc_charge = (peak_rate * mycc_charge) / 100;
                                fn_book_row.session_tutor_charge = peak_rate - fn_book_row.session_mycc_charge; //it deduct  my cc charge after amount 

                                objDs_trastration.fn_session_booking.Addfn_session_bookingRow(fn_book_row);

                                #endregion

                                #region token balance update
                                DataTable dtstudent = objmasters.gettokenbal(parameter["tokendata"]["member_id"].ToString());

                                dtstudent.Rows[0]["total_debit"] = (Convert.ToDecimal(dtstudent.Rows[0]["total_debit"].ToString()) + discount_rate);
                                dtstudent.Rows[0]["balance_token"] = (Convert.ToDecimal(dtstudent.Rows[0]["balance_token"].ToString()) - discount_rate);
                                objDs_trastration.fn_token_balance.ImportRow(dtstudent.Rows[0]);
                                #endregion


                                #region mycc acount balance
                                DataTable dtcc = objmasters.gettokenbal(mycc_id);
                                dtcc.Rows[0]["total_credit"] = (Convert.ToDecimal(dtcc.Rows[0]["total_credit"].ToString()) + discount_rate);
                                dtcc.Rows[0]["balance_token"] = (Convert.ToDecimal(dtcc.Rows[0]["balance_token"].ToString()) + discount_rate);
                                objDs_trastration.fn_token_balance.ImportRow(dtcc.Rows[0]);

                                #endregion

                                #region  fn transation 2 update student and cc account
                                DS_Transtration.fn_token_transtionRow token_transtration_row = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                                token_transtration_row.doc_no = "1";
                                token_transtration_row.doc_date = System.DateTime.Now;
                                token_transtration_row.ref_transtion_type = "sessionbook";
                                token_transtration_row.ref_transtion_doc_date = System.DateTime.Now;
                                token_transtration_row.ref_transtion_doc_no = "12";
                                token_transtration_row.amount = discount_rate;
                                token_transtration_row.type_credit_debit = "debit";
                                token_transtration_row.balance_after_transtion = (Convert.ToDecimal(dtstudent.Rows[0]["balance_token"].ToString()));
                                token_transtration_row.member_id = parameter["tokendata"]["member_id"].ToString();
                                token_transtration_row.created_by = parameter["tokendata"]["member_id"].ToString();
                                token_transtration_row.created_date = System.DateTime.Now;
                                token_transtration_row.created_host = HttpContext.Current.Request.UserHostName;
                                objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row);


                                DS_Transtration.fn_token_transtionRow token_transtration_row2 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                                token_transtration_row2.doc_no = "2";
                                token_transtration_row2.doc_date = System.DateTime.Now;
                                token_transtration_row2.ref_transtion_type = "sessionbook";
                                token_transtration_row2.ref_transtion_doc_date = System.DateTime.Now;
                                token_transtration_row2.ref_transtion_doc_no = "12";
                                token_transtration_row2.amount = discount_rate;
                                token_transtration_row2.type_credit_debit = "Credit";
                                token_transtration_row2.member_id = mycc_id;
                                token_transtration_row2.balance_after_transtion = (Convert.ToDecimal(dtcc.Rows[0]["balance_token"].ToString()));

                                token_transtration_row2.created_by = mycc_id;
                                token_transtration_row2.created_date = System.DateTime.Now;
                                token_transtration_row2.created_host = HttpContext.Current.Request.UserHostName;
                                objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row2);

                                #endregion


                                objBLReturnObject = objmasters.SaveSessiontimedata(objDS_ScheduleAppointment, objDs_trastration);
                                if (objBLReturnObject.ExecutionStatus == 1)
                                {
                                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Save");
                                    return "Save";
                                }
                            }

                            else
                            {
                                #region update session data
                                if (parameter["sessiontime"]["isupdate"].ToString() == "true")
                                {


                                    DataTable dtsession = objmasters.getPerticularSessionDetail(parameter["sessiontime"]["sessionid"].ToString());
                                    DataRow dr_session_detail = dtsession.Rows[0];
                                    dtsession.ImportRow(dr_session_detail);

                                    dr_session_detail["session_date"] = Convert.ToDateTime(sessiondata.session_date);
                                    dr_session_detail["session_start_time"] = sessiondata.session_start_time;
                                    dr_session_detail["session_end_time"] = sessiondata.session_end_time;
                                    dr_session_detail["session_type"] = sessiondata.session_type;
                                    dr_session_detail["session_venue"] = sessiondata.session_venue;
                                    dr_session_detail["last_modified_by"] = parameter["tokendata"]["member_id"].ToString();
                                    dr_session_detail["last_modified_date"] = System.DateTime.Now;
                                    dr_session_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                                    objDS_ScheduleAppointment.session_timing.ImportRow(dr_session_detail);

                                    if (parameter["member"] != null)
                                    {
                                        //if (Memberid.member_list != null && Memberid.member_list.Length > 0)
                                        //    for (int i = 0; i < Memberid.member_list.Length; i++)
                                        //    {
                                        //        DS_ScheduleAppointment.session_participantsRow sessionparticipationRow = objDS_ScheduleAppointment.session_participants.Newsession_participantsRow();


                                        //        sessionparticipationRow.university_id = sessiondata.university_id;
                                        //        sessionparticipationRow.session_id = parameter["sessiontime"]["sessionid"].ToString();
                                        //        sessionparticipationRow.member_id = Memberid.member_list[i];
                                        //        sessionparticipationRow.role_id = "ST";
                                        //        sessionparticipationRow.course_id = sessiondata.course_id;
                                        //        sessionparticipationRow.is_active = "Y";
                                        //        sessionparticipationRow.created_by = parameter["tokendata"]["member_id"].ToString();
                                        //        sessionparticipationRow.created_date = System.DateTime.Now;
                                        //        sessionparticipationRow.created_host = HttpContext.Current.Request.UserHostName;

                                        //        objDS_ScheduleAppointment.session_participants.Addsession_participantsRow(sessionparticipationRow);
                                        //    }

                                    }
                                    #region alert message
                                    DataTable dt_student = objmasters.user_for_set_appointment(dtsession.Rows[0]["booked_by"].ToString(), dtsession.Rows[0]["course_id"].ToString());
                                    DateTime timealert = objmasters.timeconevert(dt_student.Rows[0]["timezone"].ToString(), dtsession.Rows[0]["session_date"].ToString(), dtsession.Rows[0]["session_start_time"].ToString());
                                    DataTable sessionpatration = objmasters.getsessionpartiscption_for_alert(parameter["sessiontime"]["sessionid"].ToString());
                                    DateTime sendtimehouse = timealert.AddHours(-5);
                                    DateTime sendtimemin = timealert.AddMinutes(-30);
                                    DateTime sendtimemin15 = timealert.AddMinutes(-15);

                                    for (var t = 0; t < sessionpatration.Rows.Count; t++)
                                    { // 5 house
                                        DS_ScheduleAppointment.alertmessageRow alert_new_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                        alert_new_row.Doc_no = t + "" + 1;
                                        alert_new_row.university_id = dt_student.Rows[0]["university"].ToString();
                                        alert_new_row.to_member = sessionpatration.Rows[t]["member_id"].ToString();
                                        alert_new_row.to_member_role = sessionpatration.Rows[t]["role_id"].ToString();
                                        alert_new_row.notification_date = System.DateTime.Now;
                                        alert_new_row._event = "session";
                                        alert_new_row.template = " Heads up!! your " + dt_student.Rows[0]["course_name"] + " session is only 6 hours away... hope you are prepared!";
                                        alert_new_row.placeholder = parameter["sessiontime"]["sessionid"].ToString();
                                        alert_new_row.is_read = "N";
                                        alert_new_row.is_active = "Y";
                                        alert_new_row.senddate = Convert.ToDateTime(sendtimehouse.ToShortDateString());
                                        alert_new_row.utc_time = TimeSpan.Parse(sendtimehouse.ToString("HH:mm:ss"));
                                        alert_new_row.created_by = "admin";
                                        alert_new_row.created_date = System.DateTime.Now;
                                        alert_new_row.created_host = HttpContext.Current.Request.UserHostName;

                                        objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_new_row);
                                        // 30 min
                                        DS_ScheduleAppointment.alertmessageRow alert_new_row3 = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                        alert_new_row3.Doc_no = t + "" + 1 + "" + 1;
                                        alert_new_row3.university_id = dt_student.Rows[0]["university"].ToString();
                                        alert_new_row3.to_member = sessionpatration.Rows[t]["member_id"].ToString();
                                        alert_new_row3.to_member_role = sessionpatration.Rows[t]["role_id"].ToString();
                                        alert_new_row3.notification_date = System.DateTime.Now;
                                        alert_new_row3._event = "session";
                                        alert_new_row3.placeholder = parameter["sessiontime"]["sessionid"].ToString();
                                        alert_new_row3.template = "Getting close.... " + dt_student.Rows[0]["course_name"] + " session is right around the corner. you have 30 mins to get your stuff together";
                                        alert_new_row3.is_read = "N";
                                        alert_new_row3.is_active = "Y";
                                        alert_new_row3.senddate = Convert.ToDateTime(sendtimemin.ToShortDateString());
                                        alert_new_row3.utc_time = TimeSpan.Parse(sendtimemin.ToString("HH:mm:ss"));
                                        alert_new_row3.created_by = "admin";
                                        alert_new_row3.created_date = System.DateTime.Now;
                                        alert_new_row3.created_host = HttpContext.Current.Request.UserHostName;

                                        objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_new_row3);
                                        //15 min
                                        if (sessionpatration.Rows[t]["role_id"].ToString() == "ST")
                                        {
                                            DS_ScheduleAppointment.alertmessageRow alert_new_row4 = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                            alert_new_row4.Doc_no = t + "" + 1 + "" + 1;
                                            alert_new_row4.university_id = dt_student.Rows[0]["university"].ToString();
                                            alert_new_row4.to_member = sessionpatration.Rows[t]["member_id"].ToString();
                                            alert_new_row4.to_member_role = sessionpatration.Rows[t]["role_id"].ToString();
                                            alert_new_row4.notification_date = System.DateTime.Now;
                                            alert_new_row4._event = "session";
                                            alert_new_row4.placeholder = parameter["sessiontime"]["sessionid"].ToString();
                                            alert_new_row4.template = "Getting close.... " + dt_student.Rows[0]["course_name"] + " session is right around the corner. you have 30 mins to get your stuff together";
                                            alert_new_row4.is_read = "N";
                                            alert_new_row4.is_active = "Y";
                                            alert_new_row4.senddate = Convert.ToDateTime(sendtimemin15.ToShortDateString());
                                            alert_new_row4.utc_time = TimeSpan.Parse(sendtimemin15.ToString("HH:mm:ss"));
                                            alert_new_row4.created_by = "admin";
                                            alert_new_row4.created_date = System.DateTime.Now;
                                            alert_new_row4.created_host = HttpContext.Current.Request.UserHostName;

                                            objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_new_row4);
                                        }
                                    }

                                    //tutor send 
                                    //DS_ScheduleAppointment.alertmessageRow alert_new_row2 = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                    //alert_new_row2.Doc_no = "2";
                                    //alert_new_row2.university_id = dt_student.Rows[0]["university"].ToString();
                                    //alert_new_row2.to_member = parameter["tutor"]["tutor_id"].ToString();
                                    //alert_new_row2.to_member_role = "TT";
                                    //alert_new_row2.notification_date = System.DateTime.Now;
                                    //alert_new_row2._event = "session";
                                    //alert_new_row2.template = " Heads up!! your " + dt_student.Rows[0]["course_name"] + " session is only 6 hours away... hope you are prepared!";
                                    //alert_new_row2.is_read = "N";
                                    //alert_new_row2.is_active = "Y";
                                    //alert_new_row2.senddate = Convert.ToDateTime(sendtimehouse.ToShortDateString());
                                    //alert_new_row2.utc_time = TimeSpan.Parse(sendtimehouse.ToString("HH:mm:ss"));
                                    //alert_new_row2.created_by = "admin";
                                    //alert_new_row2.created_date = System.DateTime.Now;
                                    //alert_new_row2.created_host = HttpContext.Current.Request.UserHostName;

                                    //objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_new_row2);
                                    //// tutor 30 min entry
                                    //DS_ScheduleAppointment.alertmessageRow alert_new_row4 = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                    //alert_new_row4.Doc_no = "2";
                                    //alert_new_row4.university_id = dt_student.Rows[0]["university"].ToString();
                                    //alert_new_row4.to_member = parameter["tutor"]["tutor_id"].ToString();
                                    //alert_new_row4.to_member_role = "TT";
                                    //alert_new_row4.notification_date = System.DateTime.Now;
                                    //alert_new_row4._event = "session";
                                    //alert_new_row4.template = "Getting close.... " + dt_student.Rows[0]["course_name"] + " session is right around the corner. you have 30 mins to get your stuff together";
                                    //alert_new_row4.is_read = "N";
                                    //alert_new_row4.is_active = "Y";
                                    //alert_new_row4.senddate = Convert.ToDateTime(sendtimemin.ToShortDateString());
                                    //alert_new_row4.utc_time = TimeSpan.Parse(sendtimemin.ToString("HH:mm:ss"));
                                    //alert_new_row4.created_by = "admin";
                                    //alert_new_row4.created_date = System.DateTime.Now;
                                    //alert_new_row4.created_host = HttpContext.Current.Request.UserHostName;

                                    //objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_new_row4);

                                    #endregion

                                    objBLReturnObject = objmasters.updateSessiontimedata(objDS_ScheduleAppointment);
                                    if (objBLReturnObject.ExecutionStatus == 1)
                                    {
                                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Save");
                                        return "Save";
                                    }

                                }

                                #endregion
                            }
                                #endregion
                        }
                        else
                        {
                            ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "amount");
                            return "amount";
                        }

                    }
                    else
                    {
                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "SessionBooked");
                        return "SessionBooked";
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
            }
            ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + result);
            return result;
        }


        [HttpPost]
        public string setSessionResponse([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();
            string result = "";
            bool flag = false;

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    #region

                    DataTable dtsession = objmasters.getPerticularSessionDetail(parameter["session_id"].ToString());
                    if (dtsession != null && dtsession.Rows.Count > 0)
                    {
                        DS_ScheduleAppointment.session_timingRow seesionttimeRow = objDS_ScheduleAppointment.session_timing.Newsession_timingRow();

                        DataRow dr_session_detail = dtsession.Rows[0];
                        dtsession.ImportRow(dr_session_detail);


                        if (parameter["appointment_status"].ToString() == "R")
                        {
                            dr_session_detail["appointment_confirmed"] = "N";
                            dr_session_detail["is_active"] = "N";

                        }
                        else
                        {
                            dr_session_detail["appointment_confirmed"] = parameter["appointment_status"].ToString();

                        }
                        dr_session_detail["last_modified_by"] = parameter["member_id"].ToString();
                        dr_session_detail["last_modified_date"] = System.DateTime.Now;
                        dr_session_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
                        objDS_ScheduleAppointment.session_timing.ImportRow(dr_session_detail);


                        #region reject to transfer

                        if (parameter["appointment_status"].ToString() == "R")
                        {
                            DataTable fn_seesion_booking = objmasters.getseeionbooking(parameter["session_id"].ToString());
                            DataTable session_time = objmasters.getsessiondetails(parameter["session_id"].ToString());

                            #region mycc acount balance
                            DataTable dtcc = objmasters.gettokenbal(mycc_id);
                            decimal temp_debit = 0;
                            decimal temp_credite = 0;
                            decimal amount_student = 0;
                            decimal amount_of_cancle_charge = 0;
                            if (fn_seesion_booking.Rows[0]["disscount_amount"].ToString() != null && fn_seesion_booking.Rows[0]["disscount_amount"].ToString() != "")
                            {
                                amount_student = ((Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString())) - (Convert.ToDecimal(fn_seesion_booking.Rows[0]["disscount_amount"].ToString())));


                            }
                            else
                            {
                                amount_student = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString()));

                            }
                            amount_of_cancle_charge = amount_student;


                            dtcc.Rows[0]["total_debit"] = (Convert.ToDecimal(dtcc.Rows[0]["total_credit"].ToString()) + (amount_student));

                            var bal_befor = dtcc.Rows[0]["balance_token"].ToString();

                            // dtcc.Rows[0]["balance_token"] = (Convert.ToDecimal(dtcc.Rows[0]["balance_token"].ToString()) + amount_of_cancle_charge);
                            dtcc.Rows[0]["balance_token"] = (Convert.ToDecimal(dtcc.Rows[0]["balance_token"].ToString()) - amount_student);
                            objDs_trastration.fn_token_balance.ImportRow(dtcc.Rows[0]);

                            #endregion
                            #region student account
                            decimal student_credit = 0;
                            DataTable dtstudentacc = objmasters.gettokenbal(session_time.Rows[0]["booked_by"].ToString());
                            // student_credit = (amount_student - amount_of_cancle_charge);

                            dtstudentacc.Rows[0]["total_credit"] = (Convert.ToDecimal(dtstudentacc.Rows[0]["total_credit"].ToString()) + (amount_student));
                            dtstudentacc.Rows[0]["balance_token"] = (Convert.ToDecimal(dtstudentacc.Rows[0]["balance_token"].ToString()) + (amount_student));
                            objDs_trastration.fn_token_balance.ImportRow(dtstudentacc.Rows[0]);
                            #endregion

                            #region token transtraion
                            //my tutor account

                            DS_Transtration.fn_token_transtionRow token_transtration_row = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                            token_transtration_row.doc_no = "1";
                            token_transtration_row.doc_date = System.DateTime.Now;
                            token_transtration_row.ref_transtion_type = "seesion Reject";
                            token_transtration_row.ref_transtion_doc_date = System.DateTime.Now;
                            token_transtration_row.ref_transtion_doc_no = parameter["session_id"].ToString();
                            token_transtration_row.amount = amount_of_cancle_charge;
                            token_transtration_row.type_credit_debit = "debit";

                            token_transtration_row.balance_after_transtion = (Convert.ToDecimal(dtcc.Rows[0]["balance_token"].ToString()));
                            token_transtration_row.member_id = mycc_id;
                            token_transtration_row.created_by = mycc_id;
                            token_transtration_row.created_date = System.DateTime.Now;
                            token_transtration_row.created_host = HttpContext.Current.Request.UserHostName;

                            objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row);

                            // my cc account
                            DS_Transtration.fn_token_transtionRow token_transtration_row2 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                            token_transtration_row2.doc_no = "2";
                            token_transtration_row2.doc_date = System.DateTime.Now;
                            token_transtration_row2.ref_transtion_type = "Session Reject";
                            token_transtration_row2.ref_transtion_doc_date = System.DateTime.Now;
                            token_transtration_row2.ref_transtion_doc_no = parameter["session_id"].ToString();
                            token_transtration_row2.amount = amount_student;
                            token_transtration_row2.type_credit_debit = "Credit";

                            token_transtration_row2.balance_after_transtion = Convert.ToDecimal(dtstudentacc.Rows[0]["balance_token"].ToString());
                            token_transtration_row2.member_id = session_time.Rows[0]["booked_by"].ToString();
                            token_transtration_row2.created_by = session_time.Rows[0]["booked_by"].ToString();
                            token_transtration_row2.created_date = System.DateTime.Now;
                            token_transtration_row2.created_host = HttpContext.Current.Request.UserHostName;

                            objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row2);






                            #endregion





                        #endregion
                        }


                    #endregion
                            #endregion
                        #region alert message

                        if (parameter["appointment_status"].ToString() != "R")
                        {


                            #region alert message
                            DataTable dt_student = objmasters.user_for_set_appointment(dtsession.Rows[0]["booked_by"].ToString(), dtsession.Rows[0]["course_id"].ToString());
                            DateTime timealert = objmasters.timeconevert(dt_student.Rows[0]["timezone"].ToString(), dtsession.Rows[0]["session_date"].ToString(), dtsession.Rows[0]["session_start_time"].ToString());
                            DataTable sessionpatration = objmasters.getsessionpartiscption_for_alert(parameter["session_id"].ToString());
                            DateTime sendtimehouse = timealert.AddHours(-5);
                            DateTime sendtimemin = timealert.AddMinutes(-30);
                            DateTime sendtimemin15 = timealert.AddMinutes(-15);
                            for (var t = 0; t < sessionpatration.Rows.Count; t++)
                            { // 5 house
                                DS_ScheduleAppointment.alertmessageRow alert_new_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                alert_new_row.Doc_no = t + "" + 1;
                                alert_new_row.university_id = dt_student.Rows[0]["university"].ToString();
                                alert_new_row.to_member = sessionpatration.Rows[t]["member_id"].ToString();
                                alert_new_row.to_member_role = sessionpatration.Rows[t]["role_id"].ToString();
                                alert_new_row.notification_date = System.DateTime.Now;
                                alert_new_row._event = "session";
                                alert_new_row.template = " Heads up!! your  " + dt_student.Rows[0]["course_name"] + " session is only 5 hours away... hope you are prepared!";
                                alert_new_row.placeholder = parameter["session_id"].ToString();
                                alert_new_row.is_read = "N";
                                alert_new_row.is_active = "Y";
                                alert_new_row.senddate = Convert.ToDateTime(sendtimehouse.ToShortDateString());
                                alert_new_row.utc_time = TimeSpan.Parse(sendtimehouse.ToString("HH:mm:ss"));
                                alert_new_row.created_by = "admin";
                                alert_new_row.created_date = System.DateTime.Now;
                                alert_new_row.created_host = HttpContext.Current.Request.UserHostName;

                                objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_new_row);
                                // 30 min
                                DS_ScheduleAppointment.alertmessageRow alert_new_row3 = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                alert_new_row3.Doc_no = t + "" + 1 + "" + 1;
                                alert_new_row3.university_id = dt_student.Rows[0]["university"].ToString();
                                alert_new_row3.to_member = sessionpatration.Rows[t]["member_id"].ToString();
                                alert_new_row3.to_member_role = sessionpatration.Rows[t]["role_id"].ToString();
                                alert_new_row3.notification_date = System.DateTime.Now;
                                alert_new_row3._event = "session";
                                alert_new_row3.placeholder = parameter["session_id"].ToString();
                                alert_new_row3.template = "Getting close....  " + dt_student.Rows[0]["course_name"] + " session is right around the corner. you have 30 mins to get your stuff together";
                                alert_new_row3.is_read = "N";
                                alert_new_row3.is_active = "Y";
                                alert_new_row3.senddate = Convert.ToDateTime(sendtimemin.ToShortDateString());
                                alert_new_row3.utc_time = TimeSpan.Parse(sendtimemin.ToString("HH:mm:ss"));
                                alert_new_row3.created_by = "admin";
                                alert_new_row3.created_date = System.DateTime.Now;
                                alert_new_row3.created_host = HttpContext.Current.Request.UserHostName;

                                objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_new_row3);
                                if (sessionpatration.Rows[t]["role_id"].ToString() == "ST")
                                {
                                    DS_ScheduleAppointment.alertmessageRow alert_new_row4 = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                    alert_new_row4.Doc_no = t + "" + 1 + "" + 2;
                                    alert_new_row4.university_id = dt_student.Rows[0]["university"].ToString();
                                    alert_new_row4.to_member = sessionpatration.Rows[t]["member_id"].ToString();
                                    alert_new_row4.to_member_role = sessionpatration.Rows[t]["role_id"].ToString();
                                    alert_new_row4.notification_date = System.DateTime.Now;
                                    alert_new_row4._event = "session";
                                    alert_new_row4.placeholder = parameter["session_id"].ToString();
                                    alert_new_row4.template = "Your Guru is on the way";
                                    alert_new_row4.is_read = "N";
                                    alert_new_row4.is_active = "Y";
                                    alert_new_row4.senddate = Convert.ToDateTime(sendtimemin15.ToShortDateString());
                                    alert_new_row4.utc_time = TimeSpan.Parse(sendtimemin15.ToString("HH:mm:ss"));
                                    alert_new_row4.created_by = "admin";
                                    alert_new_row4.created_date = System.DateTime.Now;
                                    alert_new_row4.created_host = HttpContext.Current.Request.UserHostName;

                                    objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_new_row4);
                                }
                            }

                            //tutor send 
                            //DS_ScheduleAppointment.alertmessageRow alert_new_row2 = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                            //alert_new_row2.Doc_no = "2";
                            //alert_new_row2.university_id = dt_student.Rows[0]["university"].ToString();
                            //alert_new_row2.to_member = parameter["tutor"]["tutor_id"].ToString();
                            //alert_new_row2.to_member_role = "TT";
                            //alert_new_row2.notification_date = System.DateTime.Now;
                            //alert_new_row2._event = "session";
                            //alert_new_row2.template = " Heads up!! your " + dt_student.Rows[0]["course_name"] + " session is only 6 hours away... hope you are prepared!";
                            //alert_new_row2.is_read = "N";
                            //alert_new_row2.is_active = "Y";
                            //alert_new_row2.senddate = Convert.ToDateTime(sendtimehouse.ToShortDateString());
                            //alert_new_row2.utc_time = TimeSpan.Parse(sendtimehouse.ToString("HH:mm:ss"));
                            //alert_new_row2.created_by = "admin";
                            //alert_new_row2.created_date = System.DateTime.Now;
                            //alert_new_row2.created_host = HttpContext.Current.Request.UserHostName;

                            //objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_new_row2);
                            //// tutor 30 min entry
                            //DS_ScheduleAppointment.alertmessageRow alert_new_row4 = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                            //alert_new_row4.Doc_no = "2";
                            //alert_new_row4.university_id = dt_student.Rows[0]["university"].ToString();
                            //alert_new_row4.to_member = parameter["tutor"]["tutor_id"].ToString();
                            //alert_new_row4.to_member_role = "TT";
                            //alert_new_row4.notification_date = System.DateTime.Now;
                            //alert_new_row4._event = "session";
                            //alert_new_row4.template = "Getting close.... " + dt_student.Rows[0]["course_name"] + " session is right around the corner. you have 30 mins to get your stuff together";
                            //alert_new_row4.is_read = "N";
                            //alert_new_row4.is_active = "Y";
                            //alert_new_row4.senddate = Convert.ToDateTime(sendtimemin.ToShortDateString());
                            //alert_new_row4.utc_time = TimeSpan.Parse(sendtimemin.ToString("HH:mm:ss"));
                            //alert_new_row4.created_by = "admin";
                            //alert_new_row4.created_date = System.DateTime.Now;
                            //alert_new_row4.created_host = HttpContext.Current.Request.UserHostName;

                            //objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_new_row4);

                            #endregion

                        }


                        #endregion


                        //objBLReturnObject = objmasters.updateSessionResponse(objDS_ScheduleAppointment);

                        objBLReturnObject = objmasters.updateSessionResponsewithtrans(objDS_ScheduleAppointment, objDs_trastration);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {

                            // return "Pass";


                            DS_ScheduleAppointment.notificationRow notifyRow = objDS_ScheduleAppointment.notification.NewnotificationRow();

                            notifyRow.doc_no = "1";
                            notifyRow.placeholder = dtsession.Rows[0]["session_id"].ToString();
                            notifyRow.university_id = dtsession.Rows[0]["university_id"].ToString();
                            notifyRow.from_member = parameter["member_id"].ToString();
                            notifyRow.from_member_role = parameter["member_role"].ToString();
                            notifyRow.to_member = dtsession.Rows[0]["booked_by"].ToString();
                            notifyRow.to_member_role = "ST";

                            notifyRow.notification_date = System.DateTime.Now;
                            if (parameter["appointment_status"].ToString() == "R")
                            {

                                notifyRow.template = "Sorry your session request was rejected by your tutor. Please select another guru and try again";


                            }
                            else
                            {
                                notifyRow.template = "Your session has been confirmed. Go to session management for more details";

                            }
                            notifyRow._event = parameter["event"].ToString();
                            notifyRow.is_read = "N";
                            notifyRow.is_accept = "N";
                            notifyRow.is_active = "Y";
                            notifyRow.created_by = parameter["member_id"].ToString();
                            notifyRow.created_date = System.DateTime.Now;
                            notifyRow.created_host = HttpContext.Current.Request.UserHostName;

                            objDS_ScheduleAppointment.notification.AddnotificationRow(notifyRow);
        #endregion
                            // push notification
                            #region push

                            DS_ScheduleAppointment.alertmessageRow alert_new_row4 = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                            alert_new_row4.Doc_no = "1";
                            alert_new_row4.university_id = dtsession.Rows[0]["university_id"].ToString();
                            alert_new_row4.to_member = dtsession.Rows[0]["booked_by"].ToString();
                            alert_new_row4.to_member_role = "TT";
                            alert_new_row4.notification_date = System.DateTime.Now;
                            alert_new_row4._event = "Message";

                            if (parameter["appointment_status"].ToString() == "R")
                            {
                                alert_new_row4.template = "Sorry your session request was rejected by your tutor. Please select another guru and try again";
                                //notifyRow.template = "Sorry your session request was rejected by your tutor. Please select another guru and try again";


                            }
                            else
                            {
                                alert_new_row4.template = "Your session has been confirmed. Go to session management for more details";
                                //notifyRow.template = "Your session has been confirmed. Go to session management for more details";

                            }

                            // alert_new_row4.template = "Your Guru is on the way";
                            alert_new_row4.is_read = "N";
                            alert_new_row4.is_active = "Y";
                            alert_new_row4.senddate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
                            alert_new_row4.utc_time = TimeSpan.Parse(System.DateTime.Now.ToString("HH:mm:ss"));
                            alert_new_row4.created_by = "system";
                            alert_new_row4.created_date = System.DateTime.Now;
                            alert_new_row4.created_host = HttpContext.Current.Request.UserHostName;

                            objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_new_row4);


                            #endregion



                        }
                        objBLReturnObject = objmasters.updateSessionResponse(objDS_ScheduleAppointment);
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
            catch (Exception ex)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);
                return "Fail";

            }


        }

        [HttpPost]
        public string SaveSession_rating_for_student([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;

            DataTable rate_feedback = JsonConvert.DeserializeObject<DataTable>(parameter["rate_feedback"].ToString());

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["tutor_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    for (int i = 0; i < rate_feedback.Rows.Count; i++)
                    {
                        DS_ScheduleAppointment.session_survey_for_studentsRow StudentRow = objDS_ScheduleAppointment.session_survey_for_students.Newsession_survey_for_studentsRow();
                        StudentRow.university_id = parameter["university_id"].ToString();
                        StudentRow.session_id = parameter["session_id"].ToString();
                        StudentRow.tutor_id = parameter["tutor_id"].ToString();
                        StudentRow.student_id = rate_feedback.Rows[i]["email"].ToString();
                        StudentRow.rating_parameter_id = "S1";
                        StudentRow.rating_parameter_value = Convert.ToDecimal(rate_feedback.Rows[i]["rate"].ToString());
                        StudentRow.rating_summary = "";
                        StudentRow.Feedback = rate_feedback.Rows[i]["feedback"].ToString();
                        StudentRow.is_active = "Y";
                        StudentRow.created_by = parameter["tutor_id"].ToString();
                        StudentRow.created_date = System.DateTime.Now;
                        StudentRow.created_host = HttpContext.Current.Request.UserHostName;

                        objDS_ScheduleAppointment.session_survey_for_students.Addsession_survey_for_studentsRow(StudentRow);
                    }
                    //update  session_participants
                    #region session_participants
                    DataTable dtsession = objmasters.getsessionpartiscptiondetail(parameter["session_id"].ToString(), parameter["role"].ToString());
                    if (dtsession != null && dtsession.Rows.Count > 0)
                    {
                        // DS_ScheduleAppointment.session_timingRow seesionttimeRow = objDS_ScheduleAppointment.session_timing.Newsession_timingRow();
                        for (var i = 0; i < dtsession.Rows.Count; i++)
                        {
                            DataRow dr_session_detail = dtsession.Rows[i];
                            // dtsession.Rows[i].ImportRow(dr_session_detail);

                            dr_session_detail["is_survey_done_by_tutor"] = 'Y';

                            dr_session_detail["is_survey_done_by_student"] = 'Y';

                            objDS_ScheduleAppointment.session_participants.ImportRow(dr_session_detail);
                        }


                    }
                    DataTable dt_session_timing = objmasters.getsession_use_rating(parameter["session_id"].ToString());
                    dt_session_timing.Rows[0]["survey_done_by_tutor"] = 'Y';

                    objDS_ScheduleAppointment.session_timing.ImportRow(dt_session_timing.Rows[0]);
                    #endregion
                    objBLReturnObject = objmasters.SaveSession__survey_student(objDS_ScheduleAppointment);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {

                        // return "Pass";
                        for (int j = 0; j < rate_feedback.Rows.Count; j++)
                        {
                            bool updateFlag = false;
                            DataTable dtAvgRate = objmasters.calculateRatingSummary(rate_feedback.Rows[j]["email"].ToString(), parameter["university_id"].ToString());
                            if (dtAvgRate != null)
                            {

                                DataTable dtAvgRateStuExists = objmasters.getAvgRateSummaryForStudent(dtAvgRate.Rows[0]["student_id"].ToString(), parameter["university_id"].ToString());
                                //fresh / new entry
                                if (dtAvgRateStuExists == null || dtAvgRateStuExists.Rows.Count == 0)
                                {
                                    updateFlag = false;
                                    DS_ScheduleAppointment.average_student_ratingRow StudentRateRow = objDS_ScheduleAppointment.average_student_rating.Newaverage_student_ratingRow();
                                    //StudentRateRow.sr_no = j.ToString();
                                    StudentRateRow.university_id = parameter["university_id"].ToString();
                                    StudentRateRow.student_id = dtAvgRate.Rows[0]["student_id"].ToString(); //ultimatly it is rate_feedback student id.
                                    if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) <= 10)
                                        StudentRateRow.parameter_id = "S1";
                                    if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) >= 10 && Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) <= 20)
                                        StudentRateRow.parameter_id = "S2";
                                    if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) >= 20 && Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) <= 30)
                                        StudentRateRow.parameter_id = "S3";
                                    if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) >= 30)
                                        StudentRateRow.parameter_id = "S4";
                                    StudentRateRow.total_no_session = dtAvgRate.Rows[0]["TotCount"].ToString();
                                    StudentRateRow.avg_value = Convert.ToDecimal(dtAvgRate.Rows[0]["avgRate"]);
                                    StudentRateRow.latest_session_id = dtAvgRate.Rows[0]["latest_session"].ToString();
                                    StudentRateRow.is_active = "Y";
                                    StudentRateRow.created_by = parameter["tutor_id"].ToString();
                                    StudentRateRow.created_date = System.DateTime.Now;
                                    StudentRateRow.created_host = HttpContext.Current.Request.UserHostName;

                                    objDS_ScheduleAppointment.average_student_rating.Addaverage_student_ratingRow(StudentRateRow);

                                    #region alert message
                                    //if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) == 1)
                                    //{
                                    //    DS_ScheduleAppointment.alertmessageRow alert_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                    //    alert_row.Doc_no = randomNo.ToString();
                                    //    alert_row.university_id = parameter["university_id"].ToString();
                                    //    alert_row.to_member = dtAvgRate.Rows[0]["student_id"].ToString();
                                    //    alert_row.to_member_role = "ST";
                                    //    alert_row.notification_date = System.DateTime.Now;
                                    //    alert_row._event = "achievementlevel";
                                    //    alert_row.template = "Congratulations" + dtAvgRateStuExists.Rows[0]["first_name"].ToString() + "" + dtAvgRateStuExists.Rows[0]["last_name"].ToString() + "!!! you have successfully unlocked a new achievement level!";
                                    //    alert_row.is_read = "N";
                                    //    alert_row.is_active = "Y";
                                    //    alert_row.senddate = System.DateTime.Now;
                                    //    alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                                    //    alert_row.created_by = dtAvgRate.Rows[0]["student_id"].ToString();
                                    //    alert_row.created_date = System.DateTime.Now;
                                    //    alert_row.created_host = HttpContext.Current.Request.UserHostName;

                                    //    objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_row);




                                    //}


                                    #endregion
                                }
                                //update already exists entry
                                else
                                {
                                    updateFlag = true;
                                    DataRow dr = dtAvgRateStuExists.Rows[0];
                                    dtAvgRateStuExists.ImportRow(dr);
                                    if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) <= 10)
                                        dr["parameter_id"] = "S1";
                                    if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) >= 10 && Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) <= 20)
                                        dr["parameter_id"] = "S2";
                                    if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) >= 20 && Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) <= 30)
                                        dr["parameter_id"] = "S3";
                                    if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) >= 30)
                                        dr["parameter_id"] = "S4";
                                    dr["total_no_session"] = dtAvgRate.Rows[0]["TotCount"].ToString();
                                    dr["avg_value"] = Convert.ToDecimal(dtAvgRate.Rows[0]["avgRate"]);
                                    dr["latest_session_id"] = dtAvgRate.Rows[0]["latest_session"].ToString();

                                    dr["last_modified_by"] = dtAvgRate.Rows[0]["student_id"].ToString(); //ultimatly it is rate_feedback student id.
                                    dr["last_modified_date"] = System.DateTime.Now;
                                    dr["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                                    objDS_ScheduleAppointment.average_student_rating.ImportRow(dr);


                                    #region alert
                                    if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) == 11)
                                    {
                                        DS_ScheduleAppointment.alertmessageRow alert_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                        alert_row.Doc_no = randomNo.ToString();
                                        alert_row.university_id = parameter["university_id"].ToString();
                                        alert_row.to_member = dtAvgRate.Rows[0]["student_id"].ToString();
                                        alert_row.to_member_role = "ST";
                                        alert_row.notification_date = System.DateTime.Now;
                                        alert_row._event = "achievementlevel";
                                        alert_row.template = "Congratulations " + " " + dtAvgRateStuExists.Rows[0]["first_name"].ToString() + " " + dtAvgRateStuExists.Rows[0]["last_name"].ToString() + "!!! you have successfully unlocked a new achievement level!";
                                        alert_row.is_read = "N";
                                        alert_row.is_active = "Y";
                                        alert_row.senddate = System.DateTime.Now;
                                        alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                                        alert_row.created_by = dtAvgRate.Rows[0]["student_id"].ToString();
                                        alert_row.created_date = System.DateTime.Now;
                                        alert_row.created_host = HttpContext.Current.Request.UserHostName;

                                        objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_row);
                                    }
                                    else if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) == 21)
                                    {
                                        DS_ScheduleAppointment.alertmessageRow alert_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                        alert_row.Doc_no = randomNo.ToString();
                                        alert_row.university_id = parameter["university_id"].ToString();
                                        alert_row.to_member = dtAvgRate.Rows[0]["student_id"].ToString();
                                        alert_row.to_member_role = "ST";
                                        alert_row.notification_date = System.DateTime.Now;
                                        alert_row._event = "achievementlevel";
                                        alert_row.template = "Congratulations" + dtAvgRateStuExists.Rows[0]["first_name"].ToString() + "" + dtAvgRateStuExists.Rows[0]["last_name"].ToString() + "!!! you have successfully unlocked a new achievement level!";
                                        alert_row.is_read = "N";
                                        alert_row.is_active = "Y";
                                        alert_row.senddate = System.DateTime.Now;
                                        alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                                        alert_row.created_by = dtAvgRate.Rows[0]["student_id"].ToString();
                                        alert_row.created_date = System.DateTime.Now;
                                        alert_row.created_host = HttpContext.Current.Request.UserHostName;

                                        objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_row);
                                    }
                                    else if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) == 31)
                                    {
                                        DS_ScheduleAppointment.alertmessageRow alert_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                        alert_row.Doc_no = randomNo.ToString();
                                        alert_row.university_id = parameter["university_id"].ToString();
                                        alert_row.to_member = dtAvgRate.Rows[0]["student_id"].ToString();
                                        alert_row.to_member_role = "ST";
                                        alert_row.notification_date = System.DateTime.Now;
                                        alert_row._event = "achievementlevel";
                                        alert_row.template = "Congratulations  " + dtAvgRateStuExists.Rows[0]["first_name"].ToString() + "" + dtAvgRateStuExists.Rows[0]["last_name"].ToString() + "!!! you have successfully unlocked a new achievement level!";
                                        alert_row.is_read = "N";
                                        alert_row.is_active = "Y";
                                        alert_row.senddate = System.DateTime.Now;
                                        alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                                        alert_row.created_by = dtAvgRate.Rows[0]["student_id"].ToString();
                                        alert_row.created_date = System.DateTime.Now;
                                        alert_row.created_host = HttpContext.Current.Request.UserHostName;

                                        objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_row);
                                    }
                                    #endregion

                                }

                                objBLReturnObject = objmasters.Save_rating_summary_for_student(objDS_ScheduleAppointment, updateFlag);
                                if (objBLReturnObject.ExecutionStatus == 1)
                                {
                                    //return "Pass";
                                    flag = true;
                                }
                                else
                                {
                                    //return "Fail";
                                    flag = false;
                                }
                            }
                            else
                            {
                                //return "Fail";
                                flag = false;
                            }
                        }

                    }
                    else
                    {
                        //return "Fail";
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                    result = "securityIssue";
                }
            }
            catch (Exception e)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + e.StackTrace);
                //return "Fail";
                flag = false;
            }

            if (flag == true)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                return "Pass";
            }
            else
            {
                if (result == "securityIssue")
                {
                    return result;
                }
                else
                {
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                    return "Fail";
                }
            }

        }

        [HttpPost]
        public string SaveSession_rating_for_tutor([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;

            DataTable rate_feedback = JsonConvert.DeserializeObject<DataTable>(parameter["rate_feedback"].ToString());

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["student_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    for (int i = 0; i < rate_feedback.Rows.Count; i++)
                    {
                        #region session_survey_for_tutor
                        DS_ScheduleAppointment.session_survey_for_tutorsRow tutorRow = objDS_ScheduleAppointment.session_survey_for_tutors.Newsession_survey_for_tutorsRow();
                        tutorRow.university_id = parameter["university_id"].ToString();
                        tutorRow.session_id = parameter["session_id"].ToString();
                        tutorRow.tutor_id = rate_feedback.Rows[i]["email"].ToString();
                        tutorRow.student_id = parameter["student_id"].ToString();
                        tutorRow.rating_parameter_id = "T1";
                        tutorRow.rating_parameter_value = Convert.ToDecimal(rate_feedback.Rows[i]["rate"].ToString());
                        tutorRow.rating_summary = "";
                        tutorRow.Feedback = rate_feedback.Rows[i]["feedback"].ToString();
                        tutorRow.is_active = "Y";
                        tutorRow.created_by = parameter["student_id"].ToString();
                        tutorRow.created_date = System.DateTime.Now;
                        tutorRow.created_host = HttpContext.Current.Request.UserHostName;

                        objDS_ScheduleAppointment.session_survey_for_tutors.Addsession_survey_for_tutorsRow(tutorRow);
                        #endregion
                    }
                    //update  session_participants
                    #region session_participants
                    DataTable dtsession = objmasters.getsessionpartiscptiondetail(parameter["session_id"].ToString(), parameter["role"].ToString());
                    if (dtsession != null && dtsession.Rows.Count > 0)
                    {
                        // DS_ScheduleAppointment.session_timingRow seesionttimeRow = objDS_ScheduleAppointment.session_timing.Newsession_timingRow();

                        DataRow dr_session_detail = dtsession.Rows[0];
                        dtsession.ImportRow(dr_session_detail);

                        dr_session_detail["is_survey_done_by_tutor"] = 'Y';

                        dr_session_detail["is_survey_done_by_student"] = 'Y';



                        objDS_ScheduleAppointment.session_participants.ImportRow(dr_session_detail);
                    }
                    DataTable dt_session_timing = objmasters.getsession_use_rating(parameter["session_id"].ToString());
                    dt_session_timing.Rows[0]["survey_done_by_student"] = 'Y';


                    objDS_ScheduleAppointment.session_timing.ImportRow(dt_session_timing.Rows[0]);
                    #endregion
                    objBLReturnObject = objmasters.SaveSession__survey_tutor(objDS_ScheduleAppointment);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        //return "Pass";
                        #region calculate rating summary for Tutor

                        for (int j = 0; j < rate_feedback.Rows.Count; j++)
                        {
                            bool updateFlag = false;
                            DataTable dtAvgRate = objmasters.calculateRatingSummaryForTutor(rate_feedback.Rows[j]["email"].ToString(), parameter["university_id"].ToString());
                            if (dtAvgRate != null)
                            {

                                DataTable dtAvgRateTutorExists = objmasters.getAvgRateSummaryForTutor(dtAvgRate.Rows[j]["tutor_id"].ToString(), parameter["university_id"].ToString());
                                //fresh / new entry
                                if (dtAvgRateTutorExists == null || dtAvgRateTutorExists.Rows.Count == 0)
                                {
                                    updateFlag = false;
                                    DS_ScheduleAppointment.average_tutor_ratingRow TutorRateRow = objDS_ScheduleAppointment.average_tutor_rating.Newaverage_tutor_ratingRow();

                                    //StudentRateRow.sr_no = j.ToString();
                                    TutorRateRow.university_id = parameter["university_id"].ToString();
                                    TutorRateRow.tutor_id = dtAvgRate.Rows[j]["tutor_id"].ToString(); //ultimatly it is rate_feedback student id.
                                    if (Convert.ToInt32(dtAvgRate.Rows[j]["TotCount"]) <= 10)
                                        TutorRateRow.parameter_id = "T1";
                                    if (Convert.ToInt32(dtAvgRate.Rows[j]["TotCount"]) >= 10 && Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) <= 20)
                                        TutorRateRow.parameter_id = "T2";
                                    if (Convert.ToInt32(dtAvgRate.Rows[j]["TotCount"]) >= 20 && Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) <= 30)
                                        TutorRateRow.parameter_id = "T3";
                                    if (Convert.ToInt32(dtAvgRate.Rows[j]["TotCount"]) >= 30 && Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) <= 40)
                                        TutorRateRow.parameter_id = "T4";
                                    if (Convert.ToInt32(dtAvgRate.Rows[j]["TotCount"]) >= 40)
                                        TutorRateRow.parameter_id = "T5";
                                    TutorRateRow.total_no_session = dtAvgRate.Rows[j]["TotCount"].ToString();
                                    TutorRateRow.avg_value = Convert.ToDecimal(dtAvgRate.Rows[j]["avgRate"]);
                                    TutorRateRow.latest_session_id = dtAvgRate.Rows[j]["latest_session"].ToString();
                                    TutorRateRow.is_active = "Y";
                                    TutorRateRow.created_by = parameter["student_id"].ToString();
                                    TutorRateRow.created_date = System.DateTime.Now;
                                    TutorRateRow.created_host = HttpContext.Current.Request.UserHostName;

                                    objDS_ScheduleAppointment.average_tutor_rating.Addaverage_tutor_ratingRow(TutorRateRow);

                                    #region alert message
                                    //if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) == 1)
                                    //{
                                    //    DS_ScheduleAppointment.alertmessageRow alert_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                    //    alert_row.Doc_no = randomNo.ToString();
                                    //    alert_row.university_id = parameter["university_id"].ToString();
                                    //    alert_row.to_member = dtAvgRate.Rows[0]["student_id"].ToString();
                                    //    alert_row.to_member_role = "ST";
                                    //    alert_row.notification_date = System.DateTime.Now;
                                    //    alert_row._event = "achievementlevel";
                                    //    alert_row.template = "Congratulations" + dtAvgRateTutorExists.Rows[0]["first_name"].ToString() + "" + dtAvgRateTutorExists.Rows[0]["last_name"].ToString() + "!!! you have successfully unlocked a new achievement level!";
                                    //    alert_row.is_read = "N";
                                    //    alert_row.is_active = "Y";
                                    //    alert_row.senddate = System.DateTime.Now;
                                    //    alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                                    //    alert_row.created_by = dtAvgRate.Rows[0]["student_id"].ToString();
                                    //    alert_row.created_date = System.DateTime.Now;
                                    //    alert_row.created_host = HttpContext.Current.Request.UserHostName;

                                    //    objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_row);




                                    //}


                                    #endregion
                                }
                                //update already exists entry
                                else
                                {
                                    updateFlag = true;
                                    DataRow dr = dtAvgRateTutorExists.Rows[0];
                                    dtAvgRateTutorExists.ImportRow(dr);
                                    if (Convert.ToInt32(dtAvgRate.Rows[j]["TotCount"]) <= 10)
                                        dr["parameter_id"] = "T1";
                                    if (Convert.ToInt32(dtAvgRate.Rows[j]["TotCount"]) >= 10 && Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) <= 20)
                                        dr["parameter_id"] = "T2";
                                    if (Convert.ToInt32(dtAvgRate.Rows[j]["TotCount"]) >= 20 && Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) <= 30)
                                        dr["parameter_id"] = "T3";
                                    if (Convert.ToInt32(dtAvgRate.Rows[j]["TotCount"]) >= 30 && Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) <= 40)
                                        dr["parameter_id"] = "T4";
                                    if (Convert.ToInt32(dtAvgRate.Rows[j]["TotCount"]) >= 40)
                                        dr["parameter_id"] = "T5";

                                    dr["total_no_session"] = dtAvgRate.Rows[j]["TotCount"].ToString();
                                    dr["avg_value"] = Convert.ToDecimal(dtAvgRate.Rows[j]["avgRate"]);
                                    dr["latest_session_id"] = dtAvgRate.Rows[j]["latest_session"].ToString();

                                    dr["last_modified_by"] = parameter["student_id"].ToString(); //ultimatly it is rate_feedback student id.
                                    dr["last_modified_date"] = System.DateTime.Now;
                                    dr["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                                    objDS_ScheduleAppointment.average_tutor_rating.ImportRow(dr);


                                    #region alert
                                    if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) == 11)
                                    {
                                        DS_ScheduleAppointment.alertmessageRow alert_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                        alert_row.Doc_no = randomNo.ToString();
                                        alert_row.university_id = parameter["university_id"].ToString();
                                        alert_row.to_member = dtAvgRate.Rows[0]["tutor_id"].ToString();
                                        alert_row.to_member_role = "TT";
                                        alert_row.notification_date = System.DateTime.Now;
                                        alert_row._event = "achievementlevel";
                                        alert_row.template = "Congratulations  " + dtAvgRateTutorExists.Rows[0]["first_name"].ToString() + "!!! you have successfully unlocked a new achievement level!";
                                        alert_row.is_read = "N";
                                        alert_row.is_active = "Y";
                                        alert_row.senddate = System.DateTime.Now;
                                        alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                                        alert_row.created_by = dtAvgRate.Rows[0]["tutor_id"].ToString();
                                        alert_row.created_date = System.DateTime.Now;
                                        alert_row.created_host = HttpContext.Current.Request.UserHostName;

                                        objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_row);
                                    }
                                    else if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) == 21)
                                    {
                                        DS_ScheduleAppointment.alertmessageRow alert_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                        alert_row.Doc_no = randomNo.ToString();
                                        alert_row.university_id = parameter["university_id"].ToString();
                                        alert_row.to_member = dtAvgRate.Rows[0]["tutor_id"].ToString();
                                        alert_row.to_member_role = "TT";
                                        alert_row.notification_date = System.DateTime.Now;
                                        alert_row._event = "achievementlevel";
                                        alert_row.template = "Congratulations  " + dtAvgRateTutorExists.Rows[0]["first_name"].ToString() + "!!! you have successfully unlocked a new achievement level!";
                                        alert_row.is_read = "N";
                                        alert_row.is_active = "Y";
                                        alert_row.senddate = System.DateTime.Now;
                                        alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                                        alert_row.created_by = dtAvgRate.Rows[0]["tutor_id"].ToString();
                                        alert_row.created_date = System.DateTime.Now;
                                        alert_row.created_host = HttpContext.Current.Request.UserHostName;

                                        objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_row);
                                    }
                                    else if (Convert.ToInt32(dtAvgRate.Rows[0]["TotCount"]) == 31)
                                    {
                                        DS_ScheduleAppointment.alertmessageRow alert_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                                        alert_row.Doc_no = randomNo.ToString();
                                        alert_row.university_id = parameter["university_id"].ToString();
                                        alert_row.to_member = dtAvgRate.Rows[0]["tutor_id"].ToString();
                                        alert_row.to_member_role = "TT";
                                        alert_row.notification_date = System.DateTime.Now;
                                        alert_row._event = "achievementlevel";
                                        alert_row.template = "Congratulations  " + dtAvgRateTutorExists.Rows[0]["first_name"].ToString() + "!!! you have successfully unlocked a new achievement level!";
                                        alert_row.is_read = "N";
                                        alert_row.is_active = "Y";
                                        alert_row.senddate = System.DateTime.Now;
                                        alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                                        alert_row.created_by = dtAvgRate.Rows[0]["tutor_id"].ToString();
                                        alert_row.created_date = System.DateTime.Now;
                                        alert_row.created_host = HttpContext.Current.Request.UserHostName;

                                        objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_row);
                                    }
                                    #endregion
                                }

                                objBLReturnObject = objmasters.Save_rating_summary_for_tutor(objDS_ScheduleAppointment, updateFlag);
                                if (objBLReturnObject.ExecutionStatus == 1)
                                {//21-6-2016
                                    //return "Pass";
                                    flag = true;

                                    //DataTable dt = objmasters.getPerticularSessionDetail(parameter["session_id"].ToString());
                                    //DataRow dr_session_detail = dt.Rows[0];
                                    //dt.ImportRow(dr_session_detail);
                                    //dr_session_detail["appointment_confirmed"] = "C";
                                    //objDS_ScheduleAppointment.session_timing.ImportRow(dr_session_detail);

                                    //objBLReturnObject = objmasters.updateSessionResponse_test(objDS_ScheduleAppointment);
                                    //if (objBLReturnObject.ExecutionStatus == 1)
                                    //{
                                    //    flag = true;

                                    //}
                                    //else
                                    //{
                                    //    flag = false;

                                    //}
                                }
                                else
                                {
                                    //return "Fail";
                                    flag = false;
                                }
                            }
                            else
                            {
                                //return "Fail";
                                flag = false;
                            }
                        }

                        #endregion

                    }
                    else
                    {
                        flag = false;
                        //return "Fail";
                    }
                }
                else
                {
                    flag = false;
                    result = "securityIssue";
                    //return "securityIssue";
                }

            }
            catch (Exception e)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + e.StackTrace);
                flag = false;
                //return "Fail";
            }


            if (flag == true)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                return "Pass";
            }
            else
            {
                if (result == "securityIssue")
                {
                    return result;
                }
                else
                {
                    ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                    return "Fail";
                }
            }

        }


        [HttpPost]
        public string Setsession_start_stop([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();
            string result = "";
            bool flag = false;

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    DataTable dtsession = objmasters.getPerticularSessionDetail(parameter["session_id"].ToString());
                    if (dtsession.Rows[0]["request_status"].ToString() == "start" && parameter["start_stop"].ToString() == "start")
                    {
                        return "start";
                    }
                    if (dtsession.Rows[0]["request_status"].ToString() == "Stop" && parameter["start_stop"].ToString() == "Stop")
                    {
                        return "stop";
                    }
                    if (dtsession != null && dtsession.Rows.Count > 0)
                    {
                        #region session_timing
                        DS_ScheduleAppointment.session_timingRow seesionttimeRow = objDS_ScheduleAppointment.session_timing.Newsession_timingRow();

                        DataRow dr_session_detail = dtsession.Rows[0];
                        dtsession.ImportRow(dr_session_detail);
                        if ((parameter["member_role"].ToString()) == "TT" && (parameter["start_stop"].ToString() == "start"))
                        {
                            dr_session_detail["start_confirm_tutor_datetime"] = System.DateTime.UtcNow;
                            dr_session_detail["session_start_confirm_by_tutor_id"] = parameter["member_id"].ToString();
                            dr_session_detail["session_start_confirm_by_host_tutor"] = HttpContext.Current.Request.UserHostName;
                            dr_session_detail["last_modified_by"] = parameter["member_id"].ToString();
                            dr_session_detail["last_modified_date"] = System.DateTime.Now;
                            dr_session_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
                            //dr_session_detail["start_confirm_student_datetime"] = System.DateTime.Now;
                            dr_session_detail["session_start_confirm_by_student_id"] = dtsession.Rows[0]["booked_by"].ToString();
                            dr_session_detail["session_start_confirm_by_student_host"] = HttpContext.Current.Request.UserHostName;
                            dr_session_detail["start_confirm_student_datetime"] = DBNull.Value;
                            dr_session_detail["end_confirm_student_datetime"] = DBNull.Value;
                            dr_session_detail["end_confirm_tutor_datetime"] = DBNull.Value;
                            dr_session_detail["start_stop"] = DBNull.Value;
                            dr_session_detail["request_status"] = "start";
                        }
                        //if ((parameter["member_role"].ToString()) == "ST" && (parameter["event"].ToString() == "SessionStartReq"))
                        //{
                        //    dr_session_detail["start_stop"] = "started";
                        //}
                        //check st MEMBER_ROLE==st,  parameter["event"].ToString()==SessionStartReq start_stop=Y

                        else if ((parameter["member_role"].ToString()) == "TT" && (parameter["start_stop"].ToString() == "Stop"))
                        {

                            dr_session_detail["end_confirm_tutor_datetime"] = System.DateTime.UtcNow;
                            dr_session_detail["session_end_confirm_by_tutor_id"] = parameter["member_id"].ToString();
                            dr_session_detail["session_end_confirm_by_host_tutor"] = HttpContext.Current.Request.UserHostName;
                            dr_session_detail["last_modified_by"] = parameter["member_id"].ToString();
                            dr_session_detail["last_modified_date"] = System.DateTime.Now;
                            dr_session_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
                            // dr_session_detail["end_confirm_student_datetime"] = System.DateTime.Now;
                            //dr_session_detail["end_confirm_tutor_datetime"] = System.DateTime.Now;
                            //dr_session_detail["session_end_confirm_by_student_id"] = dtsession.Rows[0]["booked_by"].ToString();
                            //dr_session_detail["session_end_confirm_by_student_host"] = HttpContext.Current.Request.UserHostName;
                            dr_session_detail["appointment_confirmed"] = "Y";
                            dr_session_detail["request_status"] = "Stop";
                        }
                        //if ((parameter["member_role"].ToString()) == "ST" && (parameter["event"].ToString() == "SessionStopReq"))
                        //{
                        //    dr_session_detail["start_stop"] = "stopped";
                        //}
                        //check st MEMBER_ROLE==st,  parameter["event"].ToString()==SessionStopReq start_stop=N
                        objDS_ScheduleAppointment.session_timing.ImportRow(dr_session_detail);

                        #endregion

                        #region notification : table
                        DS_ScheduleAppointment.notificationRow notifyRow = objDS_ScheduleAppointment.notification.NewnotificationRow();

                        notifyRow.doc_no = "1";
                        // notifyRow.placeholder = dtsession.Rows[0]["session_id"].ToString();
                        notifyRow.university_id = parameter["university_id"].ToString();
                        notifyRow.from_member = parameter["member_id"].ToString();
                        notifyRow.from_member_role = parameter["member_role"].ToString();
                        notifyRow.to_member = dtsession.Rows[0]["booked_by"].ToString();
                        notifyRow.to_member_role = "ST";

                        notifyRow.notification_date = System.DateTime.Now;
                        notifyRow.template = parameter["template"].ToString();
                        notifyRow._event = parameter["event"].ToString();
                        notifyRow.is_read = "N";
                        notifyRow.is_accept = "N";
                        notifyRow.is_active = "Y";
                        notifyRow.created_by = parameter["member_id"].ToString();
                        notifyRow.created_date = System.DateTime.Now;
                        notifyRow.created_host = HttpContext.Current.Request.UserHostName;

                        objDS_ScheduleAppointment.notification.AddnotificationRow(notifyRow);

                    }
                        #endregion
                    #region alert table

                    DS_ScheduleAppointment.alertmessageRow alert_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                    alert_row.Doc_no = "1";
                    alert_row.university_id = parameter["university_id"].ToString();
                    alert_row.to_member = dtsession.Rows[0]["booked_by"].ToString();
                    alert_row.to_member_role = "ST";
                    alert_row.notification_date = System.DateTime.Now;
                    alert_row._event = "Message";
                    alert_row.template = parameter["template"].ToString();
                    alert_row.is_read = "N";
                    alert_row.is_active = "Y";
                    alert_row.senddate = System.DateTime.Now;
                    alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                    alert_row.created_by = "system";
                    alert_row.created_date = System.DateTime.Now;
                    alert_row.created_host = HttpContext.Current.Request.UserHostName;

                    objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_row);
                    #endregion

                    objBLReturnObject = objmasters.updateSessionResponse(objDS_ScheduleAppointment);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + objBLReturnObject.ServerMessage);
                        return objBLReturnObject.ServerMessage;
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

        //[HttpPost]
        //public string Setsession_start_stop([FromBody]JObject parameter)
        //{

        //    BLReturnObject objBLReturnObject = new BLReturnObject();
        //    sessiontime sessiondata = new sessiontime();
        //    Random rand = new Random();
        //    int randomNo = rand.Next(1, 1000000);
        //    member Memberid = new member();
        //    string result = "";
        //    bool flag = false;

        //    try
        //    {
        //        bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
        //        if (security_flag == true)
        //        {
        //            DataTable dtsession = objmasters.getPerticularSessionDetail(parameter["session_id"].ToString());
        //            if (dtsession != null && dtsession.Rows.Count > 0)
        //            {
        //                #region session_timing
        //                DS_ScheduleAppointment.session_timingRow seesionttimeRow = objDS_ScheduleAppointment.session_timing.Newsession_timingRow();

        //                DataRow dr_session_detail = dtsession.Rows[0];
        //                dtsession.ImportRow(dr_session_detail);
        //                if ((parameter["member_role"].ToString()) == "TT" && (parameter["start_stop"].ToString() == "start"))
        //                {
        //                    dr_session_detail["start_confirm_tutor_datetime"] = System.DateTime.Now;
        //                    dr_session_detail["session_start_confirm_by_tutor_id"] = parameter["member_id"].ToString();
        //                    dr_session_detail["session_start_confirm_by_host_tutor"] = HttpContext.Current.Request.UserHostName;
        //                    dr_session_detail["last_modified_by"] = parameter["member_id"].ToString();
        //                    dr_session_detail["last_modified_date"] = System.DateTime.Now;
        //                    dr_session_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
        //                    dr_session_detail["start_confirm_student_datetime"] = System.DateTime.Now;
        //                    dr_session_detail["session_start_confirm_by_student_id"] = dtsession.Rows[0]["booked_by"].ToString();
        //                    dr_session_detail["session_start_confirm_by_student_host"] = HttpContext.Current.Request.UserHostName;

        //                }

        //                else if ((parameter["member_role"].ToString()) == "TT" && (parameter["start_stop"].ToString() == "Stop"))
        //                {

        //                    dr_session_detail["end_confirm_tutor_datetime"] = System.DateTime.Now;
        //                    dr_session_detail["session_end_confirm_by_tutor_id"] = parameter["member_id"].ToString();
        //                    dr_session_detail["session_end_confirm_by_host_tutor"] = HttpContext.Current.Request.UserHostName;
        //                    dr_session_detail["last_modified_by"] = parameter["member_id"].ToString();
        //                    dr_session_detail["last_modified_date"] = System.DateTime.Now;
        //                    dr_session_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
        //                    dr_session_detail["end_confirm_student_datetime"] = System.DateTime.Now;
        //                    dr_session_detail["session_end_confirm_by_student_id"] = dtsession.Rows[0]["booked_by"].ToString();
        //                    dr_session_detail["session_end_confirm_by_student_host"] = HttpContext.Current.Request.UserHostName;
        //                    dr_session_detail["appointment_confirmed"] = "Y";
        //                }


        //                objDS_ScheduleAppointment.session_timing.ImportRow(dr_session_detail);

        //                #endregion

        //                #region notification : table
        //                DS_ScheduleAppointment.notificationRow notifyRow = objDS_ScheduleAppointment.notification.NewnotificationRow();

        //                notifyRow.doc_no = "1";
        //                // notifyRow.placeholder = dtsession.Rows[0]["session_id"].ToString();
        //                notifyRow.university_id = parameter["university_id"].ToString();
        //                notifyRow.from_member = parameter["member_id"].ToString();
        //                notifyRow.from_member_role = parameter["member_role"].ToString();
        //                notifyRow.to_member = dtsession.Rows[0]["booked_by"].ToString();
        //                notifyRow.to_member_role = "ST";

        //                notifyRow.notification_date = System.DateTime.Now;
        //                notifyRow.template = parameter["template"].ToString();
        //                notifyRow._event = parameter["event"].ToString();
        //                notifyRow.is_read = "N";
        //                notifyRow.is_accept = "N";
        //                notifyRow.is_active = "Y";
        //                notifyRow.created_by = parameter["member_id"].ToString();
        //                notifyRow.created_date = System.DateTime.Now;
        //                notifyRow.created_host = HttpContext.Current.Request.UserHostName;

        //                objDS_ScheduleAppointment.notification.AddnotificationRow(notifyRow);
        //            }
        //                #endregion

        //            objBLReturnObject = objmasters.updateSessionResponse(objDS_ScheduleAppointment);
        //            if (objBLReturnObject.ExecutionStatus == 1)
        //            {

        //                return objBLReturnObject.ServerMessage;
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


        //[HttpPost]
        //public string updatenotifications([FromBody]JObject parameter)
        //{

        //    BLReturnObject objBLReturnObject = new BLReturnObject();
        //    sessiontime sessiondata = new sessiontime();
        //    Random rand = new Random();
        //    int randomNo = rand.Next(1, 1000000);
        //    member Memberid = new member();
        //    string result = "";
        //    bool flag = false;

        //    try
        //    {
        //        bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
        //        if (security_flag == true)
        //        {
        //            DataTable dtNotification = objmasters.getNotificationFromDocNo(parameter["doc_no"].ToString());
        //            if (dtNotification != null && dtNotification.Rows.Count > 0)
        //            {
        //                #region notification : table : update

        //                DataRow dr = dtNotification.Rows[0];
        //                dtNotification.ImportRow(dr);

        //                dr["is_read"] = "Y";
        //                dr["is_accept"] = "Y";

        //                dr["last_modified_by"] = dtNotification.Rows[0]["to_member"].ToString();
        //                dr["last_modified_date"] = System.DateTime.Now;
        //                dr["last_modified_host"] = HttpContext.Current.Request.UserHostName;

        //                objDS_ScheduleAppointment.notification.ImportRow(dr);
        //                #endregion
        //            }


        //            objBLReturnObject = objmasters.updateNotification(objDS_ScheduleAppointment);
        //            if (objBLReturnObject.ExecutionStatus == 1)
        //            {
        //                return "Pass";
        //                //return objBLReturnObject.ServerMessage;
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
        [HttpPost]
        public string updatenotifications([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();
            string result = "";
            bool flag = false;

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    //if (parameter["session_id"] != null && parameter["session_id"].ToString() != String.Empty)
                    //{
                    //    DataTable dtsession = objmasters.getPerticularSessionDetail(parameter["session_id"].ToString());

                    //    if (dtsession.Rows[0]["start_confirm_student_datetime"] != null && dtsession.Rows[0]["start_confirm_student_datetime"].ToString().Trim().Length > 0  && parameter["role_id"].ToString() == "ST")
                    //    { return "start"; }
                    //    if (dtsession.Rows[0]["end_confirm_student_datetime"] != null && dtsession.Rows[0]["end_confirm_student_datetime"].ToString().Trim().Length > 0 && parameter["role_id"].ToString() == "ST" )
                    //    { return "stop"; }
                    //}
                    DataTable dtNotification = objmasters.getNotificationFromDocNo(parameter["doc_no"].ToString());
                    if (dtNotification != null && dtNotification.Rows.Count > 0)
                    {
                        #region notification : table : update

                        DataRow dr = dtNotification.Rows[0];
                        dtNotification.ImportRow(dr);

                        dr["is_read"] = "Y";
                        dr["is_accept"] = "Y";

                        dr["last_modified_by"] = dtNotification.Rows[0]["to_member"].ToString();
                        dr["last_modified_date"] = System.DateTime.Now;
                        dr["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                        objDS_ScheduleAppointment.notification.ImportRow(dr);
                        #endregion

                        //Nirav 30/06/2016
                        if (parameter["session_id"] != null && parameter["session_id"].ToString() != String.Empty)
                        {
                            DataTable dtsession = objmasters.getPerticularSessionDetail(parameter["session_id"].ToString());

                            if (dtsession.Rows[0]["start_confirm_student_datetime"] != null && dtsession.Rows[0]["start_confirm_student_datetime"].ToString().Trim().Length > 0 && dtNotification.Rows[0]["_event"].ToString() == "SessionStartReq" && parameter["role_id"].ToString() == "ST")
                            { return "start"; }
                            if (dtsession.Rows[0]["end_confirm_student_datetime"] != null && dtsession.Rows[0]["end_confirm_student_datetime"].ToString().Trim().Length > 0 && parameter["role_id"].ToString() == "ST" && dtNotification.Rows[0]["_event"].ToString() == "SessionStopReq")
                            { return "stop"; }

                            if (dtsession != null && dtsession.Rows.Count > 0)
                            {
                                DataRow dr_session_detail = dtsession.Rows[0];
                                dtsession.ImportRow(dr_session_detail);
                                if (parameter["role_id"].ToString() == "ST" && dtNotification.Rows[0]["_event"].ToString() == "SessionStartReq")
                                {
                                    dr_session_detail["start_stop"] = "started";
                                    dr_session_detail["start_confirm_student_datetime"] = System.DateTime.UtcNow;
                                    dr_session_detail["session_start_confirm_by_student_id"] = parameter["member_id"].ToString();
                                    dr_session_detail["session_start_confirm_by_student_host"] = HttpContext.Current.Request.UserHostName;
                                }

                                if (parameter["role_id"].ToString() == "ST" && dtNotification.Rows[0]["_event"].ToString() == "SessionStopReq")
                                {
                                    dr_session_detail["start_stop"] = "stopped";
                                    dr_session_detail["end_confirm_student_datetime"] = System.DateTime.UtcNow;
                                    dr_session_detail["session_end_confirm_by_student_id"] = parameter["member_id"].ToString();
                                    dr_session_detail["session_end_confirm_by_student_host"] = HttpContext.Current.Request.UserHostName;
                                    dr_session_detail["appointment_confirmed"] = "C";



                                }
                                objDS_ScheduleAppointment.session_timing.ImportRow(dr_session_detail);

                                #region transtration of session booking transtion token balance
                                if (parameter["role_id"].ToString() == "ST" && dtNotification.Rows[0]["_event"].ToString() == "SessionStopReq")
                                {
                                    decimal excharge = 0;
                                    // int excharge = 0;
                                    int minutes = 0;
                                    //DateTime timealert = objmasters.timeconevert(dtsession.Rows[0]["timezone"].ToString(), dtsession.Rows[0]["session_date"].ToString(), dtsession.Rows[0]["session_end_time"].ToString());
                                    DataTable user_info = objmasters.time_diff_ex_hours("", Convert.ToDateTime(dtsession.Rows[0]["start_confirm_student_datetime"].ToString()));

                                    minutes = ((Convert.ToInt16(user_info.Rows[2]["diff"])) - Convert.ToInt16(dtsession.Rows[0]["duration"].ToString()));
                                    DataTable fn_seesion_booking = objmasters.getseeionbooking(parameter["session_id"].ToString());
                                    //extra hour's transtartion
                                    //5$ per 10 min
                                    DataTable dtstudent = objmasters.gettokenbal(dtsession.Rows[0]["booked_by"].ToString());
                                    if (minutes > 60)
                                    {//get hour
                                        #region student update
                                        excharge = Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString());
                                        dtstudent.Rows[0]["total_debit"] = (Convert.ToDecimal(dtstudent.Rows[0]["total_debit"].ToString()) + (Convert.ToDecimal(excharge)));
                                        dtstudent.Rows[0]["balance_token"] = (Convert.ToDecimal(dtstudent.Rows[0]["balance_token"].ToString()) - (Convert.ToDecimal(excharge)));
                                        #endregion
                                        #region student excharge


                                        DS_Transtration.fn_token_transtionRow token_transtration_row4 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                                        token_transtration_row4.doc_no = "5";
                                        token_transtration_row4.doc_date = System.DateTime.Now;
                                        token_transtration_row4.ref_transtion_type = "excharge_session";
                                        token_transtration_row4.ref_transtion_doc_date = System.DateTime.Now;
                                        token_transtration_row4.ref_transtion_doc_no = parameter["session_id"].ToString();
                                        token_transtration_row4.type_credit_debit = "debit";
                                        token_transtration_row4.amount = (Convert.ToDecimal(excharge));
                                        token_transtration_row4.balance_after_transtion = Convert.ToDecimal(dtstudent.Rows[0]["balance_token"].ToString());
                                        token_transtration_row4.member_id = dtsession.Rows[0]["booked_by"].ToString();
                                        token_transtration_row4.created_by = dtsession.Rows[0]["booked_by"].ToString();
                                        token_transtration_row4.created_date = System.DateTime.Now;
                                        token_transtration_row4.created_host = HttpContext.Current.Request.UserHostName;

                                        objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row4);

                                        #endregion

                                        #region update session booking


                                        fn_seesion_booking.Rows[0]["total_session_charge"] = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString()) + (excharge));

                                        decimal temp_mycc = Convert.ToDecimal(excharge * mycc_charge) / 100;
                                        fn_seesion_booking.Rows[0]["session_mycc_charge"] = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_mycc_charge"].ToString()) + (temp_mycc));

                                        fn_seesion_booking.Rows[0]["session_tutor_charge"] = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_tutor_charge"].ToString()) + ((Convert.ToDecimal(excharge - temp_mycc))));
                                        fn_seesion_booking.Rows[0]["session_ex_charge"] = Convert.ToDecimal(excharge);

                                        #endregion

                                    }
                                    else if (minutes > 0)
                                    { // get minutes
                                        // DataTable dtstudent = objmasters.gettokenbal(dtsession.Rows[0]["booked_by"].ToString());



                                        if (minutes >= 10 && minutes <= 20)
                                        {
                                            excharge = 5;

                                        }
                                        else if (minutes >= 21 && minutes <= 30)
                                        {
                                            excharge = 10;

                                        }
                                        else if (minutes >= 31 && minutes <= 40)
                                        {
                                            excharge = 15;


                                        }
                                        else if (minutes >= 41 && minutes <= 50)
                                        {
                                            excharge = 20;


                                        }
                                        else if (minutes >= 51 && minutes <= 60)
                                        {
                                            excharge = 25;

                                        }
                                        else
                                        {

                                        }
                                        if (excharge > 0)
                                        {
                                            dtstudent.Rows[0]["total_debit"] = (Convert.ToDecimal(dtstudent.Rows[0]["total_debit"].ToString()) + (Convert.ToDecimal(excharge)));
                                            dtstudent.Rows[0]["balance_token"] = (Convert.ToDecimal(dtstudent.Rows[0]["balance_token"].ToString()) - (Convert.ToDecimal(excharge)));

                                            objDs_trastration.fn_token_balance.ImportRow(dtstudent.Rows[0]);

                                            #region student excharge


                                            DS_Transtration.fn_token_transtionRow token_transtration_row4 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                                            token_transtration_row4.doc_no = "5";
                                            token_transtration_row4.doc_date = System.DateTime.Now;
                                            token_transtration_row4.ref_transtion_type = "excharge_session";
                                            token_transtration_row4.ref_transtion_doc_date = System.DateTime.Now;
                                            token_transtration_row4.ref_transtion_doc_no = parameter["session_id"].ToString();
                                            token_transtration_row4.type_credit_debit = "debit";
                                            token_transtration_row4.amount = (Convert.ToDecimal(excharge));
                                            token_transtration_row4.balance_after_transtion = Convert.ToDecimal(dtstudent.Rows[0]["balance_token"].ToString());
                                            token_transtration_row4.member_id = dtsession.Rows[0]["booked_by"].ToString();
                                            token_transtration_row4.created_by = dtsession.Rows[0]["booked_by"].ToString();
                                            token_transtration_row4.created_date = System.DateTime.Now;
                                            token_transtration_row4.created_host = HttpContext.Current.Request.UserHostName;

                                            objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row4);

                                            #endregion

                                            #region update   session booking
                                            fn_seesion_booking.Rows[0]["total_session_charge"] = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString()) + (excharge));

                                            decimal temp_mycc = Convert.ToDecimal(excharge * mycc_charge) / 100;
                                            fn_seesion_booking.Rows[0]["session_mycc_charge"] = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_mycc_charge"].ToString()) + (temp_mycc));

                                            fn_seesion_booking.Rows[0]["session_tutor_charge"] = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_tutor_charge"].ToString()) + ((Convert.ToDecimal(excharge - temp_mycc))));
                                            fn_seesion_booking.Rows[0]["session_ex_charge"] = Convert.ToDecimal(excharge);


                                            #endregion
                                        }

                                    }
                                    else
                                    {

                                    }
                                    #region  session booking

                                    //DataTable fn_seesion_booking = objmasters.getseeionbooking(parameter["session_id"].ToString());
                                    fn_seesion_booking.Rows[0]["is_token_transfer_tutor"] = "Y";
                                    fn_seesion_booking.Rows[0]["transfer_date_time_tutor"] = System.DateTime.Now;

                                    objDs_trastration.fn_session_booking.ImportRow(fn_seesion_booking.Rows[0]);

                                    #endregion

                                    #region token balance update
                                    DataTable dttutor = objmasters.gettokenbal(fn_seesion_booking.Rows[0]["session_tutor"].ToString());

                                    dttutor.Rows[0]["total_credit"] = (Convert.ToDecimal(dttutor.Rows[0]["total_credit"].ToString()) + (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_tutor_charge"].ToString())));
                                    dttutor.Rows[0]["balance_token"] = (Convert.ToDecimal(dttutor.Rows[0]["balance_token"].ToString()) + (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_tutor_charge"].ToString())));
                                    objDs_trastration.fn_token_balance.ImportRow(dttutor.Rows[0]);
                                    #endregion


                                    #region mycc acount balance
                                    DataTable dtcc = objmasters.gettokenbal(mycc_id);

                                    dtcc.Rows[0]["total_debit"] = (Convert.ToDecimal(dtcc.Rows[0]["total_debit"].ToString()) + (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_tutor_charge"].ToString())));
                                    dtcc.Rows[0]["total_credit"] = (Convert.ToDecimal(dtcc.Rows[0]["total_credit"].ToString()) + (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_mycc_charge"].ToString())));
                                    var bal_befor = dtcc.Rows[0]["balance_token"].ToString();
                                    dtcc.Rows[0]["balance_token"] = (Convert.ToDecimal(dtcc.Rows[0]["balance_token"].ToString()) + Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_mycc_charge"].ToString())) - (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_tutor_charge"].ToString()));
                                    objDs_trastration.fn_token_balance.ImportRow(dtcc.Rows[0]);

                                    #endregion

                                    #region token transtraion
                                    //my tutor account

                                    DS_Transtration.fn_token_transtionRow token_transtration_row = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                                    token_transtration_row.doc_no = "1";
                                    token_transtration_row.doc_date = System.DateTime.Now;
                                    token_transtration_row.ref_transtion_type = "sessiontransfer";
                                    token_transtration_row.ref_transtion_doc_date = System.DateTime.Now;
                                    token_transtration_row.ref_transtion_doc_no = parameter["session_id"].ToString();
                                    token_transtration_row.type_credit_debit = "Credit";
                                    token_transtration_row.amount = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_tutor_charge"].ToString()));
                                    token_transtration_row.balance_after_transtion = Convert.ToDecimal(dttutor.Rows[0]["balance_token"].ToString());
                                    token_transtration_row.member_id = fn_seesion_booking.Rows[0]["session_tutor"].ToString();
                                    token_transtration_row.created_by = fn_seesion_booking.Rows[0]["session_tutor"].ToString();
                                    token_transtration_row.created_date = System.DateTime.Now;
                                    token_transtration_row.created_host = HttpContext.Current.Request.UserHostName;

                                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row);

                                    // my cc account
                                    DS_Transtration.fn_token_transtionRow token_transtration_row6 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                                    token_transtration_row6.doc_no = "6";
                                    token_transtration_row6.doc_date = System.DateTime.Now;
                                    token_transtration_row6.ref_transtion_type = "excharge_session";
                                    token_transtration_row6.ref_transtion_doc_date = System.DateTime.Now;
                                    token_transtration_row6.ref_transtion_doc_no = parameter["session_id"].ToString();
                                    token_transtration_row6.type_credit_debit = "debit";
                                    token_transtration_row6.amount = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_tutor_charge"].ToString()));
                                    token_transtration_row6.balance_after_transtion = Convert.ToDecimal(Convert.ToDecimal(bal_befor) - (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_tutor_charge"].ToString())));
                                    token_transtration_row6.member_id = mycc_id;
                                    token_transtration_row6.created_by = mycc_id;
                                    token_transtration_row6.created_date = System.DateTime.Now;
                                    token_transtration_row6.created_host = HttpContext.Current.Request.UserHostName;

                                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row6);
                                    DS_Transtration.fn_token_transtionRow token_transtration_row2 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                                    token_transtration_row2.doc_no = "2";
                                    token_transtration_row2.doc_date = System.DateTime.Now;
                                    token_transtration_row2.ref_transtion_type = "sessiontransfer";
                                    token_transtration_row2.ref_transtion_doc_date = System.DateTime.Now;
                                    token_transtration_row2.ref_transtion_doc_no = parameter["session_id"].ToString();
                                    token_transtration_row2.type_credit_debit = "Credit";
                                    token_transtration_row2.amount = (Convert.ToDecimal(excharge));
                                    token_transtration_row2.balance_after_transtion = Convert.ToDecimal(Convert.ToDecimal(bal_befor) + (Convert.ToDecimal(excharge)));
                                    token_transtration_row2.member_id = mycc_id;
                                    token_transtration_row2.created_by = mycc_id;
                                    token_transtration_row2.created_date = System.DateTime.Now;
                                    token_transtration_row2.created_host = HttpContext.Current.Request.UserHostName;

                                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row2);

                                    DS_Transtration.fn_token_transtionRow token_transtration_row3 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                                    token_transtration_row3.doc_no = "3";
                                    token_transtration_row3.doc_date = System.DateTime.Now;
                                    token_transtration_row3.ref_transtion_type = "mycccharge";
                                    token_transtration_row3.ref_transtion_doc_date = System.DateTime.Now;
                                    token_transtration_row3.ref_transtion_doc_no = parameter["session_id"].ToString();
                                    token_transtration_row3.type_credit_debit = "Credit";
                                    token_transtration_row3.amount = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_mycc_charge"].ToString()));
                                    token_transtration_row3.balance_after_transtion = Convert.ToDecimal(Convert.ToDecimal(bal_befor) - ((Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_tutor_charge"].ToString())) - (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_mycc_charge"].ToString()))));
                                    token_transtration_row3.member_id = mycc_id;
                                    token_transtration_row3.created_by = mycc_id;
                                    token_transtration_row3.created_date = System.DateTime.Now;
                                    token_transtration_row3.created_host = HttpContext.Current.Request.UserHostName;

                                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row3);


                                    #endregion

                                }

                                #endregion


                                objBLReturnObject = objmasters.updateSessionResponsewithtrans(objDS_ScheduleAppointment, objDs_trastration);
                                if (objBLReturnObject.ExecutionStatus == 1)
                                {
                                    objBLReturnObject = objmasters.updateNotification(objDS_ScheduleAppointment);
                                    if (objBLReturnObject.ExecutionStatus == 1)
                                    {
                                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                                        return "Pass";
                                        //return objBLReturnObject.ServerMessage;
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
                                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                                return "Fail";
                            }
                        }
                        else
                        {
                            objBLReturnObject = objmasters.updateNotification(objDS_ScheduleAppointment);
                            if (objBLReturnObject.ExecutionStatus == 1)
                            {
                                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                                return "Pass";
                                //return objBLReturnObject.ServerMessage;
                            }
                            else
                            {
                                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                                return "Fail";
                            }
                        }
                    }
                    else
                    {
                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                        return "Fail";
                    }

                    //objBLReturnObject = objmasters.updateNotification(objDS_ScheduleAppointment);
                    //if (objBLReturnObject.ExecutionStatus == 1)
                    //{
                    //    return "Pass";
                    //    //return objBLReturnObject.ServerMessage;
                    //}
                    //else
                    //{
                    //    return "Fail";
                    //}
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
        public string SetCancle_Reschedule([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();
            string result = "";
            bool apply_charge = false;
            bool flag = false;

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    DataTable dtsession = objmasters.getPerticularSessionDetail(parameter["session_id"].ToString());


                    if (dtsession != null && dtsession.Rows.Count > 0)
                    {
                        DS_ScheduleAppointment.session_timingRow seesionttimeRow = objDS_ScheduleAppointment.session_timing.Newsession_timingRow();

                        DataRow dr_session_detail = dtsession.Rows[0];
                        dtsession.ImportRow(dr_session_detail);
                        if (parameter["status"].ToString() == "C")
                        {
                            dr_session_detail["is_active"] = 'N';

                            dr_session_detail["last_modified_by"] = parameter["member_id"].ToString();
                            dr_session_detail["last_modified_date"] = System.DateTime.Now;
                            dr_session_detail["last_modified_host"] = HttpContext.Current.Request.UserHostName;
                        }

                        objDS_ScheduleAppointment.session_timing.ImportRow(dr_session_detail);

                        #region notification : table
                        DS_ScheduleAppointment.notificationRow notifyRow = objDS_ScheduleAppointment.notification.NewnotificationRow();

                        notifyRow.doc_no = "1";
                        // notifyRow.placeholder = dtsession.Rows[0]["session_id"].ToString();
                        notifyRow.university_id = dtsession.Rows[0]["university_id"].ToString();
                        if (parameter["member_id"].ToString() == dtsession.Rows[0]["booked_by"].ToString())
                        {
                            notifyRow.from_member = dtsession.Rows[0]["booked_by"].ToString();
                            notifyRow.from_member_role = "ST";
                            notifyRow.to_member = dtsession.Rows[0]["tutor_id"].ToString();
                            notifyRow.to_member_role = "TT";
                        }
                        else
                        {
                            notifyRow.from_member = dtsession.Rows[0]["tutor_id"].ToString();
                            notifyRow.from_member_role = "TT";
                            notifyRow.to_member = dtsession.Rows[0]["booked_by"].ToString();
                            notifyRow.to_member_role = "ST";

                        }
                        notifyRow.notification_date = System.DateTime.Now;
                        notifyRow.template = "Your Session has been cancelled. Please set a new appointment with another guru";
                        notifyRow._event = "SessionScheduleCancle";
                        notifyRow.is_read = "N";
                        notifyRow.is_accept = "N";
                        notifyRow.is_active = "Y";
                        notifyRow.created_by = parameter["member_id"].ToString();
                        notifyRow.created_date = System.DateTime.Now;
                        notifyRow.created_host = HttpContext.Current.Request.UserHostName;

                        objDS_ScheduleAppointment.notification.AddnotificationRow(notifyRow);

                        #endregion
                        #region alert table

                        DS_ScheduleAppointment.alertmessageRow alert_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                        alert_row.Doc_no = "1";
                        alert_row.university_id = parameter["university_id"].ToString();
                        if (parameter["member_id"].ToString() == dtsession.Rows[0]["booked_by"].ToString())
                        {

                            alert_row.to_member = dtsession.Rows[0]["tutor_id"].ToString();
                            alert_row.to_member_role = "TT";
                        }
                        else
                        {
                            alert_row.to_member = dtsession.Rows[0]["booked_by"].ToString();
                            alert_row.to_member_role = "ST";
                        }
                        alert_row.to_member = dtsession.Rows[0]["booked_by"].ToString();
                        alert_row.to_member_role = "ST";
                        alert_row.notification_date = System.DateTime.Now;
                        alert_row._event = "Message";
                        alert_row.template = "Your Session has been cancelled. Please set a new appointment with another guru";
                        alert_row.is_read = "N";
                        alert_row.is_active = "Y";
                        alert_row.senddate = System.DateTime.Now;
                        alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                        alert_row.created_by = "system";
                        alert_row.created_date = System.DateTime.Now;
                        alert_row.created_host = HttpContext.Current.Request.UserHostName;

                        objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_row);
                        #endregion

                        //payment
                        #region session cancle student /tutor


                        #region  session booking

                        DataTable fn_seesion_booking = objmasters.getseeionbooking(parameter["session_id"].ToString());
                        DataTable session_time = objmasters.getsessiondetails(parameter["session_id"].ToString());
                        // DataTable user_detail=objmasters.gettimezone_by_user(session_time.Rows[0]["tutor_id"].ToString());

                        DateTime timealert = objmasters.timeconevert(session_time.Rows[0]["timezone"].ToString(), session_time.Rows[0]["session_date"].ToString(), session_time.Rows[0]["session_start_time"].ToString());
                        DataTable user_info = objmasters.time_diff("", timealert);
                        if (Convert.ToInt32(user_info.Rows[0]["diff"]) == 0)
                        {
                            if (Convert.ToInt32(user_info.Rows[1]["diff"]) <= 12)
                            {
                                cancell_tutor_charge = 0;

                            }
                        }
                        else
                        {



                        }
                        //cancle of tutor calculation
                        if (session_time.Rows[0]["tutor_id"].ToString() == parameter["member_id"].ToString())
                        {


                            #region token balance update tutor
                            DataTable dttutor = objmasters.gettokenbal(fn_seesion_booking.Rows[0]["session_tutor"].ToString());

                            dttutor.Rows[0]["total_debit"] = (Convert.ToDecimal(dttutor.Rows[0]["total_debit"].ToString()) + (cancell_tutor_charge));
                            dttutor.Rows[0]["balance_token"] = (Convert.ToDecimal(dttutor.Rows[0]["balance_token"].ToString()) - (cancell_tutor_charge));
                            objDs_trastration.fn_token_balance.ImportRow(dttutor.Rows[0]);
                            #endregion

                            #region mycc acount balance
                            DataTable dtcc = objmasters.gettokenbal(mycc_id);
                            decimal temp_debit = 0;
                            decimal temp_credite = 0;

                            if (fn_seesion_booking.Rows[0]["disscount_amount"].ToString() != null && fn_seesion_booking.Rows[0]["disscount_amount"].ToString() != "")
                            {
                                temp_debit = ((Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString())) - (Convert.ToDecimal(fn_seesion_booking.Rows[0]["disscount_amount"].ToString())));
                                dtcc.Rows[0]["total_debit"] = (Convert.ToDecimal(dtcc.Rows[0]["total_debit"].ToString()) + ((Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString())) - (Convert.ToDecimal(fn_seesion_booking.Rows[0]["disscount_amount"].ToString()))));
                            }
                            else
                            {
                                temp_debit = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString()));
                                dtcc.Rows[0]["total_debit"] = (Convert.ToDecimal(dtcc.Rows[0]["total_debit"].ToString()) + (Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString())));
                            }
                            temp_credite = cancell_tutor_charge;
                            dtcc.Rows[0]["total_credit"] = (Convert.ToDecimal(dtcc.Rows[0]["total_credit"].ToString()) + (cancell_tutor_charge));

                            var bal_befor = dtcc.Rows[0]["balance_token"].ToString();

                            dtcc.Rows[0]["balance_token"] = (Convert.ToDecimal(dtcc.Rows[0]["balance_token"].ToString()) + temp_credite);
                            dtcc.Rows[0]["balance_token"] = (Convert.ToDecimal(dtcc.Rows[0]["balance_token"].ToString()) - temp_debit);
                            objDs_trastration.fn_token_balance.ImportRow(dtcc.Rows[0]);

                            #endregion
                            #region student account
                            decimal student_debit = 0;
                            DataTable dtstudentacc = objmasters.gettokenbal(session_time.Rows[0]["booked_by"].ToString());
                            if (fn_seesion_booking.Rows[0]["disscount_amount"].ToString() != null)
                            {
                                student_debit = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString())) - (Convert.ToDecimal(fn_seesion_booking.Rows[0]["disscount_amount"].ToString()));

                                dtstudentacc.Rows[0]["total_credit"] = (Convert.ToDecimal(dtstudentacc.Rows[0]["total_credit"].ToString()) + ((Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString())) - (Convert.ToDecimal(fn_seesion_booking.Rows[0]["disscount_amount"].ToString()))));
                            }
                            else
                            {
                                student_debit = (Convert.ToDecimal(dtstudentacc.Rows[0]["total_credit"].ToString()) + (Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString())));
                                dtstudentacc.Rows[0]["total_credit"] = (Convert.ToDecimal(dtstudentacc.Rows[0]["total_credit"].ToString()) + (Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString())));
                            }
                            dtstudentacc.Rows[0]["balance_token"] = (Convert.ToDecimal(dtstudentacc.Rows[0]["balance_token"].ToString()) + student_debit);
                            objDs_trastration.fn_token_balance.ImportRow(dtstudentacc.Rows[0]);
                            #endregion

                            #region token transtraion
                            //my tutor account

                            DS_Transtration.fn_token_transtionRow token_transtration_row = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                            token_transtration_row.doc_no = "1";
                            token_transtration_row.doc_date = System.DateTime.Now;
                            token_transtration_row.ref_transtion_type = "sessioncancle";
                            token_transtration_row.ref_transtion_doc_date = System.DateTime.Now;
                            token_transtration_row.ref_transtion_doc_no = parameter["session_id"].ToString();
                            token_transtration_row.amount = cancell_tutor_charge;
                            token_transtration_row.balance_after_transtion = Convert.ToDecimal(dttutor.Rows[0]["balance_token"].ToString());
                            token_transtration_row.type_credit_debit = "debit";
                            token_transtration_row.member_id = fn_seesion_booking.Rows[0]["session_tutor"].ToString();
                            token_transtration_row.created_by = fn_seesion_booking.Rows[0]["session_tutor"].ToString();
                            token_transtration_row.created_date = System.DateTime.Now;
                            token_transtration_row.created_host = HttpContext.Current.Request.UserHostName;
                            fn_seesion_booking.Rows[0]["cancelled_charge"] = cancell_tutor_charge;
                            fn_seesion_booking.Rows[0]["session_mycc_charge"] = cancell_tutor_charge;

                            objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row);

                            // my cc account
                            DS_Transtration.fn_token_transtionRow token_transtration_row2 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                            token_transtration_row2.doc_no = "2";
                            token_transtration_row2.doc_date = System.DateTime.Now;
                            token_transtration_row2.ref_transtion_type = "mycccharge_cancle";
                            token_transtration_row2.ref_transtion_doc_date = System.DateTime.Now;
                            token_transtration_row2.ref_transtion_doc_no = parameter["session_id"].ToString();
                            token_transtration_row2.type_credit_debit = "Credit";

                            token_transtration_row2.amount = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["session_tutor_charge"].ToString()));
                            token_transtration_row2.balance_after_transtion = Convert.ToDecimal(Convert.ToDecimal(bal_befor) + (cancell_tutor_charge));
                            token_transtration_row2.member_id = mycc_id;
                            token_transtration_row2.created_by = mycc_id;
                            token_transtration_row2.created_date = System.DateTime.Now;
                            token_transtration_row2.created_host = HttpContext.Current.Request.UserHostName;

                            objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row2);

                            DS_Transtration.fn_token_transtionRow token_transtration_row3 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                            token_transtration_row3.doc_no = "3";
                            token_transtration_row3.doc_date = System.DateTime.Now;
                            token_transtration_row3.ref_transtion_type = "cancle_tranf_stud";
                            token_transtration_row3.ref_transtion_doc_date = System.DateTime.Now;
                            token_transtration_row3.ref_transtion_doc_no = parameter["session_id"].ToString();
                            token_transtration_row3.type_credit_debit = "debit";
                            token_transtration_row3.amount = student_debit;
                            token_transtration_row3.balance_after_transtion = ((Convert.ToDecimal(bal_befor) - student_debit) + cancell_tutor_charge);
                            token_transtration_row3.member_id = mycc_id;
                            token_transtration_row3.created_by = mycc_id;
                            token_transtration_row3.created_date = System.DateTime.Now;
                            token_transtration_row3.created_host = HttpContext.Current.Request.UserHostName;

                            objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row3);




                            DS_Transtration.fn_token_transtionRow token_transtration_row4 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                            token_transtration_row4.doc_no = "4";
                            token_transtration_row4.doc_date = System.DateTime.Now;
                            token_transtration_row4.ref_transtion_type = "mycc_tranf_cancle";
                            token_transtration_row4.ref_transtion_doc_date = System.DateTime.Now;
                            token_transtration_row4.ref_transtion_doc_no = parameter["session_id"].ToString();
                            token_transtration_row4.amount = student_debit;
                            token_transtration_row4.type_credit_debit = "Credit";
                            token_transtration_row4.balance_after_transtion = Convert.ToDecimal(dtstudentacc.Rows[0]["balance_token"].ToString());
                            token_transtration_row4.member_id = session_time.Rows[0]["booked_by"].ToString();
                            token_transtration_row4.created_by = session_time.Rows[0]["booked_by"].ToString();
                            token_transtration_row4.created_date = System.DateTime.Now;
                            token_transtration_row4.created_host = HttpContext.Current.Request.UserHostName;

                            objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row4);



                            #endregion
                            #region  session cancle

                            DS_Transtration.fn_token_cancelRow session_cancle = objDs_trastration.fn_token_cancel.Newfn_token_cancelRow();
                            session_cancle.doc_date = System.DateTime.Now;
                            session_cancle.doc_no = "1";
                            session_cancle.session_id = parameter["session_id"].ToString();
                            session_cancle.session_tutor = session_time.Rows[0]["tutor_id"].ToString();
                            session_cancle.session_cost = Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString());
                            session_cancle.session_cancelled_by = session_time.Rows[0]["tutor_id"].ToString();
                            session_cancle.session_refunded_amount = student_debit;
                            session_cancle.is_active = "Y";
                            session_cancle.created_by = parameter["member_id"].ToString();
                            session_cancle.created_host = HttpContext.Current.Request.UserHostName;
                            session_cancle.created_date = System.DateTime.Now;

                            objDs_trastration.fn_token_cancel.Addfn_token_cancelRow(session_cancle);

                            #endregion
                        }
                        //cancle of student calculation
                        if (session_time.Rows[0]["booked_by"].ToString() == parameter["member_id"].ToString())
                        {


                            #region mycc acount balance
                            DataTable dtcc = objmasters.gettokenbal(mycc_id);
                            decimal temp_debit = 0;
                            decimal temp_credite = 0;
                            decimal amount_student = 0;
                            decimal amount_of_cancle_charge = 0;
                            if (fn_seesion_booking.Rows[0]["disscount_amount"].ToString() != null && fn_seesion_booking.Rows[0]["disscount_amount"].ToString() != "")
                            {
                                amount_student = ((Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString())) - (Convert.ToDecimal(fn_seesion_booking.Rows[0]["disscount_amount"].ToString())));


                            }
                            else
                            {
                                amount_student = (Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString()));

                            }
                            // amount_of_cancle_charge = (((amount_student) * 50) / 100);
                            amount_of_cancle_charge = 10;
                            dtcc.Rows[0]["total_credit"] = (Convert.ToDecimal(dtcc.Rows[0]["total_debit"].ToString()) + amount_of_cancle_charge);
                            temp_credite = cancell_tutor_charge;
                            dtcc.Rows[0]["total_debit"] = (Convert.ToDecimal(dtcc.Rows[0]["total_credit"].ToString()) + ((amount_student - amount_of_cancle_charge)));

                            var bal_befor = dtcc.Rows[0]["balance_token"].ToString();

                            dtcc.Rows[0]["balance_token"] = (Convert.ToDecimal(dtcc.Rows[0]["balance_token"].ToString()) + amount_of_cancle_charge);
                            dtcc.Rows[0]["balance_token"] = (Convert.ToDecimal(dtcc.Rows[0]["balance_token"].ToString()) - amount_student);
                            objDs_trastration.fn_token_balance.ImportRow(dtcc.Rows[0]);

                            #endregion
                            #region student account
                            decimal student_credit = 0;
                            DataTable dtstudentacc = objmasters.gettokenbal(session_time.Rows[0]["booked_by"].ToString());
                            student_credit = (amount_student - amount_of_cancle_charge);

                            dtstudentacc.Rows[0]["total_credit"] = (Convert.ToDecimal(dtstudentacc.Rows[0]["total_credit"].ToString()) + ((amount_student) - (amount_of_cancle_charge)));
                            dtstudentacc.Rows[0]["balance_token"] = (Convert.ToDecimal(dtstudentacc.Rows[0]["balance_token"].ToString()) + ((amount_student) - (amount_of_cancle_charge)));
                            objDs_trastration.fn_token_balance.ImportRow(dtstudentacc.Rows[0]);
                            #endregion

                            #region token transtraion
                            //my tutor account

                            DS_Transtration.fn_token_transtionRow token_transtration_row = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                            token_transtration_row.doc_no = "1";
                            token_transtration_row.doc_date = System.DateTime.Now;
                            token_transtration_row.ref_transtion_type = "mycccharge_cancle";
                            token_transtration_row.ref_transtion_doc_date = System.DateTime.Now;
                            token_transtration_row.ref_transtion_doc_no = parameter["session_id"].ToString();
                            token_transtration_row.amount = amount_of_cancle_charge;
                            token_transtration_row.type_credit_debit = "Credit";

                            token_transtration_row.balance_after_transtion = (Convert.ToDecimal(Convert.ToDecimal(bal_befor)) + (amount_of_cancle_charge));
                            token_transtration_row.member_id = mycc_id;
                            token_transtration_row.created_by = mycc_id;
                            token_transtration_row.created_date = System.DateTime.Now;
                            token_transtration_row.created_host = HttpContext.Current.Request.UserHostName;
                            fn_seesion_booking.Rows[0]["cancelled_charge"] = amount_of_cancle_charge;
                            fn_seesion_booking.Rows[0]["session_mycc_charge"] = amount_of_cancle_charge;

                            objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row);

                            // my cc account
                            DS_Transtration.fn_token_transtionRow token_transtration_row2 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                            token_transtration_row2.doc_no = "2";
                            token_transtration_row2.doc_date = System.DateTime.Now;
                            token_transtration_row2.ref_transtion_type = "my_tranf_stud";
                            token_transtration_row2.ref_transtion_doc_date = System.DateTime.Now;
                            token_transtration_row2.ref_transtion_doc_no = parameter["session_id"].ToString();
                            token_transtration_row2.amount = amount_student - amount_of_cancle_charge;
                            token_transtration_row2.type_credit_debit = "debit";

                            token_transtration_row2.balance_after_transtion = Convert.ToDecimal(Convert.ToDecimal(bal_befor) + (amount_student - amount_of_cancle_charge));
                            token_transtration_row2.member_id = mycc_id;
                            token_transtration_row2.created_by = mycc_id;
                            token_transtration_row2.created_date = System.DateTime.Now;
                            token_transtration_row2.created_host = HttpContext.Current.Request.UserHostName;

                            objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row2);

                            DS_Transtration.fn_token_transtionRow token_transtration_row3 = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                            token_transtration_row3.doc_no = "3";
                            token_transtration_row3.doc_date = System.DateTime.Now;
                            token_transtration_row3.ref_transtion_type = "cancle_tranf_stud";
                            token_transtration_row3.ref_transtion_doc_date = System.DateTime.Now;
                            token_transtration_row3.ref_transtion_doc_no = parameter["session_id"].ToString();
                            token_transtration_row3.amount = student_credit;
                            token_transtration_row3.type_credit_debit = "Credit";
                            token_transtration_row3.balance_after_transtion = (Convert.ToDecimal(bal_befor) - student_credit);
                            token_transtration_row3.member_id = session_time.Rows[0]["booked_by"].ToString();
                            token_transtration_row3.created_by = session_time.Rows[0]["booked_by"].ToString();
                            token_transtration_row3.created_date = System.DateTime.Now;
                            token_transtration_row3.created_host = HttpContext.Current.Request.UserHostName;

                            objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row3);

                            #region  session cancle

                            DS_Transtration.fn_token_cancelRow session_cancle = objDs_trastration.fn_token_cancel.Newfn_token_cancelRow();
                            session_cancle.doc_date = System.DateTime.Now;
                            session_cancle.doc_no = "1";
                            session_cancle.session_id = parameter["session_id"].ToString();
                            session_cancle.session_tutor = session_time.Rows[0]["tutor_id"].ToString();
                            session_cancle.session_cost = Convert.ToDecimal(fn_seesion_booking.Rows[0]["total_session_charge"].ToString());
                            session_cancle.session_cancelled_by = parameter["member_id"].ToString();
                            session_cancle.session_refunded_amount = student_credit;
                            session_cancle.is_active = "Y";
                            session_cancle.created_by = parameter["member_id"].ToString();
                            session_cancle.created_host = HttpContext.Current.Request.UserHostName;
                            session_cancle.created_date = System.DateTime.Now;

                            objDs_trastration.fn_token_cancel.Addfn_token_cancelRow(session_cancle);

                            #endregion





                            #endregion


                        }




                        fn_seesion_booking.Rows[0]["is_cancelled_id"] = parameter["member_id"].ToString();

                        fn_seesion_booking.Rows[0]["is_session_cancel"] = "Y";


                        objDs_trastration.fn_session_booking.ImportRow(fn_seesion_booking.Rows[0]);

                        #endregion


                        #endregion

                        objBLReturnObject = objmasters.updateSessionResponsewithtrans(objDS_ScheduleAppointment, objDs_trastration);
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
        public string SaveChating([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();

            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);

            string result = "";
            bool flag = false;

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["tokendata"]["member_id"].ToString(), parameter["tokendata"]["Token"].ToString());
                if (security_flag == true)
                {




                    #region save fresh data /new

                    if (parameter["chatdata"] != null)
                    {

                        #region notification : table

                        DS_MemberTables.chatingRow chatRow = objDS_MemberTables.chating.NewchatingRow();
                        chatRow.chat_id = "1";
                        chatRow.university_id = parameter["chatdata"]["university_id"].ToString();
                        chatRow.from_member = parameter["chatdata"]["from_member"].ToString();
                        chatRow.from_member_role = parameter["chatdata"]["from_member_role"].ToString();
                        chatRow.to_member = parameter["chatdata"]["to_member"].ToString();
                        chatRow.to_member_role = parameter["chatdata"]["to_member_role"].ToString();
                        chatRow.description = parameter["chatdata"]["desc"].ToString();
                        // chatRow.time =Convert.ToDateTime(parameter["chatdata"]["time"].ToString());
                        chatRow.chat_date = Convert.ToDateTime(parameter["chatdata"]["chat_date"].ToString());
                        chatRow.is_read = "N";
                        chatRow.divas_type = parameter["chatdata"]["session_id"].ToString();

                        chatRow.created_by = parameter["tokendata"]["member_id"].ToString();
                        chatRow.created_date = System.DateTime.Now;
                        chatRow.created_host = HttpContext.Current.Request.UserHostName;
                        objDS_MemberTables.chating.AddchatingRow(chatRow);

                        DataTable dt = objmasters.getuserdetai(parameter["chatdata"]["from_member"].ToString());

                        #endregion
                        #region alert message

                        DS_ScheduleAppointment.alertmessageRow alert_row = objDS_ScheduleAppointment.alertmessage.NewalertmessageRow();
                        alert_row.Doc_no = randomNo.ToString();
                        alert_row.university_id = parameter["chatdata"]["university_id"].ToString();
                        alert_row.to_member = parameter["chatdata"]["to_member"].ToString();
                        alert_row.to_member_role = parameter["chatdata"]["to_member_role"].ToString();
                        alert_row.notification_date = System.DateTime.Now;
                        alert_row._event = "Message";
                        alert_row.template = "You have got a new Message  from " + dt.Rows[0]["first_name"].ToString() + "  " + dt.Rows[0]["last_name"].ToString();
                        alert_row.is_read = "N";
                        alert_row.is_active = "Y";
                        alert_row.senddate = System.DateTime.Now;
                        alert_row.utc_time = System.DateTime.UtcNow.TimeOfDay;
                        alert_row.created_by = parameter["chatdata"]["from_member"].ToString();
                        alert_row.created_date = System.DateTime.Now;
                        alert_row.created_host = HttpContext.Current.Request.UserHostName;

                        objDS_ScheduleAppointment.alertmessage.AddalertmessageRow(alert_row);







                        #endregion



                    }

                    objBLReturnObject = objmasters.Savechat(objDS_MemberTables, objDS_ScheduleAppointment);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Save");
                        return "Save";
                    }
                    #endregion



                }



                else
                {

                    return "securityIssue";
                }



            }

            catch (Exception e)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + e.StackTrace);
                return result;
            }
            return result;


        }
        [HttpPost]
        public string savegururate([FromBody]JObject parameter)
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




                    #region save fresh data /new

                    DataTable dt = objmasters.getgutudata(parameter["member_id"].ToString());
                    if (dt != null)
                    {
                        dt.Rows[0]["amount"] = parameter["amount"].ToString();

                        objDS_MemberTables.tutor_guru_rate_mst.ImportRow(dt.Rows[0]);


                        objBLReturnObject = objmasters.Save_guru_rate(objDS_MemberTables, true);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {
                            ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Save");
                            return "Save";
                        }
                    }
                    else
                    {

                        #region notification : table

                        DS_MemberTables.tutor_guru_rate_mstRow gururow = objDS_MemberTables.tutor_guru_rate_mst.Newtutor_guru_rate_mstRow();
                        gururow.sr_no = "1";
                        gururow.university_id = parameter["university_id"].ToString();
                        gururow.amount = parameter["amount"].ToString();
                        gururow.is_active = "Y";
                        gururow.tutor_id = parameter["member_id"].ToString();

                        gururow.created_by = parameter["member_id"].ToString();
                        gururow.created_date = System.DateTime.Now;
                        gururow.created_host = HttpContext.Current.Request.UserHostName;

                        objDS_MemberTables.tutor_guru_rate_mst.Addtutor_guru_rate_mstRow(gururow);


                        #endregion




                        objBLReturnObject = objmasters.Save_guru_rate(objDS_MemberTables, false);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {
                            ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Save");
                            return "Save";
                        }
                    #endregion


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
                return result;
            }
            return result;


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
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
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
        //            if (dtrole != null && dtrole.Rows.Count > 0)
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
        //                objDS_MemberTables.member_role.ImportRow(dr_role_detail);


        //                objBLReturnObject = objmasters.updatetutorbyadmin(objDS_MemberTables);
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


        // update tutor avaliblity flag set 
        [HttpPost]
        public string updatetutoravailability([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();
            string result = "";
            bool flag = false;
            string msg = "";

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    DataTable dtmebermaster = objmasters.getuserdetai(parameter["member_id"].ToString());
                    if (dtmebermaster != null && dtmebermaster.Rows.Count > 0)
                    {
                        #region member master : table : update

                        DataRow dr = dtmebermaster.Rows[0];
                        dtmebermaster.ImportRow(dr);
                        if (dr["availability"].ToString() == "Y")
                        {

                            dr["availability"] = "N";
                            msg = "1";

                        }
                        else
                        {
                            dr["availability"] = "Y";
                            msg = "0";


                        }
                        // dr["availability"] = parameter["availability"].ToString();

                        dr["last_modified_by"] = dtmebermaster.Rows[0]["email_id"].ToString();
                        dr["last_modified_date"] = System.DateTime.Now;
                        dr["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                        objDS_MemberTables.member_master.ImportRow(dr);
                        #endregion
                    }


                    objBLReturnObject = objmasters.dsupdate_User(objDS_MemberTables);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        if (msg == "1")
                        {
                            ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                            return "1";
                        }
                        else
                        {
                            ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Pass");
                            return "0";
                        }
                        //return "Pass";
                        //return objBLReturnObject.ServerMessage;
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

        //update Deactive/and acctive  flag account
        [HttpPost]
        public string UpdateAccountDeactive([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();
            string result = "";
            bool flag = false;

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    DataTable dtmebermaster = objmasters.getuserdetai(parameter["member_id"].ToString());
                    if (dtmebermaster != null && dtmebermaster.Rows.Count > 0)
                    {
                        #region member master : table : update

                        DataRow dr = dtmebermaster.Rows[0];
                        dtmebermaster.ImportRow(dr);

                        dr["is_active"] = parameter["is_active"].ToString();

                        dr["last_modified_by"] = dtmebermaster.Rows[0]["email_id"].ToString();
                        dr["last_modified_date"] = System.DateTime.Now;
                        dr["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                        objDS_MemberTables.member_master.ImportRow(dr);
                        #endregion
                    }


                    objBLReturnObject = objmasters.dsupdate_User(objDS_MemberTables);
                    if (objBLReturnObject.ExecutionStatus == 1)
                    {
                        return "Pass";
                        //return objBLReturnObject.ServerMessage;
                    }
                    else
                    {
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

        public string SetUserAccount([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["member_id"].ToString(), parameter["Token"].ToString());
                if (security_flag == true)
                {
                    DataTable dtuser = objmasters.getreedemcheck(parameter["member_id"].ToString(), parameter["amount"].ToString());
                    if (dtuser == null || dtuser.Rows.Count < 0)
                    {
                        return "insuffince";
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
            catch (Exception e)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + e.StackTrace);
                return "Fail";
            }

        }


        public string savetokentran([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();

            try
            {
                bool security_flag = objmasters.securityCheck(parameter["memberid"].ToString(), parameter["token_id"].ToString());
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
                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "ExceedAmmount");
                        return "ExceedAmmount";
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
        public string alertupdate([FromBody]JObject parameter)
        {

            BLReturnObject objBLReturnObject = new BLReturnObject();
            sessiontime sessiondata = new sessiontime();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            member Memberid = new member();
            string result = "";
            bool flag = false;

            try
            {
                bool security_flag = true;//= objmasters.securityCheck(parameter["member_id"].ToString(), parameter["loginToken"].ToString());
                if (security_flag == true)
                {
                    //if (parameter["session_id"] != null && parameter["session_id"].ToString() != String.Empty)
                    //{
                    //    DataTable dtsession = objmasters.getPerticularSessionDetail(parameter["session_id"].ToString());

                    //    if (dtsession.Rows[0]["start_confirm_student_datetime"] != null && dtsession.Rows[0]["start_confirm_student_datetime"].ToString().Trim().Length > 0  && parameter["role_id"].ToString() == "ST")
                    //    { return "start"; }
                    //    if (dtsession.Rows[0]["end_confirm_student_datetime"] != null && dtsession.Rows[0]["end_confirm_student_datetime"].ToString().Trim().Length > 0 && parameter["role_id"].ToString() == "ST" )
                    //    { return "stop"; }
                    //}
                    DataTable dtNotification = objmasters.getalertfromdocno(parameter["doc_no"].ToString());
                    if (dtNotification != null && dtNotification.Rows.Count > 0)
                    {
                        #region notification : table : update

                        DataRow dr = dtNotification.Rows[0];
                        dtNotification.ImportRow(dr);

                        dr["is_read"] = "Y";
                        dr["is_accept"] = "Y";

                        dr["last_modified_by"] = parameter["member_id"].ToString();
                        dr["last_modified_date"] = System.DateTime.Now;
                        dr["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                        objDS_ScheduleAppointment.alertmessage.ImportRow(dr);
                        #endregion


                        objBLReturnObject = objmasters.updatealert(objDS_ScheduleAppointment);
                        if (objBLReturnObject.ExecutionStatus == 1)
                        {
                            ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status " + "Pass");
                            return "Pass";
                            //return objBLReturnObject.ServerMessage;
                        }
                        else
                        {
                            ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status " + "Fail");
                            return "Fail";
                        }

                    }
                    else
                    {
                        ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status " + "Fail");
                        return "Fail";
                    }

                    //objBLReturnObject = objmasters.updateNotification(objDS_ScheduleAppointment);
                    //if (objBLReturnObject.ExecutionStatus == 1)
                    //{
                    //    return "Pass";
                    //    //return objBLReturnObject.ServerMessage;
                    //}
                    //else
                    //{
                    //    return "Fail";
                    //}
                }
                else
                {
                    return "securityIssue";
                }
            }
            catch (Exception ex)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status " + ex.StackTrace);
                return "Fail";
            }

        }




        #region Local Method






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

        #region Stripe Methods

        //[WebMethod(EnableSession = true)]
        [HttpPost]
        public string getStripeResponse([FromBody]JObject parameter)
        {


            BLReturnObject objBLReturnObject = new BLReturnObject();
            Random rand = new Random();
            int randomNo = rand.Next(1, 1000000);
            //token_id = "tok_16iOB6BIGKxqA1ymSp6lWoFR";
            StripeCharge chrg = new StripeCharge();
            StripeBalanceTransaction bal_service = new StripeBalanceTransaction();

            string statusCharge = "";
            try
            {
                //chrg = ChargeCustomer(parameter["token_id"]["id"].ToString(), (int)(Convert.ToDouble(parameter["token_id"]["amount_total_stripe"].ToString()) * 100), parameter["token_id"]["email_id"].ToString());
                DataTable dt = new DataTable();
                dt = objmasters.getuserdetai(parameter["token_id"]["email_id"].ToString());

                DS_Transtration.fn_stripe_managementRow transtion_row = objDs_trastration.fn_stripe_management.Newfn_stripe_managementRow();


                transtion_row.Doc_type = "1";
                transtion_row.university_id = dt.Rows[0]["university"].ToString();
                transtion_row.member_id = parameter["token_id"]["email_id"].ToString();
                transtion_row.payment_id = parameter["token_id"]["id"].ToString();

                transtion_row.Amount = (Decimal)(Convert.ToDouble(parameter["token_id"]["amount_total_stripe"].ToString()));

                // transtion_row.token = Convert.ToInt16(parameter["token_id"]["total_token"].ToString());
                transtion_row.payment_status = "proceed";
                transtion_row.ip_host_type = parameter["token_id"]["client_ip"].ToString();
                // transtion_row.geteway_transtration_id = parameter["token_id"]["id"].ToString();
                transtion_row.payment_type = parameter["token_id"]["card"]["brand"].ToString();
                transtion_row.is_active = "Y";
                transtion_row.created_by = parameter["token_id"]["created"].ToString();
                transtion_row.created_date = System.DateTime.Now;
                transtion_row.created_host = HttpContext.Current.Request.UserHostName;

                objDs_trastration.fn_stripe_management.Addfn_stripe_managementRow(transtion_row);


                objBLReturnObject = objmasters.SaveTranstionstratipe(objDs_trastration, false);
                if (objBLReturnObject.ExecutionStatus == 1)
                {

                    chrg = ChargeCustomer(parameter["token_id"]["id"].ToString(), (int)(Convert.ToDouble(parameter["token_id"]["amount_total_stripe"].ToString()) * 100), parameter["token_id"]["email_id"].ToString());
                    //chrg = ChargeCustomer(token_id);
                    if (chrg.Status == "succeeded")
                    {
                        bal_service = service_balance(chrg.BalanceTransactionId);
                        DataTable dtstripe = objmasters.getStripe(parameter["token_id"]["id"].ToString());
                        if (dtstripe != null && dtstripe.Rows.Count > 0)
                        {
                            #region Stript update : table : update

                            //DataRow dr = dtstripe.Rows[0];
                            //dtstripe.ImportRow(dr);

                            dtstripe.Rows[0]["payment_status"] = "succeeded";
                            dtstripe.Rows[0]["fees_strip"] = bal_service.Fee / 100;
                            dtstripe.Rows[0]["token"] = (bal_service.Net / 100);
                            dtstripe.Rows[0]["geteway_transtration_id"] = chrg.BalanceTransactionId.ToString();

                            objDs_trastration.Clear();
                            objDs_trastration.fn_stripe_management.ImportRow(dtstripe.Rows[0]);
                            #endregion
                            objBLReturnObject = objmasters.SaveTranstionstratipe(objDs_trastration, true);
                            if (objBLReturnObject.ExecutionStatus == 1)
                            {

                                DataTable dtbalance = objmasters.gettokenbal(parameter["token_id"]["email_id"].ToString());

                                if (dtbalance != null && dtbalance.Rows.Count > 0)
                                {
                                    DataRow dr = dtbalance.Rows[0];
                                    dtbalance.ImportRow(dr);
                                    dr["balance_token"] = (Convert.ToDecimal(dtbalance.Rows[0]["balance_token"].ToString()) + (((Convert.ToDecimal(bal_service.Net)) / 100)));
                                    // dr["balance_token"] = (Convert.ToDecimal(dtbalance.Rows[0]["balance_token"].ToString()) + (Convert.ToDecimal(parameter["token_id"]["total_token"].ToString())));
                                    dr["total_credit"] = Convert.ToDecimal(dtbalance.Rows[0]["total_credit"].ToString()) + ((Convert.ToDecimal(bal_service.Net)) / 100);
                                    dr["balance_amount"] = Convert.ToDecimal(dtbalance.Rows[0]["balance_amount"].ToString()) + ((Convert.ToDecimal(bal_service.Net)) / 100);
                                    //update is pending
                                    dr["last_modified_by"] = dtbalance.Rows[0]["member_id"].ToString();
                                    dr["last_modified_date"] = System.DateTime.Now;
                                    dr["last_modified_host"] = HttpContext.Current.Request.UserHostName;

                                    objDs_trastration.fn_token_balance.ImportRow(dr);


                                    DS_Transtration.fn_token_transtionRow token_transtration_row = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                                    token_transtration_row.doc_no = "1";
                                    token_transtration_row.doc_date = System.DateTime.Now;
                                    token_transtration_row.ref_transtion_type = "stripe";
                                    token_transtration_row.ref_transtion_doc_date = System.DateTime.Now;
                                    token_transtration_row.ref_transtion_doc_no = dtstripe.Rows[0]["doc_type"].ToString();
                                    token_transtration_row.amount = ((Convert.ToDecimal(bal_service.Net)) / 100);
                                    token_transtration_row.balance_after_transtion = Convert.ToDecimal(objDs_trastration.fn_token_balance.Rows[0]["balance_amount"].ToString());
                                    token_transtration_row.type_credit_debit = "Credit";
                                    token_transtration_row.member_id = parameter["token_id"]["email_id"].ToString();
                                    token_transtration_row.created_by = parameter["token_id"]["email_id"].ToString();
                                    token_transtration_row.created_date = System.DateTime.Now;
                                    token_transtration_row.created_host = HttpContext.Current.Request.UserHostName;



                                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row);

                                    objBLReturnObject = objmasters.Savetokenbalance(objDs_trastration, true);
                                }
                                else
                                {
                                    DS_Transtration.fn_token_balanceRow token_bal_row = objDs_trastration.fn_token_balance.Newfn_token_balanceRow();
                                    token_bal_row.doc_no = "1";
                                    token_bal_row.member_id = parameter["token_id"]["email_id"].ToString();
                                    token_bal_row.total_credit = ((Convert.ToDecimal(bal_service.Net)) / 100);
                                    token_bal_row.balance_token = Convert.ToDecimal(parameter["token_id"]["total_token"].ToString());
                                    token_bal_row.balance_amount = ((Convert.ToDecimal(bal_service.Net)) / 100);

                                    token_bal_row.is_active = "Y";
                                    token_bal_row.created_by = parameter["token_id"]["created"].ToString();
                                    token_bal_row.created_date = System.DateTime.Now;
                                    token_bal_row.created_host = HttpContext.Current.Request.UserHostName;

                                    objDs_trastration.fn_token_balance.Addfn_token_balanceRow(token_bal_row);



                                    DS_Transtration.fn_token_transtionRow token_transtration_row = objDs_trastration.fn_token_transtion.Newfn_token_transtionRow();
                                    token_transtration_row.doc_no = "1";
                                    token_transtration_row.doc_date = System.DateTime.Now;
                                    token_transtration_row.ref_transtion_type = "stripe";
                                    token_transtration_row.ref_transtion_doc_date = System.DateTime.Now;
                                    token_transtration_row.ref_transtion_doc_no = "2";
                                    token_transtration_row.amount = ((Convert.ToDecimal(bal_service.Net)) / 100);
                                    token_transtration_row.balance_after_transtion = Convert.ToDecimal(objDs_trastration.fn_token_balance.Rows[0]["balance_amount"].ToString());
                                    token_transtration_row.type_credit_debit = "Credit";
                                    token_transtration_row.member_id = parameter["token_id"]["email_id"].ToString();
                                    token_transtration_row.created_by = parameter["token_id"]["email_id"].ToString();
                                    token_transtration_row.created_date = System.DateTime.Now;
                                    token_transtration_row.created_host = HttpContext.Current.Request.UserHostName;



                                    objDs_trastration.fn_token_transtion.Addfn_token_transtionRow(token_transtration_row);

                                    objBLReturnObject = objmasters.Savetokenbalance(objDs_trastration, false);
                                }
                            }
                            else
                            {
                                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + "Fail");
                                statusCharge = "error";
                            }
                        }

                    }
                    statusCharge = chrg.Status;

                }


                //string stp = GetToken("4242424242424242", "2022", "10", "1223");

            }
            catch (Exception ex)
            {
                ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);
                statusCharge = ex.StackTrace;
            }
            // return null;
            return statusCharge;
        }

        private static StripeCharge ChargeCustomer(string tokenid, int amt, string email)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var mycharge = new StripeChargeCreateOptions();
            // mycharge.Amount = 10330;
            mycharge.Amount = amt;
            mycharge.Currency = "usd";

            mycharge.Source = new StripeSourceOptions()
            {
                //TokenId ="",
                TokenId = tokenid,
                // ReceiptEmail = email,
            };

            mycharge.Capture = true;

            var chargeService = new StripeChargeService("sk_live_QsoYGWum1bwVDdpUw54imlVX");
            StripeCharge stripeCharge = chargeService.Create(mycharge);


            //StripeBalanceService balance_service = new StripeBalanceService("sk_test_I4kPrFc2064RSEKgsZ9OeAep");

            //var lst_balance = balance_service.Get(stripeCharge.BalanceTransactionId);

            return stripeCharge;

        }
        public static StripeBalanceTransaction service_balance(string balanceid)
        {

            StripeBalanceService balance_service = new StripeBalanceService("sk_live_QsoYGWum1bwVDdpUw54imlVX");

            StripeBalanceTransaction bal_transtration = balance_service.Get(balanceid);

            return bal_transtration;
        }
        #endregion
    }
}
