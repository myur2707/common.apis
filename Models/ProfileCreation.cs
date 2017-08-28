using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiCampusConcierge.Models
{
    public class ProfileCreation
    {
        //public string userName { get; set; }
        //public string email_id { get; set; }
        //public string password { get; set; }
        public string university { get; set; }
        public string university_icon { get; set; }
        //public string member_code { get; set; }
        //public string university { get; set; }
        public string first_name { get; set; }

        public string middle_name { get; set; }
        public string nick_name { get; set; }
        public string short_bio { get; set; }

        public string fun_campus { get; set; }
        public string major { get; set; }

        public string last_name { get; set; }
        public string image { get; set; }
        public string email_id { get; set; }
        public string birthdate { get; set; }
        public string phone_number1 { get; set; }
        public string phone_number2 { get; set; }
        public string password { get; set; }
        public string email_rand_no { get; set; }
        public string token_id { get; set; }
        public string feternity_id { get; set; }
        public string soriety_id { get; set; }
        public string student_rating_id { get; set; }
        public string tutor_rating_id { get; set; }
        public string is_active { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string created_host { get; set; }
        public string last_modified_by { get; set; }
        public string last_modified_date { get; set; }
        public string last_modified_host { get; set; }
        public string classification { get; set; }

        public string security_question { get; set; }
        public string question_ans { get; set; }
        public string sr_no { get; set; }
        public string member_id { get; set; }
        public string role_id { get; set; }
        public string course_code { get; set; }
        public string is_approved { get; set; }
        public string approved_by_whome { get; set; }

        public string transprit_file_desc { get; set; }
        public string file_path { get; set; }
        public string default_role { get; set; }
    }

    public class course_list
    {
        public string[] course_list_learning { get; set; }
        public string[] course_list_tutoring { get; set; }
    }
   
    public class member_master
    {
        public string member_code { get; set; }
        public string university { get; set; }
        
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string image { get; set; }
        public string email_id { get; set; }
        public string birthdate { get; set; }
        public string phone_number1 { get; set; }
        public string phone_number2 { get; set; }
        public string password { get; set; }
        
        public string email_rand_no { get; set; }
        public string token_id { get; set; }

        public string feternity_id { get; set; }
        public string soriety_id { get; set; }
        public string student_rating_id { get; set; }
        public string tutor_rating_id { get; set; }
        
        public string is_active { get; set; }
        
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string created_host { get; set; }
        
        public string last_modified_by { get; set; }
        public string last_modified_date { get; set; }
        public string last_modified_host { get; set; }
    }

    public class member_role
    {
        public string sr_no { get; set; }
        public string university_id { get; set; }
        public string member_id { get; set; }
        public string role_id { get; set; }
        public string course_code { get; set; }
        public string is_approved { get; set; }
        public string approved_by_whome { get; set; }
        public string is_active { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string created_host { get; set; }
        public string last_modified_by { get; set; }
        public string last_modified_date { get; set; }
        public string last_modified_host { get; set; }
    }

    public class member_tutor_transcript
    {

        public string sr_no { get; set; }
        public string university_id { get; set; }
        public string member_id { get; set; }
        public string transprit_file_desc { get; set; }
        public string file_path { get; set; }

        public string is_active { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string created_host { get; set; }
        public string last_modified_by { get; set; }
        public string last_modified_date { get; set; }
        public string last_modified_host { get; set; }
    }
}
