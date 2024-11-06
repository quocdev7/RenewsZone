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
    partial class sys_videoController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_video",
            icon = "live_help",
            module = "system",
            id = "sys_video",
            url = "/sys_video_index",
            title = "sys_video",
            translate = "NAV.sys_video",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_video;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_video;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_video;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_video;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_video;edit",
                           "sys_video;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_video;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_video;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_video;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_video;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_video_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_video_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_video_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }

            if (string.IsNullOrEmpty(item.db.link))
            {
                ModelState.AddModelError("db.link", "required");
            }
            if (item.db.stt==null)
            {
                ModelState.AddModelError("db.stt", "required");
            }

            var search = repo.FindAll().Where(d => d.db.link == item.db.link && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.link", "existed");
            }
             search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }

            return ModelState.IsValid;
        }

    }
}
