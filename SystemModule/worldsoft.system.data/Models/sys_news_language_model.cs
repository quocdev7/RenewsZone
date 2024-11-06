using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_news_language_model
    {
        public sys_news_language_model()
        {
            db = new sys_news_language_db();
         
        }
        public sys_news_language_db db { get; set; }
    }

  


}
