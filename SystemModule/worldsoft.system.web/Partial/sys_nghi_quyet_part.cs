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
    partial class sys_nghi_quyetController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_nghi_quyet",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_nghi_quyet",
            url = "/sys_nghi_quyet_index",
            title = "sys_nghi_quyet",
            translate = "NAV.sys_nghi_quyet",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_nghi_quyet;getListUse",
                "sys_nghi_quyet;getListNghiQuyetInfo",
                 "sys_nghi_quyet;getNghiQuyetInfo",

            },
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_nghi_quyet;getListNghiQuyetInfo",
                 "sys_nghi_quyet;getNghiQuyetInfo",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_nghi_quyet;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_nghi_quyet;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_nghi_quyet;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_nghi_quyet;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_nghi_quyet;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_nghi_quyet;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_nghi_quyet;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_nghi_quyet;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_nghi_quyet_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_nghi_quyet_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_nghi_quyet_model item)
        {
            if (string.IsNullOrEmpty(item.db.noi_dung))
            {
                ModelState.AddModelError("db.noi_dung", "required");
            }
            if (string.IsNullOrEmpty(item.db.title))
            {
                ModelState.AddModelError("db.title", "required");
            }
            var search = repo.FindAll().Where(d => d.db.noi_dung == item.db.noi_dung && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.noi_dung", "existed");
            }


            return ModelState.IsValid;
        }

    }
}
