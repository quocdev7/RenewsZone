using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_anh_san_pham")]
    public class sys_anh_san_pham_db
    {
       
        public string id { get; set; }
        public string id_san_pham { get; set; }

        
        public string name { get; set; }
        public string link { get; set; }
        public string image { get; set; }
        public string image_mobile { get; set; }
        public string update_by { get; set; }
        public int? stt { get; set; }

        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }


    }
   
}
