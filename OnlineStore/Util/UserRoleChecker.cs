using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Util
{
    public static class UserRoleChecker
    {
        public static bool IsUserAdmin()
        {
            return HttpContext.Current.User.IsInRole("ORA_DBA");
        }
    }
}