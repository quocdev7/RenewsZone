using System;
using System.Collections.Generic;
using System.Text;

namespace worldsoft.DataBase.Function
{
	public class Fn_get_sys_approval
    {
		public string id { get; set; }
		public DateTime? deadline { get; set; }
		public int? status_action { get; set; }
		public int? status_finish { get; set; }
		public string to_user { get; set; }
		public string from_user { get; set; }


		public string to_user_name { get; set; }
		public string from_user_name { get; set; }
		public int? step_num { get; set; }
		public string step_name { get; set; }
		public string last_note { get; set; }
		public DateTime? last_date_action { get; set; }
	}

}
