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
    partial class sys_dien_giaController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_dien_gia",
            icon = "badge",
            module = "system",
            id = "sys_dien_gia",
            url = "/sys_dien_gia_index",
            title = "sys_dien_gia",
            translate = "NAV.sys_dien_gia",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_dien_gia;getListUse",
                "sys_dien_gia;getElementById",
                   "sys_dien_gia;getListUseNew",
                "sys_dien_gia;load_ngon_ngu",

            },
            list_controller_action_publicNonLogin = new List<string>(){

                "sys_dien_gia;getListUse",

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_dien_gia;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_dien_gia;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_dien_gia;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_dien_gia;edit",
                          "sys_dien_gia;edit_en",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_dien_gia;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_dien_gia;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_dien_gia;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_dien_gia;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_dien_gia_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_dien_gia_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateEditEn(sys_dien_gia_en_model item)
        {
            return checkModelStateCreateEditEn(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_dien_gia_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            //if (item.db.stt==null)
            //{
            //    ModelState.AddModelError("db.stt", "required");
            //}
            if (string.IsNullOrEmpty(item.db.chuc_danh))
            {
                ModelState.AddModelError("db.chuc_danh", "required");
            }
            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }
            var search1 = repo.FindAll().Where(d => d.db.chuc_danh == item.db.chuc_danh && d.db.id != item.db.id).Count();
            if (search1 > 0)
            {
                ModelState.AddModelError("db.chuc_danh", "existed");
            }

            return ModelState.IsValid;
        }
        private bool checkModelStateCreateEditEn(ActionEnumForm action, sys_dien_gia_en_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            //if (item.db.stt==null)
            //{
            //    ModelState.AddModelError("db.stt", "required");
            //}
            if (string.IsNullOrEmpty(item.db.chuc_danh))
            {
                ModelState.AddModelError("db.chuc_danh", "required");
            }
            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }
            var search1 = repo.FindAll().Where(d => d.db.chuc_danh == item.db.chuc_danh && d.db.id != item.db.id).Count();
            if (search1 > 0)
            {
                ModelState.AddModelError("db.chuc_danh", "existed");
            }

            return ModelState.IsValid;
        }
    }
}
