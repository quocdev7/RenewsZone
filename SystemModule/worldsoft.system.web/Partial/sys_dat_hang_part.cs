using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_dat_hangController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_dat_hang",
            icon = "live_help",
            module = "system",
            id = "sys_dat_hang",
            url = "/sys_dat_hang_index",
            title = "sys_dat_hang",
            translate = "NAV.sys_dat_hang",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_dat_hang;create_portal",
                "sys_dat_hang;get_san_pham_dat_hang",
                "sys_dat_hang;getElementById",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_dat_hang;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_dat_hang;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_dat_hang;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_dat_hang;edit",
                           "sys_dat_hang;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_dat_hang;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_dat_hang;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_dat_hang;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_dat_hang;DataHandler",
                    }
                }
            }
        };
        public string getCode()
        {
            var max = "";
            var date = DateTime.Now.ToString("yyMMdd");
            var prefix = date;
            var numIncrease = 6;
            var max_query = repo._context.sys_dat_hangs
            .Where(d => d.ma_don_hang.StartsWith(prefix))
            .Where(d => d.ma_don_hang.Length == prefix.Length + numIncrease)
            .Select(d => d.ma_don_hang);
            if (max_query.Count() > 0)
            {
                max = max_query.Max();
            }
            var code = generateCode(prefix, numIncrease, max);

            return code;
        }
        public string generateCode(string preFixCode, int Num, string max)
        {
            var result = preFixCode;
            int numGenerate = 1;
            for (int i = 0; i < Num; i++)
            {
                numGenerate = numGenerate * 10;
            }
            if (System.String.IsNullOrEmpty(max))
            {
                result += ((numGenerate + 1) + "").Remove(0, 1);
            }
            else
            {
                var parse = int.Parse(max.Replace(preFixCode, ""));
                result += (numGenerate + (parse + 1)).ToString().Remove(0, 1);
            }
            return result;
        }
        private bool checkModelStateCreatePortal(sys_dat_hang_model item)
        {

            if (string.IsNullOrEmpty(item.db.full_name))
            {
                ModelState.AddModelError("db.full_name", "required");
            }

            if (string.IsNullOrEmpty(item.db.phone))
            {
                ModelState.AddModelError("db.phone", "required");
            }
            else
            {
                if (item.db.phone.Length > 10)
                {
                    ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                }
                else
                {
                    var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
            |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
            |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
            |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

                    //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
                    var checkSDT = rgSoDienThoai.IsMatch(item.db.phone);
                    if (checkSDT == false)
                    {
                        ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                    }

                }

            }
            if (string.IsNullOrEmpty(item.db.email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            else
            {

                var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                var checkEmail = rgEmail.IsMatch(item.db.email);
                if (checkEmail == false)
                {
                    ModelState.AddModelError("db.email", "system.emailKhongHopLe");
                }

            }

            if ((item.db.tinh_thanh ?? 0) == 0)
            {
                ModelState.AddModelError("db.tinh_thanh_user", "required");
            }


            if ((item.db.quan_huyen ?? 0) == 0)
            {
                ModelState.AddModelError("db.quan_huyen_user", "required");
            }
            if (string.IsNullOrEmpty(item.db.dia_chi))
            {
                ModelState.AddModelError("db.dia_chi_user", "required");
            }
            if (!string.IsNullOrEmpty(item.db.ten_cong_ty))
            {
                if ((item.db.tinh_thanh_cong_ty ?? 0) == 0)
                {
                    ModelState.AddModelError("db.tinh_thanh_cong_ty", "required");
                }

                if ((item.db.quan_huyen_cong_ty ?? 0) == 0)
                {
                    ModelState.AddModelError("db.quan_huyen_cong_ty", "required");
                }
                if (string.IsNullOrEmpty(item.db.dia_chi_cong_ty))
                {
                    ModelState.AddModelError("db.dia_chi_cong_ty", "required");
                }
                if (string.IsNullOrEmpty(item.db.ma_so_thue))
                {
                    ModelState.AddModelError("db.ma_so_thue", "required");
                }
            }

            if (string.IsNullOrEmpty(item.captcha))
            {
                ModelState.AddModelError("captcha", "required");
            }
            else
            {
                var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");
                if (CaptchaCode.ToLower() != item.captcha.ToLower())
                {
                    ModelState.AddModelError("captcha", "captcha_invalid");
                }

            }
            return ModelState.IsValid;
        }
        private bool checkModelStateCreate(sys_dat_hang_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_dat_hang_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_dat_hang_model item)
        {
            if (string.IsNullOrEmpty(item.db.full_name))
            {
                ModelState.AddModelError("db.full_name", "required");
            }

            if (string.IsNullOrEmpty(item.db.phone))
            {
                ModelState.AddModelError("db.phone", "required");
            }
            else
            {
                if (item.db.phone.Length > 10)
                {
                    ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                }
                else
                {
                    var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
            |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
            |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
            |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

                    //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
                    var checkSDT = rgSoDienThoai.IsMatch(item.db.phone);
                    if (checkSDT == false)
                    {
                        ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                    }

                }

            }
            if (string.IsNullOrEmpty(item.db.email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            else
            {

                var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                var checkEmail = rgEmail.IsMatch(item.db.email);
                if (checkEmail == false)
                {
                    ModelState.AddModelError("db.email", "system.emailKhongHopLe");
                }

            }

            if ((item.db.tinh_thanh ?? 0) == 0)
            {
                ModelState.AddModelError("db.tinh_thanh_user", "required");
            }


            if ((item.db.quan_huyen ?? 0) == 0)
            {
                ModelState.AddModelError("db.quan_huyen_user", "required");
            }
            if (string.IsNullOrEmpty(item.db.dia_chi))
            {
                ModelState.AddModelError("db.dia_chi_user", "required");
            }
            if (!string.IsNullOrEmpty(item.db.ten_cong_ty))
            {
                if ((item.db.tinh_thanh_cong_ty ?? 0) == 0)
                {
                    ModelState.AddModelError("db.tinh_thanh_cong_ty", "required");
                }

                if ((item.db.quan_huyen_cong_ty ?? 0) == 0)
                {
                    ModelState.AddModelError("db.quan_huyen_cong_ty", "required");
                }
                if (string.IsNullOrEmpty(item.db.dia_chi_cong_ty))
                {
                    ModelState.AddModelError("db.dia_chi_cong_ty", "required");
                }
                if (string.IsNullOrEmpty(item.db.ma_so_thue))
                {
                    ModelState.AddModelError("db.ma_so_thue", "required");
                }
            }

            return ModelState.IsValid;
        }

    }
}
