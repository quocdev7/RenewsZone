using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_news_model
    {
        public sys_news_model()
        {
            db = new sys_news_db();

        }
        public sys_news_db db { get; set; }
        public string create_by_name { get; set; }

        public string ten_tien_te { get; set; }

        public string updateby_name { get; set; }
        public string aprroval_by_name { get; set; }
        public string group_news_name { get; set; }
        public string type_news_name { get; set; }
        public string code { get; set; }
        public string avatar { get; set; }
        public string hinh_anh { get; set; }
        public string image { get; set; }
        public string position { get; set; }
        
        public string ten_khoa { get; set; }
        public string quyen_rieng_tu { get; set; }
        public int? is_quan_tri { get; set; }


        public string tieu_de_language {get;set;}
        public string noi_dung_language { get; set; }
        public string noi_dung_language_mobile { get; set; }
        public string  noi_dung_trang_bia_language { get; set; }
        public string nguon_tin_tuc_language { get; set; }



        public List<string> khoa { get; set; }
        public List<string> hinhthuc { get; set; }

    }

    public class sys_news_group_model
    {

        public string id { get; set; }
        public string id_group_news { get; set; }
        public string id_type_news { get; set; }
        public string code { get; set; }
        public int? status_del { get; set; }

        public string group_news_name { get; set; }
        public string type_news_name { get; set; }
        public int? status { get; set; }
    }
    public class sys_news_comment_model
    {
        public long? id { get; set; }
        public string id_news { get; set; }
        public string comment { get; set; }
        public string user_id { get; set; }
        public DateTime? comment_date { get; set; }
        public string news_name { get; set; }
        public string user_comment { get; set; }

    }

    public class sys_news_ref
    {
        public int? count { get; set; }
        public string id_type_news { get; set; }
        public string type_news_name { get; set; }
        public string group_news_name { get; set; }
        public string id_group_news { get; set; }
    }
}
