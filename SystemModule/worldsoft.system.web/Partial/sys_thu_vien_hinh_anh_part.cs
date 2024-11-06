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
    partial class sys_thu_vien_hinh_anhController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_thu_vien_hinh_anh",
            icon = "live_help",
            module = "system",
            id = "sys_thu_vien_hinh_anh",
            url = "/sys_thu_vien_hinh_anh_index",
            title = "sys_thu_vien_hinh_anh",
            translate = "NAV.sys_thu_vien_hinh_anh",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_thu_vien_hinh_anh;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_thu_vien_hinh_anh;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_thu_vien_hinh_anh;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_thu_vien_hinh_anh;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_thu_vien_hinh_anh;edit",
                           "sys_thu_vien_hinh_anh;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_thu_vien_hinh_anh;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_thu_vien_hinh_anh;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_thu_vien_hinh_anh;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_thu_vien_hinh_anh;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_thu_vien_hinh_anh_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_thu_vien_hinh_anh_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_thu_vien_hinh_anh_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (string.IsNullOrEmpty(item.db.id_nhom_thu_vien_hinh_anh))
            {
                ModelState.AddModelError("db.id_nhom_thu_vien_hinh_anh", "required");
            }

            

            if (item.db.stt==null)
            {
                ModelState.AddModelError("db.stt", "required");
            }
            if (string.IsNullOrEmpty(item.db.image))
            {
                ModelState.AddModelError("db.image", "required");
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
