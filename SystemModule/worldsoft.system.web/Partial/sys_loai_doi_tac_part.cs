﻿using System;
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
    partial class sys_loai_doi_tacController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_loai_doi_tac",
            icon = "reduce_capacity",
            module = "system",
            id = "sys_loai_doi_tac",
            url = "/sys_loai_doi_tac_index",
            title = "sys_loai_doi_tac",
            translate = "NAV.sys_loai_doi_tac",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_loai_doi_tac;getListUse",
                 
            },
            list_controller_action_publicNonLogin = new List<string>(){
                "sys_loai_doi_tac;getListUse",
                "sys_loai_doi_tac;load_ngon_ngu",
                "sys_loai_doi_tac;edit_language",

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_loai_doi_tac;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_loai_doi_tac;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_loai_doi_tac;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_loai_doi_tac;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_loai_doi_tac;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_loai_doi_tac;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_loai_doi_tac;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_loai_doi_tac;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_loai_doi_tac_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }
        private bool checkModelStateEditLanguage(sys_loai_doi_tac_language_model item)
        {
            return checkModelStateCreateEditLanguage(ActionEnumForm.edit, item);
        }
        private bool checkModelStateEdit(sys_loai_doi_tac_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_loai_doi_tac_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }


            return ModelState.IsValid;
        }
        private bool checkModelStateCreateEditLanguage(ActionEnumForm action, sys_loai_doi_tac_language_model item)
        {
            if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }
            return ModelState.IsValid;
        }

    }
}