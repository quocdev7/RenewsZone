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
    partial class person_eventController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "person_event",
            icon = "menu",
            module = "system",
            id = "person_event",
            url = "/person_event_index",
            title = "person_event",
            translate = "NAV.person_event",
            type = "item",
            list_controller_action_public = new List<string>(){
               "person_event;getListUse",
                                  "person_event;getEventParticipate",
                                  "person_event;getListUse",
                                  "person_event;register_event",
                                  "person_event;countSuKien",
                                  "person_event;create",
                                  "person_event;edit",
                                  "person_event;DataHandler",
                                  "person_event;delete",
                                  "person_event;openDialogDetail",

                                          "person_event;approval",
                                                  "person_event;reject",




            },
            list_controller_action_publicNonLogin = new List<string>()
            {

                     "person_event;getEventCurrents",
                   "person_event;getDetailEvent",
                   "person_event;getEvents",
                   "person_event;getListUse",
                        "person_event;register_event",
                         "person_event;get_list_file",
                            "person_event;get_list_image",
                                "person_event;get_list_event_program",
                                   "person_event;get_list_tham_du",
                                    "person_event;getQAEvent",
                                     "person_event;get_list_danh_gia",
                                         "person_event;getMyEvent",
                                           "person_event;get_list_hinh_anh",
                                           "person_event;create",
                                        


            },

        };
        private bool checkModelStateCreate(person_event_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }




        private bool checkModelStateEdit(person_event_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, person_event_model item)
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
