using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_loai_doi_tac_language")]
    public class sys_loai_doi_tac_language_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string name { get; set; }
        public string id_loai_doi_tac { get; set; }
    }
   
}
