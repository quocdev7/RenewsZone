using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_event_en")]
    public class sys_event_en_db
    {
        public string id { get; set; }
        public string id_event { get; set; }
        public string title { get; set; }      
        public string location { get; set; }     
        public string mo_ta { get; set; }
        public string mo_ta_mobile { get; set; }
        public string ban_to_chuc { get; set; }
        public string luu_y_tham_gia { get; set; }
        public string luu_y_tham_gia_mobile { get; set; }
        public string dieu_kien_tham_gia { get; set; }
    }
}
