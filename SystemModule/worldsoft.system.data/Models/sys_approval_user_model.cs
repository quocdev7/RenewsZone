using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_approval_user_model
    {
        public sys_approval_user_model()
        {
            db = new User();

            list_user_education = new List<sys_education_user_model>();
            list_user_experience = new List<sys_experience_user_model>();
            list_user_work_history = new List<sys_work_history_user_model>();
            list_user_certificate = new List<sys_certificate_user_model>();
            list_user_success = new List<sys_success_user_model>();
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

        public string capcha { get; set; }
        public int? type_update { get; set; }


        public List<sys_education_user_model> list_user_education { get; set; }
        public List<sys_experience_user_model> list_user_experience { get; set; }
        public List<sys_work_history_user_model> list_user_work_history { get; set; }
        public List<sys_certificate_user_model> list_user_certificate { get; set; }
        public List<sys_success_user_model> list_user_success { get; set; }


    }

 
}
