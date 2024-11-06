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
    partial class sys_anh_noi_bat_su_kienController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_anh_noi_bat_su_kien",
            icon = "live_help",
            module = "system",
            id = "sys_anh_noi_bat_su_kien",
            url = "/sys_anh_noi_bat_su_kien_index",
            title = "sys_anh_noi_bat_su_kien",
            translate = "NAV.sys_anh_noi_bat_su_kien",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_anh_noi_bat_su_kien;getListUse",
                "sys_anh_noi_bat_su_kien;getListHinhAnhNew",
                
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_anh_noi_bat_su_kien;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_anh_noi_bat_su_kien;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_anh_noi_bat_su_kien;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_anh_noi_bat_su_kien;edit",
                           "sys_anh_noi_bat_su_kien;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_anh_noi_bat_su_kien;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_anh_noi_bat_su_kien;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_anh_noi_bat_su_kien;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_anh_noi_bat_su_kien;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_anh_noi_bat_su_kien_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_anh_noi_bat_su_kien_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_anh_noi_bat_su_kien_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
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
