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
    partial class sys_tu_ngu_camController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_tu_ngu_cam",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_tu_ngu_cam",
            url = "/sys_tu_ngu_cam_index",
            title = "sys_tu_ngu_cam",
            translate = "NAV.sys_tu_ngu_cam",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_tu_ngu_cam;getListUse",
                 
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_tu_ngu_cam;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_tu_ngu_cam;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_tu_ngu_cam;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_tu_ngu_cam;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_tu_ngu_cam;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_tu_ngu_cam;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_tu_ngu_cam;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_tu_ngu_cam;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_tu_ngu_cam_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_tu_ngu_cam_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_tu_ngu_cam_model item)
        {
            if (string.IsNullOrEmpty(item.db.note))
            {
                ModelState.AddModelError("db.note", "required");
            }
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
