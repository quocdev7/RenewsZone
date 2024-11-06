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
using worldsoft.common.Helpers;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Extensions.Options;
using System.Threading;
using worldsoft.DataBase.System;
using worldsoft.common.Common;

namespace worldsoft.system.web.Controller
{
    public partial class sys_logController : BaseAuthenticationController
    {
        public sys_log_repo repo;
        private readonly AppSettings _appSettings;
        public sys_logController(IUserService userService, worldsoftDefautContext context, IOptions<AppSettings> appSettings) : base(userService)
        {
            repo = new sys_log_repo(context);
            _appSettings = appSettings.Value;
        }

      }
}
