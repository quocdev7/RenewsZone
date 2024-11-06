﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_nang_luc_file_upload")]
    public class sys_nang_luc_file_upload_db
    {
       
        public string id { get; set; }
        public string id_nang_luc { get; set; }

        
        public string file_name { get; set; }
        public string file_path { get; set; }
        public long? file_size { get; set; }
        public string file_type { get; set; }

        public string note { get; set; }
        public string stt { get; set; }
        public string upload_by { get; set; }
        public DateTime? upload_date { get; set; }

        public string remove_by { get; set; }
        public DateTime? remove_date { get; set; }


        public int? status_del { get; set; }


    }
   
}