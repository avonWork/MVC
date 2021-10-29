using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 縱火0709.Filters;
using 縱火0709.Models;

namespace 縱火0709.Areas.Admin.Controllers
{
    [PriemissionAttrbute]
    [MyAuthorize(Roles = "A01,A02,B01,B02,C01,C02,D01,E01")]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        [NoCache] // 將使「上一頁」無效 --- 掛上Attribute即可
        public ActionResult Index()
        {
            return View();
        }
    }
}