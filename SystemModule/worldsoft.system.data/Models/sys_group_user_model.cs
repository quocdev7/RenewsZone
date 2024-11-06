using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_group_user_model
    {
        public sys_group_user_model()
        {
            db = new sys_group_user_db();
            list_item = new List<sys_group_user_detail_model>();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_group_user_db db { get; set; }
        public List<sys_group_user_detail_model> list_item { get; set; }
        public List<sys_group_user_role_model> list_role { get; set; }
    }
    public class sys_group_user_detail_model
    {
        public string user_name { get; set; }
        public string user_id { get; set; }
        public string department_name { get; set; }
        public string position_name { get; set; }
        public bool? isCheck { get; set; }
        public int? type_user { get; set; }
    }
    public class sys_group_user_role_model
    {
        public sys_group_user_role_model()
        {
            db = new sys_group_user_role_db();
        }
        public string user_name { get; set; }
        public sys_group_user_role_db db { get; set; }

    }
}
