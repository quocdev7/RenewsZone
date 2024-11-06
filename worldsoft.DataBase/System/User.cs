using System;

namespace worldsoft.DataBase.System
{
    public class User
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string avatar_path { get; set; }
        public string cover_image { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string id_department { get; set; }
        public string id_job_title { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string dia_chi { get; set; }
        public string position { get; set; }
        public int? status_del { get; set; }
        // 1 admin user, 2 normal user
        public int? type { get; set; }
         public string id_khoa { get; set; }
        public string token_reset_pass { get; set; }
        public DateTime? expiration_date_reset_pass { get; set; }
        public int? sex { get; set; }
        public DateTime? date_of_birth { get; set; }
        public int? school_year { get; set; }
        public string id_company { get; set; }
        
        public string token_notification { get; set; }
        public int? status_graduate { get; set; }


        public string nguoi_duyet { get; set; }
        public DateTime? ngay_duyet { get; set; }


        public string facebook_link { get; set; }
        public string linkedin_link { get; set; }
        public string youtube_link { get; set; }
        public string website_link { get; set; }
        public string instagram_link { get; set; }
        public string twitter_link { get; set; }
        public string full_name { get; set; }
        public string cv_link { get; set; }
        //public string cv_name { get; set; }
        //public string cv_file_size { get; set; }
        //public string cv_file_type { get; set; }
        public string otp { get; set; }
        public DateTime? time_input { get; set; }
        public string ly_do { get; set; }
        public string company { get; set; }

    }
}