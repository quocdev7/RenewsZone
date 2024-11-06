﻿using System;
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
    partial class sys_group_userController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_group_user",
            icon = "groups",
            module = "system",
            id = "sys_group_user",
            url = "/sys_group_user_index",
            title = "sys_group_user",
            translate = "NAV.sys_group_user",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_group_user;getListUse",
                "sys_group_user;getListItem",
                "sys_group_user;getListRole",
                   "sys_group_user;getListUser",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_group_user;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_group_user;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_group_user;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_group_user;edit",
                           "sys_group_user;revert"
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_group_user;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_group_user;delete",

                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_group_user;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_group_user;DataHandler",
                    }
                }
            }
        };
        private  bool checkModelStateCreate(sys_group_user_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_group_user_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_group_user_model item)
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
            if (item.list_item.Count == 0 && action == ActionEnumForm.create)
            {
                ModelState.AddModelError("list_item", "required");
            }
            
           


            return ModelState.IsValid;
        }

    }
}
