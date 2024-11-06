using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_logController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_log",
            icon = "menu",
            module = "system",
            id = "sys_log",
            url = "/sys_log_index",
            title = "sys_log",
            translate = "NAV.sys_log",
            type = "item",
            list_controller_action_public = new List<string>() {
                "sys_log;getListTask"
            },
            list_role = new List<ControllerRoleModel>()
            {

                  new ControllerRoleModel()
                {
                    id="sys_log;view",
                    name="view",
                    list_controller_action = new List<string>()
                    {
                          "sys_log;getLog",
                    }
                }
            }
        };

     

    }
}
