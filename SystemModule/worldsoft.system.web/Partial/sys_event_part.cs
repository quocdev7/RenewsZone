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
    partial class sys_eventController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_event",
            icon = "menu",
            module = "system",
            id = "sys_event",
            url = "/sys_event_index",
            title = "sys_event",
            translate = "NAV.sys_event",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_event;getListUse",
                       "sys_event;getEventParticipate",
                             "sys_event;getListUse",
                        "sys_event;register_event",
                               "sys_event;countSuKien",
                                "sys_event;getNguoiNhanHocBongEvent",
                                "sys_event;getNhaTaiTroEvent",
                                   "sys_event;get_hot_events",
                                      "sys_event;get_list_group_event_program",
                                           "sys_event;approval",
                                           "sys_event;cancel",
                                           "sys_event;getListUseKhachMoi",
                                            "sys_event;load_ngon_ngu",
                                            "sys_event;getListEventUser",


            },
            list_controller_action_publicNonLogin = new List<string>()
            {

                     "sys_event;getEventCurrents",
                   "sys_event;getDetailEvent",
                   "sys_event;getEvents",
                   "sys_event;getListUse",
                        "sys_event;register_event",
                         "sys_event;get_list_file",
                            "sys_event;get_list_image",
                                "sys_event;get_list_event_program",
                                   "sys_event;get_list_tham_du",
                                    "sys_event;getQAEvent",
                                     "sys_event;getQAEventnew",
                                     "sys_event;get_list_danh_gia",
                                         "sys_event;getMyEvent",
                                          "sys_event;getNguoiNhanHocBongEvent",
                                             "sys_event;get_list_group_event_program",
                                           "sys_event;get_list_hinh_anh",
                                           "sys_event;getNhaTaiTroEvent",
                                           "sys_event;getListHinhAnhNew",
                                           "sys_event;load_ngon_ngu",
                                           "sys_event;get_title_event",



            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_event;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_event;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_event;edit",

                          "sys_event;edit_en",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_event;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_event;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_event;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_event_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }




        private bool checkModelStateEdit(sys_event_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateEditEn(sys_event_en_model item)
        {
            return checkModelStateCreateEditEn(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_event_model item)
        {
         
                if (item.types.Count() == 0) { ModelState.AddModelError("db.type", "required"); }
             
         
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
            //if (string.IsNullOrEmpty(item.db.logo))
            //{
            //    ModelState.AddModelError("db.logo", "required");
            //}
            if (item.db.time_end < item.db.time_start)
            {
                ModelState.AddModelError("thoi_gian_ket_thuc", "system.thoigianketthucphailonhonthoigianbatdau");
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
            if (item.khoa.Count() == 0)
            {
                ModelState.AddModelError("db.id_khoa", "required");
            }

            if (item.hinhthuc.Count() == 0)
            {
                ModelState.AddModelError("db.hinh_thuc", "required");
            }

            if ((item.db.max_person_participate ?? 0) == 0)
            {
                ModelState.AddModelError("db.max_person_participate", "required");
            }

            if(item.db.is_register_event == null)
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
                            ModelState.AddModelError("den_han_dang_ky", "system.ngaydenhandangkyphainhohonthoigianketthuc");
                        }
                    }

                }
                else
                {
                   
                }

            }

            return ModelState.IsValid;
        }
        private bool checkModelStateCreateEditEn(ActionEnumForm action, sys_event_en_model item)
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
            //if (string.IsNullOrEmpty(item.db.ban_to_chuc))
            //{
            //    ModelState.AddModelError("db.ban_to_chuc", "required");
            //}

            //if (item.db.time_start == null)
            //{
            //    ModelState.AddModelError("db.time_start", "required");
            //}
            //if (item.db.time_end == null)
            //{
            //    ModelState.AddModelError("db.time_end", "required");
            //}
            //if (string.IsNullOrEmpty(item.db.logo))
            //{
            //    ModelState.AddModelError("db.logo", "required");
            //}
            //if (item.db.time_end < item.db.time_start)
            //{
            //    ModelState.AddModelError("db.time_end", "system.thoigianketthucphailonhonthoigianbatdau");
            //}

            if (string.IsNullOrEmpty(item.db.mo_ta))
            {
                ModelState.AddModelError("db.mo_ta", "required");
            }



            //if (item.db.quyen_rieng_tu == null)
            //{
            //    ModelState.AddModelError("db.quyen_rieng_tu", "required");
            //}


            //if (item.db.so_tien != null && item.db.so_tien != 0)
            //{
            //    if (string.IsNullOrEmpty(item.db.id_tiente))
            //    {
            //        ModelState.AddModelError("db.id_tiente", "required");
            //    }
            //}
            //if (item.khoa.Count() == 0)
            //{
            //    ModelState.AddModelError("db.id_khoa", "required");
            //}

            //if (item.hinhthuc.Count() == 0)
            //{
            //    ModelState.AddModelError("db.hinh_thuc", "required");
            //}





            //if ((item.db.max_person_participate ?? 0) == 0)
            //{
            //    ModelState.AddModelError("db.max_person_participate", "required");
            //}

            //if (item.db.is_register_event == null)
            //{
            //    ModelState.AddModelError("db.is_register_event", "required");
            //}
            //else
            //{
            //    if ((item.db.is_register_event ?? 0) == 1)
            //    {
            //        if (item.db.ngay_den_han_dang_ky == null)
            //        {
            //            ModelState.AddModelError("db.ngay_den_han_dang_ky", "required");
            //        }
            //        else
            //        {
            //            if (item.db.ngay_den_han_dang_ky > item.db.time_end)
            //            {
            //                ModelState.AddModelError("db.ngay_den_han_dang_ky", "system.ngaydenhandangkyphainhohonthoigianketthuc");
            //            }
            //        }

            //    }
            //    else
            //    {

            //    }

            //}












            return ModelState.IsValid;
        }
    }
}
