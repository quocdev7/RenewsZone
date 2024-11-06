using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_contactusController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_contactus",
            icon = "live_help",
            module = "system",
            id = "sys_contactus",
            url = "/sys_contactus_index",
            title = "sys_contactus",
            translate = "NAV.sys_contactus",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_contactus;create",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_contactus;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_contactus;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_contactus;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_contactus;edit",
                           "sys_contactus;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_contactus;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_contactus;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_contactus;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_contactus;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_contactus_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_contactus_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_contactus_model item)
        {

            if (string.IsNullOrEmpty(item.captcha))
            {
                ModelState.AddModelError("captcha", "required");
            }
            else
            {
                var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");
                if (CaptchaCode.ToLower() != item.captcha.ToLower())
                {
                    ModelState.AddModelError("captcha", "captcha_invalid");
                }

            }

            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (string.IsNullOrEmpty(item.db.email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            if (string.IsNullOrEmpty(item.db.phone))
            {
                ModelState.AddModelError("db.phone", "required");
            }
            if (string.IsNullOrEmpty(item.db.content))
            {
                ModelState.AddModelError("db.content", "required");
            }

            if (item.db.content.Length >500 )
            {
                ModelState.AddModelError("db.content", "max500");
            }


            return ModelState.IsValid;
        }

    }
}
