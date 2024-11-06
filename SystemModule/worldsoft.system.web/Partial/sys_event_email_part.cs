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
    partial class sys_event_emailController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_event_email",
            icon = "menu",
            module = "system",
            id = "sys_event_email",
            url = "/sys_event_email_index",
            title = "sys_event_email",
            translate = "NAV.sys_log_event_email",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_event_email;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                  new ControllerRoleModel()
                {
                    id="sys_event_email;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_email;DataHandler",
                    }
                }
            }
        };
        private  bool checkModelStateCreate(sys_event_email_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_event_email_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_event_email_model item)
        {
            if (string.IsNullOrEmpty(item.db.event_id))
            {
                ModelState.AddModelError("db.event_id", "required");
            }

            if (string.IsNullOrEmpty(item.db.id_template))
            {
                ModelState.AddModelError("db.id_template", "required");
            }
            if (string.IsNullOrEmpty(item.db.id_type_email))
            {
                ModelState.AddModelError("db.id_type_email", "required");
            }
            //var check_ma = repo.FindAll().Where(d => d.db.title == item.db.title && d.db.id != item.db.id && d.db.status_del == 1).Count();
            //if (check_ma > 0)
            //{
            //    ModelState.AddModelError("db.ma", "existed");
            //}

            //var check_ten = repo.FindAll().Where(d => d.db.title == item.db.title && d.db.id != item.db.id && d.db.status_del == 1).Count();
            //if (check_ten > 0)
            //{
            //    ModelState.AddModelError("db.ten", "existed");
            //}


            return ModelState.IsValid;
        }

    }
}
