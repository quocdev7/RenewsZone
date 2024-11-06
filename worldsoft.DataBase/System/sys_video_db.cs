using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_video")]
    public class sys_video_db
    {
       
        public string id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string link { get; set; }
        public string image { get; set; }
        public string image_mobile { get; set; }
        public string update_by { get; set; }
        public int? stt { get; set; }

        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
        public string image_thumnail { get; set; }
        
    }
   
}
