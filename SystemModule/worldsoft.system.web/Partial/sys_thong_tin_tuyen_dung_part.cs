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
    partial class sys_thong_tin_tuyen_dungController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_thong_tin_tuyen_dung",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_thong_tin_tuyen_dung",
            url = "/sys_thong_tin_tuyen_dung_index",
            title = "sys_thong_tin_tuyen_dung",
            translate = "NAV.sys_thong_tin_tuyen_dung",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_thong_tin_tuyen_dung;getListUse",
                 "sys_thong_tin_tuyen_dung;getListUseNoiBac",

            },
            list_controller_action_publicNonLogin = new List<string>(){
            "sys_thong_tin_tuyen_dung;getListUse",
                 "sys_thong_tin_tuyen_dung;getListUseNoiBac",


            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_thong_tin_tuyen_dung;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_thong_tin_tuyen_dung;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_thong_tin_tuyen_dung;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_thong_tin_tuyen_dung;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_thong_tin_tuyen_dung;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_thong_tin_tuyen_dung;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_thong_tin_tuyen_dung;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_thong_tin_tuyen_dung;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_thong_tin_tuyen_dung_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_thong_tin_tuyen_dung_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_thong_tin_tuyen_dung_model item)
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
