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
    partial class sys_khoaController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_khoa",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_khoa",
            url = "/sys_khoa_index",
            title = "sys_khoa",
            translate = "NAV.sys_khoa",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_khoa;getListUse",
                 
            },
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_khoa;getListUse",

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_khoa;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_khoa;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_khoa;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_khoa;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_khoa;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_khoa;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_khoa;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_khoa;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_khoa_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_khoa_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_khoa_model item)
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
