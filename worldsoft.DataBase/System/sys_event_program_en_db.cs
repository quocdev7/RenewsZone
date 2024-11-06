using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_event_program_en")]
    public class sys_event_program_en_db
    {
      
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public string id_event_program { get; set; }

    
    }
}
