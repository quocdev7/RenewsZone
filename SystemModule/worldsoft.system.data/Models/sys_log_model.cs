using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
   
        public class sys_log_mail_model
    {
        public sys_log_mail_model()
            {
                db = new sys_log_mail_db();
            }
  
        public sys_log_mail_db db { get; set; }
        }
    
}
