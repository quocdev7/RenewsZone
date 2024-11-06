using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_work_history_user_model
    {
        public sys_work_history_user_model()
        {
            db = new sys_work_history_user_db();
        }
        public sys_work_history_user_db db { get; set; }
    }
}
