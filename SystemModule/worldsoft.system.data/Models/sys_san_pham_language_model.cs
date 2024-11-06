using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_san_pham_language_model
    {
        public sys_san_pham_language_model()
        {
            db = new sys_san_pham_language_db();

        }
        public sys_san_pham_language_db db { get; set; }
    }




}
