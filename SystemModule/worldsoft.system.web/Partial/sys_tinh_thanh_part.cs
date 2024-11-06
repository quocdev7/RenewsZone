using System.Collections.Generic;
using System.Linq;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_tinh_thanhController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_tinh_thanh",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_tinh_thanh",
            url = "/sys_tinh_thanh_index",
            title = "sys_tinh_thanh",
            translate = "NAV.sys_tinh_thanh",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_tinh_thanh;getListUse",
                "sys_tinh_thanh;getListNghiQuyetInfo",
                 "sys_tinh_thanh;getNghiQuyetInfo",
                "sys_tinh_thanh;getListTinhThanh",
                "sys_tinh_thanh;exportExcel",
            },
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_tinh_thanh;getListNghiQuyetInfo",
                 "sys_tinh_thanh;getNghiQuyetInfo",
                "sys_tinh_thanh;getListUse",
                "sys_tinh_thanh;load_tinh_thanh",
                "sys_tinh_thanh;getListUseNonLog",


            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_tinh_thanh;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_tinh_thanh;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_tinh_thanh;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_tinh_thanh;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_tinh_thanh;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_tinh_thanh;delete",
                          "sys_tinh_thanh;update_status_del",

                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_tinh_thanh;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_tinh_thanh;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_tinh_thanh_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_tinh_thanh_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_tinh_thanh_model item)
        {
            if (string.IsNullOrEmpty(item.db.ten))
            {
                ModelState.AddModelError("db.ten", "required");
            }
            var search = repo.FindAll()
                .Where(d => d.db.ten == item.db.ten && d.db.id != item.db.id)
                .Where(d => d.db.id_quoc_gia == item.db.id_quoc_gia)
                .Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.ten", "existed");
            }


            return ModelState.IsValid;
        }

    }
}
