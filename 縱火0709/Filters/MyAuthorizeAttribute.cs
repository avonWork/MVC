using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using 縱火0709.Models;

namespace 縱火0709.Filters
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            if (!httpContext.User.Identity.IsAuthenticated)//判斷是否已驗證
                return false;

            string data = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;
            User user = JsonConvert.DeserializeObject<User>(data);

            String[] roles = Roles.Split(',');//取得輸入role清單

            if (roles.Length != 0)
            {
                //使用者權限
                string[] auth = user.Authority.Split(new char[] { ',' });

                bool Isright = false;//角色是否正確

                foreach (String role in roles)//循環比對角色資料
                {
                    if (auth.Contains(role))
                    {
                        return true;
                    }
                    else
                    {
                        Isright = false;
                    }
                }
                return Isright;
            }
            return true;
        }

        // 驗證失敗要做什麼
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Controller.TempData["error"] = "沒有權限!無法進入~";
            filterContext.Result = new RedirectResult("~/Home/Login");
        }
    }
}