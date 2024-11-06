using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_type_news_model
    {
        public sys_type_news_model()
        {
            db = new sys_type_news_db();
            
        }
        public sys_type_news_db db { get; set; }
        public string create_by_name { get; set; }
        public string group_news_name { get; set; }

    }
    public class sys_type_news_ref    
    {
        public string id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string type { get; set; }
        public int? count { get; set; }
        public string color { get; set; }
    }

    public class sys_view_type_news_model
    {
     
        public string id_type_news { get; set; }
        public string image { get; set; }
        public string type_news_name { get; set; }
        public sys_news_model hot_news { get; set; }
        public List<sys_news_model> lst_news { get; set; }
    }

}
