using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_log_email")]
    public class sys_log_mail_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string tieu_de { get; set; }
        public string noi_dung { get; set; }
        public string user_id { get; set; }
        public int ket_qua { get; set; }

        public string email { get; set; }
        public DateTime? send_date { get; set; }
        public string id_template { get; set; }

    }
   
}
