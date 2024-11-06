using System.Collections.Generic;
using System.Linq;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_khuyen_maiController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_khuyen_mai",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_khuyen_mai",
            url = "/sys_khuyen_mai_index",
            title = "sys_khuyen_mai",
            translate = "NAV.sys_khuyen_mai",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_khuyen_mai;getListUse",

            },
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_khuyen_mai;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_khuyen_mai;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_khuyen_mai;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_khuyen_mai;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_khuyen_mai;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_khuyen_mai;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_khuyen_mai;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_khuyen_mai;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_khuyen_mai;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_khuyen_mai_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }
        private bool checkModelStateEdit(sys_khuyen_mai_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_khuyen_mai_model item)
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

            if (string.IsNullOrEmpty(item.db.content))
            {
                ModelState.AddModelError("db.content", "required");
            }

            return ModelState.IsValid;
        }
    }
}
