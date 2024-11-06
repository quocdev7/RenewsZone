﻿using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_person_news_model
    {
        public sys_person_news_model()
        {
            db = new sys_news_db();
            
        }
        public sys_news_db db { get; set; }
        public string create_by_name { get; set; }
        public string group_news_name { get; set; }
        public string type_news_name { get; set; }
                public string code { get; set; }

    }

    public class sys_person_news_group_model
    {

        public string id { get; set; }
        public string id_group_news { get; set; }
        public string id_type_news { get; set; }
        public string code { get; set; }
        public int? status_del { get; set; }

        public string group_news_name { get; set; }
        public string type_news_name { get; set; }
        public int? status { get; set; }
    }

    public class sys_person_news_ref
    {
        public int? count { get; set; }
        public string id_type_news { get; set; }
        public string type_news_name { get; set; }
        public string group_news_name { get; set; }
        public string id_group_news { get; set; }
    }
}