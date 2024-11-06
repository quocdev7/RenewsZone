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
    partial class sys_cau_hinh_duyet_userController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_cau_hinh_duyet_user",
            icon = "live_help",
            module = "system",
            id = "sys_cau_hinh_duyet_user",
            url = "/sys_cau_hinh_duyet_user_index",
            title = "sys_cau_hinh_duyet_user",
            translate = "NAV.sys_cau_hinh_duyet_user",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_cau_hinh_duyet_user;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_cau_hinh_duyet_user;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_duyet_user;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cau_hinh_duyet_user;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_duyet_user;edit",
                           "sys_cau_hinh_duyet_user;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cau_hinh_duyet_user;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_duyet_user;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_cau_hinh_duyet_user;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_duyet_user;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_cau_hinh_duyet_user_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_cau_hinh_duyet_user_model item)
        {

            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_cau_hinh_duyet_user_model item)
        {
            if (item.hinh_thuc.Count()==0)
            {
                ModelState.AddModelError("db.id_hinh_thuc", "required");
            }
            if (item.khoa.Count() == 0)
            {
                ModelState.AddModelError("db.id_khoa", "required");
            }

            var check_nhom = repo._context.sys_cau_hinh_duyet_users.Where(q => q.user_id == item.db.user_id && q.id != item.db.id).Count();
            if (String.IsNullOrEmpty(item.db.user_id))
            {
                ModelState.AddModelError("db.user_id", "required");
            }
            else
            {
                if (check_nhom > 0)
                {
                    ModelState.AddModelError("db.user_id", "User đã tồn tại");
                }
            }


            return ModelState.IsValid;
        }

    }
}
