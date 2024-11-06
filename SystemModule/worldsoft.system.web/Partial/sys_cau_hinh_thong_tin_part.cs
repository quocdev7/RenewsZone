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
    partial class sys_cau_hinh_thong_tinController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_cau_hinh_thong_tin",
            icon = "badge",
            module = "system",
            id = "sys_cau_hinh_thong_tin",
            url = "/sys_cau_hinh_thong_tin_index",
            title = "sys_cau_hinh_thong_tin",
            translate = "NAV.sys_cau_hinh_thong_tin",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_cau_hinh_thong_tin;getListUse",
                "sys_cau_hinh_thong_tin;getCauHinhThongTin",
                "sys_cau_hinh_thong_tin;edit_language",
                


            },
            
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_cau_hinh_thong_tin;getListUse",
                "sys_cau_hinh_thong_tin;getCauHinhThongTin",
               "sys_cau_hinh_thong_tin;getLoaiThongTin",
                 "sys_cau_hinh_thong_tin;getInfo",
                   "sys_cau_hinh_thong_tin;getListUseNew",
                 

            },
             
            
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_cau_hinh_thong_tin;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_thong_tin;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cau_hinh_thong_tin;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_thong_tin;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cau_hinh_thong_tin;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_thong_tin;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_cau_hinh_thong_tin;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_cau_hinh_thong_tin;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_cau_hinh_thong_tin_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_cau_hinh_thong_tin_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        //private bool checkModelStateEdit(sys_cau_hinh_thong_tin_language_model item)
        //{
        //    return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        //}
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_cau_hinh_thong_tin_model item)
        {
            if (string.IsNullOrEmpty(item.db.tieu_de))
            {
                ModelState.AddModelError("db.tieu_de", "required");
            }
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
        //private bool checkModelStateCreateEdit(ActionEnumForm action, sys_cau_hinh_thong_tin_language_model item)
        //{
        //    if (string.IsNullOrEmpty(item.db.tieu_de))
        //    {
        //        ModelState.AddModelError("db.tieu_de", "required");
        //    }
        //    //if (string.IsNullOrEmpty(item.db.noi_dung))
        //    //{
        //    //    ModelState.AddModelError("db.noi_dung", "required");
        //    //}
        //    var search = repo.FindAll().Where(d => d.db.tieu_de == item.db.tieu_de && d.db.id != item.db.id).Count();
        //    if (search > 0)
        //    {
        //        ModelState.AddModelError("db.tieu_de", "existed");
        //    }


        //    return ModelState.IsValid;
        //}

    }
}
