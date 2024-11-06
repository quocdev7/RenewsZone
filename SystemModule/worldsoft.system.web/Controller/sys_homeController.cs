using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using worldsoft.common.BaseClass;
using worldsoft.common.Services;
using worldsoft.DataBase.Provider;
using worldsoft.common.Models;
using Newtonsoft.Json;
namespace worldsoft.system.web.Controller
{
    public partial class sys_homeController : BaseAuthenticationController
    {
        private worldsoftDefautContext context;

        public sys_homeController(IUserService userService, worldsoftDefautContext _context) : base(userService)
        {
            context = _context;
        }
        public async Task<ActionResult> checkLogin()
        {
            return generateSuscess();

        }

        public async Task<ActionResult> getListRoleFull()
        {
            var UserId = getUserId();
            var model = ListControlller.list;
            model = model.Where(d => (d.type_user ?? 1) == 1).ToList();
            var listdynamic = new List<dynamic>();
            for (int i = 0; i < model.Count; i++)
            {
                var listRole = model[i].list_role
                    .Select(d => new
                    {
                        role = d,
                        controller_name = model[i].translate
                    });
                listdynamic.AddRange(listRole);

            }
            return Json(listdynamic);

        }


       
        public async Task<ActionResult> getModule()
        {
            var UserId = getUserId();
            var typeuser = context.users.Where(t => t.Id == UserId).Select(d => d.type).FirstOrDefault();
            var model = JsonConvert.DeserializeObject<List<ControllerAppModel>>(JsonConvert.SerializeObject(ListControlller.list)) ;
            var groupID = context.sys_group_user_details.Where(d => d.user_id == UserId).Select(d => d.id_group_user).ToList();
            model = model.Where(d => (d.type_user ?? 1) == typeuser).ToList();
           
            var modelfilerRole = model;
            if (typeuser == 1)
            {
                model.ForEach((menu) =>
                {
                    menu.list_role = context.sys_group_user_roles.Where(d => groupID.Contains(d.id_group_user)).ToList().Where(d => d.id_controller_role.Split(";")[0] == menu.controller).Select(d => new ControllerRoleModel
                    {
                        id = d.id_controller_role,
                        name = d.role_name,
                        list_controller_action = new List<string>()
                    {
                          d.id_controller_role,
                    }
                    }).ToList();
                });
                  var controller_names = context.sys_group_user_roles.Where(d => groupID.Contains(d.id_group_user))
                .Select(d => d.controller_name).Distinct().ToList();
                    modelfilerRole= modelfilerRole.Where(d => controller_names.Contains(d.translate) || d.is_show_all_user == true)
                .ToList();
            }
            else
            {
               
            }
            var listdynamic = new List<dynamic>();
            for (int i = 0; i < modelfilerRole.Count; i++)
            {
                var count = 0;

                var countreturn = 0;

                var item = new
                {
                    menu = modelfilerRole[i],
                    badge_approval = count,
                    badge_return = countreturn,
                };
                listdynamic.Add(item);

            }
            if(getUser().type==2)
            {
                ControllerAppModel controller = new ControllerAppModel();
                var item1 = new
                {
                    menu = controller,
                    badge_approval = 0,
                    badge_return = 0,

                };
                item1.menu.is_show_all_user = true;
                item1.menu.controller = "maintain_san_pham";
                item1.menu.url = "maintain_san_pham_index";
                item1.menu.id = "maintain_san_pham";
                item1.menu.title = "maintain_san_pham";
                item1.menu.translate = "NAV.maintain_san_pham";
                item1.menu.module = "maintain";
                item1.menu.icon = "history";
                listdynamic.Add(item1);
            }    
           

            //var item1 = new ControllerAppModel
            //{
            //    controller
            //};
            //var menu = new
            //{
            //    menu =,
            //    badge_approval = 0,
            //    badge_return = 0,


            //}
            return Json(listdynamic);

        }

    }
}
