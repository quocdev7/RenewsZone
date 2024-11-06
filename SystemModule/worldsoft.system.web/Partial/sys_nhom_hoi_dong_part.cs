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
    partial class sys_nhom_hoi_dongController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_nhom_hoi_dong",
            icon = "badge",
            module = "system",
            id = "sys_nhom_hoi_dong",
            url = "/sys_nhom_hoi_dong_index",
            title = "sys_nhom_hoi_dong",
            translate = "NAV.sys_nhom_hoi_dong",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_nhom_hoi_dong;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_nhom_hoi_dong;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_nhom_hoi_dong;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_nhom_hoi_dong;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_nhom_hoi_dong;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_nhom_hoi_dong;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_nhom_hoi_dong;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_nhom_hoi_dong;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_nhom_hoi_dong;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_nhom_hoi_dong_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_nhom_hoi_dong_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_nhom_hoi_dong_model item)
        {

            if (item.db.stt==null)
            {
                ModelState.AddModelError("db.stt", "required");
            }


            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
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
