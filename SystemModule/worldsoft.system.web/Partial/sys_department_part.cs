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
    partial class sys_departmentController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_department",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_department",
            url = "/sys_department_index",
            title = "sys_department",
            translate = "NAV.sys_department",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_department;getListUse",
                 
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_department;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_department;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_department;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_department;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_department;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_department;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_department;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_department;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_department_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_department_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_department_model item)
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
