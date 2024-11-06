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
    partial class sys_cuu_sinh_vienController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_cuu_sinh_vien",
            icon = "badge",
            module = "system",
            id = "sys_cuu_sinh_vien",
            url = "/sys_cuu_sinh_vien_index",
            title = "sys_cuu_sinh_vien",
            translate = "NAV.sys_cuu_sinh_vien",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_cuu_sinh_vien;getListUse",
                "sys_cuu_sinh_vien;getElementById",
              
            },
            list_controller_action_publicNonLogin = new List<string>(){

                "sys_cuu_sinh_vien;getListUse",
                "sys_cuu_sinh_vien;getListGioiThieu",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_cuu_sinh_vien;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_cuu_sinh_vien;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cuu_sinh_vien;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_cuu_sinh_vien;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cuu_sinh_vien;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_cuu_sinh_vien;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_cuu_sinh_vien;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_cuu_sinh_vien;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_cuu_sinh_vien_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_cuu_sinh_vien_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_cuu_sinh_vien_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (item.db.stt==null)
            {
                ModelState.AddModelError("db.stt", "required");
            }
            if (item.db.id_nhom_hoi_dong == null)
            {
                ModelState.AddModelError("db.id_nhom_hoi_dong", "required");
            }

            if (item.db.chuc_danh_hoi_dong == null)
            {
                ModelState.AddModelError("db.chuc_danh_hoi_dong", "required");
            }

            if (string.IsNullOrEmpty(item.db.name_company))
            {
                ModelState.AddModelError("db.name_company", "required");
            }
          
            var search1 = repo.FindAll().Where(d => d.db.name_company == item.db.name_company && d.db.id != item.db.id).Count();
            if (search1 > 0)
            {
                ModelState.AddModelError("db.name_company", "existed");
            }

            return ModelState.IsValid;
        }

    }
}
