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
    partial class sys_quyen_loiController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_quyen_loi",
            icon = "badge",
            module = "system",
            id = "sys_quyen_loi",
            url = "/sys_quyen_loi_index",
            title = "sys_quyen_loi",
            translate = "NAV.sys_quyen_loi",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_quyen_loi;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_quyen_loi;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_quyen_loi;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_quyen_loi;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_quyen_loi;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_quyen_loi;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_quyen_loi;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_quyen_loi;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_quyen_loi;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_quyen_loi_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_quyen_loi_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_quyen_loi_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (string.IsNullOrEmpty(item.db.noi_dung))
            {
                ModelState.AddModelError("db.noi_dung", "required");
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
