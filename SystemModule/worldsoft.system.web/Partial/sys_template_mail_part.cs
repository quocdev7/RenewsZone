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
    partial class sys_template_mailController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_template_mail",
            icon = "email",
            module = "system",
            id = "sys_template_mail",
            url = "/sys_template_mail_index",
            title = "sys_template_mail",
            translate = "NAV.sys_template_mail",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_template_mail;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_template_mail;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_template_mail;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_template_mail;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_template_mail;edit",
                           "sys_template_mail;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_template_mail;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_template_mail;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_template_mail;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_template_mail;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_template_mail_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_template_mail_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_template_mail_model item)
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
