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
    partial class sys_job_titleController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_job_title",
            icon = "badge",
            module = "system",
            id = "sys_job_title",
            url = "/sys_job_title_index",
            title = "sys_job_title",
            translate = "NAV.sys_job_title",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_job_title;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_job_title;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_job_title;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_job_title;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_job_title;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_job_title;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_job_title;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_job_title;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_job_title;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_job_title_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_job_title_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_job_title_model item)
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
