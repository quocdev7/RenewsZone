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
    partial class sys_type_mailController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_type_mail",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_type_mail",
            url = "/sys_type_mail_index",
            title = "sys_type_mail",
            translate = "NAV.sys_type_mail",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_type_mail;getListUse",
                 
            },
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_type_mail;getListUse",

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_type_mail;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_type_mail;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_type_mail;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_type_mail;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_type_mail;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_type_mail;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_type_mail;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_type_mail;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_type_mail_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_type_mail_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_type_mail_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.code == item.db.code && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }


            return ModelState.IsValid;
        }

    }
}
