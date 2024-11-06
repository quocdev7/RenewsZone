using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_doi_tac")]
    public class sys_doi_tac_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string note { get; set; }
        public string id_loai_doi_tac { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public string update_by { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }

        public int? stt { get; set; }
        public string link { get; set; }
    }
   
}
