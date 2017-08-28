using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiCampusConcierge.Models
{
    public class Dashboard
    {
        public string member_id { get; set; }
        public string member_role { get; set; }

    }
    public class emailSendFrined
    {
        public string[] email_list_send { get; set; }
    }

    public class sessiontime
    {

        public string session_id { get; set; }
        public string university_id { get; set; }
        public string session_type { get; set; }
        public string course_id { get; set; }
        public string session_date { get; set; }
        public string course_type { get; set; }
        public TimeSpan session_start_time { get; set; }
        public TimeSpan session_end_time { get; set; }
        public string duration { get; set; }
        public string start_confirm_tutor_datetime { get; set; }
        public string start_confirm_student_datetime { get; set; }
        public string end_confirm_tutor_datetime { get; set; }
        public string end_confirm_student_datetime { get; set; }
        public string session_start_confirm_by_tutor_id { get; set; }

        public string session_start_confirm_by_student_host { get; set; }
        public string session_end_confirm_by_student_id { get; set; }
        public string session_end_confirm_by_student_host { get; set; }
        public string session_venue { get; set; }
        public string Bookby { get; set; }
        public string payment_process_status { get; set; }
        public string is_active { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string created_host { get; set; }
        public string last_modified_by { get; set; }
        public string last_modified_date { get; set; }
        public string last_modified_host { get; set; }
    }

    public class rate_feedback
    {
        public Dictionary<string, string> email { get; set; }
        public Dictionary<string, string> rate { get; set; }
        public Dictionary<string, string> feedback { get; set; }
    }


    public class session_participants
    {

        public string sr_no { get; set; }
        public string university_id { get; set; }
        public string session_id { get; set; }
        public string member_id { get; set; }
        public string role_id { get; set; }
        public string is_survey_done_by_tutor { get; set; }
        public string is_survey_done_by_student { get; set; }
        public string course_id { get; set; }
        public string is_active { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string created_host { get; set; }
        public string last_modified_by { get; set; }
        public string last_modified_date { get; set; }
        public string last_modified_host { get; set; }

    }
    public class member
    {

        public string[] member_list { get; set; }

    }
    public class rate
    {

        public string amount { get; set; }
        public string peekrate { get; set; }
        public string discount { get; set; }

    }
}