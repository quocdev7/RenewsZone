using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_user_ung_tuyen_model
    {
        public sys_user_ung_tuyen_model()
        {
            db = new sys_user_ung_tuyen_db();
        }
        public sys_user_ung_tuyen_db db { get; set; }
        public string ten_tien_te { get; set; }
    }
}
