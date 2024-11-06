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
    partial class sys_helpController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_help",
            icon = "live_help",
            module = "system",
            id = "sys_help",
            url = "/sys_help_index",
            title = "sys_help",
            translate = "NAV.sys_help",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_help;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_help;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_help;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_help;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_help;edit",
                           "sys_help;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_help;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_help;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_help;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_help;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_help_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_help_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_help_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }


            return ModelState.IsValid;
        }

    }
}
