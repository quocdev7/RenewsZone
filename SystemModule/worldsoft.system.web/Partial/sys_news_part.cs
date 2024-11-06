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
    partial class sys_newsController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_news",
            icon = "badge",
            module = "system",
            id = "sys_news",
            url = "/sys_news_index",
            title = "sys_news",
            translate = "NAV.sys_news",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_news;getListUse",

                        "sys_news;getGroupNewsInfo",
                          "sys_news;getNews",
                                     "sys_news;getNewsByUser",
                                      "sys_news;DataHandlerPersonNews",
                                       "sys_news;create",
                                         "sys_news;edit",
                                         "sys_news;list_type_news_quan_tam",
                                                    "sys_news;getHotNews",
                                                     "sys_news;getListTinNoiBat",
                                                  "sys_news;DataHandlerComment",
                                                  "sys_news;hien_binh_luan",
                                                  "sys_news;an_binh_luan",
                                                  "sys_news;approval",
                                                  "sys_news;cancel",

                                            "sys_news;close_create",
                                            "sys_news;edit_language",



            },
            
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_news;getListNews",
                    "sys_news;getGroupNewsInfo",
                         "sys_news;getNews",
                           "sys_news;getNewsByUser",
                             "sys_news;getAllTypeNewsDetail",
                               "sys_news;get_comment",
                                "sys_news;getStats",
                               "sys_news;add_comment",
                               "sys_news;search_common",
                              "sys_news;getHomePageHotNews",
                              "sys_news;save_tin_tuc_noi_bat",
                              "sys_news;update_vi_tri_tin_noi_bat",
                              "sys_news;generateSearch_text",
                 "sys_news;search_news_common",

                   "sys_news;getNewsTuyenDung",
                   "sys_news;load_ngon_ngu",

                      "sys_news;sync_search_all",
                      "sys_news;load_icon",
                      "sys_news;get_title_news",

            },
             
            
            list_role = new List<ControllerRoleModel>()
            {
                new ControllerRoleModel()
                {
                    id="sys_news;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_news;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_news;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_news;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_news_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_news_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateEditLanguage(sys_news_language_model item)
        {
            return checkModelStateCreateEditLanguage(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_news_model item)
        {
            if (String.IsNullOrEmpty(item.db.noi_dung))
            {
                ModelState.AddModelError("db.noi_dung", "required");
            }
            else {
                var error = "";
                var msg = "";
                var lst_tu_ngu_cam = repo._context.sys_tu_ngu_cams.Where(q=>q.status_del==1).ToList();
                for (int i = 0; i < lst_tu_ngu_cam.Count; i++)
                {
                    var lst = lst_tu_ngu_cam[i].note.Split(';').ToList();

                    for (int j = 0; j < lst.Count; j++)
                    {
                        if (item.db.noi_dung.Contains(lst[j]))
                        {
                            error += lst[j] + ';';
                        }
                    }
                    msg =  error;

                };
                if (!String.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("db.noi_dung", "Nội dung có chứa từ ngữ nhạy cảm " + msg +". Hãy chỉnh sửa để có thể đăng bài");
                }
            }

            if (item.db.ngay_dang== null)
            {
                ModelState.AddModelError("db.ngay_dang", "required");
            }

            if (item.db.quyen_rieng_tu== null)
            {
                ModelState.AddModelError("db.quyen_rieng_tu", "required");
            }
            else
            {
              
              
            }
            if (item.khoa.Count()==0)
            {
                ModelState.AddModelError("db.id_khoa", "required");
            }

            if (item.hinhthuc.Count() == 0)
            {
                ModelState.AddModelError("db.hinh_thuc", "required");
            }

            if (string.IsNullOrEmpty(item.db.tieu_de))
            {
                ModelState.AddModelError("db.tieu_de", "required");
            }
            else
            {
                var error = "";
                var msg = "";
                var lst_tu_ngu_cam = repo._context.sys_tu_ngu_cams.Where(q => q.status_del == 1).ToList();
                for (int i = 0; i < lst_tu_ngu_cam.Count; i++)
                {
                    var lst = lst_tu_ngu_cam[i].note.Split(';').ToList();

                    for (int j = 0; j < lst.Count; j++)
                    {
                        if (item.db.tieu_de.Contains(lst[j]))
                        {
                            error += lst[j] + ';';
                        }
                    }
                    msg =  error;

                };
                if (!String.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("db.tieu_de", "Tiêu đề có chứa từ ngữ nhạy cảm " + msg);
                }
            }
            if (!String.IsNullOrEmpty(item.db.noi_dung_trang_bia))
            {
                var error = "";
                var msg = "";
                var lst_tu_ngu_cam = repo._context.sys_tu_ngu_cams.Where(q => q.status_del == 1).ToList();
                for (int i = 0; i < lst_tu_ngu_cam.Count; i++)
                {
                    var lst = lst_tu_ngu_cam[i].note.Split(';').ToList();

                    for (int j = 0; j < lst.Count; j++)
                    {
                        if (item.db.noi_dung_trang_bia.Contains(lst[j]))
                        {
                            error += lst[j] + ';';
                        }
                    }
                    msg =  error;

                };
                if (!String.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("db.tom_tat", "Tóm tắt có chứa từ ngữ nhạy cảm " + msg);
                }
            }
            if (!String.IsNullOrEmpty(item.db.nguon_tin_tuc))
            {
                var error = "";
                var msg = "";
                var lst_tu_ngu_cam = repo._context.sys_tu_ngu_cams.Where(q => q.status_del == 1).ToList();
                for (int i = 0; i < lst_tu_ngu_cam.Count; i++)
                {
                    var lst = lst_tu_ngu_cam[i].note.Split(';').ToList();

                    for (int j = 0; j < lst.Count; j++)
                    {
                        if (item.db.nguon_tin_tuc.Contains(lst[j]))
                        {
                            error += lst[j] + ';';
                        }
                    }
                    msg = error;

                };
                if (!String.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("db.nguon_tin_tuc", "Nguồn tin tức có chứa từ ngữ nhạy cảm " + msg);
                }
            }
            if (string.IsNullOrEmpty(item.db.id_type_news))
            {
                ModelState.AddModelError("db.id_type_news", "required");
            }
            if (string.IsNullOrEmpty(item.db.id_group_news))
            {
                ModelState.AddModelError("db.id_group_news", "required");
            }
            //if (string.IsNullOrEmpty(item.db.image))
            //{
            //    ModelState.AddModelError("db.image", "required");
            //}

            var search = repo.FindAll().Where(d => d.db.tieu_de == item.db.tieu_de && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.tieu_de", "existed");
            }



            return ModelState.IsValid;
        }
        private bool checkModelStateCreateEditLanguage(ActionEnumForm action, sys_news_language_model item)
        {
            if (String.IsNullOrEmpty(item.db.noi_dung))
            {
                ModelState.AddModelError("db.noi_dung", "required");
            }
            else
            {
                var error = "";
                var msg = "";
                var lst_tu_ngu_cam = repo._context.sys_tu_ngu_cams.Where(q => q.status_del == 1).ToList();
                for (int i = 0; i < lst_tu_ngu_cam.Count; i++)
                {
                    var lst = lst_tu_ngu_cam[i].note.Split(';').ToList();

                    for (int j = 0; j < lst.Count; j++)
                    {
                        if (item.db.noi_dung.Contains(lst[j]))
                        {
                            error += lst[j] + ';';
                        }
                    }
                    msg = error;

                };
                if (!String.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("db.noi_dung", "Nội dung có chứa từ ngữ nhạy cảm " + msg + ". Hãy chỉnh sửa để có thể đăng bài");
                }
            }


            if (string.IsNullOrEmpty(item.db.tieu_de))
            {
                ModelState.AddModelError("db.tieu_de", "required");
            }
            else
            {
                var error = "";
                var msg = "";
                var lst_tu_ngu_cam = repo._context.sys_tu_ngu_cams.Where(q => q.status_del == 1).ToList();
                for (int i = 0; i < lst_tu_ngu_cam.Count; i++)
                {
                    var lst = lst_tu_ngu_cam[i].note.Split(';').ToList();

                    for (int j = 0; j < lst.Count; j++)
                    {
                        if (item.db.tieu_de.Contains(lst[j]))
                        {
                            error += lst[j] + ';';
                        }
                    }
                    msg = error;

                };
                if (!String.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("db.tieu_de", "Tiêu đề có chứa từ ngữ nhạy cảm " + msg);
                }
            }
            if (!String.IsNullOrEmpty(item.db.noi_dung_trang_bia))
            {
                var error = "";
                var msg = "";
                var lst_tu_ngu_cam = repo._context.sys_tu_ngu_cams.Where(q => q.status_del == 1).ToList();
                for (int i = 0; i < lst_tu_ngu_cam.Count; i++)
                {
                    var lst = lst_tu_ngu_cam[i].note.Split(';').ToList();

                    for (int j = 0; j < lst.Count; j++)
                    {
                        if (item.db.noi_dung_trang_bia.Contains(lst[j]))
                        {
                            error += lst[j] + ';';
                        }
                    }
                    msg = error;

                };
                if (!String.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("db.tom_tat", "Tóm tắt có chứa từ ngữ nhạy cảm " + msg);
                }
            }
            if (!String.IsNullOrEmpty(item.db.nguon_tin_tuc))
            {
                var error = "";
                var msg = "";
                var lst_tu_ngu_cam = repo._context.sys_tu_ngu_cams.Where(q => q.status_del == 1).ToList();
                for (int i = 0; i < lst_tu_ngu_cam.Count; i++)
                {
                    var lst = lst_tu_ngu_cam[i].note.Split(';').ToList();

                    for (int j = 0; j < lst.Count; j++)
                    {
                        if (item.db.nguon_tin_tuc.Contains(lst[j]))
                        {
                            error += lst[j] + ';';
                        }
                    }
                    msg = error;

                };
                if (!String.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("db.nguon_tin_tuc", "Nguồn tin tức có chứa từ ngữ nhạy cảm " + msg);
                }
            }
           
            var search = repo._context.sys_news_languages.Where(d => d.tieu_de == item.db.tieu_de && d.id_news != item.db.id_news).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.tieu_de", "existed");
            }



            return ModelState.IsValid;
        }
    }
}
