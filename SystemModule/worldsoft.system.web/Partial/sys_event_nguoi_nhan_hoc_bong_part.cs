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
    partial class sys_event_nguoi_nhan_hoc_bongController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_event_nguoi_nhan_hoc_bong",
            icon = "badge",
            module = "system",
            id = "sys_event_nguoi_nhan_hoc_bong",
            url = "/sys_event_nguoi_nhan_hoc_bong_index",
            title = "sys_event_nguoi_nhan_hoc_bong",
            translate = "NAV.sys_event_nguoi_nhan_hoc_bong",
            type = "item",

            list_controller_action_public = new List<string>(){
                "sys_event_nguoi_nhan_hoc_bong;ImportFromExcel",
                
            },
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_event_nguoi_nhan_hoc_bong;getListUse",
                "sys_event_nguoi_nhan_hoc_bong;downloadtemp",

                
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_event_nguoi_nhan_hoc_bong;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_nguoi_nhan_hoc_bong;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event_nguoi_nhan_hoc_bong;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_nguoi_nhan_hoc_bong;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event_nguoi_nhan_hoc_bong;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_nguoi_nhan_hoc_bong;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_event_nguoi_nhan_hoc_bong;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_nguoi_nhan_hoc_bong;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_event_nguoi_nhan_hoc_bong_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_event_nguoi_nhan_hoc_bong_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_event_nguoi_nhan_hoc_bong_model item)
        {
            
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (item.db.so_tien == null)
            {
                ModelState.AddModelError("db.so_tien", "required");
            }
            else
            {
                if (item.db.so_tien < 0)
                {
                    ModelState.AddModelError("db.so_tien", "system.sotientaitrokhongduoclasoam");
                }
            }


            //if (item.db.so_tien == null)
            //{
            //    ModelState.AddModelError("db.so_tien", "required");
            //}
            //if (item.db.stt == null)
            //{
            //    ModelState.AddModelError("db.stt", "required");
            //}
            //if (item.db.mssv.Count() == 0)
            //{
            //    ModelState.AddModelError("db.mssv", "required");
            //}
            //if (item.db.dien_thoai.Count() == 0)
            //{
            //    ModelState.AddModelError("db.dien_thoai", "required");
            //}
            //var search = repo._context.sys_event_nguoi_nhan_hoc_bongs.Where(q => q.mssv == item.db.mssv && q.id != item.db.id).Count();

            var search = repo.FindAll().Where(d => d.db.mssv == item.db.mssv && d.db.name == item.db.name && d.db.id != item.db.id).Count();
            
            if (String.IsNullOrEmpty(item.db.mssv))
            {
                ModelState.AddModelError("db.mssv", "required");
            }
            else
            {
                if (search > 0)
                {
                    ModelState.AddModelError("db.mssv", "Đã tồn tại");
                }
            }

            //var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            //if (search > 0)
            //{
            //    ModelState.AddModelError("db.name", "Đã tồn tại");
            //}


            return ModelState.IsValid;
        }

    }
}
