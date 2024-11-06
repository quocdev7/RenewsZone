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
    partial class sys_group_newsController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_group_news",
            icon = "badge",
            module = "system",
            id = "sys_group_news",
            url = "/sys_group_news_index",
            title = "NAV.sys_group_news",
            translate = "NAV.sys_group_news",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_group_news;getListUse",
            },
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_group_news;getListUse",
                  "sys_group_news;getListUseDetail",
                
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_group_news;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_group_news;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_group_news;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_group_news;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_group_news;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_group_news;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_group_news;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_group_news;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_group_news_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_group_news_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_group_news_model item)
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

            if (string.IsNullOrEmpty(item.db.code))
            {
                ModelState.AddModelError("db.code", "required");
            }
            var checkCode = repo.FindAll().Where(d => d.db.code == item.db.code && d.db.id != item.db.id).Count();
            if (checkCode > 0)
            {
                ModelState.AddModelError("db.code", "existed");
            }

            return ModelState.IsValid;
        }

    }
}
