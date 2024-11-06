using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_user_typenews_model
    {
        public sys_user_typenews_model()
        {
            db = new sys_user_typenews_db();
            types = new List<string>();
        }
        public string full_name { get; set; }
        public string type_news_name { get; set; }
        public string khoa_name { get; set; }
        public string updateby_name { get; set; }
        public List<string> types { get; set; }
        public List<string> khoa { get; set; }

        public string type_news { get; set; }
        public sys_user_typenews_db db { get; set; }
    }
}
