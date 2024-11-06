using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_user_ung_tuyen")]
    public class sys_user_ung_tuyen_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public decimal? muc_luong { get; set; }
        public string vi_tri { get; set; }
        public string hinh_thuc { get; set; }
        public string user_id { get; set; }
 
        public DateTime? update_date { get; set; }
        public string update_by { get; set; }
        public string tien_te { get; set; }

    }
   
}
