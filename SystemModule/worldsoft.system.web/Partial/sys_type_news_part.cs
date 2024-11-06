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
    partial class sys_type_newsController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_type_news",
            icon = "badge",
            module = "system",
            id = "sys_type_news",
            url = "/sys_type_news_index",
            title = "NAV.sys_type_news",
            translate = "NAV.sys_type_news",
            type = "item",
            list_controller_action_public = new List<string>(){
              
                "sys_type_news;getListUseByGroupNew",
                "sys_type_news;getListUse",
            },
            list_controller_action_publicNonLogin = new List<string>()
            { 
                "sys_type_news;getListUse",
                  "sys_type_news;getListUseByGroup",
                     "sys_type_news;getInfoByTypeNews",
                  

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_type_news;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_type_news;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_type_news;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_type_news;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_type_news;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_type_news;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_type_news;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_type_news;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_type_news_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_type_news_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_type_news_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (string.IsNullOrEmpty(item.db.id_group_news))
            {
                ModelState.AddModelError("db.id_group_news", "required");
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
