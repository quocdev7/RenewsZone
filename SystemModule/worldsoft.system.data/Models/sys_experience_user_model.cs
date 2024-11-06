using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_experience_user_model
    {
        public sys_experience_user_model()
        {
            db = new sys_experience_user_db();
        }
        public sys_experience_user_db db { get; set; }
    }
}
