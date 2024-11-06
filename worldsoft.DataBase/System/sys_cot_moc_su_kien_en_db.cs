using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_cot_moc_su_kien_en")]
    public class sys_cot_moc_su_kien_language_db
    {
       
        public string id { get; set; }
        public string name { get; set; }

     
        public string time { get; set; }
        public string note { get; set; }
        public string note_mobile { get; set; }
        public string id_cot_moc { get; set; }
    }
   
}
