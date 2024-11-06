using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_dien_gia_en")]
    public class sys_dien_gia_en_db
    {
       
        public string id { get; set; }
        public string name { get; set; }
        public string chuc_danh { get; set; }
        public string cong_ty { get; set; }
        public string id_dien_gia { get; set; }

    }
   
}
