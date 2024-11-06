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
    partial class sys_nhom_nganh_ngheController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_nhom_nganh_nghe",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_nhom_nganh_nghe",
            url = "/sys_nhom_nganh_nghe_index",
            title = "sys_nhom_nganh_nghe",
            translate = "NAV.sys_nhom_nganh_nghe",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_nhom_nganh_nghe;getListUse",
                 
            },

            list_controller_action_publicNonLogin = new List<string>(){
                "sys_nhom_nganh_nghe;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_nhom_nganh_nghe;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_nhom_nganh_nghe;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_nhom_nganh_nghe;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_nhom_nganh_nghe;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_nhom_nganh_nghe;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_nhom_nganh_nghe;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_nhom_nganh_nghe;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_nhom_nganh_nghe;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_nhom_nganh_nghe_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_nhom_nganh_nghe_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_nhom_nganh_nghe_model item)
        {
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
