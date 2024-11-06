using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_user_model
    {
        public sys_user_model()
        {
            db = new User();
            file = new sys_user_fileupload_model();
            list_user_education = new List<sys_education_user_model>();
            list_user_experience = new List<sys_experience_user_model>();
            list_user_work_history = new List<sys_work_history_user_model>();
            list_user_certificate = new List<sys_certificate_user_model>();
            list_user_success = new List<sys_success_user_model>();
            badges = new Dictionary<string, int>();
            cau_hinh_quyen_rieng_tu = new sys_cau_hinh_quyen_rieng_tu_model();
            user_ung_tuyen = new sys_user_ung_tuyen_model();
        }
        public User db { get; set; }
        public string password { get; set; }
        public string repassword { get; set; }
        public string oldPassword { get; set; }
        public string job_title_name { get; set; }
        public string department_name { get; set; }
        public string company_name { get; set; }
        public string company_logo { get; set; }
        public string khoa_name { get; set; }
        public string full_name { get; set; }
        public int? action { get; set; }
        public string capcha { get; set; }
        public int? showCaptcha { get; set; }
        public int? type_update { get; set; }
        public bool? agree { get; set; }
        public bool? type { get; set; }

        public List<sys_education_user_model> list_user_education { get; set; }
        public List<sys_experience_user_model> list_user_experience { get; set; }
        public List<sys_work_history_user_model> list_user_work_history { get; set; }
        public List<sys_certificate_user_model> list_user_certificate { get; set; }
        public List<sys_success_user_model> list_user_success { get; set; }
        public sys_user_fileupload_model file { get; set; }
        public sys_cau_hinh_quyen_rieng_tu_model cau_hinh_quyen_rieng_tu { get; set; }

        public Dictionary<string,int> badges { get; set; }
        public sys_user_ung_tuyen_model user_ung_tuyen { get; set; }

    }

    public class sys_user_info_model
    {
        public User db { get; set; }
        public string password { get; set; }
        public string oldPassword { get; set; }
        public string job_title_name { get; set; }
        public string department_name { get; set; }
        public string company_name { get; set; }
        public string company_logo { get; set; }

        public string noi_xu_ly { get; set; }
        public string nation_name { get; set; }
        public string avatar_path { get; set; }
        public string full_name { get; set; }

        public string capcha { get; set; }

    }


    public class sys_user_register_model
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string password { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        
        public string phone { get; set; }
        public string email { get; set; }
        public int? status_del { get; set; }

        // 1 building user, 2 company user, 3 visitor user
        public int? type { get; set; }
        public string agreements { get; set; }
     


    }
    public class sys_experience_model
    {
        public string id { get; set; }
        public string description { get; set; }
        public string image { get; set; }
    }
    public class sys_success_model
    {
        public string id { get; set; }
        public string description { get; set; }
        public string image { get; set; }
    }
    public class sys_certificate_model
    {
        public string id { get; set; }
        public string description { get; set; }
        public string image { get; set; }
    }
    public class sys_work_history_model
    {
        public string id { get; set; }
        public string description { get; set; }
        public string image { get; set; }
    }
}
