using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_search")]
    public class sys_search_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string search_text { get; set; }
        public string search_text_language { get; set; }
        public int? type { get; set; }
        public string id_ref { get; set; }
        public DateTime? order_date { get; set; }
        public DateTime? create_date { get; set; }
    }
   
}
