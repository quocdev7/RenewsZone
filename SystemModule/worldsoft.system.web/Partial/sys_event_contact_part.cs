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
    partial class sys_event_contactController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_event_contact",
            icon = "menu",
            module = "system",
            id = "sys_event_contact",
            url = "/sys_event_contact_index",
            title = "sys_event_contact",
            translate = "NAV.sys_event_contact",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_event_contact;getListUse",
                "sys_event_contact;get_list_danh_gia",
                "sys_event_contact;danh_gia",

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_event_contact;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_contact;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event_contact;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_contact;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event_contact;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_contact;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_event_contact;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_contact;DataHandler",
                    }
                }
            }
        };
        private  bool checkModelStateCreate(sys_event_contact_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_event_contact_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_event_contact_model item)
        {
            if (string.IsNullOrEmpty(item.db.event_id))
            {
                ModelState.AddModelError("db.event_id", "required");
            }
            if (string.IsNullOrEmpty(item.db.user_id))
            {
                ModelState.AddModelError("db.user_id", "required");
            }



            //var check_ma = repo.FindAll().Where(d => d.db.title == item.db.title && d.db.id != item.db.id && d.db.status_del == 1).Count();
            //if (check_ma > 0)
            //{
            //    ModelState.AddModelError("db.ma", "existed");
            //}

         
            var check_nguoi_lien_he = repo.FindAll().Where(d => d.db.user_id == item.db.user_id && d.db.id != item.db.id && d.db.role == 2 && d.db.event_id == item.db.event_id).Count();
            if (check_nguoi_lien_he > 0)
            {
                ModelState.AddModelError("db.user_id", "existed");
            }

            var so_nguoi_tham_gia = repo._context.sys_events.Where(q => q.id == item.db.event_id).Select(q => q.max_person_participate).SingleOrDefault() ?? 0;

            var so_nguoi = repo._context.sys_event_participates.Where(q => q.event_id == item.db.event_id && q.check_in_status != 2 && q.role ==1).Count()+1;

            if (action == ActionEnumForm.create && (so_nguoi > so_nguoi_tham_gia))
            {
                {
                    ModelState.AddModelError("db.user_id", "system.vuotquasoluongchophep");
                }
            }
            return ModelState.IsValid;
        }

    }
}
