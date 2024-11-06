using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_user_fileupload_model
    {
        public sys_user_fileupload_model()
        {
            db = new sys_user_fileupload_db();
        }
        public sys_user_fileupload_db db { get; set; }
    }
}
