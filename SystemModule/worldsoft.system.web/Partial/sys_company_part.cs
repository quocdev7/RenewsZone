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
    partial class sys_companyController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_company",
            icon = "badge",
            module = "system",
            id = "sys_company",
            url = "/sys_company_index",
            title = "sys_company",
            translate = "NAV.sys_company",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_company;getListUse",
                "sys_company;getListCompanyInfo",
                "sys_company;getElementById",
                    "sys_company;getCompanyInfo"

            },
            list_controller_action_publicNonLogin = new List<string>(){
             
                "sys_company;getListCompanyInfo",
                    "sys_company;getElementById",
                      "sys_company;getCompanyInfo"

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_company;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_company;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_company;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_company;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_company;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_company;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_company;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_company;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_company_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_company_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_company_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (string.IsNullOrEmpty(item.db.phone_contact))
            {
                ModelState.AddModelError("db.phone_contact", "required");
            }
            if (string.IsNullOrEmpty(item.db.address))
            {
                ModelState.AddModelError("db.address", "required");
            }
            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }
            if (string.IsNullOrEmpty(item.db.id_field))
            {
                ModelState.AddModelError("db.id_field", "required");
            }

            return ModelState.IsValid;
        }

    }
}
