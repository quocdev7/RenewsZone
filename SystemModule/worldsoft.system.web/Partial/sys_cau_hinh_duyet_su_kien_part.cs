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
    partial class sys_cau_hinh_duyet_su_kienController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_cau_hinh_duyet_su_kien",
            icon = "live_help",
            module = "system",
            id = "sys_cau_hinh_duyet_su_kien",
            url = "/sys_cau_hinh_duyet_su_kien_index",
            title = "sys_cau_hinh_duyet_su_kien",
            translate = "NAV.sys_cau_hinh_duyet_su_kien",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_cau_hinh_duyet_su_kien;getListUse",
            },
            list_controller_action_public = new List<string>(){
              
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_cau_hinh_duyet_su_kien;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_duyet_su_kien;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cau_hinh_duyet_su_kien;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_duyet_su_kien;edit",
                           "sys_cau_hinh_duyet_su_kien;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cau_hinh_duyet_su_kien;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_duyet_su_kien;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_cau_hinh_duyet_su_kien;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_duyet_su_kien;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_cau_hinh_duyet_su_kien_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_cau_hinh_duyet_su_kien_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_cau_hinh_duyet_su_kien_model item)
        {
            if (item.khoa.Count() == 0)
            {
                ModelState.AddModelError("db.id_khoa", "required");
            }

            var check_nhom = repo._context.sys_cau_hinh_duyet_su_kiens.Where(q => q.user_id == item.db.user_id && q.id != item.db.id).Count();
            if (String.IsNullOrEmpty(item.db.user_id))
            {
                ModelState.AddModelError("db.user_id", "required");
            }
            else
            {
                if (check_nhom > 0)
                {
                    ModelState.AddModelError("db.user_id", "Người duyệt sự kiễn đã tồn tại");
                }
            }




            return ModelState.IsValid;
        }

    }
}
