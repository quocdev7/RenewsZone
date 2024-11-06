using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.DataBase.System;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_approved_eventController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_approved_event",
            icon = "menu",
            module = "system",
            id = "sys_approved_event",
            url = "/sys_approved_event_index",
            title = "sys_approved_event",
            translate = "NAV.sys_approved_event",
            type = "item",
            list_controller_action_public = new List<string>(){
               "sys_approved_event;getListUse",
                                  "sys_approved_event;getEventParticipate",
                                  "sys_approved_event;getListUse",
                                  "sys_approved_event;register_event",
                                  "sys_approved_event;countSuKien",
                                  "sys_approved_event;create",
                                  "sys_approved_event;edit",
                                  "sys_approved_event;DataHandler",
                                  "sys_approved_event;delete",
                                  "sys_approved_event;openDialogDetail",

                                          "sys_approved_event;approval",
                                                  "sys_approved_event;reject",
                                          "sys_approved_event;cancel",



            },
            list_controller_action_publicNonLogin = new List<string>()
            {

                     "sys_approved_event;getEventCurrents",
                   "sys_approved_event;getDetailEvent",
                   "sys_approved_event;getEvents",
                   "sys_approved_event;getListUse",
                        "sys_approved_event;register_event",
                         "sys_approved_event;get_list_file",
                            "sys_approved_event;get_list_image",
                                "sys_approved_event;get_list_event_program",
                                   "sys_approved_event;get_list_tham_du",
                                    "sys_approved_event;getQAEvent",
                                     "sys_approved_event;get_list_danh_gia",
                                         "sys_approved_event;getMyEvent",
                                           "sys_approved_event;get_list_hinh_anh",
                                           "sys_approved_event;create",
                                           "sys_approved_event;openDialogDetail",


            },
        
        };
        private bool checkModelStateCreate(sys_approved_event_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }




        private bool checkModelStateEdit(sys_approved_event_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_approved_event_model item)
        {
            if (string.IsNullOrEmpty(item.db.title))
            {
                ModelState.AddModelError("db.title", "required");
            }
            else
            {
                var title = repo.FindAll().Where(d => d.db.title == item.db.title && d.db.id != item.db.id && d.db.status_del == 1).Count();
                if (title > 0)
                {
                    ModelState.AddModelError("db.title", "existed");
                }

            }
            if (string.IsNullOrEmpty(item.db.ban_to_chuc))
            {
                ModelState.AddModelError("db.ban_to_chuc", "required");
            }

            if (item.db.time_start == null)
            {
                ModelState.AddModelError("db.time_start", "required");
            }
            if (item.db.time_end == null)
            {
                ModelState.AddModelError("db.time_end", "required");
            }
            if (string.IsNullOrEmpty(item.db.logo))
            {
                ModelState.AddModelError("db.logo", "required");
            }
            if (item.db.time_end < item.db.time_start)
            {
                ModelState.AddModelError("db.time_end", "system.thoigianketthucphailonhonthoigianbatdau");
            }

            if (string.IsNullOrEmpty(item.db.mo_ta))
            {
                ModelState.AddModelError("db.mo_ta", "required");
            }


            if (item.db.quyen_rieng_tu == null)
            {
                ModelState.AddModelError("db.quyen_rieng_tu", "required");
            }


            if (item.db.so_tien != null && item.db.so_tien != 0)
            {
                if (string.IsNullOrEmpty(item.db.id_tiente))
                {
                    ModelState.AddModelError("db.id_tiente", "required");
                }
            }





            if ((item.db.max_person_participate ?? 0) == 0)
            {
                ModelState.AddModelError("db.max_person_participate", "required");
            }

            if (item.db.is_register_event == null)
            {
                ModelState.AddModelError("db.is_register_event", "required");
            }
            else
            {
                if ((item.db.is_register_event ?? 0) == 1)
                {
                    if (item.db.ngay_den_han_dang_ky == null)
                    {
                        ModelState.AddModelError("db.ngay_den_han_dang_ky", "required");
                    }
                    else
                    {
                        if (item.db.ngay_den_han_dang_ky > item.db.time_end)
                        {
                            ModelState.AddModelError("db.ngay_den_han_dang_ky", "system.ngaydenhandangkyphainhohonthoigianketthuc");
                        }
                    }

                }
                else
                {

                }

            }


            return ModelState.IsValid;
        }

    }
}
