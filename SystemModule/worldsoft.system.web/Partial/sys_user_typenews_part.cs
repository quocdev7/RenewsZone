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
    partial class sys_user_typenewsController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_user_typenews",
            icon = "support",
            module = "system",
            id = "sys_user_typenews",
            url = "/sys_user_typenews_index",
            title = "sys_user_typenews",
            translate = "NAV.sys_user_typenews",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_user_typenews;getListUse",
                    "sys_user_typenews;add_nguoi_dung",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_user_typenews;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_user_typenews;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_user_typenews;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_user_typenews;edit",
                           "sys_user_typenews;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_user_typenews;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_user_typenews;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_user_typenews;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_user_typenews;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_user_typenews_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_user_typenews_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_user_typenews_model item)
        {

            if (item.types.Count()==0)
            {
                ModelState.AddModelError("db.id_type_news", "required");
            }
            if (item.khoa.Count() == 0)
            {
                ModelState.AddModelError("db.id_khoa", "required");
            }

            var check_nhom = repo._context.sys_user_typenews.Where(q => q.id_user == item.db.id_user && q.id != item.db.id).Count();
            if (String.IsNullOrEmpty(item.db.id_user))
            {
                ModelState.AddModelError("db.id_user", "required");
            }
            else
            {
                if (check_nhom > 0)
                {
                    ModelState.AddModelError("db.id_user", "Đã cấu hình được duyệt tin cho người này");
                }
            }

       
           



            return ModelState.IsValid;
        }

    }
}
