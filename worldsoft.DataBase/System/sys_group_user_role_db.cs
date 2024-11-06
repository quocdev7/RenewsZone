using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_group_user_role")]
    public class sys_group_user_role_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string id_group_user{ get; set; }
        public string id_controller_role { get; set; }
        public string controller_name { get; set; }
        public string role_name { get; set; }
    }
}
