using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using worldsoft.common.BaseClass;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_approvalController
    {
        private  bool checkModelStateCreate(sys_approval_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }
        private bool checkModelStateCancel(sys_approval_model item)
        {
            return ModelState.IsValid;
        }
        private bool checkModelStateClose(sys_approval_model item)
        {
            return ModelState.IsValid;
        }
        private bool checkModelStateOpen(sys_approval_model item)
        {
            return ModelState.IsValid;
        }
        private bool checkModelStateApproval(sys_approval_model item)
        {
            return ModelState.IsValid;
        }
        private bool checkModelStateEdit(sys_approval_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_approval_model item)
        {
            if (string.IsNullOrEmpty(item.db.id_sys_approval_config))
            {
                ModelState.AddModelError("db.id_sys_approval_config", "required");
            }
            return ModelState.IsValid;
        }

    }
}
