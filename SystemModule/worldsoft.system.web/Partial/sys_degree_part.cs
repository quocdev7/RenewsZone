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
    partial class sys_degreeController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_degree",
            icon = "badge",
            module = "system",
            id = "sys_degree",
            url = "/sys_degree_index",
            title = "sys_degree",
            translate = "NAV.sys_degree",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_degree;getListUse",
                "sys_degree;getElementById",
              
            },
            list_controller_action_publicNonLogin = new List<string>(){

                "sys_degree;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_degree;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_degree;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_degree;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_degree;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_degree;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_degree;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_degree;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_degree;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_degree_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_degree_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_degree_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
           

            return ModelState.IsValid;
        }

    }
}
