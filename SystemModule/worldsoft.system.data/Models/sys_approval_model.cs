using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_approval_model
    {
        public sys_approval_model()
        {
            db = new sys_approval_db();
            list_item = new List<sys_approval_detail_model>();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_approval_db db { get; set; }
        public List<sys_approval_detail_model> list_item { get; set; }
        public List<sys_approval_step_model> list_step { get; set; }
    }
    public class sys_approval_detail_model
    {
        public sys_approval_detail_model()
        {
            db = new sys_approval_detail_db();
        }
        public string from_user_name { get; set; }
        public string to_user_name { get; set; }
        public sys_approval_detail_db db { get; set; }
    }

    public class sys_approval_step_model
    {
        public sys_approval_step_model()
        {
            db = new sys_approval_step_db();
        }
        public string user_name { get; set; }
        public sys_approval_step_db db { get; set; }
    }
}
