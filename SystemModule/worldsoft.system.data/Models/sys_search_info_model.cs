using System;
using System.Collections.Generic;
using System.Text;

namespace worldsoft.system.data.Models
{

    public class sys_search_info_model
    {
        public string id { get; set; }
        public int?  type_info { get; set; }
        public string title { get; set; }
        public string content_brief { get; set; }
        
        public string content { get; set; }
        public string image { get; set; }

        public DateTime? post_date { get; set; }
        public long? view_count { get; set; }
        public long? comment_count { get; set; }

        public string create_by_name { get; set; }
        public string group_news_name { get; set; }
        public string type_news_name { get; set; }
        public string code { get; set; }
        public string avatar { get; set; }
        public int? status_del { get; set; }
        public long? id_invite { get; set; }
        public string tieu_de_language { get; set; }
        public string nguon_tin_tuc_language { get; set; }
        public string noi_dung_trang_bia_language { get; set; }
        public string noi_dung_language { get; set; }
        

    }

  
}
