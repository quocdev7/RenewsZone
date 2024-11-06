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
    partial class sys_bannerController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_banner",
            icon = "live_help",
            module = "system",
            id = "sys_banner",
            url = "/sys_banner_index",
            title = "sys_banner",
            translate = "NAV.sys_banner",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_banner;getListUse",
                  "sys_banner;getListUseContactUs",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_banner;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_banner;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_banner;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_banner;edit",
                           "sys_banner;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_banner;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_banner;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_banner;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_banner;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_banner_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_banner_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_banner_model item)
        {
            if (item.db.loai == null)
            {
                ModelState.AddModelError("db.loai", "required");
            }
            if (string.IsNullOrEmpty(item.db.image))
            {
                ModelState.AddModelError("db.image", "required");
            }
            if (item.db.stt == null)
            {
                ModelState.AddModelError("db.stt", "required");
            }

            if (string.IsNullOrEmpty(item.db.image_mobile))
            {
                ModelState.AddModelError("db.img_mobile", "required");
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
