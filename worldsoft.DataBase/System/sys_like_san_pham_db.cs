using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_like_san_pham")]
    public class sys_like_san_pham_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string email { get; set; }
        public string noi_dung { get; set; }

        public DateTime? create_date { get; set; }
    }
   
}
