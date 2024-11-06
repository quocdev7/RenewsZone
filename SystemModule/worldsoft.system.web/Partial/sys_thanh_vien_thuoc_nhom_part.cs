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
    partial class sys_thanh_vien_thuoc_nhomController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_thanh_vien_thuoc_nhom",
            icon = "menu",
            module = "system",
            id = "sys_thanh_vien_thuoc_nhom",
            url = "/sys_thanh_vien_thuoc_nhom_index",
            title = "sys_thanh_vien_thuoc_nhom",
            translate = "NAV.sys_thanh_vien_thuoc_nhom",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>() {
                 "sys_thanh_vien_thuoc_nhom;downloadtemp",

            },
            list_controller_action_public = new List<string>(){
                "sys_thanh_vien_thuoc_nhom;getListUse",
                  "sys_thanh_vien_thuoc_nhom;add_nguoi_dung",
                        "sys_thanh_vien_thuoc_nhom;getListNhomNguoiDung",
                     
                            "sys_thanh_vien_thuoc_nhom;ImportFromExcel",

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_thanh_vien_thuoc_nhom;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_thanh_vien_thuoc_nhom;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_thanh_vien_thuoc_nhom;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_thanh_vien_thuoc_nhom;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_thanh_vien_thuoc_nhom;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_thanh_vien_thuoc_nhom;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_thanh_vien_thuoc_nhom;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_thanh_vien_thuoc_nhom;DataHandler",
                    }
                }
            }
        };
        private  bool checkModelStateCreate(sys_thanh_vien_thuoc_nhom_model item,string user_id)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item, user_id);
        }

        private bool checkModelStateEdit(sys_thanh_vien_thuoc_nhom_model item, string user_id)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item, user_id);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_thanh_vien_thuoc_nhom_model item,string user_id)
        {

            var check_nhom = repo._context.sys_thanh_vien_thuoc_nhoms.Where(q => q.user_id == user_id && q.id_nhom== item.db.id_nhom).Count();
            if (check_nhom > 0)
            {
                ModelState.AddModelError("user_nhom", "existedGroup");
            }



            return ModelState.IsValid;
        }

    }
}
