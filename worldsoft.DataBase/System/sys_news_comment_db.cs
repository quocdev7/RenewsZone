using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_news_comment")]
    public class sys_news_comment_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string id_news { get; set; }
        public string comment { get; set; }
        public string user_id { get; set; }
        public DateTime? comment_date { get; set; }
        public long? id_comment { get; set; }
        public int? status { get; set; }
    }
  
}
