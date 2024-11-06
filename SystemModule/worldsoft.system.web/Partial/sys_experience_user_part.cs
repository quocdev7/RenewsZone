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
    partial class sys_experience_userController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_experience_user",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_experience_user",
            url = "/sys_experience_user_index",
            title = "sys_experience_user",
            translate = "NAV.sys_experience_user",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_experience_user;getListUse",
                 
            },
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_experience_user;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_experience_user;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_experience_user;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_experience_user;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_experience_user;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_experience_user;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_experience_user;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_experience_user;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_experience_user;DataHandler",
                    }
                }
            }
        };

        //private bool checkModelStateCreate(sys_experience_user_model item)
        //{
        //    return checkModelStateCreateEdit(ActionEnumForm.create, item);
        //}

        //private bool checkModelStateEdit(sys_experience_user_model item)
        //{
        //    return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        //}
        //private bool checkModelStateCreateEdit(ActionEnumForm action, sys_experience_user_model item)
        //{
        //    //if (string.IsNullOrEmpty(item.db.name))
        //    //{
        //    //    ModelState.AddModelError("db.name", "required");
        //    //}
        //    //var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
        //    //if (search > 0)
        //    //{
        //    //    ModelState.AddModelError("db.name", "existed");
        //    //}


        //    //return ModelState.IsValid;
        //}

    }
}
