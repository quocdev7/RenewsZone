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
    partial class sys_cau_hinh_thong_tin_websiteController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_cau_hinh_thong_tin_website",
            icon = "badge",
            module = "system",
            id = "sys_cau_hinh_thong_tin_website",
            url = "/sys_cau_hinh_thong_tin_website_index",
            title = "sys_cau_hinh_thong_tin_website",
            translate = "NAV.sys_cau_hinh_thong_tin_website",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_cau_hinh_thong_tin_website;getListUse",
                "sys_cau_hinh_thong_tin_website;getCauHinhThongTin",

                


            },
            
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_cau_hinh_thong_tin_website;getListUse",
                "sys_cau_hinh_thong_tin_website;getCauHinhThongTin",
               "sys_cau_hinh_thong_tin_website;getLoaiThongTin",
                 "sys_cau_hinh_thong_tin_website;getInfo",
               
            },
             
            
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_cau_hinh_thong_tin_website;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_thong_tin_website;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cau_hinh_thong_tin_website;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_thong_tin_website;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cau_hinh_thong_tin_website;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_thong_tin_website;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_cau_hinh_thong_tin_website;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_thong_tin_website;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_cau_hinh_thong_tin_website_model item)
        {
            if (string.IsNullOrEmpty(item.db.ten_truong))
            {
                ModelState.AddModelError("db.ten_truong", "required");
            }
            if (string.IsNullOrEmpty(item.db.dia_chi))
            {
                ModelState.AddModelError("db.dia_chi", "required");
              
            }

            if (string.IsNullOrEmpty(item.db.email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_cau_hinh_thong_tin_website_model item)
        {
            if (string.IsNullOrEmpty(item.db.ten_truong))
            {
                ModelState.AddModelError("db.ten_truong", "required");
            }
            if (string.IsNullOrEmpty(item.db.dia_chi))
            {
                ModelState.AddModelError("db.dia_chi", "required");

            }

            if (string.IsNullOrEmpty(item.db.email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_cau_hinh_thong_tin_website_model item)
        {
   
            //if (string.IsNullOrEmpty(item.db.noi_dung))
            //{
            //    ModelState.AddModelError("db.noi_dung", "required");
            //}
            var search = repo.FindAll().Where(d => d.db.tieu_de == item.db.tieu_de && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.tieu_de", "existed");
            }


            return ModelState.IsValid;
        }

    }
}
