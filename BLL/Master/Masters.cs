using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Utilities;
using System.Data;
using WebApiCampusConcierge.XSD;
using System.Text;
using System.Globalization;

using WebApiCampusConcierge.Models;
using WebApiCampusConcierge.BLL.Utilities;

namespace BLL.Master
{
    public class Masters : ServerBase
    {
        #region variable declaration
        DS_MemberTables objDS_MasterTables = new DS_MemberTables();

        #endregion
        #region admin data
        public DataTable admindata()
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("select COUNT(member_code)as TotalUser  from member_master where is_quantina='Y' ");

            sb.Append("select count(member_code)as ActiveUser from member_master where created_date between DATEADD(week, DATEDIFF(day, 0, getdate())/7, 0) and DATEADD(week, DATEDIFF(day, 0, getdate())/7, 5) and is_active='Y' and is_quantina='Y' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();
            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Get Methods
        //new admin

        public DataTable get_total_user_count()
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT     COUNT(member_code) AS no_of_User FROM         member_master WHERE     (is_active = 'Y') ");
            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();
            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable totaluser()
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from  ");
            sb.Append(" (select COUNT(member_code)as total_User from member_master where is_active='Y') as a ");
            sb.Append(" ,(select COUNT(session_id)as total_Session from session_timing where is_active='Y' and appointment_confirmed='C') as b ");
            sb.Append(" ,(select sum(token) as total_revenue from fn_stripe_management where payment_status='succeeded' )as c ");
            sb.Append(" ,(select sum(amount)as net_profit from fn_token_transtion where ref_transtion_type='mycccharge' )as d ");
            sb.Append(" ,(select count(member_code)as total_month_user from member_master where is_active='Y' and created_date>=(select dateadd(dd,-day(getdate())+1,getdate())))as e ");


            //this for add after run
            sb.Append("  select IIF (sum(token) is null ,0,sum(token)/DATEPART(d, GETDATE())) as avg_revenue from fn_stripe_management where payment_status='succeeded'  ");
            sb.Append(" and created_date>=(select dateadd(dd,-day(getdate())+1,getdate()))  ");
            sb.Append(" and created_by<=(select dateadd(dd,-day(dateadd(mm,1,getdate())),dateadd(mm,1,getdate()))) ");

            //ths avg session
            sb.Append(" select IIF (sum(cast(session_id AS INT)) is null ,0,sum(cast(session_id AS INT))/DATEPART(d, GETDATE())) as avg_Session from session_timing where is_active='Y' and appointment_confirmed='C' ");
            sb.Append(" and created_date>=(select dateadd(dd,-day(getdate())+1,getdate()))  ");
            sb.Append(" and created_by<=(select dateadd(dd,-day(dateadd(mm,1,getdate())),dateadd(mm,1,getdate()))) ");


            // Master List of Users
            sb.Append("select distinct member_master.first_name,member_master.last_name,member_master.email_id,member_master.classification,member_master.is_active, ");
            sb.Append(" university_master.university_name, ");
            sb.Append(" (select Distinct Top(1) role_id from member_role as a Where a.member_id=member_role.member_id order by role_id asc) as ST, ");
            sb.Append(" (select Distinct Top(1) role_id from member_role as a Where a.member_id=member_role.member_id   ");
            sb.Append(" order by role_id desc) as 'TT', ");
            sb.Append(" case when ((select Distinct Top(1) role_id from member_role as a Where a.member_id=member_role.member_id order by role_id asc)='ST' ");
            sb.Append(" and (select Distinct Top(1) role_id from member_role as a Where a.member_id=member_role.member_id  order by role_id desc)='TT') then 'Both' ");
            sb.Append(" when member_role.role_id='ST' then 'Student' ");
            sb.Append(" when member_role.role_id='TT' then 'Tutor' ");
            sb.Append(" else null  end  as memberrole, ");
            sb.Append(" (select  parameter_id from average_tutor_rating where tutor_id=member_master.email_id) as Achievement_tutor , ");
            sb.Append(" (select  parameter_id from average_student_rating where student_id=member_master.email_id) as Achievement_student , ");

            sb.Append(" (select  total_no_session from average_tutor_rating where tutor_id=member_master.email_id) as total_tutor_session , ");
            sb.Append(" (select  total_no_session from average_student_rating where student_id=member_master.email_id) as total_student_session,  ");
            sb.Append("  (select count(member_code)  from  member_master where default_role not in ('AD'))  as total_count ");
            sb.Append("  from member_role ");
            sb.Append(" left join member_master on member_role.member_id=member_master.email_id ");
            sb.Append(" inner join university_master on member_master.university=university_master.university_id ");

            sb.Append(" order by email_id ");









            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();
            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable total_session()
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("select COUNT(session_id) from session_timing where is_active='Y' and appointment_confirmed='C' ");
            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();
            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //end admin
        //datetiem convert
        public DateTime timeconevert(string zone, string date, string time)
        {

            TimeZoneInfo timeZoneInfo;
            DateTime dateTime, FetchedDate;
            //Set the time zone information to US Mountain Standard Time 
            //timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(drpToTimeZone.SelectedValue);
            //Get date and time in US Mountain Standard Time 
            //dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

            FetchedDate = Convert.ToDateTime((Convert.ToDateTime(date)).ToShortDateString() + " " + time);

            var tZone = TimeZoneInfo.GetSystemTimeZones();
            String temp = tZone.Where(e1 => e1.DisplayName.Contains(zone)).Select(t => t.Id).FirstOrDefault().ToString();

            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(temp);

            dateTime = TimeZoneInfo.ConvertTimeToUtc(FetchedDate, timeZoneInfo);


            return dateTime;
        }
        //public Nullable<DateTime> member_timezone(string email)
        //{
        //    try
        //    {
        //        DBDataAdpterObject.SelectCommand.Parameters.Clear();
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append(" select timezone from member_master where email_id='" + email + "'");

        //        DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            DBDataAdpterObject.Fill(ds);
        //            if (ds.Tables[0].Rows.Count <= 0)
        //                return null;
        //            else
        //            {
        //                DateTime ConvertedDate =timeconevert(ds.Tables[0].Rows[0]["timezone"].ToString());
        //                return ConvertedDate;

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}
        public DataTable GetNewUsers(string Dates)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("select member_master.*, university_master.university_name from member_master left join university_master on member_master.university=university_master.university_id");
            sb.Append(" where member_master.created_date >='" + Dates + "'");





            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getuserlist(string uni)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append(" select first_name,last_name,email_id from  member_master where university='" + uni + "' and is_active='Y' ");





            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getlegerforstudent(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            //sb.Append(" select * from  fn_token_transtion  where ");
            //sb.Append("doc_date > cast('2016-08-10 15:40:38.920' as date) and doc_date< getdate()  and  member_id='naisudesai@gmail.com' ");


            sb.Append(" SELECT     CONVERT(VARCHAR(10), DOC_DATE, 101) AS date, type_credit_debit, amount, member_id ");
            sb.Append(" FROM    fn_token_transtion where member_id='" + member_id + "'  ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable time_diff(string current_time, DateTime session_date)
        {
            try
            {
                //DateTime FetchedDate = Convert.ToDateTime((Convert.ToDateTime(session_date)).ToShortDateString() + " " + session_time);

                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                // String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT   DATEDIFF(day,'" + System.DateTime.UtcNow + "','" + session_date + "') diff ");
                sb.Append(" UNION all  ");
                sb.Append(" SELECT  DATEDIFF(hour,'" + System.DateTime.UtcNow + "','" + session_date + "')  ");
                sb.Append("       UNION all   ");
                sb.Append("  SELECT  DATEDIFF(MINUTE,'" + System.DateTime.UtcNow + "','" + session_date + "')  ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable time_diff_ex_hours(string current_time, DateTime session_date)
        {
            try
            {
                //DateTime FetchedDate = Convert.ToDateTime((Convert.ToDateTime(session_date)).ToShortDateString() + " " + session_time);

                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                // String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT   DATEDIFF(day,'" + session_date + "','" + System.DateTime.UtcNow + "') diff ");
                sb.Append(" UNION all  ");
                sb.Append(" SELECT  DATEDIFF(hour,'" + session_date + "','" + System.DateTime.UtcNow + "')  ");
                sb.Append("       UNION all   ");
                sb.Append("  SELECT  DATEDIFF(MINUTE,'" + session_date + "','" + System.DateTime.UtcNow + "')  ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetCCEarnigs(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("select * from fn_token_balance    ");
            sb.Append(" where member_id ='mycc@gmail.com'");





            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAlertmessage(string member_id, string role, string currentTime)
        {

            try
            {

                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                //sb.Append("  declare @UTC_Time datetime='2016-08-18 09:59:39.187' ");
                sb.Append("  declare @UTC_Time datetime='" + Convert.ToDateTime(currentTime).ToUniversalTime() + "' ");
                //sb.Append(" declare @member_id varchar(50) ='jaiminpatel520.mca@gmail.com'  ");
                sb.Append(" declare @member_id varchar(50) ='" + member_id + "'  ");
                sb.Append(" declare @member_Role varchar(50)='" + role + "' ");

                sb.Append(" select *  ");
                sb.Append(" from alertmessage  ");
                sb.Append(" Where (_event='Session') and (DATEDIFF(minute, cast(CAST(senddate AS DATE) as varchar(10))+' '+cast(utc_time as varchar(10)) ");
                sb.Append(" , @UTC_Time)>=-5 and DATEDIFF(minute, cast(CAST(senddate AS DATE) as varchar(10))+' '+cast(utc_time as varchar(10)) ");
                sb.Append(" , @UTC_Time)<=0) and to_member = @member_id and to_member_role=@member_Role ");
                sb.Append(" Union All  ");
                sb.Append(" select *  ");
                sb.Append(" from alertmessage  ");
                sb.Append(" Where is_read='N' and (_event not in('Session','Message')) and to_member = @member_id and to_member_role=@member_Role ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        //public DataTable GetAlertmessage(string email, string role, string time)
        //{

        //    try
        //    {
        //        DBDataAdpterObject.SelectCommand.Parameters.Clear();
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("  declare @UTC_Time datetime='2016-08-18 09:59:39.187' ");
        //        sb.Append(" declare @member_id varchar(50) ='jaiminpatel520.mca@gmail.com'  ");
        //        sb.Append(" declare @member_Role varchar(50)='TT' ");

        //        sb.Append(" select *  ");
        //        sb.Append(" from alertmessage  ");
        //        sb.Append(" Where (_event='Session') and (DATEDIFF(minute, cast(CAST(senddate AS DATE) as varchar(10))+' '+cast(utc_time as varchar(10)) ");
        //        sb.Append(" , @UTC_Time)>=-5 and DATEDIFF(minute, cast(CAST(senddate AS DATE) as varchar(10))+' '+cast(utc_time as varchar(10)) ");
        //        sb.Append(" , @UTC_Time)<=0) and to_member = @member_id and to_member_role=@member_Role ");
        //        sb.Append(" Union All  ");
        //        sb.Append(" select *  ");
        //        sb.Append(" from alertmessage  ");
        //        sb.Append(" Where is_read='N' and (_event<>'Session') and to_member = @member_id and to_member_role=@member_Role ");

        //        DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            DBDataAdpterObject.Fill(ds);
        //            if (ds.Tables[0].Rows.Count <= 0)
        //                return null;
        //            else
        //                return ds.Tables[0];
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}
        public DataTable forgotpwd(string emailID, string datePicker, string security_que, string Security_ans)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM member_master 
                              WHERE (email_rand_no = '0') AND (is_active = 'Y') AND (email_id = '" + emailID + "') AND (birthdate = '" + Convert.ToDateTime(datePicker).ToString("yyyy-MM-dd") + "') AND (security_question = '" + security_que + "') AND (question_ans = '" + Security_ans + "')";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool securityCheck(string mem_id, string token_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM member_master left join login_token on member_master.email_id=login_token.member_id WHERE (email_id = '" + mem_id + "') AND (login_token.token_id = '" + token_id + "') AND (is_active = 'Y')";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataTable getRoleWiseMember(string member_id, string role_id, string university)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                if (role_id == "TT")
                {
                    SqlSelect = @"select * from member_role where member_id = '" + member_id + "' and role_id = '" + role_id + "' and is_active = 'Y' and university_id = '" + university + "' and is_approved='Y' and approved_by_whome is not null";
                }
                else
                {

                    SqlSelect = @"select * from member_role where member_id = '" + member_id + "' and role_id = '" + role_id + "' and is_active = 'Y' and university_id = '" + university + "'";

                }

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable getMostRecentSessions(string mem_id, string mem_role, string course)
        {

            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                if (mem_role == "ST")
                {
                    SqlSelect = @"SELECT  avg_value,member_master.email_id,session_timing.session_start_confirm_by_tutor_id,session_timing.course_id,course_mst.course_type,course_mst.course_name,DATEDIFF(day,session_timing.end_confirm_tutor_datetime,CAST(GETDATE() AS DATE)) AS days_before,member_master.first_name,member_master.last_name,member_master.image,session_timing.session_date FROM session_participants
                              left join session_timing on session_participants.session_id = session_timing.session_id and session_participants.course_id = session_timing.course_id and session_participants.university_id = session_timing.university_id
                              left join member_master on member_master.email_id = session_timing.session_start_confirm_by_tutor_id and member_master.university = session_timing.university_id
                              left join course_mst on course_mst.course_id = session_timing.course_id and course_mst.university_id = session_timing.university_id
                              left join average_tutor_rating on average_tutor_rating.tutor_id = member_master.email_id and average_tutor_rating.university_id = member_master.university " +
                              "where session_participants.member_id = '" + mem_id + "' and session_participants.role_id = '" + mem_role + "' " +
                              "and member_master.is_active = 'Y' and session_timing.session_start_confirm_by_tutor_id is not null and average_tutor_rating.is_active = 'Y' " +
                              "order by session_timing.session_date desc";
                }
                else if (mem_role == "TT")
                {
                    SqlSelect = @"select  avg_value,member_master.email_id,session_timing.session_id,member_master.first_name,member_master.last_name,session_timing.course_id,course_mst.course_name,member_master.image,session_timing.session_start_confirm_by_student_id,DATEDIFF(day,session_timing.end_confirm_student_datetime,CAST(GETDATE() AS DATE)) AS days_before,session_timing.end_confirm_student_datetime from session_participants 
                                    left join session_timing on session_timing.session_id = session_participants.session_id
                                    left join member_master on member_master.email_id = session_timing.session_start_confirm_by_student_id and member_master.university = session_timing.university_id
                                    left join course_mst on course_mst.course_id = session_timing.course_id and course_mst.university_id = session_timing.university_id
                                    left join average_student_rating on average_student_rating.student_id = member_master.email_id and average_student_rating.university_id = member_master.university " +
                                    "where session_participants.member_id = '" + mem_id + "' and session_participants.role_id = '" + mem_role + "' and session_timing.is_active = 'Y' and member_master.is_active = 'Y' and course_mst.is_active = 'Y' and session_participants.is_active='Y' and member_master.is_active='Y'and  session_timing.course_id='" + course + "' and session_timing.session_start_confirm_by_student_id is not null " +
                                    "order by session_timing.session_date desc";
                }

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable tutorname(string UniId, string member_id)
        {
            try
            {
                DataTable dtStuLearningCourses = new DataTable();
                dtStuLearningCourses = getStudentLearningCources(member_id, "ST");



                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(" select DISTINCT member_id,member_master.first_name from member_role ");
                sb.Append(" left join member_master on member_role.member_id=member_master.email_id ");
                sb.Append(" left join course_mst on member_role.course_code=course_mst.course_id  ");
                sb.Append(" where member_master.university='" + UniId + "' and member_role.role_id='TT' ");

                if (dtStuLearningCourses != null)
                {
                    sb.Append(" and (course_mst.course_id='" + dtStuLearningCourses.Rows[0]["course_code"].ToString() + "' ");
                    for (int i = 1; i < dtStuLearningCourses.Rows.Count; i++)
                    {
                        sb.Append(" or course_mst.course_id='" + dtStuLearningCourses.Rows[i]["course_code"].ToString() + "' ");
                    }
                    sb.Append(" )");
                }

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();



                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //public DataTable tutorname(string UniId)
        //{
        //    try
        //    {
        //        DBDataAdpterObject.SelectCommand.Parameters.Clear();
        //        String SqlSelect = "";
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append(" select DISTINCT member_id,member_master.first_name from member_role ");
        //        sb.Append(" left join member_master on member_role.member_id=member_master.email_id ");
        //        sb.Append(" where university_id='" + UniId + "' and role_id='TT' ");

        //        DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            DBDataAdpterObject.Fill(ds);
        //            if (ds.Tables[0].Rows.Count <= 0)
        //                return null;
        //            else
        //                return ds.Tables[0];
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        public DataTable getalluserofuniversity(string UniId)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from member_master where university='" + UniId + "' ");


                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable logintokendata(string member_id, string div)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                sb.Append("  select * from login_token where device_id='" + div + "' and member_id='" + member_id + "' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getMemberListCourseRoleWise(string role_id, string course, string university, string member_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                //                SqlSelect = @"select  * from member_master
                //                                left join member_role on member_role.member_id = member_master.email_id
                //                                where member_role.role_id = '" + role_id + "' and member_role.course_code = '" + course + "' 
                //                                "and member_master.university = '" + university + "' and member_master.is_active = 'Y' " +
                //                                "and member_master.email_id != '" + member_id + "' " +
                //                                "order by member_master.email_id ";

                sb.Append(" select   DISTINCT email_id,member_master.first_name,member_master.last_name from member_master ");
                sb.Append(" left join member_role on member_role.member_id = member_master.email_id ");
                sb.Append(" where member_role.role_id = '" + role_id + "'   ");
                sb.Append(" and member_master.university = '" + university + "' and member_master.is_active = 'Y' ");
                sb.Append("and member_master.email_id != '" + member_id + "'  and member_role.course_code='" + course + "'  order by member_master.email_id  ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable getSessionDetail(string role_id, string university, string member_id, string session_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select tutor_location.name,course_mst.course_name,member_master.image,member_master.first_name,member_master.last_name,session_timing.* from session_timing
                                left join session_participants on session_participants.session_id = session_timing.session_id
                                left join member_master on member_master.email_id = session_participants.member_id and member_master.university = session_participants.university_id
                                left join course_mst on course_mst.course_id = session_timing.course_id                                
                                left join tutor_location on session_timing.session_venue=tutor_location.doc_no
                                where session_timing.session_id = '" + session_id + "' and " +
                                "session_participants.member_id = '" + member_id + "' and session_participants.role_id = '" + role_id + "' and session_timing.university_id = '" + university + "' " +
                                "and session_participants.is_active = 'Y' and session_timing.is_active = 'Y' and course_mst.is_active = 'Y'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getcoursetypemst(string id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from course_type_mst where doc_id='" + id + "'");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getgutudata(string id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from  tutor_guru_rate_mst where tutor_id='" + id + "'");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable gettimezone_by_user(string email)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from  member_master  ");
                sb.Append(" left join university_master on member_master.university=university_master.university_id  ");
                sb.Append(" where email_id='" + email + "' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getUniversityWiseNotifications(string university_id, string current_member, string current_member_role)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select member_master.image,member_master.first_name,member_master.last_name,notification.* from notification 
                                left join member_master on member_master.email_id = notification.from_member
                                where CAST(notification_date as DATE) >= CAST(GETDATE() as DATE) " +
                                "and to_member = '" + current_member + "' and to_member_role = '" + current_member_role + "' " +
                                "and is_read = 'N' and notification.is_active = 'Y' and member_master.is_active = 'Y' " +
                                "and notification.university_id = '" + university_id + "' order by created_date desc ";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable getSession_Requests(string role_id, string member_id, string university)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select CAST(session_timing.created_date As TIME) as ReqTime ,member_master.image,member_master.first_name,member_master.last_name,session_timing.* from session_timing
                            left join session_participants on session_participants.session_id = session_timing.session_id
                            left join member_master on member_master.email_id = session_timing.booked_by and member_master.university = session_participants.university_id
                            where session_timing.appointment_confirmed = 'N' and CAST (session_timing.session_date AS DATE) >= CAST(GETDATE() AS DATE) 
                            and session_participants.member_id = '" + member_id + "' and session_participants.role_id = '" + role_id + "' and session_timing.university_id = '" + university + "' " +
                            "and session_participants.is_active = 'Y' and session_timing.is_active = 'Y'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //        //original method
        //        public DataTable getSessionManagmentDetailsWHENST(string role_id, string current_member_id, string current_member_role)
        //        {
        //            try
        //            {
        //                DBDataAdpterObject.SelectCommand.Parameters.Clear();
        //                String SqlSelect = "";

        //                //Old query bfr 18/5/16
        ////                SqlSelect = @"select session_timing.session_id,DATEDIFF(mi, CONVERT(TIME, session_timing.session_start_time),CONVERT(TIME, session_timing.session_end_time)) as timeDiffInMinute,session_timing.session_venue,session_timing.session_date,session_timing.session_start_time,session_timing.session_end_time,course_mst.course_name,session_participants.member_id,member_master.image,member_master.first_name,member_master.last_name from session_timing
        ////                                left join session_participants on session_participants.session_id = session_timing.session_id
        ////                                left join member_master on member_master.email_id = session_participants.member_id 
        ////                                left join course_mst on course_mst.course_id = session_timing.course_id
        ////                                where CAST( session_date AS DATE)= CAST(GETDATE() AS DATE)
        ////                                and session_timing.appointment_confirmed = 'Y' 
        ////                                and session_timing.session_start_time >= CONVERT(VARCHAR, GETDATE(), 108) " +
        ////                                "and session_participants.role_id = '"+role_id+"' "+
        ////                                "and member_master.is_active = 'Y' and session_timing.is_active = 'Y' and session_participants.is_active = 'Y' and course_mst.is_active = 'Y' " +
        ////                                "order by session_start_time ";

        //                SqlSelect = @"select session_timing.session_id,DATEDIFF(mi, CONVERT(TIME, session_timing.session_start_time),CONVERT(TIME, session_timing.session_end_time)) as timeDiffInMinute,session_timing.session_venue,session_timing.session_date,session_timing.session_start_time,session_timing.session_end_time,course_mst.course_name,session_participants.member_id,member_master.image,member_master.first_name,member_master.last_name from session_timing
        //                                left join session_participants on session_participants.session_id = session_timing.session_id
        //                                left join member_master on member_master.email_id = session_participants.member_id 
        //                                left join member_role on member_role.course_code = session_timing.course_id and member_role.university_id = session_timing.university_id                               
        //                                left join course_mst on course_mst.course_id = session_timing.course_id
        //                                where CAST( session_date AS DATE)= CAST(GETDATE() AS DATE)
        //                                and session_timing.appointment_confirmed = 'Y' 
        //                                and session_timing.session_start_time >= CONVERT(VARCHAR, GETDATE(), 108) " +
        //                                "and session_participants.role_id = '" + role_id + "' " +
        //                                "and member_role.role_id = '" + current_member_role + "' and member_role.member_id = '" + current_member_id + "' and member_role.is_active = 'Y' " +
        //                                "and member_master.is_active = 'Y' and session_timing.is_active = 'Y' and session_participants.is_active = 'Y' and course_mst.is_active = 'Y' " +
        //                                "order by session_start_time ";

        //                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

        //                DataSet ds = new DataSet();
        //                try
        //                {
        //                    DBDataAdpterObject.Fill(ds);
        //                    if (ds.Tables[0].Rows.Count <= 0)
        //                        return null;
        //                    else
        //                        return ds.Tables[0];
        //                }
        //                catch (Exception ex)
        //                {
        //                    return null;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return null;
        //            }
        //        }


        public DataTable getSessionManagmentDetails(string current_member_id, string current_member_role, string current_university)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                //                SqlSelect = @"select session_timing.session_id,DATEDIFF(mi, CONVERT(TIME, session_timing.session_start_time),CONVERT(TIME, session_timing.session_end_time)) as timeDiffInMinute,session_timing.session_venue,session_timing.session_date,session_timing.session_start_time,session_timing.session_end_time,course_mst.course_name,member_master.image,member_master.first_name,member_master.last_name from session_participants 
                //                                left join session_timing on session_timing.session_id = session_participants.session_id
                //                                left join member_master on member_master.email_id = session_timing.booked_by 
                //                                left join course_mst on course_mst.course_id = session_timing.course_id "+
                //                                "where session_participants.member_id = '" + current_member_id + "' and session_participants.role_id = '" + current_member_role + "' " +
                //                                "and CAST( session_date AS DATE)= CAST(GETDATE() AS DATE) " +
                //                                "and session_timing.session_start_time >= CONVERT(VARCHAR, GETDATE(), 108) " +
                //                                "and session_timing.appointment_confirmed = 'Y' and session_timing.is_active = 'Y' " +
                //                                "and member_master.is_active = 'Y' and member_master.university = '"+current_university+"' " +
                //                                "and session_participants.is_active = 'Y' " +
                //                                "and course_mst.is_active = 'Y' order by session_start_time";

                if (current_member_role == "ST")
                {
                    //                    SqlSelect = @" select   average_student_rating.parameter_id,course_mst.course_type,tutor_location.name,session_timing.course_id,session_timing.session_type,session_timing.session_id,DATEDIFF(mi, CONVERT(TIME, session_timing.session_start_time),CONVERT(TIME, session_timing.session_end_time)) as timeDiffInMinute,session_timing.session_venue,session_timing.session_date,session_timing.session_start_time,session_timing.session_end_time,course_mst.course_name,member_master.image,member_master.email_id,member_master.first_name,member_master.last_name from session_participants 
                    //                                left join session_timing on session_timing.session_id = session_participants.session_id
                    //                                left join member_master on member_master.email_id = session_timing.tutor_id 
                    //                                left join course_mst on course_mst.course_id = session_timing.course_id
                    //                                left join  average_student_rating on session_timing.booked_by=average_student_rating.student_id
                    //
                    //                                left join tutor_location on  session_timing.session_venue= tutor_location.doc_no
                    //                                    where session_participants.member_id = '" + current_member_id + "' and session_participants.role_id = '" + current_member_role + "' " +
                    //                                    "and cast(CAST(session_timing.session_date AS DATE) as varchar)+' '+CONVERT(VARCHAR, session_timing.session_start_time, 108) > GETDATE() " +
                    //                                    "and session_timing.appointment_confirmed = 'Y' and session_timing.is_active = 'Y' " +
                    //                                    "and member_master.is_active = 'Y' and member_master.university = '" + current_university + "' and session_participants.is_active = 'Y' " +
                    //                                    "and course_mst.is_active = 'Y' order by session_timing.session_date,session_start_time";

                    SqlSelect = @" select   average_student_rating.parameter_id,course_mst.course_type,tutor_location.name,session_timing.course_id,session_timing.session_type,session_timing.session_id,DATEDIFF(mi, CONVERT(TIME, session_timing.session_start_time),CONVERT(TIME, session_timing.session_end_time)) as timeDiffInMinute,session_timing.session_venue,session_timing.session_date,session_timing.start_confirm_student_datetime,session_timing.session_start_time,session_timing.session_end_time,course_mst.course_name,member_master.image,member_master.email_id,member_master.first_name,member_master.last_name from session_participants 
                                left join session_timing on session_timing.session_id = session_participants.session_id
                                left join member_master on member_master.email_id = session_timing.tutor_id 
                                left join course_mst on course_mst.course_id = session_timing.course_id
                                left join  average_student_rating on session_timing.booked_by=average_student_rating.student_id

                                left join tutor_location on  session_timing.session_venue= tutor_location.doc_no
                                    where session_participants.member_id = '" + current_member_id + "' and session_participants.role_id = '" + current_member_role + "' " +
                                    "and session_timing.appointment_confirmed = 'Y' and session_timing.is_active = 'Y' " +
                                    "and member_master.is_active = 'Y' and member_master.university = '" + current_university + "' and session_participants.is_active = 'Y' " +
                                    "and course_mst.is_active = 'Y' order by session_timing.session_date,session_start_time";
                }
                else if (current_member_role == "TT")
                {
                    //jaimin time remove 
                    //                    SqlSelect = @" select  tutor_location.name,session_timing.course_id,session_timing.session_type,session_timing.session_id,DATEDIFF(mi, CONVERT(TIME, session_timing.session_start_time),CONVERT(TIME, session_timing.session_end_time)) as timeDiffInMinute,session_timing.session_venue,session_timing.session_date,session_timing.session_start_time,session_timing.session_end_time,course_mst.course_name,member_master.image,member_master.email_id,member_master.first_name,member_master.last_name from session_participants 
                    //                                left join session_timing on session_timing.session_id = session_participants.session_id
                    //                                left join member_master on member_master.email_id = session_timing.booked_by 
                    //                                left join course_mst on course_mst.course_id = session_timing.course_id
                    //                                left join tutor_location on  session_timing.session_venue= tutor_location.doc_no
                    //                                where session_participants.member_id = '" + current_member_id + "' and session_participants.role_id = '" + current_member_role + "' " +
                    //                                   "and cast(CAST(session_timing.session_date AS DATE) as varchar)+' '+CONVERT(VARCHAR, session_timing.session_start_time, 108) > GETDATE() " +
                    //                                    "and session_timing.appointment_confirmed = 'Y' and session_timing.is_active = 'Y' " +
                    //                                    "and member_master.is_active = 'Y' and member_master.university = '" + current_university + "' and session_participants.is_active = 'Y' " +
                    //                                    "and course_mst.is_active = 'Y' order by session_timing.session_date,session_start_time";

                    SqlSelect = @" select  tutor_location.name,session_timing.course_id,session_timing.session_type,session_timing.start_confirm_student_datetime,session_timing.session_id,DATEDIFF(mi, CONVERT(TIME, session_timing.session_start_time),CONVERT(TIME, session_timing.session_end_time)) as timeDiffInMinute,session_timing.session_venue,session_timing.session_date,session_timing.session_start_time,session_timing.session_end_time,course_mst.course_name,member_master.image,member_master.email_id,member_master.first_name,member_master.last_name from session_participants 
                                left join session_timing on session_timing.session_id = session_participants.session_id
                                left join member_master on member_master.email_id = session_timing.booked_by 
                                left join course_mst on course_mst.course_id = session_timing.course_id
                                left join tutor_location on  session_timing.session_venue= tutor_location.doc_no
                                where session_participants.member_id = '" + current_member_id + "' and session_participants.role_id = '" + current_member_role + "' " +
                                    "and session_timing.appointment_confirmed = 'Y' and session_timing.is_active = 'Y' " +
                                    "and member_master.is_active = 'Y' and member_master.university = '" + current_university + "' and session_participants.is_active = 'Y' " +
                                    "and course_mst.is_active = 'Y' order by session_timing.session_date,session_start_time";
                }
                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getPastSessionManagmentDetails(string university, string current_member_id, string current_member_role)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                //                SqlSelect = @"select session_timing.session_id,DATEDIFF(mi, CONVERT(TIME, session_timing.session_start_time),CONVERT(TIME, session_timing.session_end_time)) as timeDiffInMinute,session_timing.session_venue,session_timing.session_date,session_timing.session_start_time,session_timing.session_end_time,course_mst.course_name,session_participants.member_id,member_master.image,member_master.first_name,member_master.last_name from session_timing
                //                                left join session_participants on session_participants.session_id = session_timing.session_id
                //                                left join member_master on member_master.email_id = session_participants.member_id 
                //                                left join member_role on member_role.course_code = session_timing.course_id and member_role.university_id = session_timing.university_id
                //                                left join course_mst on course_mst.course_id = session_timing.course_id
                //                                where CAST( session_date AS DATE) < CAST(GETDATE() AS DATE)
                //                                and session_timing.appointment_confirmed = 'Y' 
                //                                and session_timing.session_end_time < CONVERT(VARCHAR, GETDATE(), 108) 
                //                                and session_participants.role_id = '" + role_id + "' " +
                //                                "and member_role.role_id = '" + current_member_role + "' and member_role.member_id = '" + current_member_id + "' and member_role.is_active = 'Y' " +
                //                                "and member_master.is_active = 'Y' and session_timing.is_active = 'Y' and session_participants.is_active = 'Y' and course_mst.is_active = 'Y' " +
                //                                "order by session_timing.session_date desc, session_timing.session_start_time desc";

                if (current_member_role == "ST")
                {
                    //                    SqlSelect = @"select session_timing.session_id,DATEDIFF(mi, CONVERT(TIME, session_timing.session_start_time),CONVERT(TIME, session_timing.session_end_time)) as timeDiffInMinute,session_timing.session_venue,session_timing.session_date,session_timing.session_start_time,session_timing.session_end_time,course_mst.course_name,member_master.image,member_master.first_name,member_master.last_name from session_participants 
                    //                                left join session_timing on session_timing.session_id = session_participants.session_id
                    //                                left join member_master on member_master.email_id = session_timing.tutor_id 
                    //                                left join course_mst on course_mst.course_id = session_timing.course_id
                    //                                where session_participants.member_id = '" + current_member_id + "' and session_participants.role_id = '" + current_member_role + "' " +
                    //                                    "-and cast(CAST(session_timing.session_date AS DATE) as varchar)+' '+CONVERT(VARCHAR, session_timing.session_start_time, 108) <= GETDATE() " +
                    //                                    "and session_timing.appointment_confirmed = 'C' and session_timing.is_active = 'Y' " +
                    //                                    "and member_master.is_active = 'Y' and member_master.university = '" + university + "' and session_participants.is_active = 'Y' " +
                    //                                    "and course_mst.is_active = 'Y' order by session_timing.session_date desc,session_start_time desc";


                    sb.Append("   select tutor_location.name,session_timing.session_id,DATEDIFF(mi, CONVERT(TIME, session_timing.session_start_time),CONVERT(TIME, session_timing.session_end_time)) as timeDiffInMinute,session_timing.session_venue,session_timing.session_date,session_timing.session_start_time,session_timing.session_end_time,course_mst.course_name,member_master.image,member_master.first_name,member_master.last_name from session_participants  ");
                    sb.Append("  left join session_timing on session_timing.session_id = session_participants.session_id ");
                    sb.Append(" left join member_master on member_master.email_id = session_timing.tutor_id  ");
                    sb.Append("  left join course_mst on course_mst.course_id = session_timing.course_id ");
                    sb.Append(" left join tutor_location on session_timing.session_venue=tutor_location.doc_no ");
                    sb.Append(" where session_participants.member_id = '" + current_member_id + "' and session_participants.role_id = '" + current_member_role + "' ");
                    sb.Append("     and session_timing.appointment_confirmed = 'C' and session_timing.is_active = 'Y' ");
                    sb.Append(" and member_master.is_active = 'Y' and member_master.university = '" + university + "' and session_participants.is_active = 'Y' ");
                    sb.Append("  and course_mst.is_active = 'Y' order by session_timing.session_date desc,session_start_time desc ");



                }
                else if (current_member_role == "TT")
                {
                    //                    SqlSelect = @"select session_timing.session_id,DATEDIFF(mi, CONVERT(TIME, session_timing.session_start_time),CONVERT(TIME, session_timing.session_end_time)) as timeDiffInMinute,session_timing.session_venue,session_timing.session_date,session_timing.session_start_time,session_timing.session_end_time,course_mst.course_name,member_master.image,member_master.first_name,member_master.last_name from session_participants 
                    //                                left join session_timing on session_timing.session_id = session_participants.session_id
                    //                                left join member_master on member_master.email_id = session_timing.booked_by 
                    //                                left join course_mst on course_mst.course_id = session_timing.course_id
                    //                                where session_participants.member_id = '" + current_member_id + "' and session_participants.role_id = '" + current_member_role + "' " +
                    //                                  "and cast(CAST(session_timing.session_date AS DATE) as varchar)+' '+CONVERT(VARCHAR, session_timing.session_start_time, 108) <= GETDATE() " +
                    //                                   "and session_timing.appointment_confirmed = 'C' and session_timing.is_active = 'Y' " +
                    //                                   "and member_master.is_active = 'Y' and member_master.university = '" + university + "' and session_participants.is_active = 'Y' " +
                    //                                   "and course_mst.is_active = 'Y' order by session_timing.session_date desc,session_start_time desc";


                    sb.Append(" select tutor_location.name,session_timing.session_id,DATEDIFF(mi, CONVERT(TIME, session_timing.session_start_time),CONVERT(TIME, session_timing.session_end_time)) as timeDiffInMinute,session_timing.session_venue,session_timing.session_date,session_timing.session_start_time,session_timing.session_end_time,course_mst.course_name,member_master.image,member_master.first_name,member_master.last_name from session_participants  ");
                    sb.Append(" left join session_timing on session_timing.session_id = session_participants.session_id  ");
                    sb.Append("  left join member_master on member_master.email_id = session_timing.booked_by  ");
                    sb.Append(" left join course_mst on course_mst.course_id = session_timing.course_id ");
                    sb.Append("left join tutor_location on session_timing.session_venue=tutor_location.doc_no ");
                    sb.Append(" where session_participants.member_id = '" + current_member_id + "' and session_participants.role_id = '" + current_member_role + "'  ");
                    sb.Append(" and session_timing.appointment_confirmed = 'C' and session_timing.is_active = 'Y' and member_master.is_active = 'Y' and member_master.university = '" + university + "' and session_participants.is_active = 'Y' and course_mst.is_active = 'Y' order by session_timing.session_date desc,session_start_time desc ");

                }


                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getParticipantFromsessionId(string session_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                //                SqlSelect = @"select member_master.first_name,member_master.last_name,* from session_participants 
                //                                left join member_master on member_master.email_id = session_participants.member_id
                //                                where session_id = '"+session_id+"' and session_participants.is_active = 'Y' and member_master.is_active = 'Y'";

                SqlSelect = @"select session_timing.booked_by,member_master.first_name,member_master.last_name,member_master.image,*,session_participants.role_id from session_participants 
                                left join member_master on member_master.email_id = session_participants.member_id
                                left join session_timing on session_timing.session_id = session_participants.session_id
                                where session_participants.session_id = '" + session_id + "' and session_participants.is_active = 'Y' and member_master.is_active = 'Y'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable Coursenameandid(string name)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append(" select *     from course_mst where course_name='" + name + "' ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetTranscript(string email)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();

                sb.Append(" select * from member_tutor_transprit where member_id ='" + email + "' ");


                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getpushalert(string member_id, string role, string time, string endtime)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("    select *  ");
            sb.Append(" from alertmessage ");
            sb.Append(" left join member_master on alertmessage.from_member=member_master.email_id ");
            sb.Append(" where is_read='N' and is_active='Y' and senddate>=convert(date,cast(GETDATE() as date) ) ");
            sb.Append(" and  sendtime>='" + time + "'  and sendtime<='" + endtime + "'  ");
            sb.Append(" and to_member='" + member_id + "' and to_member_role='" + role + "' ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getCourseWiseTutor(string course, string university)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT avg_value,course_mst.*,course_mst.course_name,member_role.university_id,member_master.first_name,member_master.last_name,member_master.email_id,member_master.image FROM member_role 
                              LEFT JOIN member_master ON member_master.email_id = member_role.member_id and member_master.university = member_role.university_id
                              left join course_mst on course_mst.course_id = member_role.course_code and course_mst.university_id = member_role.university_id
                              left join average_tutor_rating on average_tutor_rating.tutor_id = member_master.email_id and average_tutor_rating.university_id = member_master.university                              
                              WHERE (member_role.role_id = 'TT') AND course_code = '" + course + "' and member_role.university_id = '" + university + "' AND (member_role.is_active = 'Y') AND average_tutor_rating.is_active = 'Y' and member_master.is_active='Y' ";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable feedback_peending(string member_id, string role)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                String SqlSelect = "";
                if (role == "ST")
                {
                    sb.Append("  select * from session_timing  ");
                    sb.Append("  left join tutor_location on session_timing.session_venue=tutor_location.doc_no  ");
                    sb.Append("  left join  course_mst on session_timing.course_id=course_mst.course_id ");
                    sb.Append("  left join member_master on session_timing.tutor_id=member_master.email_id  ");
                    sb.Append("  where booked_by='" + member_id + "' and survey_done_by_student is null and session_timing.appointment_confirmed='C' and member_master.is_active='Y' ");
                }
                else
                {

                    sb.Append("  select * from session_timing  ");
                    sb.Append(" left join tutor_location on session_timing.session_venue=tutor_location.doc_no  ");
                    sb.Append(" left join  course_mst on session_timing.course_id=course_mst.course_id ");
                    sb.Append(" left join member_master on session_timing.booked_by=member_master.email_id ");
                    sb.Append(" where tutor_id='" + member_id + "' and survey_done_by_tutor is null and session_timing.appointment_confirmed='C' and member_master.is_active='Y' ");
                }

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getSessionToGo(string email_id, string university_id, string role)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                //                SqlSelect = @"select member_master.image,member_master.first_name,member_master.last_name,session_timing.session_id,session_timing.start_confirm_student_datetime,session_timing.end_confirm_student_datetime,member_master.email_id from member_master
                //                    left join session_timing on session_timing.university_id = member_master.university and session_timing.session_start_confirm_by_student_id = member_master.email_id
                //                    where email_id = '" +email_id+"' and session_timing.is_active = 'Y' and member_master.is_active = 'Y'"+
                //                    "AND (CAST(GETDATE() AS DATE) >= CAST(session_timing.end_confirm_student_datetime AS DATE) or session_timing.end_confirm_student_datetime is null)and member_master.university = '"+university_id+"' ";

                //                SqlSelect = @"select session_participants.session_id,member_master.email_id,session_participants.course_id from member_master
                //                    left join session_participants on session_participants.university_id = member_master.university and session_participants.member_id = member_master.email_id
                //                    where member_master.email_id = '" + email_id + "' and session_participants.is_active = 'Y' and member_master.is_active = 'Y' and member_master.university = '" + university_id + "' " +
                //                    "and session_participants.is_survey_done_by_student = 'Y' and session_participants.is_survey_done_by_tutor = 'Y'";
                sb.Append("select session_participants.session_id,member_master.email_id,session_participants.course_id from member_master ");
                //--select count(*) from member_master
                sb.Append("  left join session_participants on session_participants.university_id = member_master.university ");
                sb.Append(" and session_participants.member_id = member_master.email_id ");
                sb.Append(" where member_master.email_id = '" + email_id + "' ");
                sb.Append(" and session_participants.is_active = 'Y' ");
                sb.Append(" and member_master.is_active = 'Y' ");
                sb.Append(" and member_master.university = '" + university_id + "'");
                sb.Append(" and session_participants.is_survey_done_by_student = 'Y' ");
                sb.Append(" and session_participants.is_survey_done_by_tutor = 'Y' ");
                sb.Append(" and session_participants.role_id='" + role + "' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getPerticularMemberDetail(string email_id, string university_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select university_master.university_name,member_master.* from member_master
                                left join university_master on  university_master.university_id = member_master.university
                                where member_master.email_id = '" + email_id + "' and university_master.is_active = 'Y' and member_master.is_active = 'Y' and university_master.university_id = '" + university_id + "'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable getStudentLearningCources(string mem_id, string mem_role)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                if (mem_role == "ST")
                    SqlSelect = @"SELECT course_mst.*, member_role.* FROM member_role 
                              LEFT JOIN course_mst ON course_mst.course_id = member_role.course_code
                              WHERE (member_role.member_id = '" + mem_id + "') AND (member_role.is_active = 'Y') AND (member_role.role_id = '" + mem_role + "')";
                if (mem_role == "TT")
                    SqlSelect = @"SELECT course_mst.*, member_role.* FROM member_role 
                              LEFT JOIN course_mst ON course_mst.course_id = member_role.course_code
                              WHERE (member_role.member_id = '" + mem_id + "') AND (member_role.is_active = 'Y') AND  (member_role.is_Approved='Y')  AND  ( approved_by_whome is not null)  AND  (member_role.role_id = '" + mem_role + "')";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable Gettokenuserinfo(string token)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM login_token
                                WHERE token_id='" + token + "'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable tutorandstudentcourselist(string mem_id, string mem_role, string uni)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                //sb.Append(" select * from(  ");
                //sb.Append("        select course_id from course_mst where university_id='" + uni + "' ");
                //sb.Append(" except  ");
                //sb.Append(" select course_code from member_role where university_id='" + uni + "' and member_id='" + mem_id + "' and role_id='" + mem_role + "' ");
                //sb.Append(" ) as t  ");
                //sb.Append(" inner join course_mst on course_mst.course_id=t.course_id");


                sb.Append(" select * from(  ");
                sb.Append("        select course_id from course_mst where university_id='" + uni + "' ");
                sb.Append(" except  ");
                sb.Append(" select course_code from member_role where university_id='" + uni + "' and member_id='" + mem_id + "'  ");
                sb.Append(" ) as t  ");
                sb.Append(" inner join course_mst on course_mst.course_id=t.course_id");
                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //22-4-2016
        public DataTable getAllSororities()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM Sororities_mst
                              WHERE (is_active = 'Y')  ";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //22-4-2016
        public DataTable getAllFraternitiesAndSororities()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM fraternity_sorority_mst
                                where is_active = 'Y'  ";


                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable securityquestion()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @" select * from security_question_mst where is_active='Y' ";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetMemberMstData(string emailID, string randomNo)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM member_master
                                WHERE (email_id = '" + emailID + "') AND (email_rand_no = '" + randomNo + "')";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getAllUniversity()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM university_master
                              WHERE (is_active = 'Y') order by university_id ";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("name");

                    dt.Rows.Add(ex.ToString());
                    dt.Rows.Add(DBDataAdpterObject.SelectCommand.Connection.ConnectionString);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetPerticularUniDetails(string uniId)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM university_master
                              WHERE (is_active = 'Y') and university_id = '" + uniId + "' order by university_id ";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //do not copy it use only for profile link 
        public DataTable LoginCheckProfilecreation(string emailID)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM member_master 
                              WHERE (is_active = 'N') AND (email_id = '" + emailID + "')";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable LoginCheck(string emailID)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM member_master 
                              WHERE  (email_id ='" + emailID + "') AND (is_active='Y')";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable logincheck_new(string emailID)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM member_master 
                              WHERE  (email_id ='" + emailID + "') ";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable checkmemberrole(string emailID)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select * from member_role where member_id='" + emailID + "'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetMemberTrspritData(string emailID)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select * from member_tutor_transprit where member_id = '" + emailID + "' and is_active = 'Y'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetMemberDataFromEmailId(string emailID)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM member_master
                                WHERE (email_id = '" + emailID + "') ";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetTokenId(string emailID)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT token_id FROM member_master
                                WHERE (email_id = '" + emailID + "')";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable user_for_set_appointment(string emailID, string course)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();

                sb.Append(" select ");
                sb.Append(" * from member_master ");
                sb.Append(" left join university_master on  member_master.university=university_master.university_id  ");
                sb.Append("left join  course_mst on university_master.university_id=course_mst.university_id ");
                sb.Append(" where member_master.email_id='" + emailID + "'  and course_mst.course_id='" + course + "'");
                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //22/4/16
        public DataTable getCourses(string UniId)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM course_mst
                            WHERE (university_id = '" + UniId + "')";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //25-4-2016
        public DataTable getuserinfo(string email)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT *  FROM member_master where email_id='" + email + "'  ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();
                // DBDataAdpterObject.SelectCommand.Parameters.Add(BLGeneralUtil.MakeParameter("@email", DbType.String, email));

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getAlluserdetai()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("select member_master.*, university_master.university_name from member_master left join university_master on member_master.university=university_master.university_id");
                //sb.Append(" FROM     member_master ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getAllFarSoro()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from  fraternity_sorority_mst ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getsession_use_rating(string session_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from  session_timing where session_id='" + session_id + "' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getsessiondetails(string session_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                //                sb.Append("select * from session_timing where session_id='" + session_id + "'" +
                //                           @"and start_confirm_student_datetime is not null and start_confirm_tutor_datetime is not null 
                //                            and end_confirm_student_datetime is null and end_confirm_tutor_datetime is null 
                //                            and start_stop='started'");

                //                sb.Append("select * from session_timing where session_id='" + session_id + "'" +
                //                      @"and start_confirm_student_datetime is not null  
                //                            and end_confirm_student_datetime is null and end_confirm_tutor_datetime is null 
                //                            and start_stop='started'");

                //change new jaimin before account time of account
                //sb.Append("select * from session_timing where session_id='" + session_id + "'");

                sb.Append("  select * from session_timing  ");
                sb.Append(" left join member_master on session_timing.booked_by=member_master.email_id ");
                sb.Append(" left join university_master on member_master.university=university_master.university_id ");
                sb.Append(" where session_id='" + session_id + "' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable tokentrationdata(string member_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                //                sb.Append("select * from session_timing where session_id='" + session_id + "'" +
                //                           @"and start_confirm_student_datetime is not null and start_confirm_tutor_datetime is not null 
                //                            and end_confirm_student_datetime is null and end_confirm_tutor_datetime is null 
                //                            and start_stop='started'");

                //                sb.Append("select * from session_timing where session_id='" + session_id + "'" +
                //                      @"and start_confirm_student_datetime is not null  
                //                            and end_confirm_student_datetime is null and end_confirm_tutor_datetime is null 
                //                            and start_stop='started'");


                //sb.Append("select * ,convert( varchar(100),doc_date ,101)as doc_date_re from fn_token_transtion where member_id='" + member_id + "' ");
                sb.Append("select * ,convert( varchar(100),doc_date ,101)as doc_date_re from fn_token_transtion where member_id='" + member_id + "' order by cast(doc_no as int)desc ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable Getsearch(string courser_id, string rate, string name, string uni, string member_id, string member_role)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("  SELECT    average_tutor_rating.avg_value,  course_mst.*,member_master.member_code,member_master.university, member_master.first_name,member_master.last_name,member_master.middle_name, member_master.nick_name, member_master.image,member_master. email_id,");
            sb.Append(" member_master.tutor_rating_id ");

            sb.Append(" FROM         member_master ");
            sb.Append(" left  join  member_role on member_master.email_id=member_role.member_id ");
            sb.Append(" left join   course_mst on  member_role.course_code=course_mst.course_id ");
            sb.Append(" left join average_tutor_rating on member_master.email_id=average_tutor_rating.tutor_id ");

            sb.Append(" where member_role.role_id='TT' ");
            sb.Append(" and member_master.is_active='Y' and  course_mst.is_active='Y'  and  member_role.is_Approved='Y' and approved_by_whome is not null   and member_master.availability='Y' ");

            if (courser_id != null && courser_id != "")
            {

                sb.Append(" and course_mst.course_id='" + courser_id + "' ");
            }
            else
            {
                DataTable dtStuLearningCourses = new DataTable();
                dtStuLearningCourses = getStudentLearningCources(member_id, member_role);
                if (dtStuLearningCourses != null)
                {
                    sb.Append(" and (course_mst.course_id='" + dtStuLearningCourses.Rows[0]["course_code"].ToString() + "' ");
                    for (int i = 1; i < dtStuLearningCourses.Rows.Count; i++)
                    {
                        sb.Append(" or course_mst.course_id='" + dtStuLearningCourses.Rows[i]["course_code"].ToString() + "' ");
                    }
                    sb.Append(" )");
                }


            }
            if (name != null && name != "")
            {
                sb.Append(" and member_master.first_name in('" + name + "')");

            }
            if (uni != null && uni != "")
            {

                sb.Append(" and member_master.university='" + uni + "'");
            }


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();
            //DBDataAdpterObject.SelectCommand.Parameters.Add(BLGeneralUtil.MakeParameter("@email", DbType.String, email));

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //11-5-2016
        //public DataTable Getsearch(string courser_id, string rate, string name, string uni)
        //{

        //    DBDataAdpterObject.SelectCommand.Parameters.Clear();
        //    StringBuilder sb = new StringBuilder();


        //    sb.Append("  SELECT    average_tutor_rating.avg_value,  course_mst.*,member_master.member_code,member_master.university, member_master.first_name,member_master.last_name,member_master.middle_name, member_master.nick_name, member_master.image,member_master. email_id,");
        //    sb.Append(" member_master.tutor_rating_id ");

        //    sb.Append(" FROM         member_master ");
        //    sb.Append(" left  join  member_role on member_master.email_id=member_role.member_id ");
        //    sb.Append(" left join   course_mst on  member_role.course_code=course_mst.course_id ");
        //    sb.Append(" left join average_tutor_rating on member_master.email_id=average_tutor_rating.tutor_id ");

        //    sb.Append(" where member_role.role_id='TT' ");
        //    sb.Append(" and member_master.is_active='Y' and  course_mst.is_active='Y'  and  member_role.is_Approved='Y' and approved_by_whome is not null   and member_master.availability='Y' ");

        //    if (courser_id != null && courser_id != "")
        //    {

        //        sb.Append(" and course_mst.course_id='" + courser_id + "' ");
        //    }
        //    if (name != null && name != "")
        //    {
        //        sb.Append(" and member_master.first_name in('" + name + "')");

        //    }
        //    if (uni != null && uni != "")
        //    {

        //        sb.Append(" and member_master.university='" + uni + "'");
        //    }


        //    DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();
        //    //DBDataAdpterObject.SelectCommand.Parameters.Add(BLGeneralUtil.MakeParameter("@email", DbType.String, email));

        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        DBDataAdpterObject.Fill(ds);
        //        if (ds.Tables[0].Rows.Count <= 0)
        //            return null;
        //        else
        //            return ds.Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public DataTable getuserdetai_byiphone(string email)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("   select * from member_master where email_id='" + email + "' and is_active='Y' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();
                DBDataAdpterObject.SelectCommand.Parameters.Add(BLGeneralUtil.MakeParameter("@email", DbType.String, email));

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getuserdetai(string email)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT * ");
                sb.Append(" FROM     member_master ");
                sb.Append("where email_id=@email ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();
                DBDataAdpterObject.SelectCommand.Parameters.Add(BLGeneralUtil.MakeParameter("@email", DbType.String, email));

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getreedemcheck(string email, string amt)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from member_master  ");
                sb.Append("  left join fn_token_balance on member_master.email_id=fn_token_balance.member_id  ");
                sb.Append("  where member_master.email_id='" + email + "'and  fn_token_balance.balance_token>='" + amt + "' ");


                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();
                DBDataAdpterObject.SelectCommand.Parameters.Add(BLGeneralUtil.MakeParameter("@email", DbType.String, email));

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getCoursesUser(string email)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                //                SqlSelect = @"SELECT * FROM    member_role
                //
                //                            WHERE (member_id = '" + email + "')";

                sb.Append("  SELECT * FROM    member_role  ");
                sb.Append(" left join  course_mst on member_role.course_code=course_mst.course_id ");
                sb.Append(" WHERE (member_role.member_id = '" + email + "') and member_role.is_active='Y' ");


                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getCoursesUser_new_delete(string email)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                //                SqlSelect = @"SELECT * FROM    member_role
                //
                //                            WHERE (member_id = '" + email + "')";

                sb.Append("  SELECT * FROM    member_role  ");
                sb.Append(" left join  course_mst on member_role.course_code=course_mst.course_id ");
                sb.Append(" WHERE (member_role.member_id = '" + email + "') and member_role.is_active='Y' and role_id='ST'");


                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable Schedulememberdetail(string email)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("   select  average_tutor_rating.avg_value, course_mst.course_name, member_role.*,member_master.* from member_role ");
            sb.Append(" left join course_mst on member_role.course_code=course_mst.course_id ");
            sb.Append("left join member_master on member_role.member_id=member_master.email_id ");
            sb.Append(" left join average_tutor_rating on member_role.member_id=average_tutor_rating.tutor_id ");
            sb.Append(" where member_id='" + email + "' and role_id='TT' and member_master.is_active='Y' ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable Schedulememberdetail_course(string email, string course_id)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("   select course_mst.course_name, member_role.*,member_master.* from member_role ");
            sb.Append(" left join course_mst on member_role.course_code=course_mst.course_id ");
            sb.Append("left join member_master on member_role.member_id=member_master.email_id ");
            sb.Append(" where member_id='" + email + "' and role_id='TT'  and member_role.course_code='" + course_id + "'");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable token_bal(string email)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append(" select * from fn_token_balance where member_id='" + email + "' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //17-5-16
        public DataTable feedback(string email, string course)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("  select member_master.first_name,member_master.last_name,CAST(session_timing.session_date as date),member_master.image,session_timing.course_id,session_survey_for_tutors.* from session_survey_for_tutors ");
            sb.Append(" left join session_timing on session_survey_for_tutors.session_id=session_timing.session_id ");
            sb.Append(" left join  member_master on session_survey_for_tutors.student_id=member_master.email_id ");
            sb.Append(" where session_survey_for_tutors.tutor_id='" + email + "' and session_timing.course_id='" + course + "' and session_survey_for_tutors.is_active='Y' and member_master.is_active='Y' and session_survey_for_tutors.Feedback is not null   ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable tutor_courselist(string email)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("select * from member_role where member_id='" + email + "' and role_id='TT' ");
            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable studnetfeedback(string email, string course, string session)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("  select member_master.first_name,CAST(session_timing.session_date as date),member_master.image, ");
            sb.Append(" session_timing.course_id,session_survey_for_students.*,course_mst.course_name from session_survey_for_students  ");
            sb.Append(" left join session_timing on session_survey_for_students.session_id=session_timing.session_id  ");
            sb.Append(" left join  member_master on session_survey_for_students.tutor_id=member_master.email_id  ");
            sb.Append(" left join course_mst on   session_timing.course_id=course_mst.course_id ");
            sb.Append("  where session_survey_for_students.session_id='" + session + "' and session_survey_for_students.Feedback is not null ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable profiledatastudent(string session_id)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("select course_mst.course_name,session_timing.*,member_master.*,average_student_rating.*  from session_timing ");
            sb.Append(" left join member_master on session_timing.booked_by=member_master.email_id ");
            sb.Append(" left join member_role on session_timing.booked_by=member_role.member_id ");
            sb.Append(" left join course_mst on member_role.course_code=course_mst.course_id");
            sb.Append(" left join average_student_rating on member_master.email_id=average_student_rating.student_id ");
            sb.Append(" where session_id='" + session_id + "' and member_role.role_id='ST'");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable checkforconfirmation(string doc_no, string notification_type)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select is_accept from notification where doc_no = '" + doc_no + "' and _event = '" + notification_type + "' " +
                                "and is_accept = 'Y' and is_read = 'Y' and is_active = 'Y'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getNotificationFromDocNo(string doc_no)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select * from notification where doc_no = '" + doc_no + "'" +
                                " and is_accept = 'N' and is_read = 'N' and is_active = 'Y'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getalertfromdocno(string doc_no)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from alertmessage where doc_no='" + doc_no + "' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable session_friendlist(string sessionid, string member_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from  session_participants where session_id='" + sessionid + "' and role_id='ST' and member_id !='" + member_id + "'");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();
                //  DBDataAdpterObject.SelectCommand.Parameters.Add(BLGeneralUtil.MakeParameter("@email", DbType.String, email));

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable courslist(string uni_id)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from course_mst where university_id='" + uni_id + "' and is_active='Y' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable sessiondata(string sessionid)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from session_timing where session_id='" + sessionid + "' ");



            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getPerticularSessionDetail(string session_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                String SqlSelect = "";

                //SqlSelect = @"select * from session_timing where session_id = '" + session_id + "' and is_active = 'Y'";
                sb.Append("     select *  ");
                sb.Append("  from session_timing  ");
                sb.Append("  left join member_master on session_timing.booked_by=member_master.email_id ");
                sb.Append("  left join university_master on member_master.university=university_master.university_id ");
                sb.Append("  where session_timing.session_id = '" + session_id + "' and session_timing.is_active = 'Y' and member_master.is_active='Y' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable getDataToRateTutor(string current_member_id, string current_member_role, string current_university)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select course_mst.course_name,member_master.image,member_master.first_name,member_master.last_name,session_timing.session_id,session_timing.tutor_id,session_timing.course_id from session_timing
                                left join session_participants on session_participants.session_id = session_timing.session_id
                                left join member_master on member_master.email_id = session_timing.tutor_id and member_master.university = session_timing.university_id
                                left join course_mst on course_mst.course_id = session_timing.course_id and course_mst.university_id = session_timing.university_id
                                where session_participants.member_id ='" + current_member_id + "' and role_id = '" + current_member_role + "' and " +
                                "session_participants.session_id not in (select session_id from session_survey_for_tutors where student_id = '" + current_member_id + "' and is_active = 'Y') " +
                                "and member_master.is_active = 'Y' and course_mst.is_active = 'Y' " +
                                "and session_participants.is_active = 'Y' and session_timing.is_active = 'Y' " +
                                "and session_timing.university_id = '" + current_university + "'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable calculateRatingSummary(string student_id, string university)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select student_id,avg(rating_parameter_value) as avgRate,TotCount,latest_session from 
                            (
	                            select top 5 tutor_id,student_id,rating_parameter_value, " +
                                "(select Count(*) from session_survey_for_students where (student_id = '" + student_id + "') and (is_active = 'Y')) as TotCount, " +
                                "(select top 1 session_id from session_survey_for_students where (student_id = '" + student_id + "') and (is_active = 'Y') order by created_date desc  ) as latest_session	" +
                                "from session_survey_for_students " +
                                "WHERE (student_id = '" + student_id + "') and (is_active = 'Y') and university_id = '" + university + "' " +
                                "order by created_date desc " +
                            ") as temp_table " +
                            "group by student_id,TotCount,latest_session";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //6/2/16
        public DataTable calculateRatingSummaryForTutor(string tutor_id, string university)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select tutor_id,avg(rating_parameter_value) as avgRate,TotCount,latest_session from 
                            (
	                            select top 5 tutor_id,student_id,rating_parameter_value, " +
                                "(select Count(*) from session_survey_for_tutors where (tutor_id = '" + tutor_id + "') and (is_active = 'Y')) as TotCount, " +
                                "(select top 1 session_id from session_survey_for_students where (tutor_id = '" + tutor_id + "') and (is_active = 'Y') order by created_date desc  ) as latest_session	" +
                                "from session_survey_for_tutors " +
                                "WHERE (tutor_id = '" + tutor_id + "') and (is_active = 'Y') and university_id = '" + university + "' " +
                                "order by created_date desc " +
                            ") as temp_table " +
                            "group by tutor_id,TotCount,latest_session";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        public DataTable getAvgRateSummaryForStudent(string student_id, string university)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from average_student_rating  ");
                sb.Append(" left join member_master on average_student_rating.student_id=member_master.email_id ");
                sb.Append("where student_id = '" + student_id + "' and university_id = '" + university + "'");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable getAvgRateSummaryForTutor(string student_id, string university)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                //SqlSelect = @"select * from average_tutor_rating where tutor_id = '" + student_id + "' and university_id = '" + university + "'";

                sb.Append(" select * from average_tutor_rating ");
                sb.Append(" left join member_master on average_tutor_rating.tutor_id=member_master.email_id ");
                sb.Append("where tutor_id = '" + student_id + "' and university_id = '" + university + "' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //dont copy this method
        public DataTable getSessionRate(string selected_date, string session_type, string course_type, string tutor_id, string university_id, string stu_rate_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();

                //                SqlSelect = @"select tutoring_rate_mst.course_type,tutoring_rate_mst.session_type,tutoring_rate_mst.amount,peak_rate_master.peak_rate_percentage,tutoring_rate_mst.tutor_id from tutoring_rate_mst
                //                                left join peak_rate_master on tutoring_rate_mst.course_type = peak_rate_master.course_type and tutoring_rate_mst.university_id = peak_rate_master.university_id " +
                //                                "where CAST(peak_period_start_date AS DATE) <= CAST ('" + selected_date + "' AS DATE) and CAST(peak_period_end_date AS DATE) >= CAST ('" + selected_date + "' AS DATE) " +
                //                                "and peak_rate_master.course_type = '" + course_type + "' " +
                //                                "and tutoring_rate_mst.session_type = '" + session_type + "' " +
                //                                "and tutoring_rate_mst.is_active = 'Y' and peak_rate_master.is_active = 'Y'";

                //sb.Append("  select tutoring_rate_mst.course_type,tutoring_rate_mst.session_type, ");
                //sb.Append(" tutoring_rate_mst.amount,peak_rate_master.peak_rate_percentage,tutoring_rate_mst.tutor_id, ");
                //sb.Append(" discount_rate_mst.discount_percentage , ");
                //sb.Append(" ((tutoring_rate_mst.amount+peak_rate_master.peak_rate_percentage)-discount_rate_mst.discount_percentage) as SumData ");
                //sb.Append(" from tutoring_rate_mst ");
                //sb.Append("  left join peak_rate_master on tutoring_rate_mst.course_type = peak_rate_master.course_type and tutoring_rate_mst.university_id = peak_rate_master.university_id  ");
                //sb.Append("  left join discount_rate_mst on tutoring_rate_mst.university_id=discount_rate_mst.university ");
                //sb.Append("  where CAST(peak_period_start_date AS DATE) <= CAST ('" + selected_date + "' AS DATE) ");
                //sb.Append("  and CAST(peak_period_end_date AS DATE) >= CAST ('" + selected_date + " ' AS DATE) ");
                //sb.Append("  and peak_rate_master.course_type = '" + course_type + "'  ");
                ////sb.Append("  and tutoring_rate_mst.session_type = '" + session_type + "' ");
                //sb.Append("  or discount_rate_mst.student_rating_id='"+ stu_rate_id+"' ");
                //sb.Append("  and tutoring_rate_mst.is_active = 'Y' and peak_rate_master.is_active = 'Y' ");
                //sb.Append("  and tutoring_rate_mst.university_id='"+university_id +"' ");
                //sb.Append(" if EXISTS(select * from tutor_guru_rate_mst where tutor_id='" + tutor_id + "') ");
                //sb.Append(" begin ");

                //sb.Append("  select *,'' AS peak_rate_percentage,'' AS discount_percentage from tutor_guru_rate_mst where tutor_id='" + tutor_id + "' ");

                //sb.Append(" end ");
                //sb.Append(" else  Begin  ");
                //sb.Append("  select tutoring_rate_mst.course_type,tutoring_rate_mst.session_type, ");
                //sb.Append("   tutoring_rate_mst.amount,peak_rate_master.peak_rate_percentage,tutoring_rate_mst.tutor_id, ");
                //sb.Append("  discount_rate_mst.discount_percentage ,  ");
                //sb.Append(" ((tutoring_rate_mst.amount+peak_rate_master.peak_rate_percentage)-discount_rate_mst.discount_percentage) ");
                //sb.Append("    as SumData  , ");
                //sb.Append("    discount_rate_mst.student_rating_id ");

                //sb.Append("  from tutoring_rate_mst   ");
                //sb.Append("  left join peak_rate_master on tutoring_rate_mst.course_type = peak_rate_master.course_type ");
                //sb.Append("   and tutoring_rate_mst.university_id = peak_rate_master.university_id   ");
                //sb.Append("   left join discount_rate_mst on tutoring_rate_mst.university_id=discount_rate_mst.university  ");
                //sb.Append("   where (tutoring_rate_mst.is_active = 'Y' and tutoring_rate_mst.university_id='" + university_id + "' ");
                //sb.Append("  and  tutoring_rate_mst.course_type='" + course_type + "') ");
                //sb.Append("   and (peak_period_start_date is null or CAST(peak_period_start_date AS DATE) <= CAST ('" + selected_date + "' AS DATE))   ");
                //sb.Append(" and (peak_period_end_date is null or CAST(peak_period_end_date AS DATE) >= CAST ('" + selected_date + " ' AS DATE))   ");
                //sb.Append("  and (peak_rate_master.course_type is null or peak_rate_master.course_type = '" + course_type + "'  ) ");
                //sb.Append("     and (peak_rate_master.is_active is null or peak_rate_master.is_active = 'Y'  ) ");
                //sb.Append("  and (discount_rate_mst.student_rating_id is null or discount_rate_mst.student_rating_id='" + stu_rate_id + "'  ) ");
                //sb.Append(" and (discount_rate_mst.university is null or discount_rate_mst.university='" + university_id + "') ");

                //sb.Append(" END ");
                sb.Append("   if EXISTS(select * from tutor_guru_rate_mst where tutor_id='oza.kaushal@gmail.com') ");
                sb.Append(" begin ");
                sb.Append("  select *,'' AS peak_rate_percentage,'' AS discount_percentage from tutor_guru_rate_mst where tutor_id='oza.kaushal@gmail.com' ");
                sb.Append("  end  ");
                sb.Append("  else begin ");
                sb.Append("  select tutoring_rate_mst.course_type,tutoring_rate_mst.session_type,tutoring_rate_mst.amount,tutoring_rate_mst.tutor_id, ");
                sb.Append("  ISNULL((select peak_rate_percentage from peak_rate_master where university_id='" + university_id + "' and course_type=' " + course_type + "' and  ");
                sb.Append("		is_active = 'Y' and (CAST ('" + selected_date + "' AS DATE) between CAST(peak_period_start_date AS DATE) and   ");
                sb.Append("		 CAST(peak_period_end_date AS DATE))),null) as peak_rate_percentage,  ");
                sb.Append("	 ISNULL((select discount_percentage from discount_rate_mst where university='" + university_id + "' and student_rating_id='" + stu_rate_id + "' and is_active = 'Y'),null) as discount_percentage ");
                sb.Append("	 from tutoring_rate_mst  ");
                sb.Append("	 where (tutoring_rate_mst.is_active = 'Y' and tutoring_rate_mst.university_id='" + university_id + "' ");
                sb.Append("	 and  tutoring_rate_mst.course_type='" + course_type + "')  ");
                sb.Append("   end ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                    {
                        return ds.Tables[0];
                        //DataRow[] dr = ds.Tables[0].Select("tutor_id = '" + tutor_id.Trim() + "'");
                        //if (dr != null && dr.Length > 0)
                        //{
                        //    DataTable dt = ds.Tables[0].Clone();
                        //    dt.ImportRow(dr[0]);
                        //    return dt;
                        //}
                        //else
                        //{
                        //    DataRow[] dr1 = ds.Tables[0].Select("tutor_id = '0'");
                        //    DataTable dt = ds.Tables[0].Clone();
                        //    dt.ImportRow(dr1[0]);
                        //    return dt;
                        //}
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable PieChartData(string email)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                sb.Append("select sum(amount) as Total_Tokens,balance_token from fn_token_transtion inner join fn_token_balance on  fn_token_transtion.member_id=fn_token_balance.member_id where fn_token_transtion.member_id='" + email + "' and type_credit_debit='debit'  Group by balance_token");
                //sb.Append(" left join member_master on member_role.member_id=member_master.email_id ");
                //sb.Append(" where university_id='" + UniId + "' and role_id='TT' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable chatdata(string to_member_id, string from_member_id, string session_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();

                sb.Append(" select t.*,member_master.first_name, member_master.image ");
                sb.Append("from( ");
                sb.Append(" select * from chating where to_member='" + to_member_id + "' and from_member='" + from_member_id + "' and divas_type='" + session_id + "' ");
                sb.Append(" UNION ALL ");
                sb.Append(" select * from chating where to_member='" + from_member_id + "' and from_member='" + to_member_id + "' and divas_type='" + session_id + "' ");
                sb.Append(") as t ");
                sb.Append(" inner join member_master on t.from_member=member_master.email_id  ");
                sb.Append("  where member_master.is_active='Y'  ORDER BY  created_date,chat_id   ");


                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable chatwithtutorlist(string email_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();


                sb.Append(" select distinct member_master.first_name,chating.from_member from chating ");
                sb.Append(" left join member_master on chating.from_member=member_master.email_id ");
                sb.Append(" where to_member='" + email_id + "' and chating.to_member_role='TT' ");


                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable tutordetail(string srno)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from member_role where sr_no='" + srno + "' ");
                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable tutorlistbyadmin(string email)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(" select member_tutor_transprit.file_path,university_master.university_name,course_mst.course_name,member_master.first_name,member_master.last_name,member_master.image,member_role.* from member_role ");
                sb.Append(" left join member_master on member_role.member_id=member_master.email_id ");
                sb.Append("left join course_mst on member_role.course_code=course_mst.course_id ");
                sb.Append(" left join university_master on member_role.university_id=university_master.university_id ");
                sb.Append("  left join member_tutor_transprit on member_role.member_id=member_tutor_transprit.member_id ");
                sb.Append(" where member_role.is_active='Y' and member_role.role_id='TT' and member_role.is_approved='N' and member_role.approved_by_whome is null and member_role.member_id='" + email + "' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable applytobetutorlist()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                //sb.Append("select member_master.first_name,member_master.last_name,member_role.* from member_role ");
                //sb.Append(" left join member_master on member_role.member_id=member_master.email_id   ");
                //sb.Append(" where member_role.is_active='N' and member_role.role_id='TT'  ");


                sb.Append("  select DISTINCT member_role.member_id,member_master.first_name,member_master.last_name from member_role ");
                sb.Append(" join member_master on member_role.member_id=member_master.email_id   ");
                sb.Append(" where member_role.is_active='Y' and member_role.role_id='TT' and member_role.is_approved='N'  ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //4/6/16
        public DataTable getUniversitywiseCourseType(string university)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select DISTINCT(course_type) from course_mst where university_id = '" + university + "' and is_active = 'Y'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //4/6/16
        public DataTable getStandardRateList()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select university_master.university_name,tutoring_rate_mst.* from tutoring_rate_mst 
                                left join university_master on tutoring_rate_mst.university_id = university_master.university_id
                                where university_master.is_active = 'Y'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getcourse_type_mster()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select * from  course_type_mst";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //6-7-2016 // mounika
        public DataTable peak_rate_master_detail()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select peak_rate_master.*,university_master.university_name, course_type_mst.Description,CONVERT(VARCHAR(10),peak_period_start_date,110) as shortdate1, CONVERT(VARCHAR(10),peak_period_end_date,110) as shortdate from peak_rate_master left join university_master on peak_rate_master.university_id=university_master.university_id left join course_type_mst on peak_rate_master.course_type=course_type_mst.doc_id ";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //4/6/16
        public DataTable getPerticularStandardRate(string sr_no)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select * from tutoring_rate_mst where sr_no = '" + sr_no + "' and is_active = 'Y'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //6-6-2016
        public DataTable getuniversitymaster()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select * from university_master where is_active='Y'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable getuniversitymaster(string uni_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select * from university_master where is_active='Y' and university_id='" + uni_id + "' ";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getAllFarSoro(string id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from  fraternity_sorority_mst where fra_soro_id='" + id + "' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getpeekreatedetail(string id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from peak_rate_master where sr_no='" + id + "' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getcourse_type_mst(string id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from course_type_mst where doc_id='" + id + "'");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable studentrate_id()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("select discount_rate_mst.*,university_master.university_name,rating_master_for_student.rating_id_desc from discount_rate_mst ");
                sb.Append(" left join university_master on discount_rate_mst.university=university_master.university_id left join rating_master_for_student on  discount_rate_mst.student_rating_id=rating_master_for_student.rating_id_code");
                //sb.Append("left join rating_master_for_student on  discount_rate_mst.student_rating_id=rating_master_for_student.rating_id_code");
                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable studentcourserate()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from rating_master_for_student ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable NotConfirmedSessionDetails(string role, string email)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            if (role == "TT")
                sb.Append(" select * from session_timing left join member_master on session_timing.booked_by=member_master.email_id left join tutor_location on session_timing.session_venue=tutor_location.doc_no left join course_mst  on session_timing.course_id=course_mst.course_id  where session_timing.appointment_confirmed='N' and session_timing.is_active='Y'   and  member_master.is_active='Y'  and session_timing.tutor_id='" + email + "'  order by session_timing.created_date desc ");

            else if (role == "ST")
                sb.Append(" select * from session_timing left join member_master on session_timing.tutor_id=member_master.email_id left join tutor_location on session_timing.session_venue=tutor_location.doc_no left join course_mst  on session_timing.course_id=course_mst.course_id  where session_timing.appointment_confirmed='N'  and  member_master.is_active='Y'  and session_timing.is_active='Y' and  session_timing.booked_by='" + email + "'  order by session_timing.created_date desc ");
            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getcourse_type()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from course_type_mst where is_active='Y'   ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getcourse_type_new(string uni)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from course_type_mst where is_active='Y' and University_id='" + uni + "'   ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //public DataTable getdisocuntrate(string id)
        //{
        //    try
        //    {
        //        DBDataAdpterObject.SelectCommand.Parameters.Clear();
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("select * from discount_rate_mst where course_id='" + id + "'  ");

        //        DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            DBDataAdpterObject.Fill(ds);
        //            if (ds.Tables[0].Rows.Count <= 0)
        //                return null;
        //            else
        //                return ds.Tables[0];
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        public DataTable getdisocuntrate(string rateid, string universityid)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from discount_rate_mst where student_rating_id= '" + rateid + "' and university= '" + universityid + "'");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getcoursemaster(string id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from course_mst where course_id='" + id + "'  ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getcoursemaster()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("select course_mst.*, university_master.university_name, course_type_mst.Description from course_mst left join university_master on course_mst.university_id=university_master.university_id left join course_type_mst on course_mst.course_type=course_type_mst.doc_id  ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable empolyeedata()
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from testData");
            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();



            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getseeionbooking(string sessionid)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from fn_session_booking where session_id='" + sessionid + "'  ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //stripe only dont copy

        public DataTable getsessionpartiscptiondetail(string session, string role)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from session_participants where session_id='" + session + "' and role_id='" + role + "'  ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getsessionpartiscption_for_alert(string session)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from session_participants where session_id='" + session + "' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getStripe(string id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select * from fn_stripe_management where payment_id='" + id + "' and is_active='Y' ";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable gettokenbal(string id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select * from fn_token_balance where member_id='" + id + "' ";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable checkuseramount_for_session(decimal amount, string email)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"select * from fn_token_balance where balance_token>=" + amount + " and member_id='" + email + "'";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable BarGraphData(string email)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            //sb.Append("   select  average_tutor_rating.avg_value, course_mst.course_name, member_role.*,member_master.* from member_role ");
            //sb.Append(" left join course_mst on member_role.course_code=course_mst.course_id ");
            //sb.Append("left join member_master on member_role.member_id=member_master.email_id ");
            //sb.Append(" left join average_tutor_rating on member_role.member_id=average_tutor_rating.tutor_id ");
            //sb.Append(" where member_id='" + email + "' and role_id='TT' ");

            // sb.Append("SELECT Yaxis, Convert(varchar(100),DATEADD (wk, DATEDIFF(wk, 6, '1/1/' + YearNum) + (WeekNum-1), 6),6) +'-'+ Convert(varchar(100),DATEADD (wk, DATEDIFF(wk, 5, '1/1/' + YearNum) + (WeekNum-1), 5) ,6)as WeekDates FROM( select Sum(amount) as Yaxis, cast(DATEPART (wk,doc_date) as varchar(50)) as WeekNum, cast (DATEPART (yy,doc_date) as varchar(50)) as YearNum from fn_token_transtion WHERE member_id='" + email + "' and type_credit_debit='credit' group by DATEPART (wk,doc_date), DATEPART (yy,doc_date)) as StartOfWeek");
            sb.Append("  SELECT Yaxis, case ");
            sb.Append(" When DATEPart(mm,DATEADD (wk, DATEDIFF(wk, 6, '1/1/' + YearNum)+ (WeekNum-1), 6))=DATEPART(MM,GETDATE()) Then Convert(varchar(100),DATEADD (wk, DATEDIFF(wk, 6, '1/1/' + YearNum) + (WeekNum-1), 6),6) ");
            sb.Append(" else CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(GETDATE())-1),GETDATE()),6) ");
            sb.Append(" END +'-'+ Convert(varchar(100), ");
            sb.Append(" DATEADD (wk, DATEDIFF(wk, 5, '1/1/' + YearNum) + (WeekNum-1), 5) ,6)as WeekDates  ");
            sb.Append(" FROM( select Sum(amount) as Yaxis, cast(DATEPART (wk,doc_date) as varchar(50)) as WeekNum,  ");
            sb.Append(" cast (DATEPART (yy,doc_date) as varchar(50))  ");
            sb.Append(" as YearNum from fn_token_transtion ");
            sb.Append("  WHERE member_id='" + email + "'  ");
            sb.Append(" and type_credit_debit='credit' and (fn_token_transtion.doc_date between DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0) and GETDATE()) ");
            sb.Append(" group by DATEPART (wk,doc_date), DATEPART (yy,doc_date) ");
            sb.Append(" ) as StartOfWeek ");
            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable Getuniversityid(string email)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            //sb.Append("   select  average_tutor_rating.avg_value, course_mst.course_name, member_role.*,member_master.* from member_role ");
            //sb.Append(" left join course_mst on member_role.course_code=course_mst.course_id ");
            //sb.Append("left join member_master on member_role.member_id=member_master.email_id ");
            //sb.Append(" left join average_tutor_rating on member_role.member_id=average_tutor_rating.tutor_id ");
            //sb.Append(" where member_id='" + email + "' and role_id='TT' ");

            // sb.Append("SELECT Yaxis, Convert(varchar(100),DATEADD (wk, DATEDIFF(wk, 6, '1/1/' + YearNum) + (WeekNum-1), 6),6) +'-'+ Convert(varchar(100),DATEADD (wk, DATEDIFF(wk, 5, '1/1/' + YearNum) + (WeekNum-1), 5) ,6)as WeekDates FROM( select Sum(amount) as Yaxis, cast(DATEPART (wk,doc_date) as varchar(50)) as WeekNum, cast (DATEPART (yy,doc_date) as varchar(50)) as YearNum from fn_token_transtion WHERE member_id='" + email + "' and type_credit_debit='credit' group by DATEPART (wk,doc_date), DATEPART (yy,doc_date)) as StartOfWeek");


            sb.Append(" SELECT *  FROM member_master where email_id='" + email + "' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable total_income_tutor(string email)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("  select sum(yaxis)as balance_token from(  ");
            sb.Append("   SELECT Yaxis, case  When DATEPart(mm,DATEADD (wk, DATEDIFF(wk, 6, '1/1/' + YearNum)+ (WeekNum-1), 6))=DATEPART(MM,GETDATE()) Then Convert(varchar(100),DATEADD (wk, DATEDIFF(wk, 6, '1/1/' + YearNum) + (WeekNum-1), 6),6)   ");
            sb.Append(" else CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(GETDATE())-1),GETDATE()),6)  END +'-'+ Convert(varchar(100),  DATEADD (wk, DATEDIFF(wk, 5, '1/1/' + YearNum) + (WeekNum-1), 5) ,6)as WeekDates   FROM( select Sum(amount) as Yaxis, cast(DATEPART (wk,doc_date) as varchar(50)) as WeekNum,   cast (DATEPART (yy,doc_date) as varchar(50))   as YearNum from fn_token_transtion   WHERE member_id='" + email + "'   and type_credit_debit='credit' and (fn_token_transtion.doc_date between DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0) and GETDATE())  group by DATEPART (wk,doc_date), DATEPART (yy,doc_date) ");
            sb.Append("   ) as StartOfWeek  ");
            sb.Append("    )as balance_token ");



            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable TotalIncomeGenerated(string email)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("SELECT ISNULL(balance_token,0) AS balance_token from fn_token_balance where member_id='" + email + "' ");



            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetTutorScedule(string email_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                // sb.Append(" select * from  session_timing WHERE     (tutor_id = '"+email_id+"') ");
                //sb.Append("SELECT     TOP (200) session_id, tutor_id, university_id, session_type, course_id, CONVERT(VARCHAR(11), session_date, 111) AS session_date, session_start_time, session_end_time, start_confirm_tutor_datetime, ");
                //sb.Append("start_confirm_student_datetime, end_confirm_tutor_datetime, end_confirm_student_datetime, session_start_confirm_by_tutor_id, ");
                //sb.Append("session_start_confirm_by_host_tutor, session_end_confirm_by_tutor_id, session_end_confirm_by_host_tutor, session_venue, session_start_confirm_by_student_id,");
                //sb.Append("session_start_confirm_by_student_host, session_end_confirm_by_student_id, session_end_confirm_by_student_host, payment_process_status, ");
                //sb.Append("appointment_confirmed, is_active, created_by, created_date, created_host, last_modified_by, last_modified_date, last_modified_host, booked_by, start_stop");
                //sb.Append(" FROM  session_timing WHERE     (tutor_id = '" + email_id + "')");

                sb.Append(" SELECT     session_id, CONVERT(VARCHAR(11), session_date, 111) AS session_date, session_start_time, session_end_time, session_venue, appointment_confirmed ");
                sb.Append(" FROM         session_timing ");
                sb.Append(" where appointment_confirmed not in('C','N') and tutor_id='" + email_id + "' ");
                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetTutorScedule_datewish(string email_id, string date)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                // sb.Append(" select * from  session_timing WHERE     (tutor_id = '"+email_id+"') ");
                //sb.Append("SELECT     TOP (200) session_id, tutor_id, university_id, session_type, course_id, CONVERT(VARCHAR(11), session_date, 111) AS session_date, session_start_time, session_end_time, start_confirm_tutor_datetime, ");
                //sb.Append("start_confirm_student_datetime, end_confirm_tutor_datetime, end_confirm_student_datetime, session_start_confirm_by_tutor_id, ");
                //sb.Append("session_start_confirm_by_host_tutor, session_end_confirm_by_tutor_id, session_end_confirm_by_host_tutor, session_venue, session_start_confirm_by_student_id,");
                //sb.Append("session_start_confirm_by_student_host, session_end_confirm_by_student_id, session_end_confirm_by_student_host, payment_process_status, ");
                //sb.Append("appointment_confirmed, is_active, created_by, created_date, created_host, last_modified_by, last_modified_date, last_modified_host, booked_by, start_stop");
                //sb.Append(" FROM  session_timing WHERE     (tutor_id = '" + email_id + "')");

                sb.Append("   SELECT     session_id, session_date, session_start_time, session_end_time, session_venue, appointment_confirmed ");
                sb.Append(" FROM         session_timing ");
                sb.Append(" where appointment_confirmed not in('C','N') and tutor_id='" + email_id + "' ");
                sb.Append(" and session_date='" + date + "' ");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable gettutorlocation()
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("select *  ");
            sb.Append(" from tutor_location  ");
            sb.Append("left join  university_master on  tutor_location.university_id=university_master.university_id  ");





            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable reedeemrequest()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("select fn_token_reedemption.*,member_master.bank_account_name,member_master.ifsc_code,member_master.account_number,fn_token_balance.balance_token from fn_token_reedemption ");
                sb.Append(" left join member_master on fn_token_reedemption.is_transtion_member_id=member_master.email_id left join fn_token_balance on fn_token_reedemption.is_transtion_member_id=fn_token_balance.member_id");
                sb.Append(" where fn_token_reedemption.payment_status='N' ");
                //sb.Append("left join rating_master_for_student on  discount_rate_mst.student_rating_id=rating_master_for_student.rating_id_code");
                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable gettutorlocation(string univerity)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("select * from  tutor_location where university_id='" + univerity + "'");



            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable custonme_alert(string UniId, string student_achivement, string tutor_achive, string role)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                sb.Append("  select distinct [member_code],[university],[first_name],[last_name],[middle_name],[birthdate],[nick_name],[short_bio],[fun_campus] ");
                sb.Append(" ,[major],[image],[email_id],[phone_number1],[phone_number2],[password],[email_rand_no],[bank_account_name],[ifsc_code],[account_number] ");
                sb.Append(" ,[feternity_id],[soriety_id],[student_rating_id],[tutor_rating_id],[classification],[default_role] ");
                sb.Append(" ,[availability],[security_question],[question_ans] from member_master");
                if (tutor_achive != null && tutor_achive != "")
                    sb.Append("  inner join average_tutor_rating on member_master.email_id=average_tutor_rating.tutor_id ");

                if (student_achivement != null && student_achivement != "")
                    sb.Append(" inner join average_student_rating on member_master.email_id=average_student_rating.student_id ");
                if (role != null && role != "")
                {
                    sb.Append("inner join member_role on member_master.email_id=member_role.member_id ");
                }
                sb.Append("   where 1=1  ");
                sb.Append(" and member_master.is_active='Y' ");
                if (UniId != null && UniId != "")
                    sb.Append("and university='" + UniId + "'  ");

                if (tutor_achive != null && tutor_achive != "")
                    sb.Append(" and  average_tutor_rating.parameter_id='" + tutor_achive + "' ");
                if (student_achivement != null && student_achivement != "")
                    sb.Append(" and  average_student_rating.parameter_id='" + student_achivement + "' ");
                if (role != null && role != "")
                {
                    sb.Append("  and  member_role.role_id='" + role + "' ");
                }

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //12-8-2016
        public Boolean CheckConfirmedSessionRequest(string sessionDate, string StartTime, string EndTime, string email_id)
        {
            try
            {

                string[] formats = { "MM/dd/yyyy" };
                var culture = System.Globalization.CultureInfo.CurrentCulture;

                //DateTime date = DateTime.ParseExact(sessionDate, "dd/mm/yyyy", culture);
                //DateTime sessionStartTime = DateTime.ParseExact(sessionDate + " " + StartTime, "dd/mm/yyyy", culture);
                //DateTime sessionEndTime = DateTime.ParseExact(sessionDate + " " + EndTime, "dd/mm/yyyy", culture);

                DateTime sessionStartTime = Convert.ToDateTime(sessionDate + " " + StartTime);
                DateTime sessionEndTime = Convert.ToDateTime(sessionDate + " " + EndTime);

                //DateTime sessionStartTime = DateTime.ParseExact(String.Format("{0:MM/dd/yyyy HH:mm:ss}", sessionDate + " " + StartTime), "yyyy/MM/dd", CultureInfo.InvariantCulture);
                //DateTime sessionEndTime = DateTime.ParseExact(String.Format("{0:MM/dd/yyyy HH:mm:ss}", sessionDate + " " + EndTime), "yyyy/MM/dd", CultureInfo.InvariantCulture);

                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                StringBuilder sb = new StringBuilder();


                sb.Append("  DECLARE @tempStart datetime='" + sessionStartTime + "' ");
                sb.Append("  DECLARE @tempEND datetime='" + sessionEndTime + "' ");

                sb.Append(" if exists(select * from session_timing where tutor_id='" + email_id + "'  ");
                sb.Append(" and ((@tempStart between CAST(session_date+' '+CONVERT(SmallDateTime, session_start_time) AS datetime)  ");
                sb.Append(" and cast(session_date+' '+CONVERT(SmallDateTime, session_end_time)  AS datetime) ");
                sb.Append(" or @tempEND between CAST(session_date+' '+CONVERT(SmallDateTime, session_start_time) AS datetime)  ");
                sb.Append(" and cast(session_date+' '+CONVERT(SmallDateTime, session_end_time)  AS datetime)) ");
                sb.Append(" or((CAST(session_date+' '+CONVERT(SmallDateTime, session_start_time) AS datetime) between @tempStart  ");
                sb.Append(" and @tempEND) or ");
                sb.Append(" CAST(session_date+' '+CONVERT(SmallDateTime, session_end_time) AS datetime) between @tempStart  ");
                sb.Append(" and @tempEND) ");
                sb.Append(" ) )");
                sb.Append(" BEGIN ");
                sb.Append(" select 'fail' as sts ");
                sb.Append(" END ");
                sb.Append(" else ");
                sb.Append(" BEGIN ");
                sb.Append(" select 'pass'  as sts ");
                sb.Append(" END ");



                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows[0]["sts"].ToString() == "pass") { return true; }
                    else
                    {
                        return false;
                    }
                    //if (ds.Tables[0].Rows.Count <= 0)
                    //    return false;
                    //else
                    //    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public DataTable GetAllStudentRating()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM session_rating_parameters_for_student
                              WHERE (is_active = 'Y')";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("name");

                    dt.Rows.Add(ex.ToString());
                    dt.Rows.Add(DBDataAdpterObject.SelectCommand.Connection.ConnectionString);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllTutorRating()
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";

                SqlSelect = @"SELECT * FROM session_rating_parameters_for_tutors
                              WHERE (is_active = 'Y')";

                DBDataAdpterObject.SelectCommand.CommandText = SqlSelect;

                DataSet ds = new DataSet();
                try
                {
                    DBDataAdpterObject.Fill(ds);
                    if (ds.Tables[0].Rows.Count <= 0)
                        return null;
                    else
                        return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("name");

                    dt.Rows.Add(ex.ToString());
                    dt.Rows.Add(DBDataAdpterObject.SelectCommand.Connection.ConnectionString);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable delete_course(string list, string member_id)
        {
            try
            {
                DBDataAdpterObject.SelectCommand.Parameters.Clear();
                String SqlSelect = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(" select * from member_role where member_id='" + member_id + "' and role_id='TT' and course_code not in(" + list + ")");

                DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

                DataSet ds = new DataSet();

                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getredeam_user(string id)
        {

            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("select * from fn_token_reedemption where id='" + id + "'");



            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();


            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Save Methods


        public BLReturnObject logoutlink(DataSet ds_UploadData, bool updateFlag)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                objDS_MasterTables = (DS_MemberTables)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_MemberTables linkData = new DS_MemberTables();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table


                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;

                linkData.login_token.ImportRow(objDS_MasterTables.login_token.Rows[0]);
                linkData.login_token.Rows[0].Delete();

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.login_token, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.login_token.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        //by pooja bhadania on 16/4/16
        public BLReturnObject saveLinkData(DataSet ds_UploadData, bool updateFlag)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                objDS_MasterTables = (DS_MemberTables)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_MemberTables linkData = new DS_MemberTables();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table
                if (updateFlag == true)
                {
                    for (int mprCnt = 0; mprCnt < objDS_MasterTables.member_master.Rows.Count; mprCnt++)
                    {
                        linkData.member_master.ImportRow(objDS_MasterTables.member_master.Rows[mprCnt]);
                        linkData.member_master[mprCnt].AcceptChanges();
                        linkData.member_master[mprCnt].SetModified();
                    }
                }
                else  //for data insert into the member_master table
                {

                    objDocument.W_GetNextDocumentNo(ref DBCommand, "MM", "", "", ref DocNo, ref message);

                    for (int mprCnt = 0; mprCnt < objDS_MasterTables.member_master.Rows.Count; mprCnt++)
                    {
                        linkData.member_master.ImportRow(objDS_MasterTables.member_master.Rows[mprCnt]);
                        linkData.member_master[mprCnt].member_code = DocNo;
                    }
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        //genrated token login check
        public BLReturnObject saveLinkData_login(DataSet ds_UploadData, bool updateFlag)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                objDS_MasterTables = (DS_MemberTables)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_MemberTables linkData = new DS_MemberTables();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table
                if (updateFlag == true)
                {
                    for (int mprCnt = 0; mprCnt < objDS_MasterTables.member_master.Rows.Count; mprCnt++)
                    {
                        linkData.member_master.ImportRow(objDS_MasterTables.member_master.Rows[mprCnt]);
                        linkData.member_master[mprCnt].AcceptChanges();
                        linkData.member_master[mprCnt].SetModified();
                    }
                }
                else  //for data insert into the member_master table
                {

                    objDocument.W_GetNextDocumentNo(ref DBCommand, "MM", "", "", ref DocNo, ref message);

                    for (int mprCnt = 0; mprCnt < objDS_MasterTables.member_master.Rows.Count; mprCnt++)
                    {
                        linkData.member_master.ImportRow(objDS_MasterTables.member_master.Rows[mprCnt]);
                        linkData.member_master[mprCnt].member_code = DocNo;
                    }
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                //if (updateFlag == true)
                //{

                //    DataTable dt = logintokendata(objDS_MasterTables.login_token.Rows[0]["member_id"].ToString(), objDS_MasterTables.login_token.Rows[0]["device_id"].ToString());
                //    //jaimin
                //    if (dt != null && dt.Rows.Count > 0)
                //    {
                //        linkData.login_token.ImportRow(dt.Rows[0]);
                //        linkData.login_token.Rows[0].Delete();
                //        //linkData.AcceptChanges();
                //    }




                //}
                //objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.login_token, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                //if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.login_token.Rows.Count)
                //{
                //    DBCommand.Transaction.Rollback();
                //    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                //    objBLReturnObject.ExecutionStatus = 2;
                //    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                //    return objBLReturnObject;
                //}
                if (updateFlag == true)
                {
                    linkData.login_token.ImportRow(objDS_MasterTables.login_token.Rows[0]);
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.login_token, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.login_token.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject savetokenbooking(DataSet ds_UploadData, bool updateFlag)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                DS_Transtration objtransatrtion = new DS_Transtration();
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                objtransatrtion = (DS_Transtration)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Transtration linkData = new DS_Transtration();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table


                objDocument.W_GetNextDocumentNo(ref DBCommand, "TOB", "", "", ref DocNo, ref message);


                linkData.fn_token_balance.ImportRow(objtransatrtion.fn_token_balance.Rows[0]);
                linkData.fn_token_balance[0].doc_no = DocNo;

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.fn_token_balance, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.fn_token_balance.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        //22/4/16
        public BLReturnObject saveProfileLinkData(DataSet ds_UploadData, DataSet ds_transtration, bool flage)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_MemberTables server = (DS_MemberTables)ds_UploadData;
                DS_Transtration server1 = (DS_Transtration)ds_transtration;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_MemberTables linkData = new DS_MemberTables();
                DS_Transtration linkData1 = new DS_Transtration();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                string Doc_tob = "";
                Document objDocument = new Document();

                //for update the member_master table
                //for (int mprCnt = 0; mprCnt < server.member_master.Rows.Count; mprCnt++)
                //{
                //    linkData.member_master.ImportRow(server.member_master.Rows[mprCnt]);
                //    linkData.member_master[mprCnt].AcceptChanges();
                //    linkData.member_master[mprCnt].SetModified();
                //}


                if (flage == true)
                {
                    linkData.member_master.ImportRow(server.member_master.Rows[0]);

                }
                else
                {

                    for (int mprCnt = 0; mprCnt < server.member_master.Rows.Count; mprCnt++)
                    {
                        objDocument.W_GetNextDocumentNo(ref DBCommand, "MM", "", "", ref DocNo, ref message);
                        linkData.member_master.ImportRow(server.member_master.Rows[mprCnt]);
                        linkData.member_master[mprCnt].member_code = DocNo;
                    }
                }

                for (int mprCnt = 0; mprCnt < server.member_role.Rows.Count; mprCnt++)
                {

                    objDocument.W_GetNextDocumentNo(ref DBCommand, "MR", "", "", ref DocNo, ref message);
                    linkData.member_role.ImportRow(server.member_role.Rows[mprCnt]);
                    linkData.member_role[mprCnt].sr_no = DocNo;
                }
                if (server.member_tutor_transprit.Rows.Count > 0)
                {
                    for (int mprCnt = 0; mprCnt < server.member_tutor_transprit.Rows.Count; mprCnt++)
                    {
                        objDocument.W_GetNextDocumentNo(ref DBCommand, "MTT", "", "", ref DocNo, ref message);
                        linkData.member_tutor_transprit.ImportRow(server.member_tutor_transprit.Rows[mprCnt]);
                        linkData.member_tutor_transprit[mprCnt].sr_no = DocNo;
                    }
                }
                if (server1.fn_token_balance.Rows.Count > 0)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "TOB", "", "", ref Doc_tob, ref message);

                    linkData1.fn_token_balance.ImportRow(server1.fn_token_balance.Rows[0]);
                    linkData1.fn_token_balance[0].doc_no = Doc_tob;
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_role, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_role.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }



                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_tutor_transprit, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_tutor_transprit.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.fn_token_balance, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.fn_token_balance.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        //10/5/16
        public BLReturnObject updateProfileLinkData(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_MemberTables server = (DS_MemberTables)ds_UploadData;
                DS_MemberTables server1 = new DS_MemberTables();

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_MemberTables linkData = new DS_MemberTables();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table
                #region member_master
                for (int mprCnt = 0; mprCnt < server.member_master.Rows.Count; mprCnt++)
                {
                    linkData.member_master.ImportRow(server.member_master.Rows[mprCnt]);
                    linkData.member_master[mprCnt].AcceptChanges();
                    linkData.member_master[mprCnt].SetModified();
                }
                #endregion

                #region member_role
                DataTable dt = new DataTable();//GetMemroleData();
                dt = getCoursesUser_new_delete(server.member_master.Rows[0]["email_id"].ToString());
                DS_MemberTables linkData1 = new DS_MemberTables();
                if (dt != null)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        linkData1.member_role.ImportRow(dt.Rows[j]);
                    }

                    for (int i = linkData1.member_role.Rows.Count - 1; i >= 0; i--)
                    {
                        linkData1.member_role.Rows[i].Delete();
                    }

                }
                for (int mprCnt = 0; mprCnt < server.member_role.Rows.Count; mprCnt++)
                {

                    objDocument.W_GetNextDocumentNo(ref DBCommand, "MR", "", "", ref DocNo, ref message);
                    linkData.member_role.ImportRow(server.member_role.Rows[mprCnt]);
                    linkData.member_role[mprCnt].sr_no = DocNo;
                }
                #endregion

                #region member_tutor_transprit
                for (int mprCnt = 0; mprCnt < server.member_tutor_transprit.Rows.Count; mprCnt++)
                {
                    //objDocument.W_GetNextDocumentNo(ref DBCommand, "MTT", "", "", ref DocNo, ref message);
                    linkData.member_tutor_transprit.ImportRow(server.member_tutor_transprit.Rows[mprCnt]);
                    //linkData.member_tutor_transprit[mprCnt].sr_no = DocNo;
                    linkData.member_tutor_transprit[mprCnt].AcceptChanges();
                    linkData.member_tutor_transprit[mprCnt].SetModified();
                }
                #endregion

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.member_role, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.Update);
                if (objUpdateTableInfo.Status == false || objUpdateTableInfo.TotalRowsAffected != linkData1.member_role.Rows.Count)
                //if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != dt.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_role, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_role.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_tutor_transprit, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_tutor_transprit.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject delete_role_course(string courselist, string memberid)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                //if (ds_UploadData == null)
                //{
                //    objBLReturnObject.ExecutionStatus = 2;
                //    objBLReturnObject.ServerMessage = "There is no data to save";
                //    return objBLReturnObject;
                //}

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                //   DS_MemberTables server = (DS_MemberTables)ds_UploadData;
                DS_MemberTables server1 = new DS_MemberTables();

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_MemberTables linkData = new DS_MemberTables();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table


                #region member_role
                DataTable dt = new DataTable();//GetMemroleData();
                dt = delete_course(courselist, memberid);
                DS_MemberTables linkData1 = new DS_MemberTables();
                if (dt != null)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        linkData1.member_role.ImportRow(dt.Rows[j]);
                    }

                    for (int i = linkData1.member_role.Rows.Count - 1; i >= 0; i--)
                    {
                        linkData1.member_role.Rows[i].Delete();
                    }

                }

                #endregion


                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;


                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.member_role, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.Update);
                if (objUpdateTableInfo.Status == false || objUpdateTableInfo.TotalRowsAffected != linkData1.member_role.Rows.Count)
                //if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != dt.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }



                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject updateDatatoBetutor(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_MemberTables server = (DS_MemberTables)ds_UploadData;
                DS_MemberTables server1 = new DS_MemberTables();

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_MemberTables linkData = new DS_MemberTables();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table
                #region member_master
                for (int mprCnt = 0; mprCnt < server.member_master.Rows.Count; mprCnt++)
                {
                    linkData.member_master.ImportRow(server.member_master.Rows[mprCnt]);
                    linkData.member_master[mprCnt].AcceptChanges();
                    linkData.member_master[mprCnt].SetModified();
                }
                #endregion

                #region member_role
                //DataTable dt = new DataTable();//GetMemroleData();
                //dt = getCoursesUser(server.member_master.Rows[0]["email_id"].ToString());
                //DS_MemberTables linkData1 = new DS_MemberTables();

                //for (int j = 0; j < dt.Rows.Count; j++)
                //{
                //    linkData1.member_role.ImportRow(dt.Rows[j]);
                //}

                //for (int i = linkData1.member_role.Rows.Count - 1; i >= 0; i--)
                //{
                //    linkData1.member_role.Rows[i].Delete();
                //}


                for (int mprCnt = 0; mprCnt < server.member_role.Rows.Count; mprCnt++)
                {

                    objDocument.W_GetNextDocumentNo(ref DBCommand, "MR", "", "", ref DocNo, ref message);
                    linkData.member_role.ImportRow(server.member_role.Rows[mprCnt]);
                    linkData.member_role[mprCnt].sr_no = DocNo;
                }
                #endregion

                #region member_tutor_transprit
                for (int mprCnt = 0; mprCnt < server.member_tutor_transprit.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "MTT", "", "", ref DocNo, ref message);
                    linkData.member_tutor_transprit.ImportRow(server.member_tutor_transprit.Rows[mprCnt]);
                    linkData.member_tutor_transprit[mprCnt].sr_no = DocNo;
                    //linkData.member_tutor_transprit[mprCnt].AcceptChanges();
                    //linkData.member_tutor_transprit[mprCnt].SetModified();
                }
                #endregion

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                //objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.member_role, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.Update);
                //if (objUpdateTableInfo.Status == false || objUpdateTableInfo.TotalRowsAffected != linkData1.member_role.Rows.Count)
                ////if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != dt.Rows.Count)
                //{
                //    DBCommand.Transaction.Rollback();
                //    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                //    objBLReturnObject.ExecutionStatus = 2;
                //    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                //    return objBLReturnObject;
                //}

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_role, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_role.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_tutor_transprit, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_tutor_transprit.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject SaveSessiontimedata(DataSet ds_UploadData, DataSet ds_transtration)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_ScheduleAppointment server = (DS_ScheduleAppointment)ds_UploadData;
                DS_Transtration server1 = (DS_Transtration)ds_transtration;
                if (DBConnection.State == ConnectionState.Closed)
                    DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_ScheduleAppointment linkData = new DS_ScheduleAppointment();
                DS_Transtration linkdata1 = new DS_Transtration();

                String message = "";
                String sessiondoc = "";
                String fnbookdoc = "";
                String fntrandoc = "";
                String notifdoc = "";
                String alertdoc = "";
                Document objDocument = new Document();

                //for update the member_master table

                linkData.session_timing.ImportRow(server.session_timing.Rows[0]);
                objDocument.W_GetNextDocumentNo(ref DBCommand, "SST  ", "", "", ref sessiondoc, ref message);
                linkData.session_timing[0]["session_id"] = sessiondoc;

                #region notification : table
                for (int mprCnt = 0; mprCnt < server.notification.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "NT", "", "", ref notifdoc, ref message);
                    linkData.notification.ImportRow(server.notification.Rows[mprCnt]);
                    linkData.notification[mprCnt].doc_no = notifdoc;
                    linkData.notification[mprCnt].placeholder = sessiondoc;   //temp solution on 27/5/16  // notification : table
                }
                #endregion
                #region push notification
                for (int i = 0; i < server.alertmessage.Rows.Count; i++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "AlT", "", "", ref alertdoc, ref message);
                    linkData.alertmessage.ImportRow(server.alertmessage.Rows[i]);
                    linkData.alertmessage[i].Doc_no = alertdoc;
                    linkData.alertmessage[i].placeholder = sessiondoc;   //temp solution on 27/5/16  // notification : table
                }
                #endregion
                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.notification, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.notification.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_timing, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_timing.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                for (int mprCnt = 0; mprCnt < server.session_participants.Rows.Count; mprCnt++)
                {

                    linkData.session_participants.ImportRow(server.session_participants.Rows[mprCnt]);
                    linkData.session_participants[mprCnt].session_id = sessiondoc;
                    //linkData.member_tutor_transprit[mprCnt].AcceptChanges();
                    //linkData.member_tutor_transprit[mprCnt].SetModified();
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_participants, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_participants.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                #region sessionbooing fn

                linkdata1.fn_session_booking.ImportRow(server1.fn_session_booking.Rows[0]);
                objDocument.W_GetNextDocumentNo(ref DBCommand, "FSB", "", "", ref fnbookdoc, ref message);
                linkdata1.fn_session_booking[0]["doc_no"] = fnbookdoc;
                linkdata1.fn_session_booking[0]["session_id"] = sessiondoc;

                #endregion

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_session_booking, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_session_booking.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                #region token transation

                for (int mprCnte = 0; mprCnte < server1.fn_token_transtion.Rows.Count; mprCnte++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "FTT ", "", "", ref fntrandoc, ref message);
                    linkdata1.fn_token_transtion.ImportRow(server1.fn_token_transtion.Rows[mprCnte]);
                    linkdata1.fn_token_transtion[mprCnte]["doc_no"] = fntrandoc;
                    linkdata1.fn_token_transtion[mprCnte]["ref_transtion_doc_no"] = sessiondoc;
                }

                #endregion

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_token_transtion, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_token_transtion.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                for (int mprCnte = 0; mprCnte < server1.fn_token_balance.Rows.Count; mprCnte++)
                {

                    linkdata1.fn_token_balance.ImportRow(server1.fn_token_balance.Rows[mprCnte]);

                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_token_balance, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_token_balance.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = sessiondoc;
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        public BLReturnObject updateSessionResponse_test(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_ScheduleAppointment server = (DS_ScheduleAppointment)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_ScheduleAppointment linkData = new DS_ScheduleAppointment();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table
                #region member_master
                for (int mprCnt = 0; mprCnt < server.session_timing.Rows.Count; mprCnt++)
                {
                    linkData.session_timing.ImportRow(server.session_timing.Rows[mprCnt]);
                    linkData.session_timing[mprCnt].AcceptChanges();
                    linkData.session_timing[mprCnt].SetModified();
                }
                #endregion

                #region notification
                //for (int mprCnt = 0; mprCnt < server.notification.Rows.Count; mprCnt++)
                //{
                //    objDocument.W_GetNextDocumentNo(ref DBCommand, "NT", "", "", ref DocNo, ref message);
                //    linkData.notification.ImportRow(server.notification.Rows[mprCnt]);
                //    linkData.notification[mprCnt].doc_no = DocNo;
                //}
                #endregion

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                //objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.notification, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                //if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.notification.Rows.Count)
                //{
                //    DBCommand.Transaction.Rollback();
                //    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                //    objBLReturnObject.ExecutionStatus = 2;
                //    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                //    return objBLReturnObject;
                //}

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_timing, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_timing.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    //26/5/16
                    //if (DocNo == "")
                    //    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    //else
                    //    objBLReturnObject.ServerMessage = DocNo;
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        //26/5/16 [Don't Copy]
        public BLReturnObject updateSessionResponse(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_ScheduleAppointment server = (DS_ScheduleAppointment)ds_UploadData;


                if (DBConnection.State == ConnectionState.Closed)
                    DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_ScheduleAppointment linkData = new DS_ScheduleAppointment();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String alert_doc = "";
                Document objDocument = new Document();

                //for update the member_master table
                #region member_master
                for (int mprCnt = 0; mprCnt < server.session_timing.Rows.Count; mprCnt++)
                {
                    linkData.session_timing.ImportRow(server.session_timing.Rows[mprCnt]);
                    linkData.session_timing[mprCnt].AcceptChanges();
                    linkData.session_timing[mprCnt].SetModified();
                }
                #endregion

                #region notification
                for (int mprCnt = 0; mprCnt < server.notification.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "NT", "", "", ref DocNo, ref message);
                    linkData.notification.ImportRow(server.notification.Rows[mprCnt]);
                    linkData.notification[mprCnt].doc_no = DocNo;
                }
                #endregion
                #region notification
                for (int mprCnt = 0; mprCnt < server.alertmessage.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "AlT", "", "", ref alert_doc, ref message);
                    linkData.alertmessage.ImportRow(server.alertmessage.Rows[mprCnt]);
                    linkData.alertmessage[mprCnt].Doc_no = alert_doc;
                }
                #endregion


                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.notification, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.notification.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_timing, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_timing.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_timing, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_timing.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;

                    //26/5/16
                    if (DocNo == "")
                        objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    else
                        objBLReturnObject.ServerMessage = DocNo;
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        public BLReturnObject updateSessionResponse_withtrans(DataSet ds_UploadData, DataSet ds_transtration)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_ScheduleAppointment server = (DS_ScheduleAppointment)ds_UploadData;
                DS_Transtration server1 = (DS_Transtration)ds_transtration;

                if (DBConnection.State == ConnectionState.Closed)
                    DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_ScheduleAppointment linkData = new DS_ScheduleAppointment();
                DS_Transtration linkData1 = new DS_Transtration();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String fn_tran_doc = "";
                Document objDocument = new Document();

                //for update the member_master table
                #region member_master
                for (int mprCnt = 0; mprCnt < server.session_timing.Rows.Count; mprCnt++)
                {
                    linkData.session_timing.ImportRow(server.session_timing.Rows[mprCnt]);
                    linkData.session_timing[mprCnt].AcceptChanges();
                    linkData.session_timing[mprCnt].SetModified();
                }
                #endregion

                #region notification
                for (int mprCnt = 0; mprCnt < server.notification.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "NT", "", "", ref DocNo, ref message);
                    linkData.notification.ImportRow(server.notification.Rows[mprCnt]);
                    linkData.notification[mprCnt].doc_no = DocNo;
                }
                #endregion

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.notification, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.notification.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_timing, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_timing.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_timing, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_timing.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                // linkData1.fn_session_booking.ImportRow(server1.fn_session_booking.Rows[0]);
                for (int mprCnte = 0; mprCnte < server1.fn_session_booking.Rows.Count; mprCnte++)
                {
                    linkData1.fn_session_booking.ImportRow(server1.fn_session_booking.Rows[mprCnte]);

                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.fn_session_booking, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.fn_session_booking.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                for (int mprCnte = 0; mprCnte < server1.fn_token_balance.Rows.Count; mprCnte++)
                {
                    linkData1.fn_token_balance.ImportRow(server1.fn_token_balance.Rows[mprCnte]);
                    linkData1.fn_token_balance[mprCnte].AcceptChanges();
                    linkData1.fn_token_balance[mprCnte].SetModified();
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.fn_token_balance, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.fn_session_booking.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                for (int mprCnte = 0; mprCnte < server1.fn_token_transtion.Rows.Count; mprCnte++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "FTT ", "", "", ref fn_tran_doc, ref message);
                    linkData1.fn_token_transtion.ImportRow(server1.fn_token_transtion.Rows[mprCnte]);
                    linkData1.fn_token_transtion[mprCnte]["doc_no"] = fn_tran_doc;

                }
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.fn_token_transtion.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;

                    //26/5/16
                    if (DocNo == "")
                        objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    else
                        objBLReturnObject.ServerMessage = DocNo;
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject updateSessionResponsewithtrans(DataSet ds_UploadData, DataSet ds_transtration)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_ScheduleAppointment server = (DS_ScheduleAppointment)ds_UploadData;
                DS_Transtration server1 = (DS_Transtration)ds_transtration;

                if (DBConnection.State == ConnectionState.Closed)
                    DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_ScheduleAppointment linkData = new DS_ScheduleAppointment();
                DS_Transtration linkdata1 = new DS_Transtration();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String doc_alert = "";
                String fn_tran_doc = "";
                String fn_cancle_doc = "";

                Document objDocument = new Document();

                //for update the member_master table
                #region member_master
                for (int mprCnt = 0; mprCnt < server.session_timing.Rows.Count; mprCnt++)
                {
                    linkData.session_timing.ImportRow(server.session_timing.Rows[mprCnt]);
                    linkData.session_timing[mprCnt].AcceptChanges();
                    linkData.session_timing[mprCnt].SetModified();
                }
                #endregion
                #region alert mst
                for (int mprCnt = 0; mprCnt < server.alertmessage.Rows.Count; mprCnt++)
                {
                    linkData.alertmessage.ImportRow(server.alertmessage.Rows[mprCnt]);
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "AlT", "", "", ref doc_alert, ref message);
                    linkData.alertmessage[mprCnt].Doc_no = doc_alert;
                }

                #endregion
                #region notification
                for (int mprCnt = 0; mprCnt < server.notification.Rows.Count; mprCnt++)
                {
                    //objDocument.W_GetNextDocumentNo(ref DBCommand, "NT", "", "", ref DocNo, ref message);
                    linkData.notification.ImportRow(server.notification.Rows[mprCnt]);
                    //linkData.notification[mprCnt].doc_no = DocNo;
                }
                #endregion

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.notification, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.notification.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_timing, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_timing.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }


                for (int mprCnt = 0; mprCnt < server1.fn_session_booking.Rows.Count; mprCnt++)
                {
                    linkdata1.fn_session_booking.ImportRow(server1.fn_session_booking.Rows[mprCnt]);
                    linkdata1.fn_session_booking[mprCnt].AcceptChanges();
                    linkdata1.fn_session_booking[mprCnt].SetModified();
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_session_booking, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_session_booking.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                for (int mprCnt = 0; mprCnt < server1.fn_token_balance.Rows.Count; mprCnt++)
                {
                    linkdata1.fn_token_balance.ImportRow(server1.fn_token_balance.Rows[mprCnt]);
                    linkdata1.fn_token_balance[mprCnt].AcceptChanges();
                    linkdata1.fn_token_balance[mprCnt].SetModified();
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_token_balance, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_token_balance.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }


                for (int mprCnte = 0; mprCnte < server1.fn_token_transtion.Rows.Count; mprCnte++)
                {

                    objDocument.W_GetNextDocumentNo(ref DBCommand, "FTT", "", "", ref fn_tran_doc, ref message);
                    linkdata1.fn_token_transtion.ImportRow(server1.fn_token_transtion.Rows[mprCnte]);
                    linkdata1.fn_token_transtion[mprCnte].doc_no = fn_tran_doc;
                }


                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_token_transtion, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_token_transtion.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                for (int mprCnte = 0; mprCnte < server1.fn_token_cancel.Rows.Count; mprCnte++)
                {

                    objDocument.W_GetNextDocumentNo(ref DBCommand, "SSC", "", "", ref fn_cancle_doc, ref message);
                    linkdata1.fn_token_cancel.ImportRow(server1.fn_token_cancel.Rows[mprCnte]);
                    linkdata1.fn_token_cancel[mprCnte].doc_no = fn_cancle_doc;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_token_cancel, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_token_cancel.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;

                    //26/5/16
                    if (DocNo == "")
                        objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    else
                        objBLReturnObject.ServerMessage = DocNo;
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject SaveSession__survey_student(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_ScheduleAppointment server = (DS_ScheduleAppointment)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_ScheduleAppointment linkData = new DS_ScheduleAppointment();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table

                //linkData.session_survey_for_students.ImportRow(server.session_survey_for_students.Rows[0]);


                for (int mprCnt = 0; mprCnt < server.session_survey_for_students.Rows.Count; mprCnt++)
                {
                    linkData.session_survey_for_students.ImportRow(server.session_survey_for_students.Rows[mprCnt]);
                }
                for (int mprCnt = 0; mprCnt < server.session_participants.Rows.Count; mprCnt++)
                {
                    linkData.session_participants.ImportRow(server.session_participants.Rows[mprCnt]);
                }
                linkData.session_timing.ImportRow(server.session_timing.Rows[0]);
                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_survey_for_students, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_survey_for_students.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_participants, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_participants.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_timing, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_timing.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }


        public BLReturnObject Save_rating_summary_for_student(DataSet ds_UploadData, bool updateFlag)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_ScheduleAppointment server = (DS_ScheduleAppointment)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_ScheduleAppointment linkData = new DS_ScheduleAppointment();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table

                //linkData.session_survey_for_students.ImportRow(server.session_survey_for_students.Rows[0]);

                //fresh / new entry
                if (updateFlag == false)
                {
                    for (int mprCnt = 0; mprCnt < server.average_student_rating.Rows.Count; mprCnt++)
                    {
                        //objDocument.W_GetNextDocumentNo(ref DBCommand, "ARS", "", "", ref DocNo, ref message);
                        linkData.average_student_rating.ImportRow(server.average_student_rating.Rows[mprCnt]);
                        //linkData.average_student_rating[mprCnt].sr_no = DocNo;
                    }
                }
                //already exists entry
                else if (updateFlag == true)
                {
                    for (int mprCnt = 0; mprCnt < server.average_student_rating.Rows.Count; mprCnt++)
                    {
                        linkData.average_student_rating.ImportRow(server.average_student_rating.Rows[mprCnt]);
                        linkData.average_student_rating[mprCnt].AcceptChanges();
                        linkData.average_student_rating[mprCnt].SetModified();
                    }
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.average_student_rating, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.average_student_rating.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                if (server.alertmessage.Rows.Count > 0)
                {
                    linkData.alertmessage.ImportRow(server.alertmessage.Rows[0]);
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "AlT  ", "", "", ref DocNo, ref message);
                    linkData.alertmessage[0].Doc_no = DocNo;
                }




                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }


        public BLReturnObject Save_rating_summary_for_tutor(DataSet ds_UploadData, bool updateFlag)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_ScheduleAppointment server = (DS_ScheduleAppointment)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_ScheduleAppointment linkData = new DS_ScheduleAppointment();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table

                //linkData.session_survey_for_students.ImportRow(server.session_survey_for_students.Rows[0]);

                //fresh / new entry
                if (updateFlag == false)
                {
                    for (int mprCnt = 0; mprCnt < server.average_tutor_rating.Rows.Count; mprCnt++)
                    {
                        //objDocument.W_GetNextDocumentNo(ref DBCommand, "ARS", "", "", ref DocNo, ref message);
                        linkData.average_tutor_rating.ImportRow(server.average_tutor_rating.Rows[mprCnt]);
                        //linkData.average_student_rating[mprCnt].sr_no = DocNo;
                    }
                }
                //already exists entry
                else if (updateFlag == true)
                {
                    for (int mprCnt = 0; mprCnt < server.average_tutor_rating.Rows.Count; mprCnt++)
                    {
                        linkData.average_tutor_rating.ImportRow(server.average_tutor_rating.Rows[mprCnt]);
                        linkData.average_tutor_rating[mprCnt].AcceptChanges();
                        linkData.average_tutor_rating[mprCnt].SetModified();
                    }
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.average_tutor_rating, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.average_tutor_rating.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                if (server.alertmessage.Rows.Count > 0)
                {
                    linkData.alertmessage.ImportRow(server.alertmessage.Rows[0]);
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "AlT  ", "", "", ref DocNo, ref message);
                    linkData.alertmessage[0].Doc_no = DocNo;
                }


                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject SaveSession__survey_tutor(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_ScheduleAppointment server = (DS_ScheduleAppointment)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_ScheduleAppointment linkData = new DS_ScheduleAppointment();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table

                //linkData.session_survey_for_students.ImportRow(server.session_survey_for_students.Rows[0]);


                for (int mprCnt = 0; mprCnt < server.session_survey_for_tutors.Rows.Count; mprCnt++)
                {
                    linkData.session_survey_for_tutors.ImportRow(server.session_survey_for_tutors.Rows[mprCnt]);
                }
                for (int mprCnt = 0; mprCnt < server.session_participants.Rows.Count; mprCnt++)
                {
                    linkData.session_participants.ImportRow(server.session_participants.Rows[mprCnt]);
                }
                linkData.session_timing.ImportRow(server.session_timing.Rows[0]);
                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_survey_for_tutors, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_survey_for_tutors.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_participants, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_participants.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_timing, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_timing.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject updateNotification(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_ScheduleAppointment server = (DS_ScheduleAppointment)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_ScheduleAppointment linkData = new DS_ScheduleAppointment();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                #region notification
                for (int mprCnt = 0; mprCnt < server.notification.Rows.Count; mprCnt++)
                {
                    //objDocument.W_GetNextDocumentNo(ref DBCommand, "NT", "", "", ref DocNo, ref message);
                    linkData.notification.ImportRow(server.notification.Rows[mprCnt]);
                    linkData.notification[mprCnt].AcceptChanges();
                    linkData.notification[mprCnt].SetModified();
                }
                #endregion

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.notification, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.notification.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        public BLReturnObject updatealert(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_ScheduleAppointment server = (DS_ScheduleAppointment)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_ScheduleAppointment linkData = new DS_ScheduleAppointment();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                #region notification
                for (int mprCnt = 0; mprCnt < server.alertmessage.Rows.Count; mprCnt++)
                {
                    //objDocument.W_GetNextDocumentNo(ref DBCommand, "NT", "", "", ref DocNo, ref message);
                    linkData.alertmessage.ImportRow(server.alertmessage.Rows[mprCnt]);
                    linkData.alertmessage[mprCnt].AcceptChanges();
                    linkData.alertmessage[mprCnt].SetModified();
                }
                #endregion

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject updateSessiontimedata(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_ScheduleAppointment server = (DS_ScheduleAppointment)ds_UploadData;
                DS_ScheduleAppointment server1 = new DS_ScheduleAppointment();

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_ScheduleAppointment linkData = new DS_ScheduleAppointment();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();
                linkData.session_timing.ImportRow(server.session_timing.Rows[0]);
                //for update the member_master table
                DataTable dt = new DataTable();//GetMemroleData();
                dt = session_friendlist(server.session_timing.Rows[0]["session_id"].ToString(), server.session_timing.Rows[0]["booked_by"].ToString());
                DS_ScheduleAppointment linkData1 = new DS_ScheduleAppointment();
                if (dt != null)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        linkData1.session_participants.ImportRow(dt.Rows[j]);
                    }

                    for (int i = linkData1.session_participants.Rows.Count - 1; i >= 0; i--)
                    {
                        linkData1.session_participants.Rows[i].Delete();
                    }


                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_timing, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_timing.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.session_participants, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.session_participants.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                for (int mprCnt = 0; mprCnt < server.session_participants.Rows.Count; mprCnt++)
                {

                    linkData.session_participants.ImportRow(server.session_participants.Rows[mprCnt]);
                    //linkData.session_participants[mprCnt].session_id = DocNo;
                    //linkData.member_tutor_transprit[mprCnt].AcceptChanges();
                    //linkData.member_tutor_transprit[mprCnt].SetModified();
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.session_participants, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.session_participants.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject Savechat(DataSet ds_UploadData, DataSet ds_pushdata)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_MemberTables server = (DS_MemberTables)ds_UploadData;
                DS_ScheduleAppointment server1 = (DS_ScheduleAppointment)ds_pushdata;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_MemberTables linkData = new DS_MemberTables();
                DS_ScheduleAppointment linkData1 = new DS_ScheduleAppointment();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String DocNoAlt = "";
                Document objDocument = new Document();




                objDocument.W_GetNextDocumentNo(ref DBCommand, "CHT", "", "", ref DocNo, ref message);
                linkData.chating.ImportRow(server.chating.Rows[0]);
                linkData.chating[0].chat_id = DocNo;
                //for (int mprCnt = 0; mprCnt < server.chating.Rows.Count; mprCnt++)
                //{
                //    linkData.chating.ImportRow(server.chating.Rows[mprCnt]);
                //}

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.chating, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.chating.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                for (int mprCnt = 0; mprCnt < server1.alertmessage.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "AlT", "", "", ref DocNoAlt, ref message);
                    linkData1.alertmessage.ImportRow(server1.alertmessage.Rows[mprCnt]);
                    linkData1.alertmessage[mprCnt].Doc_no = DocNoAlt;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject updatetutorbyadmin(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_MemberTables server = (DS_MemberTables)ds_UploadData;
                DS_MemberTables server1 = new DS_MemberTables();

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_MemberTables linkData = new DS_MemberTables();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table
                #region member_master
                for (int mprCnt = 0; mprCnt < server.member_role.Rows.Count; mprCnt++)
                {
                    linkData.member_role.ImportRow(server.member_role.Rows[mprCnt]);
                    linkData.member_role[mprCnt].AcceptChanges();
                    linkData.member_role[mprCnt].SetModified();
                }
                #endregion








                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;




                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_role, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_role.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        //4/6/16

        public BLReturnObject Save_guru_rate(DataSet ds_UploadData, bool flag)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_MemberTables server = (DS_MemberTables)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_MemberTables linkData = new DS_MemberTables();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();


                if (flag == true)
                {
                    linkData.tutor_guru_rate_mst.ImportRow(server.tutor_guru_rate_mst.Rows[0]);
                }
                else
                {
                    for (int mprCnt = 0; mprCnt < server.tutor_guru_rate_mst.Rows.Count; mprCnt++)
                    {
                        objDocument.W_GetNextDocumentNo(ref DBCommand, "GURU ", "", "", ref DocNo, ref message);
                        linkData.tutor_guru_rate_mst.ImportRow(server.tutor_guru_rate_mst.Rows[mprCnt]);
                        linkData.tutor_guru_rate_mst[0].sr_no = DocNo;
                    }
                }
                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.tutor_guru_rate_mst, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.tutor_guru_rate_mst.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        public BLReturnObject Save_Standard_Rate(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();



                for (int mprCnt = 0; mprCnt < server.tutoring_rate_mst.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "TRM", "", "", ref DocNo, ref message);
                    linkData.tutoring_rate_mst.ImportRow(server.tutoring_rate_mst.Rows[mprCnt]);
                    linkData.tutoring_rate_mst[0].sr_no = DocNo;
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.tutoring_rate_mst, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.tutoring_rate_mst.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject Save_University_Mst(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();



                for (int mprCnt = 0; mprCnt < server.university_master.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "UNM  ", "", "", ref DocNo, ref message);
                    linkData.university_master.ImportRow(server.university_master.Rows[mprCnt]);
                    linkData.university_master[0].university_id = DocNo;
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.university_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.university_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }


        public BLReturnObject update_University_Mst(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();



                for (int mprCnt = 0; mprCnt < server.university_master.Rows.Count; mprCnt++)
                {
                    //objDocument.W_GetNextDocumentNo(ref DBCommand, "UNM  ", "", "", ref DocNo, ref message);
                    linkData.university_master.ImportRow(server.university_master.Rows[mprCnt]);
                    //linkData.university_master[0].university_id = DocNo;
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.university_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.university_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject Save_fratenity_soroty(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();



                for (int mprCnt = 0; mprCnt < server.fraternity_sorority_mst.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "FSM  ", "", "", ref DocNo, ref message);
                    linkData.fraternity_sorority_mst.ImportRow(server.fraternity_sorority_mst.Rows[mprCnt]);
                    linkData.fraternity_sorority_mst[0].fra_soro_id = DocNo;
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.fraternity_sorority_mst, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.fraternity_sorority_mst.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject Save_peek_rate_mst(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();



                for (int mprCnt = 0; mprCnt < server.peak_rate_master.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "PRM    ", "", "", ref DocNo, ref message);
                    linkData.peak_rate_master.ImportRow(server.peak_rate_master.Rows[mprCnt]);
                    linkData.peak_rate_master[0].sr_no = DocNo;
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.peak_rate_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.peak_rate_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                for (int mprCnt = 0; mprCnt < server.alertmessage.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "AlT", "", "", ref DocNo, ref message);
                    linkData.alertmessage.ImportRow(server.alertmessage.Rows[mprCnt]);
                    linkData.alertmessage[mprCnt].Doc_no = DocNo;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject Save_course_type_mst(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();



                for (int mprCnt = 0; mprCnt < server.course_type_mst.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "CTM", "", "", ref DocNo, ref message);
                    linkData.course_type_mst.ImportRow(server.course_type_mst.Rows[mprCnt]);
                    linkData.course_type_mst[0].doc_id = DocNo;
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.course_type_mst, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.course_type_mst.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject update_fratenity_soroty(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                linkData.fraternity_sorority_mst.ImportRow(server.fraternity_sorority_mst.Rows[0]);

                //for (int mprCnt = 0; mprCnt < server.fraternity_sorority_mst.Rows.Count; mprCnt++)
                //{
                //    objDocument.W_GetNextDocumentNo(ref DBCommand, "FSM  ", "", "", ref DocNo, ref message);

                //    linkData.fraternity_sorority_mst[0].fra_soro_id = DocNo;
                //}

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.fraternity_sorority_mst, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.fraternity_sorority_mst.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject update_peek_rate_mst(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();


                linkData.peak_rate_master.ImportRow(server.peak_rate_master.Rows[0]);
                //for (int mprCnt = 0; mprCnt < server.peak_rate_master.Rows.Count; mprCnt++)
                //{
                //    objDocument.W_GetNextDocumentNo(ref DBCommand, "PRM    ", "", "", ref DocNo, ref message);

                //    linkData.peak_rate_master[0].sr_no = DocNo;
                //}

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.peak_rate_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.peak_rate_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                for (int mprCnt = 0; mprCnt < server.alertmessage.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "AlT", "", "", ref DocNo, ref message);
                    linkData.alertmessage.ImportRow(server.alertmessage.Rows[mprCnt]);
                    linkData.alertmessage[mprCnt].Doc_no = DocNo;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject update_course_type_mst(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                linkData.course_type_mst.ImportRow(server.course_type_mst.Rows[0]);

                //for (int mprCnt = 0; mprCnt < server.course_type_mst.Rows.Count; mprCnt++)
                //{
                //    objDocument.W_GetNextDocumentNo(ref DBCommand, "CTM", "", "", ref DocNo, ref message);

                //    linkData.course_type_mst[0].doc_id = DocNo;
                //}

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.course_type_mst, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.course_type_mst.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }


        public BLReturnObject Save_discount_rate(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();


                linkData.discount_rate_mst.ImportRow(server.discount_rate_mst.Rows[0]);
                //for (int mprCnt = 0; mprCnt < server.course_type_mst.Rows.Count; mprCnt++)
                //{
                //    objDocument.W_GetNextDocumentNo(ref DBCommand, "CTM", "", "", ref DocNo, ref message);
                //    linkData.course_type_mst.ImportRow(server.course_type_mst.Rows[mprCnt]);
                //    linkData.course_type_mst[0].doc_id = DocNo;
                //}

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.discount_rate_mst, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.discount_rate_mst.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject Save_tutoring_location(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();


                //linkData.discount_rate_mst.ImportRow(server.discount_rate_mst.Rows[0]);
                for (int mprCnt = 0; mprCnt < server.tutor_location.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "TLoc", "", "", ref DocNo, ref message);
                    linkData.tutor_location.ImportRow(server.tutor_location.Rows[mprCnt]);
                    linkData.tutor_location[0].doc_no = DocNo;
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.tutor_location, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.tutor_location.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject Save_course(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();


                //  linkData.discount_rate_mst.ImportRow(server.discount_rate_mst.Rows[0]);
                for (int mprCnt = 0; mprCnt < server.course_mst.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "CM", "", "", ref DocNo, ref message);
                    linkData.course_mst.ImportRow(server.course_mst.Rows[mprCnt]);
                    linkData.course_mst[0].course_id = DocNo;
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.course_mst, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.course_mst.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject update_course(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                linkData.course_mst.ImportRow(server.course_mst.Rows[0]);
                //  linkData.discount_rate_mst.ImportRow(server.discount_rate_mst.Rows[0]);
                //for (int mprCnt = 0; mprCnt < server.course_mst.Rows.Count; mprCnt++)
                //{
                //    objDocument.W_GetNextDocumentNo(ref DBCommand, "CM", "", "", ref DocNo, ref message);

                //    linkData.course_mst[0].course_id = DocNo;
                //}

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.course_mst, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.course_mst.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject dsupdate_User(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_MemberTables server = (DS_MemberTables)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_MemberTables linkData = new DS_MemberTables();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                linkData.member_master.ImportRow(server.member_master.Rows[0]);
                //  linkData.discount_rate_mst.ImportRow(server.discount_rate_mst.Rows[0]);
                //for (int mprCnt = 0; mprCnt < server.course_mst.Rows.Count; mprCnt++)
                //{
                //    objDocument.W_GetNextDocumentNo(ref DBCommand, "CM", "", "", ref DocNo, ref message);

                //    linkData.course_mst[0].course_id = DocNo;
                //}

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject updateuseraccount(DataSet ds_UploadData, DataSet ds_transtration)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;
                DS_Transtration server1 = (DS_Transtration)ds_transtration;
                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                DS_Transtration linkData1 = new DS_Transtration();

                //  Document DocNo = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();
                String tras_doc = "";
                String token_req_doc = "";

                //for update the member_master table
                #region member_master
                for (int mprCnt = 0; mprCnt < server.member_master.Rows.Count; mprCnt++)
                {
                    linkData.member_master.ImportRow(server.member_master.Rows[mprCnt]);
                    linkData.member_master[mprCnt].AcceptChanges();
                    linkData.member_master[mprCnt].SetModified();
                }
                #endregion

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                for (int mprCnte = 0; mprCnte < server1.fn_token_reedemption.Rows.Count; mprCnte++)
                {
                    //objDocument.W_GetNextDocumentNo(ref DBCommand, "TRD", "", "", ref tras_doc, ref message);
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "RRD  ", "", "", ref token_req_doc, ref message);
                    linkData1.fn_token_reedemption.ImportRow(server1.fn_token_reedemption.Rows[mprCnte]);
                    linkData1.fn_token_reedemption[mprCnte].doc_no = token_req_doc;
                    //   linkData1.fn_token_reedemption[mprCnte].token_request_doc_no = DocNo;
                    //linkData1.fn_token_reedemption.Rows[mprCnte].doc_no = tras_doc;
                    //linkData1.fn_token_reedemption.Rows[mprCnte].token_request_doc_no = token_req_doc;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.fn_token_reedemption, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.fn_token_reedemption.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                linkData1.fn_token_transtion.ImportRow(server1.fn_token_transtion.Rows[0]);
                objDocument.W_GetNextDocumentNo(ref DBCommand, "FTT ", "", "", ref DocNo, ref message);
                linkData1.fn_token_transtion[0]["doc_no"] = DocNo;
                linkData1.fn_token_transtion[0]["ref_transtion_doc_no"] = token_req_doc;


                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.fn_token_transtion, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.fn_token_transtion.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                linkData1.fn_token_balance.ImportRow(server1.fn_token_balance.Rows[0]);
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.fn_token_balance, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.fn_token_balance.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject updatetokentranfer(DataSet ds_transtration)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_transtration == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];


                DS_Transtration server1 = (DS_Transtration)ds_transtration;
                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();

                DS_Transtration linkData1 = new DS_Transtration();

                Document Doc = new Document();
                String message = "";
                //String DocNo = "";
                Document objDocument = new Document();
                String tf = "";
                String fn_tran_doc = "";

                //for update the member_master table






                for (int mprCnte = 0; mprCnte < server1.fn_token_transfer.Rows.Count; mprCnte++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "TF", "", "", ref tf, ref message);

                    linkData1.fn_token_transfer.ImportRow(server1.fn_token_transfer.Rows[mprCnte]);
                    linkData1.fn_token_transfer[mprCnte].doc_no = tf;

                    //linkData1.fn_token_reedemption.Rows[mprCnte].doc_no = tras_doc;
                    //linkData1.fn_token_reedemption.Rows[mprCnte].token_request_doc_no = token_req_doc;
                }
                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.fn_token_transfer, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.fn_token_transfer.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                for (int mprCnt = 0; mprCnt < server1.fn_token_balance.Rows.Count; mprCnt++)
                {
                    linkData1.fn_token_balance.ImportRow(server1.fn_token_balance.Rows[mprCnt]);
                    linkData1.fn_token_balance[mprCnt].AcceptChanges();
                    linkData1.fn_token_balance[mprCnt].SetModified();
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.fn_token_balance, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.fn_token_balance.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                for (int mprCnte = 0; mprCnte < server1.fn_token_transtion.Rows.Count; mprCnte++)
                {

                    objDocument.W_GetNextDocumentNo(ref DBCommand, "FTT", "", "", ref fn_tran_doc, ref message);
                    linkData1.fn_token_transtion.ImportRow(server1.fn_token_transtion.Rows[mprCnte]);
                    linkData1.fn_token_transtion[mprCnte].doc_no = fn_tran_doc;
                    linkData1.fn_token_transtion[mprCnte].ref_transtion_doc_no = tf;
                }


                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.fn_token_transtion, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.fn_token_transtion.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        //Transtration Method


        public BLReturnObject SaveTranstionstratipe(DataSet ds_UploadData, bool flag)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Transtration server = (DS_Transtration)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Transtration linkData = new DS_Transtration();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String DocNo1 = "";
                Document objDocument = new Document();

                //for update the member_master table
                if (flag == false)
                {
                    linkData.fn_stripe_management.ImportRow(server.fn_stripe_management.Rows[0]);
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "STP  ", "", "", ref DocNo, ref message);
                    linkData.fn_stripe_management[0]["Doc_type"] = DocNo;
                }
                else
                {
                    linkData.fn_stripe_management.ImportRow(server.fn_stripe_management.Rows[0]);
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.fn_stripe_management, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.fn_stripe_management.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject Savetokenbalance(DataSet ds_UploadData, bool flag)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Transtration server = (DS_Transtration)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Transtration linkData = new DS_Transtration();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String DocNo1 = "";
                Document objDocument = new Document();

                //for update the member_master table
                if (flag == false)
                {
                    linkData.fn_token_balance.ImportRow(server.fn_token_balance.Rows[0]);
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "TOB  ", "", "", ref DocNo, ref message);
                    linkData.fn_token_balance[0]["doc_no"] = DocNo;
                }
                else
                {
                    linkData.fn_token_balance.ImportRow(server.fn_token_balance.Rows[0]);
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.fn_token_balance, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.fn_token_balance.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                linkData.fn_token_transtion.ImportRow(server.fn_token_transtion.Rows[0]);
                objDocument.W_GetNextDocumentNo(ref DBCommand, "FTT ", "", "", ref DocNo, ref message);
                linkData.fn_token_transtion[0]["doc_no"] = DocNo;

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.fn_token_transtion, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.fn_token_transtion.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        public BLReturnObject savesessionbooking(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Transtration server = (DS_Transtration)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Transtration linkData = new DS_Transtration();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String DocNo1 = "";
                Document objDocument = new Document();

                //for update the member_master table

                linkData.fn_session_booking.ImportRow(server.fn_token_balance.Rows[0]);
                objDocument.W_GetNextDocumentNo(ref DBCommand, "FSB", "", "", ref DocNo, ref message);
                linkData.fn_session_booking[0]["doc_no"] = DocNo;



                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.fn_session_booking, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.fn_session_booking.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }


                linkData.fn_token_transfer.ImportRow(server.fn_token_transfer.Rows[0]);
                objDocument.W_GetNextDocumentNo(ref DBCommand, "FT", "", "", ref DocNo1, ref message);
                linkData.fn_token_transfer[0]["doc_no"] = DocNo1;

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.fn_token_transfer, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.fn_token_transfer.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    //objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    objBLReturnObject.ServerMessage = DocNo1;
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        //save session booking
        public BLReturnObject alert_custome(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                for (int mprCnt = 0; mprCnt < server.alertmessage.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "AlT", "", "", ref DocNo, ref message);
                    linkData.alertmessage.ImportRow(server.alertmessage.Rows[mprCnt]);
                    linkData.alertmessage[mprCnt].Doc_no = DocNo;
                }
                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        public BLReturnObject alert_customeQUA(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Admin server = (DS_Admin)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Admin linkData = new DS_Admin();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                for (int mprCnt = 0; mprCnt < server.alertmessage.Rows.Count; mprCnt++)
                {
                    objDocument.QUA_GetNextDocumentNo(ref DBCommand, "AlT", "", "", ref DocNo, ref message);
                    linkData.alertmessage.ImportRow(server.alertmessage.Rows[mprCnt]);
                    linkData.alertmessage[mprCnt].Doc_no = DocNo;
                }
                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        public BLReturnObject updateTrans(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_MemberTables server = (DS_MemberTables)ds_UploadData;
                DS_MemberTables server1 = new DS_MemberTables();

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_MemberTables linkData = new DS_MemberTables();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                Document objDocument = new Document();

                //for update the member_master table
                #region member_master


                #endregion

                #region member_role

                #endregion

                #region member_tutor_transprit
                for (int mprCnt = 0; mprCnt < server.member_tutor_transprit.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "MTT", "", "", ref DocNo, ref message);
                    linkData.member_tutor_transprit.ImportRow(server.member_tutor_transprit.Rows[mprCnt]);
                    linkData.member_tutor_transprit[mprCnt].sr_no = DocNo;
                    linkData.member_tutor_transprit[mprCnt].AcceptChanges();
                    linkData.member_tutor_transprit[mprCnt].SetModified();
                }
                #endregion

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;





                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_tutor_transprit, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_tutor_transprit.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }


        public BLReturnObject update_redeam(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_Transtration server = (DS_Transtration)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_Transtration linkData = new DS_Transtration();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String DocNo1 = "";
                Document objDocument = new Document();

                //for update the member_master table




                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;


                linkData.fn_token_reedemption.ImportRow(server.fn_token_reedemption.Rows[0]);
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.fn_token_reedemption, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.fn_token_reedemption.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }




                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }


        public Byte UpdateUnreadCount(String AppName, String RepId, String DeviceId, int UnreadCount, ref String Message)
        {
            try
            {
                //Establish DataBase Connection.
                if (DBConnection.State == ConnectionState.Closed)
                    DBConnection.Open();
                //Begin Transaction.
                DBCommand.Transaction = DBConnection.BeginTransaction();

                DBCommand.Parameters.Clear();
                StringBuilder SQLUpdate = new StringBuilder();
                SQLUpdate.Append("UPDATE DeviceInfo ");
                SQLUpdate.Append("SET    UnreadCount=@UnreadCount ");
                SQLUpdate.Append("WHERE  AppName=@AppName AND RepId=@RepId AND DeviceId=@DeviceId");
                DBCommand.CommandText = SQLUpdate.ToString();
                DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@UnreadCount", DbType.Int32, UnreadCount));
                DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@AppName", DbType.String, AppName));
                DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@RepId", DbType.String, RepId));
                DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@DeviceId", DbType.String, DeviceId));
                int NoOfRowsDelete = DBCommand.ExecuteNonQuery();
                if (NoOfRowsDelete <= 0)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    Message = "UnreadCount update fail.";
                    return 2;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    Message = "UnreadCount updated successfully.";
                    return 1;
                }
            }
            catch (Exception ex)
            {
                if (DBCommand.Transaction != null)
                    DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                Message = ex.Message;
                //    ServerLog.MgmtExceptionLog(ex.Message + Environment.NewLine + ex.StackTrace);
                return 2;
            }
        }
        public Byte AddDeviceInfo(String AppName, String RepId, String DeviceId, String TokenId, String DeviceInfo, String OS, String IMEINo, ref String Message)
        {
            try
            {
                DS_MemberTables dS_DeviceInfo = new DS_MemberTables();
                dS_DeviceInfo.EnforceConstraints = false;
                DS_MemberTables.DeviceInfoRow row = dS_DeviceInfo.DeviceInfo.NewDeviceInfoRow();
                row.AppName = AppName;
                row.RepId = RepId;
                row.DeviceId = (DeviceId == null ? String.Empty : DeviceId);
                row.TokenId = TokenId;
                if (DeviceInfo != null && DeviceInfo.Trim() != String.Empty)
                    row.DeviceInfo = DeviceInfo;
                else
                    row.SetDeviceInfoNull();
                row.OS = OS;
                if (IMEINo != null && IMEINo.Trim() != String.Empty)
                    row.IMEINo = IMEINo;
                else
                    row.SetIMEINoNull();
                dS_DeviceInfo.DeviceInfo.AddDeviceInfoRow(row);

                try
                {
                    dS_DeviceInfo.EnforceConstraints = true;
                }
                catch (ConstraintException ce)
                {
                    Message = ce.Message;
                    //ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);

                    return 2;
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    // ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);

                    return 2;
                }

                //Establish DataBase Connection.
                if (DBConnection.State == ConnectionState.Closed)
                    DBConnection.Open();
                //Begin Transaction.
                DBCommand.Transaction = DBConnection.BeginTransaction();

                //Delete already existing DeviceInfo with same AppName and DeviceId
                if (OS.ToUpper() == Constant.OS_Android.ToUpper())
                {
                    DBCommand.Parameters.Clear();
                    StringBuilder SQLDelete = new StringBuilder();
                    SQLDelete.Append("DELETE FROM DeviceInfo WHERE AppName=@AppName AND (DeviceId=@DeviceId");
                    //if (IMEINo != null && IMEINo.Trim() != String.Empty)
                    //{
                    //    SQLDelete.Append(" OR IMEINo=@IMEINo");
                    //    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@IMEINo", DbType.String, IMEINo));
                    //}
                    SQLDelete.Append(")");
                    DBCommand.CommandText = SQLDelete.ToString();
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@AppName", DbType.String, AppName));
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@DeviceId", DbType.String, (DeviceId == null ? String.Empty : DeviceId)));
                    DBCommand.ExecuteNonQuery();

                    DBCommand.Parameters.Clear();
                    SQLDelete = new StringBuilder();
                    SQLDelete.Append("DELETE FROM MessageQueueFinal WHERE AppName=@AppName AND SentStatus='N' AND (DeviceId=@DeviceId");
                    if (IMEINo != null && IMEINo.Trim() != String.Empty)
                    {
                        SQLDelete.Append(" OR IMEINo=@IMEINo");
                        DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@IMEINo", DbType.String, IMEINo));
                    }
                    SQLDelete.Append(")");
                    DBCommand.CommandText = SQLDelete.ToString();
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@AppName", DbType.String, AppName));
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@DeviceId", DbType.String, (DeviceId == null ? String.Empty : DeviceId)));
                    DBCommand.ExecuteNonQuery();
                }
                else
                {
                    DBCommand.Parameters.Clear();
                    StringBuilder SQLDelete = new StringBuilder();
                    SQLDelete.Append("DELETE FROM DeviceInfo WHERE AppName=@AppName AND (DeviceId=@DeviceId OR TokenId=@TokenId)");
                    DBCommand.CommandText = SQLDelete.ToString();
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@AppName", DbType.String, AppName));
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@DeviceId", DbType.String, (DeviceId == null ? String.Empty : DeviceId)));
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@TokenId", DbType.String, TokenId));
                    DBCommand.ExecuteNonQuery();

                    DBCommand.Parameters.Clear();
                    SQLDelete = new StringBuilder();
                    SQLDelete.Append("DELETE FROM MessageQueueFinal WHERE AppName=@AppName AND (DeviceId=@DeviceId OR TokenId=@TokenId) AND is_read='N'");
                    DBCommand.CommandText = SQLDelete.ToString();
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@AppName", DbType.String, AppName));
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@DeviceId", DbType.String, (DeviceId == null ? String.Empty : DeviceId)));
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@TokenId", DbType.String, TokenId));
                    DBCommand.ExecuteNonQuery();
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, dS_DeviceInfo.DeviceInfo, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly);
                if (!objUpdateTableInfo.Status || objUpdateTableInfo.TotalRowsAffected != dS_DeviceInfo.DeviceInfo.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    Message = "Device registered fail." + Environment.NewLine + objUpdateTableInfo.ErrorMessage;
                    return 2;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    Message = "Device registered successfully.";
                    return 1;
                }
            }
            catch (Exception ex)
            {
                if (DBCommand.Transaction != null)
                    DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                Message = ex.Message;
                //ServerLog.Log((System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath).ToString() + " " + parameter.ToString() + "status" + ex.StackTrace);

                return 2;
            }
        }


        public Byte RemoveDeviceInfo(String AppName, String DeviceId, ref String Message)
        {
            try
            {
                //Establish DataBase Connection.
                if (DBConnection.State == ConnectionState.Closed)
                    DBConnection.Open();
                //Begin Transaction.
                DBCommand.Transaction = DBConnection.BeginTransaction();

                DBCommand.Parameters.Clear();
                StringBuilder SQLDelete = new StringBuilder();
                SQLDelete.Append("DELETE FROM DeviceInfo WHERE AppName=@AppName AND (DeviceId=@DeviceId OR TokenId=@TokenId)");
                DBCommand.CommandText = SQLDelete.ToString();
                DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@AppName", DbType.String, AppName));
                DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@DeviceId", DbType.String, DeviceId));
                DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@TokenId", DbType.String, DeviceId));
                int NoOfRowsDelete = DBCommand.ExecuteNonQuery();
                if (NoOfRowsDelete <= 0)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    Message = "Device unregistered fail.";
                    return 2;
                }
                else
                {
                    DBCommand.Parameters.Clear();
                    SQLDelete = new StringBuilder();
                    SQLDelete.Append("DELETE FROM MessageQueueFinal WHERE AppName=@AppName AND (DeviceId=@DeviceId OR TokenId=@TokenId) AND SentStatus='N'");
                    DBCommand.CommandText = SQLDelete.ToString();
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@AppName", DbType.String, AppName));
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@DeviceId", DbType.String, DeviceId));
                    DBCommand.Parameters.Add(DBObjectFactory.MakeParameter("@TokenId", DbType.String, DeviceId));
                    NoOfRowsDelete = DBCommand.ExecuteNonQuery();

                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    Message = "Device unregistered successfully.";
                    return 1;
                }
            }
            catch (Exception ex)
            {
                if (DBCommand.Transaction != null)
                    DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                Message = ex.Message;
                //ServerLog.ExceptionLog(ex.Message + Environment.NewLine + ex.StackTrace);
                return 2;
            }
        }
        #endregion




        #region Get mathod

        public DataTable getusermaster_admin(string app)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT     university_master.university_name, member_master.first_name, member_master.last_name, member_master.middle_name, member_master.image,  ");

            sb.Append("       member_master.email_id, member_master.phone_number1, member_master.bank_account_name, member_master.ifsc_code, member_master.account_number,  ");

            sb.Append("         member_master.is_active ");

            sb.Append(" FROM         member_master INNER JOIN ");

            sb.Append("           university_master ON member_master.university = university_master.university_id ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable ParameterMaster()
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("select * from dbo.ParameterMst ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetQUAPriceMaster()
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("select * from dbo.QUA_PricesMaster ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable ismemberexits(string email_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("select * from member_master where email_id='" + email_id + "' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable getcategories()
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT        categories_id, name, type ");
            sb.Append("FROM            QUA_Categories_master where is_active='Y' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getcategorieswise(string id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT      * ");
            sb.Append("FROM            QUA_Categories_master where is_active='Y' and categories_id='" + id + "' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getmail()
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("select * from email_template ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getitem_list(string cat_id, string sr_no, string email, string uni)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();





            //sb.Append(" select   top(4)  total_rows,sr_no,item_id,name_title,description,categories_id,price,small_image_path,big_name_path from( ");

            //sb.Append("  select  (select COUNT(*) from QUA_posting_master where categories_id='" + cat_id + "' and status in('A','B')  and is_active='Y'   ) total_rows,ROW_NUMBER() over(order by date_of_post desc) sr_no,* from QUA_posting_master where  categories_id='" + cat_id + "' and status in('A','B')  and is_active='Y' ");
            //sb.Append("  ) as t ");
            //sb.Append("  where categories_id='" + cat_id + "' and status in('A','B') and sr_no>=" + sr_no + " and is_active='Y' ");

            DataTable dt = getcategorieswise(cat_id);

            if (dt.Rows[0]["type"].ToString() == "sales")
            {
                sb.Append("   select   top(4)  total_rows,sr_no,item_id,name_title,member_id,description,categories_id,price,University_id, ");
                sb.Append(" small_image_path,big_name_path ,BlobId,first_name,last_name,image,ISNULL((SELECT 'TRUE' FROM QUA_wish_list as w WHERE w.item_id =t.item_id and member_id='" + email + "' ),'FALSE') AS isWishlist ");
                sb.Append("  from ");
                sb.Append("  (   select  (select COUNT(*) from QUA_posting_master where categories_id='" + cat_id + "' and status in('A','B')   ");
                sb.Append("  and is_active='Y' and   QUA_posting_master.university_id ='" + uni + "'   ) total_rows,ROW_NUMBER() over(order by date_of_post desc) sr_no,*  ");
                sb.Append(" from QUA_posting_master  ");
                sb.Append(" where QUA_posting_master.university_id ='" + uni + "' and categories_id='" + cat_id + "' and status in('A','B')  and is_active='Y'    ");
                sb.Append(" ) as t    ");
                sb.Append(" left join member_master on t.Member_id=member_master.email_id ");
                sb.Append(" where  categories_id='" + cat_id + "' ");
                sb.Append(" and status in('A','B') and sr_no>=" + sr_no + " and t.is_active='Y'  ");

            }
            else
            {

                sb.Append("   select   top(4)  total_rows,sr_no,item_id,name_title,member_id,description,categories_id,price,University_id, ");
                sb.Append(" small_image_path,big_name_path ,BlobId,first_name,last_name,image,ISNULL((SELECT 'TRUE' FROM QUA_wish_list as w WHERE w.item_id =t.item_id and member_id='" + email + "' ),'FALSE') AS isWishlist ");
                sb.Append("  from ");
                sb.Append("  (   select  (select COUNT(*) from QUA_posting_master where categories_id='" + cat_id + "'   ");
                sb.Append("  and is_active='Y' and University_id='" + uni + "'  ) total_rows,ROW_NUMBER() over(order by date_of_post desc) sr_no,*  ");
                sb.Append(" from QUA_posting_master  ");
                sb.Append(" where  categories_id='" + cat_id + "'    and is_active='Y' and  QUA_posting_master.University_id='" + uni + "'     ");
                sb.Append(" ) as t    ");
                sb.Append(" left join member_master on t.Member_id=member_master.email_id ");
                sb.Append(" where categories_id='" + cat_id + "' ");
                sb.Append("  and sr_no>=" + sr_no + " and t.is_active='Y'  ");
            }
            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        public DataTable mypost(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();







            //sb.Append(" SELECT       null as order_id, null as doc_no,item_id, Name_title, Description, categories_id, Price, Small_image_path,  ");

            //sb.Append("  Big_name_path, Date_of_post, Member_id, status ");

            //sb.Append("  FROM           QUA_posting_master ");

            //sb.Append("  where          is_active='Y'    and QUA_posting_master.member_id='" + member_id + "' ");

            //sb.Append("  Union ");

            //sb.Append("  SELECT       QUA_order_master.order_id,QUA_notification.doc_no,QUA_posting_master.item_id,QUA_posting_master.Name_title, ");

            //sb.Append("  QUA_posting_master.Description,QUA_posting_master.categories_id,QUA_posting_master.Price, ");

            //sb.Append("  QUA_posting_master.Small_image_path,QUA_posting_master.Big_name_path,QUA_posting_master.Date_of_post, ");

            //sb.Append(" QUA_posting_master.Member_id,status ");

            //sb.Append(" FROM            QUA_posting_master ");

            //sb.Append(" left join QUA_order_master on QUA_posting_master.item_id=QUA_order_master.item_id  and QUA_order_master.is_active='Y' ");

            //sb.Append(" left join QUA_notification on QUA_posting_master.item_id=QUA_notification.item_id  and QUA_notification.is_active='Y' ");

            //sb.Append(" where QUA_posting_master.status in('R','B') and QUA_posting_master.is_active='Y' and QUA_posting_master.member_id='" + member_id + "' ");



            //sb.Append("  Select *,(select count(*) from QUA_barter_master where item_id=t.item_id and is_accepted not in('C','S')) as total_barter , ");
            //sb.Append(" (select count(*) from  QUA_order_master where item_id=t.item_id and Order_status not in ('C','S')) as total_order ");
            //sb.Append(" from( ");
            //sb.Append(" SELECT       null as order_id, item_id, Name_title, Description, categories_id, Price, Small_image_path,    Big_name_path, Date_of_post, Member_id, status,QUA_posting_master.created_date   ");
            //sb.Append("  FROM           QUA_posting_master   ");
            //sb.Append(" where          is_active='Y' and QUA_posting_master.status not in('R','B','Q')   and QUA_posting_master.member_id='" + member_id + "'    ");

            //sb.Append("  Union    ");

            //sb.Append(" SELECT       QUA_order_master.order_id,QUA_posting_master.item_id,QUA_posting_master.Name_title,   QUA_posting_master.Description,QUA_posting_master.categories_id,QUA_posting_master.Price,   QUA_posting_master.Small_image_path,QUA_posting_master.Big_name_path,QUA_posting_master.Date_of_post,  QUA_posting_master.Member_id,status,QUA_posting_master.created_date   ");
            //sb.Append(" FROM            QUA_posting_master   ");
            //sb.Append(" left join QUA_order_master on QUA_posting_master.item_id=QUA_order_master.item_id   ");
            //sb.Append("  and QUA_order_master.is_active='Y'   ");
            //sb.Append("   where QUA_order_master.Order_status in('R','B','Q')  ");
            //sb.Append("  and QUA_posting_master.is_active='Y' and QUA_posting_master.member_id='" + member_id + "'  ");
            //sb.Append("  ) as t  order by cast(t.created_date as datetime)desc ");


            //sb.Append(" Select *,(select count(*) from QUA_barter_master where item_id=t.item_id and is_accepted not in('C','S')) ");
            //sb.Append(" as total_barter ,  (select count(*) from  QUA_order_master where item_id=t.item_id and Order_status not in ('C','S')) ");
            //sb.Append(" as total_order  from(  SELECT        ");
            //sb.Append(" '2' as sr_no,item_id, item_id, Name_title,  ");
            //sb.Append(" Description, categories_id, Price, Small_image_path,    Big_name_path, Date_of_post, Member_id,  ");
            //sb.Append(" status,QUA_posting_master.created_date,last_modified_date     FROM           QUA_posting_master    where           ");
            //sb.Append(" is_active='Y' and QUA_posting_master.status  in('S')       ");
            //sb.Append(" and QUA_posting_master.member_id='" + member_id + "'      ");
            //sb.Append("  Union    ");
            //sb.Append(" SELECT      ");
            //sb.Append(" '1' as sr_no, QUA_posting_master.item_id,QUA_posting_master.Name_title,   ");
            //sb.Append(" QUA_posting_master.Description,QUA_posting_master.categories_id,QUA_posting_master.Price,    ");
            //sb.Append(" QUA_posting_master.Small_image_path,QUA_posting_master.Big_name_path,QUA_posting_master.Date_of_post,   ");
            //sb.Append(" QUA_posting_master.Member_id,QUA_posting_master.status as status,QUA_posting_master.created_date,QUA_posting_master.last_modified_date     FROM            ");
            //sb.Append(" QUA_posting_master     ");

            //sb.Append(" where ");
            //sb.Append("   QUA_posting_master.is_active='Y' and QUA_posting_master.member_id='" + member_id + "'   and QUA_posting_master.status not in('S')      ");
            //sb.Append("  ) as t  order by  ");
            //sb.Append(" sr_no, cast(t.created_date as datetime) desc   ");

            sb.Append("  Select *,(select count(*) from QUA_barter_master where item_id=t.item_id and is_accepted not in('C','S')) ");
            sb.Append(" as total_barter ,  (select count(*) from  QUA_order_master where item_id=t.item_id and Order_status not in ('C','S')) ");
            sb.Append(" as total_order  from( ");
            sb.Append("   SELECT '2' as sr_no,item_id, Name_title,  ");
            sb.Append(" Description, categories_id, Price, Small_image_path,    Big_name_path, Date_of_post, Member_id,  ");
            sb.Append(" status,QUA_posting_master.created_date,last_modified_date     FROM           QUA_posting_master    where           ");
            sb.Append(" is_active='Y' and QUA_posting_master.status in('S')      ");
            sb.Append(" and QUA_posting_master.member_id='" + member_id + "'    ");
            sb.Append("  Union    ");
            sb.Append(" 	SELECT '1' as sr_no,QUA_posting_master.item_id,QUA_posting_master.Name_title,   ");
            sb.Append(" QUA_posting_master.Description,QUA_posting_master.categories_id,QUA_posting_master.Price,    ");
            sb.Append(" QUA_posting_master.Small_image_path,QUA_posting_master.Big_name_path,QUA_posting_master.Date_of_post,   ");
            sb.Append(" QUA_posting_master.Member_id,QUA_posting_master.status as status,QUA_posting_master.created_date,QUA_posting_master.last_modified_date     FROM            ");
            sb.Append(" QUA_posting_master     ");

            sb.Append(" where ");
            sb.Append("   QUA_posting_master.is_active='Y' and QUA_posting_master.member_id='" + member_id + "' ");
            sb.Append("   and QUA_posting_master.status not in('S')     ");
            sb.Append("  ) as t   ");
            sb.Append(" order by sr_no,cast(t.created_date as datetime) desc   ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable message(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from QUA_notification where to_member='" + member_id + "' and is_read='N' and is_accept='N' and is_active='Y' order by CAST(created_date as int) desc ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable myorder(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from QUA_notification where to_member='" + member_id + "' and is_read='N' and is_accept='N' and is_active='Y' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getwishlist(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("  select * from QUA_wish_list ");
            sb.Append(" left  join  QUA_posting_master on QUA_wish_list.item_id=QUA_posting_master.item_id ");
            sb.Append(" where QUA_wish_list.Member_id='" + member_id + "' and QUA_wish_list.is_active='Y' order by CAST(QUA_wish_list.created_date as date) desc");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable my_order_list(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from QUA_order_master  ");
            sb.Append(" left join QUA_posting_master on QUA_order_master.item_id=QUA_posting_master.item_id ");
            sb.Append(" left join QUA_Categories_master on QUA_posting_master.categories_id=QUA_Categories_master.categories_id ");
            sb.Append("  left join   member_master on QUA_posting_master.Member_id=member_master.email_id ");
            sb.Append(" where QUA_order_master.Member_id='" + member_id + "' and QUA_order_master.is_active='Y' order by  cast(QUA_order_master.created_date as datetime) desc ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable my_sales(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("   select * ");
            sb.Append("  from ( ");
            sb.Append(" SELECT     Order_id as id, QUA_posting_master.member_id as member,QUA_posting_master.item_id,QUA_order_master.last_modified_date, QUA_posting_master.Name_title, QUA_posting_master.Description, QUA_posting_master.Price,  ");
            sb.Append("       QUA_posting_master.Small_image_path, QUA_posting_master.Big_name_path, QUA_posting_master.status, member_master.email_id, member_master.first_name,  ");
            sb.Append("    member_master.last_name,member_master.image , 'Order' as Type ,'S' as Req_type ");
            sb.Append(" FROM         QUA_posting_master  ");

            sb.Append(" left join  ");
            sb.Append(" QUA_order_master on QUA_posting_master.item_id=QUA_order_master.Item_id ");
            sb.Append(" left join  ");
            sb.Append(" member_master on QUA_order_master.member_id=member_master.email_id ");
            sb.Append(" where  QUA_posting_master.member_id='" + member_id + "' ");

            sb.Append(" UNION ");

            sb.Append(" SELECT    Barter_id as id, QUA_posting_master.member_id as member ,QUA_posting_master.item_id,QUA_barter_master.last_modified_date, QUA_posting_master.Name_title, QUA_posting_master.Description, QUA_posting_master.Price,  ");
            sb.Append(" QUA_posting_master.Small_image_path, QUA_posting_master.Big_name_path, QUA_posting_master.status, member_master.email_id, member_master.first_name,  ");
            sb.Append(" member_master.last_name,member_master.image,'Barter' as Type,'B' as Req_type  ");
            sb.Append(" FROM         QUA_posting_master  ");

            sb.Append(" left join  ");
            sb.Append(" QUA_barter_master on QUA_posting_master.item_id=QUA_barter_master.Item_id ");
            sb.Append(" left join ");
            sb.Append("   member_master on QUA_barter_master.Buyer_Member_id=member_master.email_id ");
            sb.Append("   where  QUA_barter_master.is_accepted='S'  and QUA_posting_master.member_id='" + member_id + "' ");

            sb.Append(" ) as t  ");

            sb.Append("  inner join QUA_feedback_participants on t.item_id=QUA_feedback_participants.item_id ");
            sb.Append("  and QUA_feedback_participants.member_id=t.member AND QUA_feedback_participants.OrderID=t.id order by  CAST(t.last_modified_date as date) desc");





            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable My_purchase(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();



            sb.Append("   select *  ");
            sb.Append("  from ( ");
            sb.Append("  SELECT    Order_id as id, QUA_order_master.Member_id as member,QUA_posting_master.item_id, QUA_order_master.last_modified_date,QUA_posting_master.Name_title, QUA_posting_master.Description, ");
            sb.Append(" QUA_posting_master.Price,         ");
            sb.Append(" QUA_posting_master.Small_image_path, QUA_posting_master.Big_name_path,  ");
            sb.Append("   QUA_posting_master.status, member_master.email_id, member_master.first_name,      ");
            sb.Append(" member_master.last_name,member_master.image,'S' as Req_type   ");
            sb.Append(" FROM         QUA_order_master ");

            sb.Append(" left join QUA_posting_master on QUA_order_master.Item_id=QUA_posting_master.Item_id ");
            sb.Append(" left join member_master on QUA_posting_master.member_id=member_master.email_id ");
            sb.Append(" where QUA_order_master.Member_id='" + member_id + "' and QUA_order_master.Order_status='S' ");


            sb.Append(" UNION  ");
            sb.Append(" SELECT     Barter_id as id, QUA_barter_master.buyer_member_id as member,QUA_posting_master.item_id,QUA_barter_master.last_modified_date, QUA_posting_master.Name_title, QUA_posting_master.Description,  ");
            sb.Append(" QUA_posting_master.Price,QUA_posting_master.Small_image_path, QUA_posting_master.Big_name_path, ");
            sb.Append("  QUA_posting_master.status, member_master.email_id, member_master.first_name,    ");
            sb.Append(" member_master.last_name,member_master.image,'B' as Req_type  ");

            sb.Append(" FROM         QUA_barter_master    ");
            sb.Append(" left join   QUA_posting_master on QUA_barter_master.Item_id=QUA_posting_master.item_id   ");
            sb.Append(" left join    member_master on QUA_posting_master.member_id=member_master.email_id           ");

            sb.Append(" where QUA_barter_master.is_accepted='S' ");
            sb.Append(" and QUA_barter_master.buyer_member_id='" + member_id + "' ");
            sb.Append("   ) as t  ");

            sb.Append("   inner join QUA_feedback_participants on t.item_id=QUA_feedback_participants.item_id ");
            sb.Append(" and QUA_feedback_participants.member_id=t.member AND QUA_feedback_participants.OrderID=t.id order by  CAST(t.last_modified_date as date) desc ");





            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable get_notification(string doc_no)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from  QUA_notification where doc_no='" + doc_no + "'  ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable get_member_detail(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from member_master where email_id='" + member_id + "' and is_active='Y' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable get_member_id_allnull()
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from member_master where  is_active='Y' and blobid is null");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable my_orderitemwise(string item_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from QUA_order_master where item_id='" + item_id + "' and order_status='R' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable itemwisebarter(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from QUA_barter_master where buyer_member_id='" + member_id + "' and is_active='Y' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getbarter_wise(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            //sb.Append(" select * from QUA_barter_master ");
            //sb.Append("  left join member_master on QUA_barter_master.buyer_member_id=member_master.email_id ");
            //sb.Append(" left join  QUA_posting_master on QUA_barter_master.item_id=QUA_posting_master.item_id ");
            //sb.Append("  where QUA_barter_master.Buyer_Member_id='" + member_id + "' and member_master.is_active='Y' ");


            //sb.Append(" SELECT     QUA_barter_master.Barter_id, QUA_posting_master.item_id, QUA_posting_master.Name_title, QUA_posting_master.Description, QUA_posting_master.Price, ");
            //sb.Append("  QUA_posting_master.Small_image_path, QUA_posting_master.Big_name_path, QUA_posting_master.Member_id, QUA_barter_master.Item_id AS Expr1, QUA_barter_master.Description AS Expr2,  ");
            //sb.Append("  QUA_barter_master.title, QUA_barter_master.smalI_image_path, QUA_barter_master.Buyer_Member_id, QUA_barter_master.Big_image_path, QUA_barter_master.is_accepted, ");
            //sb.Append("   member_master.first_name, member_master.last_name, member_master.nick_name, member_master.image ");
            //sb.Append(" FROM         QUA_barter_master  ");
            //sb.Append(" left join member_master on QUA_barter_master.buyer_member_id=member_master.email_id ");
            //sb.Append("  left join  QUA_posting_master on QUA_barter_master.item_id=QUA_posting_master.item_id ");
            //sb.Append("  where QUA_barter_master.item_id='P1617000045' and member_master.is_active='Y' ");


            sb.Append("   SELECT     QUA_barter_master.Barter_id, QUA_posting_master.item_id, QUA_posting_master.Name_title AS B_TITLE, QUA_posting_master.Description AS B_Description, QUA_posting_master.Price,  ");
            sb.Append("  QUA_posting_master.Small_image_path AS B_Small_image_path, QUA_posting_master.Big_name_path AS B_Big_name_path, QUA_posting_master.Member_id, QUA_barter_master.Description ,   ");
            sb.Append("  QUA_barter_master.title, QUA_barter_master.smalI_image_path, QUA_barter_master.Buyer_Member_id, QUA_barter_master.Big_image_path, QUA_barter_master.is_accepted,  ");
            sb.Append("  member_master.first_name, member_master.last_name, member_master.nick_name, member_master.image,member_master.BlobId   ");
            sb.Append("  FROM         QUA_barter_master   ");
            sb.Append("  left join member_master on QUA_barter_master.buyer_member_id=member_master.email_id  ");
            sb.Append("  left join  QUA_posting_master on QUA_barter_master.item_id=QUA_posting_master.item_id  ");
            sb.Append("  where QUA_barter_master.item_id='" + member_id + "' and member_master.is_active='Y'  ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getorder_wise(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            //sb.Append(" select * from QUA_barter_master ");
            //sb.Append("  left join member_master on QUA_barter_master.buyer_member_id=member_master.email_id ");
            //sb.Append(" left join  QUA_posting_master on QUA_barter_master.item_id=QUA_posting_master.item_id ");
            //sb.Append("  where QUA_barter_master.Buyer_Member_id='" + member_id + "' and member_master.is_active='Y' ");


            //sb.Append(" SELECT     QUA_barter_master.Barter_id, QUA_posting_master.item_id, QUA_posting_master.Name_title, QUA_posting_master.Description, QUA_posting_master.Price, ");
            //sb.Append("  QUA_posting_master.Small_image_path, QUA_posting_master.Big_name_path, QUA_posting_master.Member_id, QUA_barter_master.Item_id AS Expr1, QUA_barter_master.Description AS Expr2,  ");
            //sb.Append("  QUA_barter_master.title, QUA_barter_master.smalI_image_path, QUA_barter_master.Buyer_Member_id, QUA_barter_master.Big_image_path, QUA_barter_master.is_accepted, ");
            //sb.Append("   member_master.first_name, member_master.last_name, member_master.nick_name, member_master.image ");
            //sb.Append(" FROM         QUA_barter_master  ");
            //sb.Append(" left join member_master on QUA_barter_master.buyer_member_id=member_master.email_id ");
            //sb.Append("  left join  QUA_posting_master on QUA_barter_master.item_id=QUA_posting_master.item_id ");
            //sb.Append("  where QUA_barter_master.item_id='P1617000045' and member_master.is_active='Y' ");


            sb.Append("  select * from dbo.QUA_order_master  ");
            sb.Append(" left join member_master on QUA_order_master.member_id=member_master.email_id ");
            sb.Append(" left join dbo.QUA_posting_master on QUA_order_master.Item_id=QUA_posting_master.item_id ");
            sb.Append(" where QUA_order_master.Item_id='" + member_id + "' and order_status in('R') ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getbarter(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            //sb.Append(" select * from QUA_barter_master ");
            //sb.Append("  left join member_master on QUA_barter_master.buyer_member_id=member_master.email_id ");
            //sb.Append(" left join  QUA_posting_master on QUA_barter_master.item_id=QUA_posting_master.item_id ");
            //sb.Append("  where QUA_barter_master.Buyer_Member_id='" + member_id + "' and member_master.is_active='Y' ");


            //sb.Append(" SELECT     QUA_barter_master.Barter_id, QUA_posting_master.item_id, QUA_posting_master.Name_title, QUA_posting_master.Description, QUA_posting_master.Price, ");
            //sb.Append("  QUA_posting_master.Small_image_path, QUA_posting_master.Big_name_path, QUA_posting_master.Member_id, QUA_barter_master.Item_id AS Expr1, QUA_barter_master.Description AS Expr2,  ");
            //sb.Append("  QUA_barter_master.title, QUA_barter_master.smalI_image_path, QUA_barter_master.Buyer_Member_id, QUA_barter_master.Big_image_path, QUA_barter_master.is_accepted, ");
            //sb.Append("   member_master.first_name, member_master.last_name, member_master.nick_name, member_master.image ");
            //sb.Append(" FROM         QUA_barter_master  ");
            //sb.Append(" left join member_master on QUA_barter_master.buyer_member_id=member_master.email_id ");
            //sb.Append("  left join  QUA_posting_master on QUA_barter_master.item_id=QUA_posting_master.item_id ");
            //sb.Append("  where QUA_barter_master.item_id='P1617000045' and member_master.is_active='Y' ");


            sb.Append("   SELECT     QUA_barter_master.Barter_id, QUA_posting_master.item_id, QUA_posting_master.Name_title AS B_TITLE, QUA_posting_master.Description AS B_Description, QUA_posting_master.Price,  ");
            sb.Append("  QUA_posting_master.Small_image_path AS B_Small_image_path, QUA_posting_master.Big_name_path AS B_Big_name_path, QUA_posting_master.Member_id, QUA_barter_master.Description ,   ");
            sb.Append("  QUA_barter_master.title, QUA_barter_master.smalI_image_path, QUA_barter_master.Buyer_Member_id, QUA_barter_master.Big_image_path, QUA_barter_master.is_accepted,  ");
            sb.Append("  member_master.first_name, member_master.last_name, member_master.nick_name, member_master.image,member_master.BlobId   ");
            sb.Append("  FROM         QUA_barter_master   ");

            sb.Append("  left join  QUA_posting_master on QUA_barter_master.item_id=QUA_posting_master.item_id  ");

            sb.Append(" left join member_master on QUA_posting_master.member_id=member_master.email_id   ");
            sb.Append("  where QUA_barter_master.buyer_member_id='" + member_id + "' and member_master.is_active='Y' order by QUA_barter_master.created_date desc ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable feedback_count_rating(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();





            sb.Append(" select count(*) as total,  ");
            sb.Append(" (select ISNULL(AVG(values_rating),0)     from QUA_feedback where feedback_member_id='" + member_id + "') as rating ");
            sb.Append(" from  QUA_feedback ");
            sb.Append(" where feedback_member_id='" + member_id + "' ");








            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable feedback(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();





            sb.Append("  select * from  QUA_feedback  ");
            sb.Append(" left join member_master on QUA_feedback.current_Member_id=member_master.email_id ");
            sb.Append(" left join dbo.QUA_posting_master on QUA_feedback.item_id=QUA_posting_master.item_id ");
            sb.Append(" where feedback_member_id='" + member_id + "' ");


            sb.Append("  select t.*, ");
            sb.Append(" case when t.req_type='S' then QUA_order_master.last_modified_date ");
            sb.Append(" when t.req_type='B' then QUA_barter_master.last_modified_date else null end as last_modified_date ");
            sb.Append(" from ( ");
            sb.Append(" select QUA_feedback.current_Member_id, QUA_feedback.values_rating, QUA_feedback.comments, QUA_feedback.item_id, QUA_posting_master.item_id AS Expr1,  ");
            sb.Append("         QUA_posting_master.Name_title, QUA_posting_master.Description, member_master.member_code, member_master.university, member_master.first_name, ");
            sb.Append(" member_master.last_name, member_master.middle_name ,member_master.image,req_type,feedback_member_id ");
            sb.Append(" from  QUA_feedback    ");
            sb.Append(" left join member_master on QUA_feedback.current_Member_id=member_master.email_id   ");
            sb.Append(" left join dbo.QUA_posting_master on QUA_feedback.item_id=QUA_posting_master.item_id  ");
            sb.Append(" ) as t  ");
            sb.Append(" left join dbo.QUA_order_master on t.item_id=QUA_order_master.item_id and QUA_order_master.Order_status='S' ");


            sb.Append("   left join  dbo.QUA_barter_master on t.item_id=dbo.QUA_barter_master.Item_id and QUA_barter_master.is_accepted='S' ");
            sb.Append(" where t.feedback_member_id='" + member_id + "' ");



            //sb.Append(" select QUA_feedback.current_Member_id, QUA_feedback.values_rating, QUA_feedback.comments,  ");
            //sb.Append(" QUA_feedback.item_id, QUA_posting_master.item_id AS Expr1,           ");
            //sb.Append(" QUA_posting_master.Name_title, QUA_posting_master.Description, member_master.member_code,  ");
            //sb.Append(" member_master.university, member_master.first_name,  member_master.last_name, member_master.middle_name , ");
            //sb.Append(" member_master.image,req_type,feedback_member_id  from  QUA_feedback     ");
            //sb.Append(" left join member_master on QUA_feedback.current_Member_id=member_master.email_id    ");
            //sb.Append(" left join dbo.QUA_posting_master on QUA_feedback.item_id=QUA_posting_master.item_id ");
            //sb.Append(" where feedback_member_id='" + member_id + "'  ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getpartication(string member_id, string item_id, string id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("  select * from QUA_feedback_participants where item_id='" + item_id + "' and member_id='" + member_id + "' and OrderID='" + id + "' ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable checkfeedback(string member_id, string item_id, string id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("  select * from  QUA_feedback_participants where item_id='" + item_id + "' and member_id='" + member_id + "' and is_survey_done='N' and OrderID='" + id + "' ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getcheckwishtlist(string member_id, string itemid)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from QUA_wish_list where member_id='" + member_id + "' and item_id='" + itemid + "' and is_active='Y' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable getitemlist(string itemid)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("  select * from QUA_posting_master  ");
            sb.Append(" left join dbo.QUA_Categories_master on dbo.QUA_posting_master.categories_id=dbo.QUA_Categories_master.categories_id  ");
            sb.Append(" where item_id='" + itemid + "'  and QUA_posting_master.is_active='Y' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getitem(string itemid)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from QUA_posting_master where item_id='" + itemid + "' and  status in('A','B')  and is_active='Y' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable getitembyid(string itemid)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from QUA_posting_master  ");
            sb.Append(" left join dbo.QUA_Categories_master on QUA_posting_master.categories_id=dbo.QUA_Categories_master.categories_id ");
            sb.Append(" where item_id='" + itemid + "' and QUA_posting_master.is_active='Y' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getbuyerinfo(string orderId)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from QUA_order_master  ");
            sb.Append(" left join  member_master on QUA_order_master.Member_id=member_master.email_id ");
            sb.Append(" where QUA_order_master.Order_id='" + orderId + "' and member_master.is_active='Y' and QUA_order_master.is_active='Y' ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public DataTable getdetailbarter(string barterid)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from QUA_barter_master where Barter_id='" + barterid + "'   ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getbarter_not(string barterid, string item_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from QUA_barter_master where item_id='" + item_id + "'and Barter_id not in('" + barterid + "') ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable checkbalance(string member_id, string price)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from dbo.fn_token_balance where member_id='" + member_id + "' and  balance_token>'" + price + "' ");


            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable advanceSearch(string name, string uni)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("     SELECT     QUA_posting_master.Name_title, QUA_posting_master.categories_id, QUA_Categories_master.name ");
            sb.Append(" FROM         QUA_posting_master INNER JOIN ");
            sb.Append(" QUA_Categories_master ON QUA_posting_master.categories_id = QUA_Categories_master.categories_id ");
            sb.Append(" where  QUA_posting_master.status in('A','B') and QUA_posting_master.University_id='" + uni + "' and  QUA_posting_master.Name_title like '%" + name + "%'  ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable order_data(string id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select * from dbo.QUA_order_master where Order_id='" + id + "' ");
            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable advanceSearch_data_list(string name, string cat, string sr_no, string email, string uni)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append("  select    total_rows,sr_no,item_id,name_title,member_id,description,categories_id,price,   ");
            sb.Append("  small_image_path,big_name_path ,BlobId,first_name,last_name,image, ");
            sb.Append("  ISNULL((SELECT 'TRUE' FROM QUA_wish_list as w WHERE w.item_id =t.item_id and member_id='" + email + "' ),'FALSE') AS isWishlist   ");
            sb.Append(" from   (    ");
            sb.Append(" select  ( ");
            sb.Append("  select COUNT(*) from QUA_posting_master  ");
            sb.Append(" inner join member_master on QUA_posting_master.Member_id=member_master.email_id  ");
            sb.Append("  where status in('A','B')   ");
            sb.Append("  and QUA_posting_master.is_active='Y'   ");

            sb.Append(" and QUA_posting_master.University_id='" + uni + "' and name_title like '%" + name + "%'  ");
            if (cat != "")
                sb.Append("  and categories_id='" + cat + "' ");

            sb.Append("  ) total_rows, ");
            sb.Append(" ROW_NUMBER() over(order by date_of_post desc) sr_no,*   from QUA_posting_master   ");
            sb.Append(" where QUA_posting_master.University_id='" + uni + "' and   status in('A','B')  and is_active='Y' ");

            sb.Append("  and name_title like '%" + name + "%' ");
            if (cat != "")
                sb.Append("  and categories_id='" + cat + "' ");
            sb.Append("   ) as t     ");
            sb.Append("     inner join member_master on t.Member_id=member_master.email_id  where  ");
            sb.Append("  status in('A','B')   and t.is_active='Y'  ");

            sb.Append("  and name_title like '%" + name + "%' ");
            if (cat != "")
                sb.Append("  and categories_id='" + cat + "' ");



            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable itemwisebarter_data(string itemid)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append(" select  * from QUA_barter_master where item_id='" + itemid + "' ");
            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable Gharph_QUA(string member_id, string year)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            //sb.Append(" SELECT YEAR(doc_date) [Year], MONTH(doc_date) [Month],");

            //sb.Append(" DATENAME(MONTH,doc_date) [Month Name], COUNT(1) [Sales Count],ref_transtion_type,SUM(amount) as amt ");

            //sb.Append(" FROM fn_token_transtion ");

            //sb.Append(" Where ref_transtion_type in ('Buy','sell_charge') and member_id='" + member_id + "' ");

            //sb.Append(" GROUP BY YEAR(doc_date), MONTH(doc_date),  ");

            //sb.Append(" DATENAME(MONTH, doc_date),ref_transtion_type ");

            //sb.Append(" ORDER BY 2");



            sb.Append(" SELECT  A.Year, A.Month, A.MonthName, ISNULL(B.SalesCount, 0) AS SalesCount,  ");
            sb.Append(" ISNULL(B.PurchaseAmount, 0) AS PurchaseAmount,ISNULL(B.SalesAmount, 0) AS SalesAmount FROM ");
            sb.Append(" (SELECT " + year + " AS Year, number AS Month, LEFT( DATENAME(MONTH, '2016-' + CAST(number as varchar(2)) + '-1'),3) MonthName ");
            sb.Append(" FROM master..spt_values ");
            sb.Append(" WHERE Type = 'P' and number between 1 and 12) AS A LEFT OUTER JOIN  ");

            sb.Append(" (SELECT YEAR(doc_date) Year, MONTH(doc_date) Month, ");
            sb.Append(" DATENAME(MONTH,doc_date) MonthName, COUNT(1) SalesCount, ");
            sb.Append(" SUM(CASE WHEN ref_transtion_type='purchase' THEN amount ELSE 0 END) AS PurchaseAmount, ");
            sb.Append(" SUM(CASE WHEN ref_transtion_type='Sales' THEN amount ELSE 0 END) AS SalesAmount  ");
            sb.Append(" FROM fn_token_transtion  ");
            sb.Append(" WHERE ref_transtion_type in ('Sales','purchase') and member_id='" + member_id + "' and YEAR(doc_date)=" + year + " ");

            sb.Append(" GROUP BY YEAR(doc_date), MONTH(doc_date),  DATENAME(MONTH, doc_date) ) AS B ");
            sb.Append(" ON A.Year = B.Year AND A.Month = B.Month ");
            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getmultipleImage(string itemId)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("select * from dbo.QUA_image_parameter where Item_id='" + itemId + "' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getitemdetailforchat(string member_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();

            sb.Append("select item_id,name_title from  dbo.QUA_posting_master where member_id='" + member_id + "' ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getUserImageAndproductName(string member_id, string product_id)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append(" select * from (select Name_title from dbo.QUA_posting_master where item_id='" + product_id + "' ) as a ");

            sb.Append("  ,(select image from dbo.member_master where member_master.email_id='" + member_id + "') as b ");

            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetUserAlert(string uni)
        {
            DBDataAdpterObject.SelectCommand.Parameters.Clear();
            StringBuilder sb = new StringBuilder();


            sb.Append(" select * from dbo.member_master ");
            sb.Append(" where  ");
            sb.Append(" member_master.is_quantina='Y'  ");
            if (uni.ToString() != "" || uni.ToString() != null)
                sb.Append(" and member_master.university='" + uni + "' ");




            DBDataAdpterObject.SelectCommand.CommandText = sb.ToString();

            DataSet ds = new DataSet();
            try
            {
                DBDataAdpterObject.Fill(ds);
                if (ds.Tables[0].Rows.Count <= 0)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #endregion

        #region  save Quantaina method






        public BLReturnObject save_regstration(DataSet ds_UploadData, DataSet ds_transtration)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_General server = (DS_General)ds_UploadData;

                DS_Transtration server1 = (DS_Transtration)ds_transtration;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_General linkData = new DS_General();
                DS_Transtration linkData1 = new DS_Transtration();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String Doc_tob = "";
                Document objDocument = new Document();

                //for update the member_master table

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;


                for (int mprCnt = 0; mprCnt < server.member_master.Rows.Count; mprCnt++)
                {
                    objDocument.W_GetNextDocumentNo(ref DBCommand, "MM", "", "", ref DocNo, ref message);

                    linkData.member_master.ImportRow(server.member_master.Rows[mprCnt]);
                    linkData.member_master[mprCnt].member_code = DocNo;
                }

                objDocument.W_GetNextDocumentNo(ref DBCommand, "TOB", "", "", ref Doc_tob, ref message);

                linkData1.fn_token_balance.ImportRow(server1.fn_token_balance.Rows[0]);
                linkData1.fn_token_balance[0].doc_no = Doc_tob;

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData1.fn_token_balance, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData1.fn_token_balance.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject update_member(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_General server = (DS_General)ds_UploadData;


                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_General linkData = new DS_General();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String Doc_tob = "";
                Document objDocument = new Document();

                //for update the member_master table

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;


                for (int mprCnt = 0; mprCnt < server.member_master.Rows.Count; mprCnt++)
                {


                    linkData.member_master.ImportRow(server.member_master.Rows[mprCnt]);

                }


                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject save_poststaff(DataSet ds_UploadData, DataSet ds_Transation)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_General server = (DS_General)ds_UploadData;
                DS_Transtration server1 = (DS_Transtration)ds_Transation;
                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_General linkData = new DS_General();
                DS_Transtration linkdata1 = new DS_Transtration();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String fn_tran_doc = "";
                String img_doc = "";

                Document objDocument = new Document();

                //for update the member_master table

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;

                if (server.QUA_posting_master.Rows.Count > 0)
                {
                    objDocument.QUA_GetNextDocumentNo(ref DBCommand, "PS", "", "", ref DocNo, ref message);

                    for (int mprCnt = 0; mprCnt < server.QUA_posting_master.Rows.Count; mprCnt++)
                    {
                        linkData.QUA_posting_master.ImportRow(server.QUA_posting_master.Rows[mprCnt]);
                        linkData.QUA_posting_master[mprCnt].item_id = DocNo;
                    }
                }
                if (server.QUA_image_parameter.Rows.Count > 0)
                {
                    for (int mprCntImage = 0; mprCntImage < server.QUA_image_parameter.Rows.Count; mprCntImage++)
                    {
                        objDocument.QUA_GetNextDocumentNo(ref DBCommand, "Img    ", "", "", ref img_doc, ref message);

                        linkData.QUA_image_parameter.ImportRow(server.QUA_image_parameter.Rows[mprCntImage]);
                        linkData.QUA_image_parameter[mprCntImage].Item_id = DocNo;
                        linkData.QUA_image_parameter[mprCntImage].SrNo = img_doc;

                    }
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_posting_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_posting_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_image_parameter, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_image_parameter.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                if (server1.fn_token_transtion.Rows.Count > 0)
                {
                    for (int mprCnte = 0; mprCnte < server1.fn_token_transtion.Rows.Count; mprCnte++)
                    {
                        objDocument.W_GetNextDocumentNo(ref DBCommand, "FTT ", "", "", ref fn_tran_doc, ref message);
                        linkdata1.fn_token_transtion.ImportRow(server1.fn_token_transtion.Rows[mprCnte]);
                        linkdata1.fn_token_transtion[mprCnte]["doc_no"] = fn_tran_doc;
                        linkdata1.fn_token_transtion[mprCnte]["ref_transtion_doc_no"] = DocNo;

                    }
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_token_transtion, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_token_transtion.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                if (server1.fn_token_balance.Rows.Count > 0)
                {
                    for (int mprCnte = 0; mprCnte < server1.fn_token_balance.Rows.Count; mprCnte++)
                    {

                        linkdata1.fn_token_balance.ImportRow(server1.fn_token_balance.Rows[mprCnte]);


                    }
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_token_balance, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_token_balance.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject save_barter(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_General server = (DS_General)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_General linkData = new DS_General();

                Document Doc = new Document();
                String message = "";
                String Brtid = "";
                String notid = "";
                String alte_doc = "";

                Document objDocument = new Document();

                //for update the member_master table

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;

                objDocument.QUA_GetNextDocumentNo(ref DBCommand, "BAT", "", "", ref Brtid, ref message);

                for (int mprCnt = 0; mprCnt < server.QUA_barter_master.Rows.Count; mprCnt++)
                {
                    linkData.QUA_barter_master.ImportRow(server.QUA_barter_master.Rows[mprCnt]);
                    linkData.QUA_barter_master[mprCnt].Barter_id = Brtid;
                }

                if (server.QUA_posting_master.Rows.Count > 0)
                {
                    linkData.QUA_posting_master.ImportRow(server.QUA_posting_master.Rows[0]);
                }

                #region create notification
                if (server.QUA_notification.Rows.Count > 0)
                {
                    objDocument.QUA_GetNextDocumentNo(ref DBCommand, "NTQ  ", "", "", ref notid, ref message);
                    linkData.QUA_notification.ImportRow(server.QUA_notification.Rows[0]);
                    linkData.QUA_notification[0].doc_no = notid;
                    linkData.QUA_notification[0].placeholder = Brtid;
                }
                #endregion
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_barter_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_barter_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_notification, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_notification.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                #region create notification
                if (server.alertmessage.Rows.Count > 0)
                {
                    for (int pushcount = 0; pushcount < server.alertmessage.Rows.Count; pushcount++)
                    {
                        objDocument.QUA_GetNextDocumentNo(ref DBCommand, "AlT    ", "", "", ref alte_doc, ref message);
                        linkData.alertmessage.ImportRow(server.alertmessage.Rows[0]);
                        linkData.alertmessage[0].Doc_no = alte_doc;
                        //linkData.QUA_notification[0].placeholder = order_doc;
                    }
                }
                #endregion
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_posting_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_posting_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        public BLReturnObject add_list(DataSet ds_UploadData, bool flage)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_General server = (DS_General)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_General linkData = new DS_General();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String fn_tran_doc = "";

                Document objDocument = new Document();

                //for update the member_master table

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;

                if (flage == false)
                {
                    objDocument.QUA_GetNextDocumentNo(ref DBCommand, "WL", "", "", ref DocNo, ref message);

                    for (int mprCnt = 0; mprCnt < server.QUA_wish_list.Rows.Count; mprCnt++)
                    {
                        linkData.QUA_wish_list.ImportRow(server.QUA_wish_list.Rows[mprCnt]);
                        linkData.QUA_wish_list[mprCnt].Doc_id = DocNo;
                    }
                }
                if (flage == true)
                {
                    linkData.QUA_wish_list.ImportRow(server.QUA_wish_list.Rows[0]);
                    linkData.QUA_wish_list.Rows[0].Delete();

                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_wish_list, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_wish_list.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }


                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }


        public BLReturnObject email_template(DataSet ds_UploadData, bool flage)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_General server = (DS_General)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_General linkData = new DS_General();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String fn_tran_doc = "";

                Document objDocument = new Document();

                //for update the member_master table

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;

                objDocument.QUA_GetNextDocumentNo(ref DBCommand, "EM   ", "", "", ref DocNo, ref message);

                linkData.email_template.ImportRow(server.email_template.Rows[0]);
                linkData.email_template[0].sr_no = DocNo;



                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.email_template, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.email_template.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }


                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }


        public BLReturnObject Buy_data(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_General server = (DS_General)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_General linkData = new DS_General();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String order_doc = "";
                String alte_doc1 = "";
                Document objDocument = new Document();

                //for update the member_master table

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;


                #region post update

                linkData.QUA_posting_master.ImportRow(server.QUA_posting_master.Rows[0]);

                #endregion

                #region create order
                if (server.QUA_order_master.Rows.Count > 0)
                {
                    objDocument.QUA_GetNextDocumentNo(ref DBCommand, "ORD ", "", "", ref order_doc, ref message);
                    linkData.QUA_order_master.ImportRow(server.QUA_order_master.Rows[0]);
                    linkData.QUA_order_master[0].Order_id = order_doc;
                }
                #endregion
                #region create notification
                if (server.alertmessage.Rows.Count > 0)
                {
                    for (int pushcount = 0; pushcount < server.alertmessage.Rows.Count; pushcount++)
                    {
                        objDocument.QUA_GetNextDocumentNo(ref DBCommand, "AlT    ", "", "", ref alte_doc1, ref message);
                        linkData.alertmessage.ImportRow(server.alertmessage.Rows[0]);
                        linkData.alertmessage[0].Doc_no = alte_doc1;
                        //linkData.QUA_notification[0].placeholder = order_doc;
                    }
                }
                #endregion
                #region create notification
                if (server.QUA_notification.Rows.Count > 0)
                {
                    objDocument.QUA_GetNextDocumentNo(ref DBCommand, "NTQ  ", "", "", ref DocNo, ref message);
                    linkData.QUA_notification.ImportRow(server.QUA_notification.Rows[0]);
                    linkData.QUA_notification[0].doc_no = DocNo;
                    linkData.QUA_notification[0].placeholder = order_doc;
                }
                #endregion




                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_posting_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_posting_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }


                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_order_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_order_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_notification, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_notification.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject save_feedback(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_General server = (DS_General)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_General linkData = new DS_General();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String fb_doc = "";

                Document objDocument = new Document();

                //for update the member_master table

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;


                #region particetpion update

                if (server.QUA_feedback_participants.Rows.Count > 0)
                    linkData.QUA_feedback_participants.ImportRow(server.QUA_feedback_participants.Rows[0]);

                #endregion

                #region create order
                if (server.QUA_feedback.Rows.Count > 0)
                {
                    objDocument.QUA_GetNextDocumentNo(ref DBCommand, "FB", "", "", ref fb_doc, ref message);
                    linkData.QUA_feedback.ImportRow(server.QUA_feedback.Rows[0]);
                    linkData.QUA_feedback[0].sr_no = fb_doc;
                }
                #endregion

                //#region create notification
                //if (server.QUA_notification.Rows.Count > 0)
                //{
                //    objDocument.QUA_GetNextDocumentNo(ref DBCommand, "NTQ  ", "", "", ref DocNo, ref message);
                //    linkData.QUA_notification.ImportRow(server.QUA_notification.Rows[0]);
                //    linkData.QUA_notification[0].doc_no = DocNo;
                //    linkData.QUA_notification[0].placeholder = order_doc;
                //}
                //#endregion




                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_feedback_participants, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_feedback_participants.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }


                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_feedback, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_feedback.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }


                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject updatemember(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_General server = (DS_General)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_General linkData = new DS_General();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String order_doc = "";

                Document objDocument = new Document();

                //for update the member_master table

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;


                #region post update

                linkData.member_master.ImportRow(server.member_master.Rows[0]);

                #endregion


                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.member_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.member_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }




                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject update_notification(DataSet ds_UploadData)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_General server = (DS_General)ds_UploadData;

                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_General linkData = new DS_General();

                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String order_doc = "";

                Document objDocument = new Document();

                //for update the member_master table

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;


                #region post update

                linkData.QUA_notification.ImportRow(server.QUA_notification.Rows[0]);

                #endregion


                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_notification, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_notification.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject Buy_data_order_complated(DataSet ds_UploadData, DataSet ds_Transation)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_General server = (DS_General)ds_UploadData;
                DS_Transtration server1 = (DS_Transtration)ds_Transation;
                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_General linkData = new DS_General();
                DS_Transtration linkdata1 = new DS_Transtration();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String order_doc = "";
                String alts_doc = "";
                String fn_tran_doc = "";
                Document objDocument = new Document();

                //for update the member_master table

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;


                #region post update
                if (server.QUA_posting_master.Rows.Count > 0)
                {
                    linkData.QUA_posting_master.ImportRow(server.QUA_posting_master.Rows[0]);
                }
                if (server.QUA_feedback_participants.Rows.Count > 0)
                {
                    for (int pushcountF = 0; pushcountF < server.QUA_feedback_participants.Rows.Count; pushcountF++)
                    {
                        linkData.QUA_feedback_participants.ImportRow(server.QUA_feedback_participants.Rows[pushcountF]);
                    }
                }
                #endregion

                #region create order
                if (server.QUA_order_master.Rows.Count > 0)
                {

                    linkData.QUA_order_master.ImportRow(server.QUA_order_master.Rows[0]);

                }
                #endregion

                #region create notification
                if (server.QUA_notification.Rows.Count > 0)
                {
                    for (int msgcount = 0; msgcount < server.QUA_notification.Rows.Count; msgcount++)
                    {
                        objDocument.QUA_GetNextDocumentNo(ref DBCommand, "NTQ  ", "", "", ref DocNo, ref message);
                        linkData.QUA_notification.ImportRow(server.QUA_notification.Rows[msgcount]);
                        linkData.QUA_notification[msgcount].doc_no = DocNo;
                        // linkData.QUA_notification[0].placeholder = order_doc;
                    }
                }
                #endregion

                if (server.QUA_barter_master.Rows.Count > 0)
                {
                    for (int msgcountB = 0; msgcountB < server.QUA_barter_master.Rows.Count; msgcountB++)
                    {
                        //objDocument.QUA_GetNextDocumentNo(ref DBCommand, "NTQ  ", "", "", ref DocNo, ref message);
                        linkData.QUA_barter_master.ImportRow(server.QUA_barter_master.Rows[msgcountB]);
                        //linkData.QUA_barter_master[msgcountB].doc_no = DocNo;
                        // linkData.QUA_notification[0].placeholder = order_doc;
                    }
                }
                #region push notification
                if (server.alertmessage.Rows.Count > 0)
                {
                    for (int pushcount = 0; pushcount < server.alertmessage.Rows.Count; pushcount++)
                    {
                        objDocument.QUA_GetNextDocumentNo(ref DBCommand, "AlT ", "", "", ref alts_doc, ref message);
                        linkData.alertmessage.ImportRow(server.alertmessage.Rows[pushcount]);
                        linkData.alertmessage[pushcount].Doc_no = alts_doc;
                    }
                    /// linkData.QUA_notification[0].placeholder = order_doc;
                }
                #endregion


                if (server1.fn_token_transtion.Rows.Count > 0)
                {
                    for (int mprCnte = 0; mprCnte < server1.fn_token_transtion.Rows.Count; mprCnte++)
                    {
                        objDocument.W_GetNextDocumentNo(ref DBCommand, "FTT ", "", "", ref fn_tran_doc, ref message);
                        linkdata1.fn_token_transtion.ImportRow(server1.fn_token_transtion.Rows[mprCnte]);
                        linkdata1.fn_token_transtion[mprCnte]["doc_no"] = fn_tran_doc;
                        linkdata1.fn_token_transtion[mprCnte]["ref_transtion_doc_no"] = DocNo;

                    }
                }
                if (server1.fn_token_balance.Rows.Count > 0)
                {
                    for (int mprCnte = 0; mprCnte < server1.fn_token_balance.Rows.Count; mprCnte++)
                    {

                        linkdata1.fn_token_balance.ImportRow(server1.fn_token_balance.Rows[mprCnte]);


                    }
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_token_transtion, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_token_transtion.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_feedback_participants, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_feedback_participants.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_posting_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_posting_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_order_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_order_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_notification, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_notification.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_barter_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_barter_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_token_balance, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_token_balance.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }

        public BLReturnObject barter_data_order_complated(DataSet ds_UploadData, DataSet ds_Transation)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();
            try
            {
                if (ds_UploadData == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                //  objDS_MasterTables.member_master = ds_UploadData.Tables["member_master"];

                DS_General server = (DS_General)ds_UploadData;
                DS_Transtration server1 = (DS_Transtration)ds_Transation;
                DBConnection.Open();
                DBCommand.Transaction = DBConnection.BeginTransaction();
                DS_General linkData = new DS_General();
                DS_Transtration linkdata1 = new DS_Transtration();
                Document Doc = new Document();
                String message = "";
                String DocNo = "";
                String order_doc = "";
                String fn_tran_doc = "";
                String alte_doc = "";
                Document objDocument = new Document();

                //for update the member_master table

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;


                #region post update
                if (server.QUA_posting_master.Rows.Count > 0)
                {
                    linkData.QUA_posting_master.ImportRow(server.QUA_posting_master.Rows[0]);
                }
                #endregion

                #region create order
                if (server.QUA_order_master.Rows.Count > 0)
                {
                    linkData.QUA_order_master.ImportRow(server.QUA_order_master.Rows[0]);

                }
                #endregion

                #region create notification
                if (server.QUA_notification.Rows.Count > 0)
                {
                    for (int pushcount1 = 0; pushcount1 < server.alertmessage.Rows.Count; pushcount1++)
                    {
                        objDocument.QUA_GetNextDocumentNo(ref DBCommand, "NTQ  ", "", "", ref DocNo, ref message);
                        linkData.QUA_notification.ImportRow(server.QUA_notification.Rows[pushcount1]);
                        linkData.QUA_notification[pushcount1].doc_no = DocNo;
                        //linkData.QUA_notification[0].placeholder = order_doc;
                    }
                }
                #endregion
                #region create notification
                if (server.alertmessage.Rows.Count > 0)
                {
                    for (int pushcount = 0; pushcount < server.alertmessage.Rows.Count; pushcount++)
                    {
                        objDocument.QUA_GetNextDocumentNo(ref DBCommand, "AlT    ", "", "", ref alte_doc, ref message);
                        linkData.alertmessage.ImportRow(server.alertmessage.Rows[pushcount]);
                        linkData.alertmessage[pushcount].Doc_no = alte_doc;
                        //linkData.QUA_notification[0].placeholder = order_doc;
                    }
                }
                #endregion
                #region barter update

                if (server.QUA_barter_master.Rows.Count > 0)
                {
                    for (int mprCntei = 0; mprCntei < server.QUA_barter_master.Rows.Count; mprCntei++)
                    {
                        //  objDocument.QUA_GetNextDocumentNo(ref DBCommand, "NTQ  ", "", "", ref DocNo, ref message);
                        linkData.QUA_barter_master.ImportRow(server.QUA_barter_master.Rows[mprCntei]);
                        //   linkData.QUA_notification[0].doc_no = DocNo;
                        //  linkData.QUA_notification[0].placeholder = order_doc;
                    }
                }
                #endregion

                if (server1.fn_token_transtion.Rows.Count > 0)
                {
                    for (int mprCnte = 0; mprCnte < server1.fn_token_transtion.Rows.Count; mprCnte++)
                    {
                        objDocument.W_GetNextDocumentNo(ref DBCommand, "FTT ", "", "", ref fn_tran_doc, ref message);
                        linkdata1.fn_token_transtion.ImportRow(server1.fn_token_transtion.Rows[mprCnte]);
                        linkdata1.fn_token_transtion[mprCnte]["doc_no"] = fn_tran_doc;
                        //linkdata1.fn_token_transtion[mprCnte]["ref_transtion_doc_no"] = DocNo;

                    }
                }
                if (server1.fn_token_balance.Rows.Count > 0)
                {
                    for (int mprCnte = 0; mprCnte < server1.fn_token_balance.Rows.Count; mprCnte++)
                    {

                        linkdata1.fn_token_balance.ImportRow(server1.fn_token_balance.Rows[mprCnte]);


                    }
                }
                if (server.QUA_feedback_participants.Rows.Count > 0)
                {
                    for (int mprCnteF = 0; mprCnteF < server.QUA_feedback_participants.Rows.Count; mprCnteF++)
                    {
                        linkData.QUA_feedback_participants.ImportRow(server.QUA_feedback_participants.Rows[mprCnteF]);
                    }
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_token_transtion, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_token_transtion.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_feedback_participants, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_feedback_participants.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.alertmessage, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.alertmessage.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_barter_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_barter_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }


                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_posting_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_posting_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }


                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_order_master, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_order_master.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkData.QUA_notification, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkData.QUA_notification.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, linkdata1.fn_token_balance, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == true && objUpdateTableInfo.TotalRowsAffected != linkdata1.fn_token_balance.Rows.Count)
                {
                    DBCommand.Transaction.Rollback();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "Fail to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    DBCommand.Transaction.Commit();
                    if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";

                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                DBCommand.Transaction.Rollback();
                if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = "Fail to Save Details.";
                return objBLReturnObject;
            }
        }
        #endregion
        public BLReturnObject UpdateTables(DataTable dt_update, ref IDbCommand DBCommand)
        {
            BLReturnObject objBLReturnObject = new BLReturnObject();

            try
            {
                if (dt_update == null)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    objBLReturnObject.ServerMessage = "There is no data to save";
                    return objBLReturnObject;
                }

                BLGeneralUtil.UpdateTableInfo objUpdateTableInfo;

                objUpdateTableInfo = BLGeneralUtil.UpdateTable(ref DBCommand, dt_update, BLGeneralUtil.UpdateWhereMode.KeyColumnsOnly, BLGeneralUtil.UpdateMethod.DeleteAndInsert);
                if (objUpdateTableInfo.Status == false || objUpdateTableInfo.TotalRowsAffected != dt_update.Rows.Count)
                {
                    objBLReturnObject.ExecutionStatus = 2;
                    ServerLog.Log(objUpdateTableInfo.ErrorMessage);
                    objBLReturnObject.ServerMessage = "Failed to Save Details.";
                    return objBLReturnObject;
                }
                else
                {
                    objBLReturnObject.ExecutionStatus = 1;
                    objBLReturnObject.ServerMessage = "Data Saved Successfully";
                    return objBLReturnObject;
                }

            }
            catch (Exception ex)
            {
                ServerLog.Log(ex.Message.ToString() + Environment.NewLine + ex.StackTrace);
                //if (DBConnection.State == ConnectionState.Open) DBConnection.Close();
                objBLReturnObject.ExecutionStatus = 2;
                objBLReturnObject.ServerMessage = ex.Message.ToString() + Environment.NewLine + ex.StackTrace;
                return objBLReturnObject;
            }
        }
    }
}