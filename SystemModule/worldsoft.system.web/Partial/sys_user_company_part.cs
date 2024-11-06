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
    partial class sys_user_companyController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_user_company",
            icon = "support",
            module = "system",
            id = "sys_user_company",
            url = "/sys_user_company_index",
            title = "sys_user_company",
            translate = "NAV.sys_user_company",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_user_company;getListUse",
                  "sys_user_company;add_nhan_vien",
                
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_user_company;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_user_company;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_user_company;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_user_company;edit",
                           "sys_user_company;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_user_company;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_user_company;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_user_company;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_user_company;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_user_company_model item, string user_id)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item, user_id);
        }

        private bool checkModelStateEdit(sys_user_company_model item, string user_id)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item, user_id);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_user_company_model item, string user_id)
        {

            var check_nhom = repo._context.sys_user_companys.Where(q => q.user_id == user_id && q.company_id== item.db.company_id).Count();
            if (check_nhom > 0)
            {
                ModelState.AddModelError("user_company", "existedGroup");
            }



            return ModelState.IsValid;
        }

     

    }
}
