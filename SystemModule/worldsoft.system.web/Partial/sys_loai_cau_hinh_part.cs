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
    partial class sys_loai_cau_hinhController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_loai_cau_hinh",
            icon = "badge",
            module = "system",
            id = "sys_loai_cau_hinh",
            url = "/sys_loai_cau_hinh_index",
            title = "sys_loai_cau_hinh",
            translate = "NAV.sys_loai_cau_hinh",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_loai_cau_hinh;getListUse",
                "sys_loai_cau_hinh;getCauHinhThongTin",

                


            },
            
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_loai_cau_hinh;getListUse",
                "sys_loai_cau_hinh;getCauHinhThongTin",
               "sys_loai_cau_hinh;getLoaiThongTin",
                 "sys_loai_cau_hinh;getInfo",
               
            },
             
            
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_loai_cau_hinh;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_loai_cau_hinh;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_loai_cau_hinh;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_loai_cau_hinh;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_loai_cau_hinh;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_loai_cau_hinh;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_loai_cau_hinh;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_loai_cau_hinh;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_loai_cau_hinh_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (string.IsNullOrEmpty(item.db.code))
            {
                ModelState.AddModelError("db.code", "required");
              
            }
            var checkCode = repo.FindAll().Where(d => d.db.code == item.db.code && d.db.id != item.db.id).Count();
            if (checkCode > 0)
            {
                ModelState.AddModelError("db.code", "existed");
            }
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_loai_cau_hinh_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (string.IsNullOrEmpty(item.db.code))
            {
                ModelState.AddModelError("db.code", "required");

            }
            var checkCode = repo.FindAll().Where(d => d.db.code == item.db.code && d.db.id != item.db.id).Count();
            if (checkCode > 0)
            {
                ModelState.AddModelError("db.code", "existed");
            }
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_loai_cau_hinh_model item)
        {
   
            //if (string.IsNullOrEmpty(item.db.noi_dung))
            //{
            //    ModelState.AddModelError("db.noi_dung", "required");
            //}
            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.tieu_de", "existed");
            }


            return ModelState.IsValid;
        }

    }
}
