using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_user_company")]
    public class sys_user_company_db
    {
        public string user_id { get; set; }
        public string id { get; set; }
        public string company_id { get; set; }
        public string note { get; set; }

        public string update_by { get; set; }
        public DateTime? update_date { get; set; }
    }

}
