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
    partial class sys_template_notificationController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_template_notification",
            icon = "email",
            module = "system",
            id = "sys_template_notification",
            url = "/sys_template_notification_index",
            title = "sys_template_notification",
            translate = "NAV.sys_template_notification",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_template_notification;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_template_notification;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_template_notification;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_template_notification;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_template_notification;edit",
                           "sys_template_notification;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_template_notification;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_template_notification;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_template_notification;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_template_notification;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_template_notification_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_template_notification_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_template_notification_model item)
        {
            if (item.db.id_type==null)
            {
                ModelState.AddModelError("db.id_type", "required");
            }
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (string.IsNullOrEmpty(item.db.template))
            {
                ModelState.AddModelError("db.template", "required");
            }
            var search = repo.FindAll().Where(d => d.db.id_type == item.db.id_type && d.db.id != item.db.id ).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.id_type", "existed");
            }
            return ModelState.IsValid;
        }

    }
}
