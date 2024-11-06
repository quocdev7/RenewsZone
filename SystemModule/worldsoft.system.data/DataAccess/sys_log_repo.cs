using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.Provider;

namespace worldsoft.system.data.DataAccess
{
    public class sys_log_repo
    {
        public worldsoftDefautContext _context;

        public sys_log_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

    }
}
