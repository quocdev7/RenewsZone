using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_approval_userController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "person_outline",
            icon = "menu",
            module = "system",
            id = "sys_approval_user",
            url = "/sys_approval_user_index",
            title = "sys_approval_user",
            translate = "NAV.sys_approval_user",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>() {
                "sys_approval_user;forgot_pass",
                "sys_approval_user;checkResetPass",
                "sys_approval_user;changePasswordNonLogin",
                 "sys_approval_user;register",
                   "sys_approval_user;approval",
                   "sys_approval_user;cancel"
            },
            list_controller_action_public = new List<string>() {
                "sys_approval_user;getListUse",
                "sys_approval_user;forgot_pass",
                 "sys_approval_user;changePassword",
                "sys_approval_user;getName",
                "sys_approval_user;changePasswordByAdmin",
                "sys_approval_user;changePasswordCustomer",
                "sys_approval_user;ImportFromExcel",
                "sys_approval_user;Download",
                "sys_approval_user;getInfomationUserLogin",
                 "sys_approval_user;getUserInfo",
                         "sys_approval_user;getUserByCompany",
                        


            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_approval_user;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_approval_user;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_approval_user;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_approval_user;edit",
                          "sys_approval_user;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_approval_user;approval",
                    name="approval",
                    list_controller_action = new List<string>()
                    {
                          "sys_approval_user;approval",
                    }
                },

                new ControllerRoleModel()
                {
                    id="sys_approval_user;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_approval_user;delete",
                    }
                },

                  new ControllerRoleModel()
                {
                    id="sys_approval_user;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_approval_user;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_user_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_user_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_user_model item)
        {
            if (string.IsNullOrEmpty(item.db.Username))
            {
                ModelState.AddModelError("db.Username", "required");
            }
            else
            {
                var search = repo.FindAll().Where(d => d.db.Username == item.db.Username && d.db.Id != item.db.Id).Count();
                if (search > 0)
                {
                    ModelState.AddModelError("db.Username", "existed");
                }
            }

                if (string.IsNullOrEmpty(item.db.LastName))
            {
                ModelState.AddModelError("db.LastName", "required");
            }
            if (string.IsNullOrEmpty(item.db.FirstName))
            {
                ModelState.AddModelError("db.FirstName", "required");
            }
           
            if (string.IsNullOrEmpty(item.password) && action == ActionEnumForm.create)
            {
                ModelState.AddModelError("password", "required");
            }
            if (item.db.school_year==0)
            {
                ModelState.AddModelError("school_year", "required");
            }
            if (string.IsNullOrEmpty(item.db.id_khoa))
            {
                ModelState.AddModelError("id_khoa", "required");
            }

            if (string.IsNullOrEmpty(item.db.email))
            {
                ModelState.AddModelError("email", "required");
            }

            if (!string.IsNullOrEmpty(item.db.phone))
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
                else
                {
                    var dienthoai = item.db.phone;  //CMAESCrypto.EncryptText(item.db.dienthoai);

                    var checkdienthoai = repo.FindAll().Where(d => d.db.phone == dienthoai && d.db.Id != item.db.Id).Count();
                    if (checkdienthoai > 0)
                    {
                        ModelState.AddModelError("db.phone", "existed");
                    }
                }
            }



            if (!string.IsNullOrEmpty(item.db.email))
            {

                var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                var checkEmail = rgEmail.IsMatch(item.db.email);
                if (checkEmail == false)
                {
                    ModelState.AddModelError("db.email", "system.emailKhongHopLe");
                }
                else
                {
                    var email = item.db.email;// CMAESCrypto.EncryptText(item.db.email);
                    var checkemail = repo.FindAll().Where(d => d.db.email == email && d.db.Id != item.db.Id).Count();
                    if (checkemail > 0)
                    {
                        ModelState.AddModelError("db.email", "existed");
                    }
                }


            }

            return ModelState.IsValid;
        }

    }
}
