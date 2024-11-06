using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using worldsoft.common.BaseClass;
using worldsoft.common.common;
using worldsoft.common.Services;
using worldsoft.system.data.DataAccess;
using worldsoft.system.data.Models;
using worldsoft.DataBase.Provider;

namespace worldsoft.system.web.Controller
{
    public partial class sys_approvalController : BaseAuthenticationController
    {
        public sys_approval_repo repo;

        public sys_approvalController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_approval_repo(context);
        }
        public async Task<IActionResult> getListItem([FromBody] JObject json)
        {
            var model = repo.FindAllItem(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_approval_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            var userid = getUserId();
            if (model.db.id == "0")
            {
                model.db.create_by = userid;
                model.db.create_date = DateTime.Now;
                model.db.start_by = getUserId();
                model.db.start_date = DateTime.Now;
                model.db.id = Guid.NewGuid().ToString();
                model.db.last_date_action = DateTime.Now;
                model.db.last_user_action = userid;
                model.db.from_user = userid;
                model.db.step_num = 1;
                model.db.status_action = 1;

                model.db.to_user = repo._context.sys_approval_config_details
                    .Where(d => d.id_approval_config == model.db.id_sys_approval_config)
                    .Where(d => d.step_num == 1)
                    .Select(d => d.user_id).FirstOrDefault();
                model.db.status_finish = 2;
                await repo.insert(model);
            }
            else
            {
                model.db.last_user_action = userid;
                model.db.from_user = userid;
                model.db.status_action = 1;
                model.db.start_by = userid;
                await repo.approval(model);
            }

            var result = repo._context.Fn_get_sys_approval(model.db.id).FirstOrDefault();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> approval([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_approval_model>(json.ToString());
            var check = checkModelStateApproval(model);
            if (!check)
            {
                return generateError();
            }
            var userid = getUserId();
            model.db.last_user_action = userid;
            model.db.from_user = userid;
            await repo.approval(model);
            var result = repo._context.Fn_get_sys_approval(model.db.id).FirstOrDefault();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> cancel([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_approval_model>(json.ToString());
            var check = checkModelStateCancel(model);
            if (!check)
            {
                return generateError();
            }
            var userid = getUserId();
            model.db.last_user_action = userid;
            model.db.from_user = userid;
            await repo.cancel(model);
            var result = repo._context.Fn_get_sys_approval(model.db.id).FirstOrDefault();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> close([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_approval_model>(json.ToString());
            var check = checkModelStateClose(model);
            if (!check)
            {
                return generateError();
            }
            var userid = getUserId();
            model.db.last_user_action = userid;
            model.db.from_user = userid;
            await repo.close(model);
            var result = repo._context.Fn_get_sys_approval(model.db.id).FirstOrDefault();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> open([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_approval_model>(json.ToString());
            var check = checkModelStateOpen(model);
            if (!check)
            {
                return generateError();
            }
            var userid = getUserId();
            model.db.last_user_action = userid;
            model.db.from_user = userid;
            await repo.open(model);
            var result = repo._context.Fn_get_sys_approval(model.db.id).FirstOrDefault();
            return Json(result);
        }

        public async Task<IActionResult> getElementById([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var model = await repo.getElementById(id);
            return Json(model);
        }

        public IActionResult getListUser()
        {
            var result = repo._context.users.
                 Select(d => new
                 {
                     id = d.Id,
                     name = d.FirstName + " " + d.LastName
                 }).ToList();
            return Json(result);
        }
       

    }
}
