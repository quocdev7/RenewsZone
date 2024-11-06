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
    partial class sys_approval_configController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_approval_config",
            icon = "menu",
            module = "system",
            id = "sys_approval_config",
            url = "/sys_approval_config_index",
            title = "sys_approval_config",
            translate = "NAV.sys_approval_config",
            type = "item",
            list_controller_action_public = new List<string>(){
               "sys_approval_config;getListUse",
                 "sys_approval_config;getListUser",
               "sys_approval_config;getListItem",
                
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_approval_config;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_approval_config;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_approval_config;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_approval_config;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_approval_config;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_approval_config;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_approval_config;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_approval_config;DataHandler",
                    }
                }
            }
        };
        private  bool checkModelStateCreate(sys_approval_config_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_approval_config_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_approval_config_model item)
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
            if (item.list_item.Count == 0)
            {
                ModelState.AddModelError("list_item", "required");
            }
            else
            {


                var maxstep = item.list_item.Max(d => d.db.step_num);
                if (maxstep > 10)
                {
                    ModelState.AddModelError("list_item", "system.step_num_maximum_is_10");
                }
                if (item.list_item.Where(d => d.db.step_num <=0).Count() > 0)
                {
                    ModelState.AddModelError("list_item", "system.step_num_minimun_is_1");
                }
                for (int i = 1; i < maxstep; i++)
                {
                    if(item.list_item.Where(d => d.db.step_num == i).Count() == 0)
                    {
                        ModelState.AddModelError("list_item", "system.step_num_must_sequence");
                    }
                }
            }
           


            return ModelState.IsValid;
        }
        
       
    }
}
