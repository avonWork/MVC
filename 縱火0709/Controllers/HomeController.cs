using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcPaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using 縱火0709.Filters;
using 縱火0709.Models;
using static 縱火0709.Models.Enum;

namespace 縱火0709.Controllers
{
    public class HomeController : Controller
    {
        private Model1 db = new Model1();
        private const int DefaultPageSize = 5;

        public ActionResult Index()
        {
            ViewBag.newList = db.News.Where(x => x.Sticky == StickyType.顯示).OrderByDescending(y => y.Date).Take(4).ToList();
            ViewBag.imgList = db.ImgClasses.OrderBy(x=>x.imgorder).ToList();
            return View();
        }

        /// <summary>
        /// 聯絡人
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult contact(Contact contact, string token)
        {
            ViewBag.swal = false; //驗證成功轉true
            ViewBag.Error = true; //機器人轉false
            bool result;
            if (token == null || token == "")
            {
                ViewBag.token = "token遺失";
            }
            string secretKey = "6LdqKVwaAAAAALusjKkkAXVjM3jdqamBs_oEynk2";
            string apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            string requestUrl = string.Format(apiUrl, secretKey, token);
            WebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    bool isSuccess = jResponse.Value<bool>("success");
                    int score = jResponse.Value<int>("score");

                    result = (isSuccess) ? true : false;

                    if (result)
                    {
                        //機器人分數
                        if (score > 0.9)
                        {
                            if (ModelState.IsValid)
                            {
                                contact.InsertTime = DateTime.Now;
                                db.Contacts.Add(contact);
                                db.SaveChanges();
                                ViewBag.swal = true;
                            }

                            //寄信給聯絡人
                            string subjectTitle = "國際縱火調查人員協會臺灣分會 回覆:";
                            string bodycontent = contact.UserName + " 您好~會有專人盡快為您服務";
                            Utility.SendEmail(contact.Email, subjectTitle, bodycontent);
                            //寄信給管理者
                            string email = ConfigurationSettings.AppSettings["email"];
                            string Title = "管理者你好: 收到來自" + contact.UserName + "的訊息,請盡快回復~";
                            string Content =
                                "<label>姓 名：</label>" + contact.UserName +
                                "<br><label>性 別：</label>" + contact.Sex +
                                "<br><label>聯絡電話：</label>" + contact.Phone +
                                "<br><label>E-mail：</label>" + contact.Email +
                                "<br><label>詢問內容：</label>" + contact.Content;
                            Utility.SendEmail(email, Title, Content);
                        }
                        else
                        {
                            ViewBag.Error = false;
                        }
                    }
                }
            }
            return View();
        }

        public ActionResult test()
        {
            return View();
        }

        /// <summary>
        /// 前台會員登入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult member_login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult member_login(LoginViewModel loginViewModel)
        {
            ModelState["Email"].Errors.Clear();

            if (ModelState.IsValid)
            {
                var mem = db.Members.FirstOrDefault(x => x.Account == loginViewModel.Account);
                if (mem == null)
                {
                    TempData["error"] = "登入失敗";
                    return View(loginViewModel);
                }

                if (!mem.CheckAccount)
                {
                    TempData["error"] = "會員帳號尚未開通";
                    return View(loginViewModel);
                }
                //密碼加密
                string pwd = Utility.GenerateHashWithSalt(loginViewModel.Password, mem.PasswordSalt);
                if (mem.Error < 5)
                {
                    //密碼都以加密比對(無法解密)
                    if (mem.Password1 != pwd)
                    {
                        mem.Error = mem.Error += 1;
                        if (db.SaveChanges() > 0)
                        {
                            TempData["error"] = "登入失敗" + mem.Error + "次,剩餘機會" + (5 - mem.Error) + "次";
                        }

                        return View(loginViewModel);
                    }
                    else
                    {
                        string dataMember = JsonConvert.SerializeObject(mem);

                        Utility.SetAuthenTicket(dataMember, mem.UserId, "Font");

                        return RedirectToAction("member_forum", "Members");
                    }
                }
                else
                {
                    TempData["error"] = "登入失敗已到達5次!請聯繫客服解鎖";
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);
        }

        /// <summary>
        /// 前台登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            HttpCookie cookie = Request.Cookies["font"];
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            Session.RemoveAll();
            return RedirectToAction("member_login");
        }

        public ActionResult calendar()
        {
            return View();
        }

        public ActionResult about()
        {
            return View();
        }

        public ActionResult knowledge(int page = 1)
        {
            int currentPageIndex = page < 0 ? 0 : page - 1;
            var fileList = db.FileClasses.OrderBy(x=>x.fileorder).ThenByDescending(y => y.AddTime).ToList();
            return View(PagingExtensions.ToPagedList(fileList, currentPageIndex, DefaultPageSize));
        }

        public ActionResult master()
        {
            return View();
        }

        public ActionResult news_show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        public ActionResult news(int page = 1)
        {
            int currentPageIndex = page < 0 ? 0 : page - 1;
            var newsList = db.News.Where(x => x.Sticky == StickyType.顯示).OrderByDescending(y => y.Date).ToList();
            return View(PagingExtensions.ToPagedList(newsList, currentPageIndex, DefaultPageSize));
        }

        /// <summary>
        /// 後臺管理員登入
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            ModelState["Email"].Errors.Clear();
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(x => x.Account == loginViewModel.Account);
                if (user == null)
                {
                    TempData["error"] = "登入失敗";
                    return View(loginViewModel);
                }

                string pwd = Utility.GenerateHashWithSalt(loginViewModel.Password, user.PasswordSalt);

                if (user.Error < 5)
                {
                    //密碼都以加密比對(無法解密)
                    if (user.UserPassword != pwd)
                    {
                        user.Error = user.Error += 1;
                        if (db.SaveChanges() > 0)
                        {
                            TempData["error"] = "登入失敗" + user.Error + "次,剩餘機會" + (5 - user.Error) + "次";
                        }
                        return View(loginViewModel);
                    }
                    else
                    {
                        string dataUser = JsonConvert.SerializeObject(user);

                        Utility.SetAuthenTicket(dataUser, user.Id.ToString(), FormsAuthentication.FormsCookieName);

                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }
                else
                {
                    TempData["error"] = "登入失敗已到達5次!請聯繫客服解鎖";
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);
        }
    }
}