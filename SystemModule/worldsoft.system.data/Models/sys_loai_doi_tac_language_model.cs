using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_loai_doi_tac_language_model
    {
        public sys_loai_doi_tac_language_model()
        {
            db = new sys_loai_doi_tac_language_db();
        }
        public sys_loai_doi_tac_language_db db { get; set; }
    }
}
