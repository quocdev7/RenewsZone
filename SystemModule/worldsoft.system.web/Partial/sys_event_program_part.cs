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
    partial class sys_event_programController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_event_program",
            icon = "menu",
            module = "system",
            id = "sys_event_program",
            url = "/sys_event_program_index",
            title = "sys_event_program",
            translate = "NAV.sys_event_program",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_event_program;getListUse",
                "sys_event_program;load_ngon_ngu",



            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_event_program;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_program;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event_program;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_program;edit",
                          "sys_event_program;edit_language",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event_program;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_program;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_event_program;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_program;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_event_program_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_event_program_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateEdit_language(sys_event_program_en_model item)
        {
            return checkModelStateCreateEdit_language(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_event_program_model item)
        {

            
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (item.db.start_time == null)
            {
                ModelState.AddModelError("db.start_time", "required");
            }
            if (item.db.end_time == null)
            {
                ModelState.AddModelError("db.end_time", "required");
            }

            if (item.db.start_time > item.db.end_time )
            {
                ModelState.AddModelError("start_time", "system.msg_thoi_gian_bat_dau_phai_hon_thoi_gian_ket_thuc");
            }else
            {

                var e = repo._context.sys_events.Where(q=>q.id == item.db.event_id).FirstOrDefault();
                if (
                    item.db.start_time.Value.Day ==  item.db.end_time.Value.Day
                    && item.db.start_time.Value.Month == item.db.end_time.Value.Month
                    && item.db.start_time.Value.Year == item.db.end_time.Value.Year 
                    && e.time_start < item.db.start_time
                    && e.time_end > item.db.end_time

                    )
                {
                   
                }
                else
                {
                    //Thòi gian bắt đầu chương trình phải nằm trong khoảng thời gian diễn ra sự kiện và phải cùng 1 ngày
                    ModelState.AddModelError("start_time", "system.msg_khong_hop_le");

                }
                    

            }


            //if (string.IsNullOrEmpty(item.db.presenter))
            //{
            //    ModelState.AddModelError("db.presenter", "required");
            //}


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

        private bool checkModelStateCreateEdit_language(ActionEnumForm action, sys_event_program_en_model item)
        {


            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }          
            return ModelState.IsValid;
        }
    }
}

