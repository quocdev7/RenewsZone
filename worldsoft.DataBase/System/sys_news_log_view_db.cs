using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_news_log_view")]
    public class sys_news_log_view_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string id_news { get; set; }
        public string user_id { get; set; }
        public DateTime? view_date { get; set; }
    }
}
