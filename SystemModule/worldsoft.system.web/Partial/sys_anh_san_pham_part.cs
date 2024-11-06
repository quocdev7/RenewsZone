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
    partial class sys_anh_san_phamController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_anh_san_pham",
            icon = "live_help",
            module = "system",
            id = "sys_anh_san_pham",
            url = "/sys_anh_san_pham_index",
            title = "sys_anh_san_pham",
            translate = "NAV.sys_anh_san_pham",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_anh_san_pham;getListUse",
                      "sys_anh_san_pham;download",
                
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_anh_san_pham;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_anh_san_pham;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_anh_san_pham;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_anh_san_pham;edit",
                           "sys_anh_san_pham;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_anh_san_pham;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_anh_san_pham;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_anh_san_pham;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_anh_san_pham;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_anh_san_pham_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_anh_san_pham_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_anh_san_pham_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }

            //var tim_kiem = repo.FindAll().Where(d => d.db.id_san_pham == item.db.id_san_pham && d.db.id != item.db.id).Count();
            //if (tim_kiem > 0)
            //{
            //    ModelState.AddModelError("db.ten_san_pham", "existed");
            //}

            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }


            return ModelState.IsValid;
        }

    }
}
