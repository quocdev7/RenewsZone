using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_event_Q_A_en")]
    public class sys_event_qa_en_db
    {
      
        public string id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string id_event_q_a { get; set; }
    }
}
