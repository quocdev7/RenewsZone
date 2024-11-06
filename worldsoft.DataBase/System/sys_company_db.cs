using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_company")]
    public class sys_company_db
    {
       
        public string id { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public string update_by { get; set; }
        public string logo { get; set; }
        public string phone_contact { get; set; }
        public string tax_num { get; set; }
        public string email { get; set; }
        public string initials { get; set; }
        public string address { get; set; }
        public string website { get; set; }
        public string country { get; set; }
        public string id_field { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
    }
   
}
