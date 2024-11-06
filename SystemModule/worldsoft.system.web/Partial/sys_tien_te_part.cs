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
    partial class sys_tien_teController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_tien_te",
            icon = "badge",
            module = "system",
            id = "sys_tien_te",
            url = "/sys_tien_te_index",
            title = "sys_tien_te",
            translate = "NAV.sys_tien_te",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_tien_te;getListUse",
                "sys_tien_te;getListCompanyInfo",
                "sys_tien_te;getElementById",
                    "sys_tien_te;getCompanyInfo"

            },
            list_controller_action_publicNonLogin = new List<string>(){
             
                "sys_tien_te;getListCompanyInfo",
                    "sys_tien_te;getElementById",
                      "sys_tien_te;getCompanyInfo"

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_tien_te;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_tien_te;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_tien_te;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_tien_te;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_tien_te;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_tien_te;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_tien_te;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_tien_te;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_tien_te_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_tien_te_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_tien_te_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
           

            return ModelState.IsValid;
        }

    }
}
