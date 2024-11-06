using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_group_news")]
    public class sys_group_news_db
    {
        public string id { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public string image { get; set; }
        public int? status_del { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public string create_by { get; set; }
        public string update_by { get; set; }
        public string code { get; set; }
        public int? stt { get; set; }
        public string name_en { get; set; }
    }
}
