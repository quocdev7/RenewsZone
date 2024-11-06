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
    partial class sys_khu_vucController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_khu_vuc",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_khu_vuc",
            url = "/sys_khu_vuc_index",
            title = "sys_khu_vuc",
            translate = "NAV.sys_khu_vuc",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_khu_vuc;getListUse",
                 
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_khu_vuc;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_khu_vuc;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_khu_vuc;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_khu_vuc;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_khu_vuc;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_khu_vuc;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_khu_vuc;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_khu_vuc;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_khu_vuc_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_khu_vuc_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_khu_vuc_model item)
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
