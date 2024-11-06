using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_group_news_model
    {
        public sys_group_news_model()
        {
            db = new sys_group_news_db();
            
        }
        public sys_group_news_db db { get; set; }
        public string create_by_name { get; set; }

    }
}
