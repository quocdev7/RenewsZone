using System.Collections.Generic;
using System.Linq;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_quan_huyenController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_quan_huyen",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_quan_huyen",
            url = "/sys_quan_huyen_index",
            title = "sys_quan_huyen",
            translate = "NAV.sys_quan_huyen",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_quan_huyen;getListUse",
                "sys_quan_huyen;getListQuanHuyen",
                "sys_quan_huyen;exportExcel",
                "sys_quan_huyen;getListQuanHuyenNonLog",
            },
            list_controller_action_public = new List<string>(){
                "sys_quan_huyen;get_list_quan_huyen_multiple",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_quan_huyen;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_quan_huyen;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_quan_huyen;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_quan_huyen;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_quan_huyen;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_quan_huyen;delete",
                          "sys_quan_huyen;update_status_del",

                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_quan_huyen;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_quan_huyen;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_quan_huyen_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_quan_huyen_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_quan_huyen_model item)
        {
            if (string.IsNullOrEmpty(item.db.ten))
            {
                ModelState.AddModelError("db.ten", "required");
            }
            var search = repo.FindAll()
                 .Where(d => d.db.id_quoc_gia == item.db.id_quoc_gia && d.db.id_tinh != item.db.id_tinh)
                .Where(d => d.db.ten == item.db.ten && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.ten", "existed");
            }


            return ModelState.IsValid;
        }

    }
}
