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
    partial class sys_cau_hinh_anh_mac_dinhController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_cau_hinh_anh_mac_dinh",
            icon = "live_help",
            module = "system",
            id = "sys_cau_hinh_anh_mac_dinh",
            url = "/sys_cau_hinh_anh_mac_dinh_index",
            title = "sys_cau_hinh_anh_mac_dinh",
            translate = "NAV.sys_cau_hinh_anh_mac_dinh",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_cau_hinh_anh_mac_dinh;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_cau_hinh_anh_mac_dinh;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_anh_mac_dinh;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cau_hinh_anh_mac_dinh;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_anh_mac_dinh;edit",
                           "sys_cau_hinh_anh_mac_dinh;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cau_hinh_anh_mac_dinh;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_anh_mac_dinh;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_cau_hinh_anh_mac_dinh;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_anh_mac_dinh;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_cau_hinh_anh_mac_dinh_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_cau_hinh_anh_mac_dinh_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_cau_hinh_anh_mac_dinh_model item)
        {


            if (item.db.type==null)
            {
                ModelState.AddModelError("db.type", "required");
            }
            else
            {
                var search = repo.FindAll().Where(d => d.db.type == item.db.type && d.db.id != item.db.id).Count();
                if (search > 0)
                {
                    ModelState.AddModelError("db.type", "existed");
                }

            }

            if (string.IsNullOrEmpty(item.db.image))
            {
                ModelState.AddModelError("db.image", "required");
            }
          
            if (item.db.type == 1)
            {
                if (string.IsNullOrEmpty(item.db.avatar))
                {
                    ModelState.AddModelError("db.avatar", "required");
                }
                var avatar = repo.FindAll().Where(d => d.db.type == item.db.type && d.db.avatar == item.db.avatar && d.db.id != item.db.id).Count();
                if (avatar > 0)
                {
                    ModelState.AddModelError("db.avatar", "existed");
                }
            }
            return ModelState.IsValid;
        }

    }
}
