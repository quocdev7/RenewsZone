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
    partial class sys_event_qaController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_event_qa",
            icon = "menu",
            module = "system",
            id = "sys_event_qa",
            url = "/sys_event_qa_index",
            title = "sys_event_qa",
            translate = "NAV.sys_event_qa",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_event_qa;getListUse",
                 "sys_event_qa;load_ngon_ngu",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_event_qa;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_qa;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event_qa;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_qa;edit",
                          "sys_event_qa;edit_language",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event_qa;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_qa;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_event_qa;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_qa;DataHandler",
                    }
                }
            }
        };
        private  bool checkModelStateCreate(sys_event_qa_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_event_qa_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateEdit_language(sys_event_qa_en_model item)
        {
            return checkModelStateCreateEdit_language(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_event_qa_model item)
        {
            if (string.IsNullOrEmpty(item.db.answer))
            {
                ModelState.AddModelError("db.answer", "required");
            }
            if (string.IsNullOrEmpty(item.db.question))
            {
                ModelState.AddModelError("db.question", "required");
            }




            //var check_ma = repo.FindAll().Where(d => d.db.title == item.db.title && d.db.id != item.db.id && d.db.status_del == 1).Count();
            //if (check_ma > 0)
            //{
            //    ModelState.AddModelError("db.ma", "existed");
            //}

            //var check_ten = repo.FindAll().Where(d => d.db.title == item.db.title && d.db.id != item.db.id && d.db.status_del == 1).Count();
            //if (check_ten > 0)
            //{
            //    ModelState.AddModelError("db.ten", "existed");
            //}


            return ModelState.IsValid;
        }
        private bool checkModelStateCreateEdit_language(ActionEnumForm action, sys_event_qa_en_model item)
        {
            if (string.IsNullOrEmpty(item.db.answer))
            {
                ModelState.AddModelError("db.answer", "required");
            }
            if (string.IsNullOrEmpty(item.db.question))
            {
                ModelState.AddModelError("db.question", "required");
            }




            //var check_ma = repo.FindAll().Where(d => d.db.title == item.db.title && d.db.id != item.db.id && d.db.status_del == 1).Count();
            //if (check_ma > 0)
            //{
            //    ModelState.AddModelError("db.ma", "existed");
            //}

            //var check_ten = repo.FindAll().Where(d => d.db.title == item.db.title && d.db.id != item.db.id && d.db.status_del == 1).Count();
            //if (check_ten > 0)
            //{
            //    ModelState.AddModelError("db.ten", "existed");
            //}


            return ModelState.IsValid;
        }
    }
}
