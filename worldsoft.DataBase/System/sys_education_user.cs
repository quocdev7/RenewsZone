using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_education_user")]
    public class sys_education_user_db
    {
       
        public string id { get; set; }
        public string school { get; set; }
        public string degree { get; set; }
        public string speciality { get; set; }
        public string user_id { get; set; }

        public int? from_year { get; set; }
        public int? to_year { get; set; }

    }
   
}
