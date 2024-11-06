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
    partial class sys_help_detailController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_help_detail",
            icon = "support",
            module = "system",
            id = "sys_help_detail",
            url = "/sys_help_detail_index",
            title = "sys_help_detail",
            translate = "NAV.sys_help_detail",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_help_detail;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_help_detail;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_help_detail;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_help_detail;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_help_detail;edit",
                           "sys_help_detail;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_help_detail;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_help_detail;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_help_detail;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_help_detail;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_help_detail_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_help_detail_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_help_detail_model item)
        {
            if (string.IsNullOrEmpty(item.db.id_help))
            {
                ModelState.AddModelError("db.id_help", "required");
            }
            if (string.IsNullOrEmpty(item.db.title))
            {
                ModelState.AddModelError("db.title", "required");
            }
            if (string.IsNullOrEmpty(item.db.note))
            {
                ModelState.AddModelError("db.note", "required");
            }
            var search = repo.FindAll().Where(d => d.db.id_help ==item.db.id_help  &&   d.db.title == item.db.title && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.title", "existed");
            }
            return ModelState.IsValid;
        }

    }
}
