using System.Collections.Generic;
using System.Linq;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_san_phamController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_san_pham",
            icon = "live_help",
            module = "system",
            id = "sys_san_pham",
            url = "/sys_san_pham_index",
            title = "sys_san_pham",
            translate = "NAV.sys_san_pham",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_san_pham;getListUse",
                "sys_san_pham;get_san_pham",
                "sys_san_pham;getListSanPhamQuanTam",
                "sys_san_pham;get_list_hinh_anh",
                 "sys_san_pham;getElementById",
                     "sys_san_pham;getElementByIdNew",
                "sys_san_pham;load_ngon_ngu",
                 "sys_san_pham;edit_language",
                 "sys_san_pham;get_title_san_pham",




            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_san_pham;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_san_pham;create",
                            "sys_san_pham;save_image",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_san_pham;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_san_pham;edit",
                           "sys_san_pham;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_san_pham;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_san_pham;delete",
                          "sys_san_pham;delete_file",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_san_pham;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_san_pham;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateEditLanguage(sys_san_pham_language_model item)
        {
            return checkModelStateCreateEditLanguage(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEditLanguage(ActionEnumForm action, sys_san_pham_language_model item)
        {

            if (string.IsNullOrEmpty(item.db.ten_san_pham))
            {
                ModelState.AddModelError("db.ten_san_pham", "required");
            }

            return ModelState.IsValid;
        }
        private bool checkModelStateCreate(sys_san_pham_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_san_pham_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateSaveImage(sys_san_pham_model item)
        {
            if (item.list_file.Count() == 0)
            {
                ModelState.AddModelError("list_file", "required");
            }
            return ModelState.IsValid;
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_san_pham_model item)
        {

            if (string.IsNullOrEmpty(item.db.id_loai))
            {
                ModelState.AddModelError("db.id_loai", "required");
            }
            //else
            //{
            //    var ma_san_pham = repo.FindAll().Where(d => d.db.ma_san_pham == item.db.ma_san_pham && d.db.id != item.db.id).Count();
            //    if (ma_san_pham > 0)
            //    {
            //        ModelState.AddModelError("db.ma_san_pham", "existed");
            //    }

            //}
            if (string.IsNullOrEmpty(item.db.ten_san_pham))
            {
                ModelState.AddModelError("db.ten_san_pham", "required");
            }

            if ((item.db.stt ?? 0) == 0)
            {
                ModelState.AddModelError("db.stt", "required");
            }
            //if ((item.db.so_tien ?? 0) == 0)
            //{
            //    ModelState.AddModelError("db.so_tien", "required");
            //}

            if (string.IsNullOrEmpty(item.db.hinh_anh))
            {
                ModelState.AddModelError("db.hinh_anh", "required");
            }
            if (string.IsNullOrEmpty(item.db.hinh_anh_mobile))
            {
                ModelState.AddModelError("db.anh_mobile", "required");
            }


            return ModelState.IsValid;
        }

    }
}
