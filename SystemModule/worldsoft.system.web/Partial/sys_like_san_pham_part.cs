using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_like_san_phamController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_like_san_pham",
            icon = "badge",
            module = "system",
            id = "sys_like_san_pham",
            url = "/sys_like_san_pham_index",
            title = "sys_like_san_pham",
            translate = "NAV.sys_like_san_pham",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_like_san_pham;getListUse",

            },
            list_controller_action_publicNonLogin = new List<string>()
            {     "sys_like_san_pham;theo_doi",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_like_san_pham;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_like_san_pham;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_like_san_pham;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_like_san_pham;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_like_san_pham;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_like_san_pham;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_like_san_pham;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_like_san_pham;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_like_san_pham_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_like_san_pham_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_like_san_pham_model item)
        {

            if (!string.IsNullOrEmpty(item.db.email))
            {

                var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                var checkEmail = rgEmail.IsMatch(item.db.email);
                if (checkEmail == false)
                {
                    ModelState.AddModelError("emailKhongHopLe", "system.emailKhongHopLe");
                }
                else
                {
                    var search = repo.FindAll().Where(d => d.db.email == item.db.email && d.db.id != item.db.id).Count();
                    if (search > 0)
                    {
                        ModelState.AddModelError("emailTonTai", "system.emailTonTai");
                    }
                }


            }
            else
            {
                ModelState.AddModelError("db.email", "required");
            }
            if (string.IsNullOrEmpty(item.capcha))
            {
                ModelState.AddModelError("capcha", "system.capcha");
            }
            else
            {
                var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");
                if (CaptchaCode.ToLower() != item.capcha.ToLower())
                {
                    ModelState.AddModelError("capcha_khong_chinh_xac", "captcha_invalid");
                }

            }



            return ModelState.IsValid;
        }

    }
}
