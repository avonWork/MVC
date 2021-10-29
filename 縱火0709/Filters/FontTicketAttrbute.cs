using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 縱火0709.Models;

namespace 縱火0709.Filters
{
    public class FontTicketAttrbute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var authCookie = filterContext.RequestContext.HttpContext.Request.Cookies["font"];
   
            //沒有通過驗證
            if (authCookie == null || authCookie.Value == "")
            {
                filterContext.Result = new RedirectResult("~/Home/member_login");
                return;
            }
        }
    }
}