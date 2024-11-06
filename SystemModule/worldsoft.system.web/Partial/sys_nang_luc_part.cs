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
    partial class sys_nang_lucController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_nang_luc",
            icon = "live_help",
            module = "system",
            id = "sys_nang_luc",
            url = "/sys_nang_luc_index",
            title = "sys_nang_luc",
            translate = "NAV.sys_nang_luc",
            type = "item",
            list_controller_action_public = new List<string>(){
              
            },

            list_controller_action_publicNonLogin = new List<string>(){
                  "sys_nang_luc;getListUse",
                 "sys_nang_luc;get_nang_luc",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_nang_luc;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_nang_luc;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_nang_luc;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_nang_luc;edit",
                           "sys_nang_luc;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_nang_luc;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_nang_luc;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_nang_luc;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_nang_luc;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_nang_luc_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_nang_luc_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_nang_luc_model item)
        {

            if (string.IsNullOrEmpty(item.db.noi_dung))
            {
                ModelState.AddModelError("db.noi_dung", "required");
            }
            if (string.IsNullOrEmpty(item.db.tieu_de_tieng_anh))
            {
                ModelState.AddModelError("db.tieu_de_tieng_anh", "required");
            }
            if (string.IsNullOrEmpty(item.db.noi_dung_tieng_anh))
            {
                ModelState.AddModelError("db.noi_dung_tieng_anh", "required");
            }
            return ModelState.IsValid;
        }

    }
}
