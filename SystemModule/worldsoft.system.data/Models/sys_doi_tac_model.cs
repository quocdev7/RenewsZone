using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_doi_tac_model
    {
        public sys_doi_tac_model()
        {
            db = new sys_doi_tac_db();
        }
        public string name_en { get; set; }
        public string note_en { get; set; }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public string loai_doi_tac { get; set; }
        public string mau { get; set; }
        public sys_doi_tac_db db { get; set; }
    }

    public class sys_loai_doi_tac_ref_model
    {
        public sys_loai_doi_tac_ref_model()
        {
            db = new sys_loai_doi_tac_db();
            lst_doi_tac = new List<sys_doi_tac_model>();
         }
    
        public sys_loai_doi_tac_db db { get; set; }
        public string name_en { get; set; }
        public List<sys_doi_tac_model> lst_doi_tac { get; set; }
    }
}
