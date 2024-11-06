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
    partial class sys_event_nha_tai_troController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_event_nha_tai_tro",
            icon = "badge",
            module = "system",
            id = "sys_event_nha_tai_tro",
            url = "/sys_event_nha_tai_tro_index",
            title = "sys_event_nha_tai_tro",
            translate = "NAV.sys_event_nha_tai_tro",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_event_nha_tai_tro;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_event_nha_tai_tro;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_nha_tai_tro;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event_nha_tai_tro;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_nha_tai_tro;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event_nha_tai_tro;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_nha_tai_tro;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_event_nha_tai_tro;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_nha_tai_tro;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_event_nha_tai_tro_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_event_nha_tai_tro_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_event_nha_tai_tro_model item)
        {

            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            else
            {
                var search = repo._context.sys_event_nha_tai_tros.Where(d => d.name == item.db.name && d.id != item.db.id).Count();
                if (search > 100000)
                {
                    ModelState.AddModelError("db.name", "Đã tồn tại");
                }
            }
            if (item.db.ma_so_thue == null)
            {
                ModelState.AddModelError("db.ma_so_thue", "required");
            }
            if (item.db.so_tien == null)
            {
                ModelState.AddModelError("db.so_tien", "required");
            }
            else
            {
                if (item.db.so_tien <0)
                {
                    ModelState.AddModelError("db.so_tien", "system.sotientaitrokhongduoclasoam");
                }
            }
            //if (string.IsNullOrEmpty(item.db.so_tien))
            //{
            //    ModelState.AddModelError("db.so_tien", "required");
            //}
            if (string.IsNullOrEmpty(item.db.logo))
            {
                ModelState.AddModelError("db.logo", "required");
            }

            //var search = repo._context.sys_cau_hinh_duyet_su_kiens.Where(q => q.name == item.db.name && q.id != item.db.id).Count();
            //if (String.IsNullOrEmpty(item.db.name))
            //{
            //    ModelState.AddModelError("db.name", "required");
            //}
            //else
            //{
            //    if (search > 0)
            //    {
            //        ModelState.AddModelError("db.name", "Đã tồn tại");
            //    }
            //}

         


            return ModelState.IsValid;
        }

    }
}
