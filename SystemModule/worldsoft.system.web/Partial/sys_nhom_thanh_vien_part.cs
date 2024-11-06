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
    partial class sys_nhom_thanh_vienController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_nhom_thanh_vien",
            icon = "menu",
            module = "system",
            id = "sys_nhom_thanh_vien",
            url = "/sys_nhom_thanh_vien_index",
            title = "sys_nhom_thanh_vien",
            translate = "NAV.sys_nhom_thanh_vien",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_nhom_thanh_vien;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_nhom_thanh_vien;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_nhom_thanh_vien;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_nhom_thanh_vien;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_nhom_thanh_vien;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_nhom_thanh_vien;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_nhom_thanh_vien;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_nhom_thanh_vien;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_nhom_thanh_vien;DataHandler",
                    }
                }
            }
        };
        private  bool checkModelStateCreate(sys_nhom_thanh_vien_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_nhom_thanh_vien_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_nhom_thanh_vien_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
        

            var check = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id && d.db.status_del == 1).Count();
            if (check > 0)
            {
                ModelState.AddModelError("db.ma", "existed");
            }

          

            return ModelState.IsValid;
        }

    }
}
