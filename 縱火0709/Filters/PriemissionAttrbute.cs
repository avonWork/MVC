using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using 縱火0709.Models;

namespace 縱火0709.Filters
{
    public class PriemissionAttrbute : ActionFilterAttribute
    {
        private Model1 db = new Model1();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
        
            if (!HttpContext.Current.User.Identity.IsAuthenticated) //沒有通過驗證
            {
                filterContext.Controller.ViewBag.menu = "";
                filterContext.Result = new RedirectResult("~/Home/Login");
                return;
            }

            //大頭照個人資料id_Layout
            string id = ((FormsIdentity)HttpContext.Current.User.Identity).Name;
            int userid = int.Parse(id);
            var user = db.Users.FirstOrDefault(x => x.Id == userid);
            filterContext.Controller.ViewBag.ID = user.Id;
            filterContext.Controller.ViewBag.Name = user.UserName;
            filterContext.Controller.ViewBag.Img = user.UserImg;
            //權限表單生成_Layout
            StringBuilder sb = new StringBuilder();
            var premission = db.Premissions.ToList();
            sb.Append(GetPrimission(premission.Where(x => x.Pid == null).ToList()));
            filterContext.Controller.ViewBag.menu = sb.ToString();
        }

        private string GetPrimission(ICollection<Premission> list)
        {
            StringBuilder sb = new StringBuilder();

            //自訂使用者(不用驗證票) 成果好了再拿掉
            //var user = db.Users.FirstOrDefault(x => x.UserName == "avon");

            //驗證票專用
            string data = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;
            User user = JsonConvert.DeserializeObject<User>(data);

            foreach (Premission premission in list)
            {
                if (user.Authority.IndexOf(premission.PValue) > -1) //有沒有一樣
                {
                    #region avon

                    if (premission.Pid == null) //爸爸
                    {
                        if (premission.PremissionsSon.Count > 0) //總數大於0 有資料
                        {
                            sb.Append($"<li class='active treeview'>");
                            sb.Append($"<a class='waves-effect waves-dark' href='#!'>");
                            sb.Append($"<i class='icon-chart'></i>");
                            sb.Append($"<span>{premission.Name}</span>");
                            sb.Append($"<span class='label label-success menu-caption'>New</span>");
                            sb.Append($"<i class='icon-arrow-down'></i></a>");
                            sb.Append($"</a>");
                            sb.Append($"<ul class='treeview-menu'>");
                            sb.Append(GetPrimission(premission.PremissionsSon));
                            sb.Append($"</ul>");
                            sb.Append($"</li>");
                        }
                        else //沒有資料 只有爸爸
                        {
                            sb.Append($"<li class='active treeview'>");
                            sb.Append($"<a class='waves-effect waves-dark' href='{premission.Url}'>");
                            sb.Append($"<i class='icon-chart'></i>");
                            sb.Append($"<span>{premission.Name}</span>");
                            sb.Append($"</a>");
                            sb.Append($"</i>");
                        }
                    }
                    else //有爸爸的子選單
                    {
                        sb.Append($"<li class='treeview'>");
                        sb.Append($"<a href = '{premission.Url}' >");
                        sb.Append($"<span>{premission.Name}</span>");
                        sb.Append($"</a>");
                    }

                    #endregion avon

                }
            }
            return sb.ToString();
        }
    }
}