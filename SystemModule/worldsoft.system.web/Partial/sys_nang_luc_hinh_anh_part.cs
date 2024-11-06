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
    partial class sys_nang_luc_hinh_anhController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_nang_luc_hinh_anh",
            icon = "live_help",
            module = "system",
            id = "sys_nang_luc_hinh_anh",
            url = "/sys_nang_luc_hinh_anh_index",
            title = "sys_nang_luc_hinh_anh",
            translate = "NAV.sys_nang_luc_hinh_anh",
            type = "item",
            list_controller_action_public = new List<string>(){
            
            },

            list_controller_action_publicNonLogin = new List<string>()
            {    "sys_nang_luc_hinh_anh;getListUse",
                 "sys_nang_luc_hinh_anh;getListHinhAnh",
                    "sys_nang_luc_hinh_anh;getListImage",


            },

            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_nang_luc_hinh_anh;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_nang_luc_hinh_anh;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_nang_luc_hinh_anh;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_nang_luc_hinh_anh;edit",
                           "sys_nang_luc_hinh_anh;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_nang_luc_hinh_anh;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_nang_luc_hinh_anh;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_nang_luc_hinh_anh;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_nang_luc_hinh_anh;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_nang_luc_hinh_anh_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_nang_luc_hinh_anh_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_nang_luc_hinh_anh_model item)
        {

            if (string.IsNullOrEmpty(item.db.image))
            {
                ModelState.AddModelError("db.image", "required");
            }
            if (string.IsNullOrEmpty(item.db.image_mobile))
            {
                ModelState.AddModelError("db.image_mobile", "required");
            }
            return ModelState.IsValid;
        }

    }
}
