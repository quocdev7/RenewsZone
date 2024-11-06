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
    partial class sys_cot_moc_su_kienController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_cot_moc_su_kien",
            icon = "live_help",
            module = "system",
            id = "sys_cot_moc_su_kien",
            url = "/sys_cot_moc_su_kien_index",
            title = "sys_cot_moc_su_kien",
            translate = "NAV.sys_cot_moc_su_kien",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_cot_moc_su_kien;getListUse",
            },
            list_controller_action_public = new List<string>(){

                     "sys_cot_moc_su_kien;load_ngon_ngu",
                     "sys_cot_moc_su_kien;edit_language",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_cot_moc_su_kien;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_cot_moc_su_kien;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cot_moc_su_kien;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_cot_moc_su_kien;edit",
                           "sys_cot_moc_su_kien;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cot_moc_su_kien;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_cot_moc_su_kien;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_cot_moc_su_kien;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_cot_moc_su_kien;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_cot_moc_su_kien_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_cot_moc_su_kien_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateEdit_language(sys_cot_moc_su_kien_language_model item)
        {
            return checkModelStateCreateEdit_language(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_cot_moc_su_kien_model item)
        {
           
            if (item.db.stt == null)
            {
                ModelState.AddModelError("db.stt", "required");
            }

            if (string.IsNullOrEmpty(item.db.time))
            {
                ModelState.AddModelError("db.time", "required");
            }


            if (string.IsNullOrEmpty(item.db.note))
            {
                ModelState.AddModelError("db.note", "required");
            }
            if (string.IsNullOrEmpty(item.db.note_mobile))
            {
                ModelState.AddModelError("db.note_mobile", "required");
            }

            var search = repo.FindAll().Where(d => d.db.time == item.db.time && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.time", "existed");
            }


            return ModelState.IsValid;
        }
        private bool checkModelStateCreateEdit_language(ActionEnumForm action, sys_cot_moc_su_kien_language_model item)
        {

            if (string.IsNullOrEmpty(item.db.note))
            {
                ModelState.AddModelError("db.note", "required");
            }
            if (string.IsNullOrEmpty(item.db.note_mobile))
            {
                ModelState.AddModelError("db.note_mobile", "required");
            }
            return ModelState.IsValid;
        }

    }
}
