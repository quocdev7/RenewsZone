using System;
using System.Collections.Generic;
using System.Text;

namespace worldsoft.system.data.Models
{
    public class sys_common_model
    {

        public string id { get; set; }
        public string name { get; set; }



    }
    public class sys_common_ref_model
    {

        public int? id { get; set; }
        public string name { get; set; }



    }
    public class cell
    {

        public string value { get; set; }
    }
    public class row
    {
        public row()
        {
            list_cell = new List<cell>();
        }


        public string key { get; set; }
        public List<cell> list_cell { get; set; }
    }

}

