using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using 縱火0709.Filters;
using 縱火0709.Models;
using PagedList; //分頁套件
using System.Web.Security;
using System.Web.UI;
using 縱火0709.Models.ViewModel;
using static 縱火0709.Models.Enum;

namespace 縱火0709.Areas.Admin.Controllers
{
    [PriemissionAttrbute]
    public class UsersController : Controller
    {
        private Model1 db = new Model1();

        private int pageSize = 5; //分頁

        // GET: /Admin/Users/Index
        [MyAuthorizeAttribute(Roles = "C02")]
        [NoCache] // 將使「上一頁」無效 --- 掛上Attribute即可
        public ActionResult Index(int page = 1)
        {
            //page用來記憶目前是哪一頁
            int currentPage = page < 1 ? 1 : page;

            //var users = db.Users.Include(u => u.UserType).Where(x => x.Sticky == StickyType.顯示).OrderBy(p => p.Id).ToList();
            //管理者
            var user1 = db.Users.Include(u => u.UserType).Where(x => x.Sticky == StickyType.顯示).Select(u => new UserViewModel()
            {
                Id = u.Id,
                TypeId = u.TypeId,
                UserImg = u.UserImg,
                Account = u.Account,
                UserName = u.UserName,
                Authority = u.Authority,
                Error = u.Error,
                UserPhone = u.UserPhone,
                UserEmail = u.UserEmail,
                Sex = u.Sex,
                UserType = u.UserType
            });
            //會員
            var user2 = db.Members.Include(u => u.UserType).Where(x => x.Sticky == StickyType.顯示).Select(u => new UserViewModel()
            {
                Id = u.Id,
                TypeId = u.TypeId,
                UserImg = "~/Images/user/clipart4058783.png",
                Account = u.Account,
                UserName = u.UserName,
                Authority = "無",
                Error = 0,
                UserPhone = "無",
                UserEmail = "無",
                Sex = u.Sex,
                UserType = u.UserType
            });
            //管理者關聯會員呈現列表
            var users = user1.Union(user2).OrderBy(x => x.Id).ToList();
            var PageResult = users.ToPagedList(currentPage, pageSize);

            return View(PageResult);
        }

        // GET: Admin/Users/Details/5
        [MyAuthorizeAttribute(Roles = "C02")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserViewModel UMD = new UserViewModel();
            //關聯中間表
            User user = db.Users
                .Include(x => x.Hobbys)
                .FirstOrDefault(x => x.Id == id);

            var allCourses = user.Hobbys.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = string.Format("{0},", vm.HobbyName),
            }).ToList();

            UMD.Account = user.Account;
            UMD.Id = user.Id;
            ViewBag.TypeId = db.UserTypes.FirstOrDefault(x => x.Id == user.TypeId).TypeName;
            UMD.Authority = user.Authority;
            UMD.UserImg = user.UserImg;
            UMD.UserName = user.UserName;
            UMD.UserPhone = user.UserPhone;
            UMD.UserEmail = user.UserEmail;
            UMD.Sex = user.Sex;
            UMD.Ramark = user.Ramark;
            UMD.HobbyItems = allCourses;
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(UMD);
        }

        // GET: Admin/Users/Create
        [MyAuthorizeAttribute(Roles = "C01")]
        public ActionResult Create()
        {
            //用戶類型
            ViewBag.TypeId = new SelectList(db.UserTypes.OrderBy(x => x.Id).Take(2).ToList(), "Id", "TypeName");
            //權限選單
            PrimissionList();

            //興趣複選框
            UserViewModel m1 = new UserViewModel();
            var item = db.Hobbys.ToList();
            m1.HobbyItems = item.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = vm.HobbyName,
                IsChecked = false
            }).ToList();
            return View(m1);
        }

        /// <summary>
        /// 產生權限樹狀選單
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public string GetPrimission(ICollection<Premission> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var premission in list)
            {
                sb.Append("{ \"id\": \"" + premission.PValue + "\", \"text\": \"" + premission.Name + "\"");
                if (premission.PremissionsSon.Count > 0)
                {
                    sb.Append(", \"children\":[");
                    sb.Append(GetPrimission(premission.PremissionsSon));
                    sb.Append("]");
                }
                sb.Append("},");
                sb.ToString().TrimEnd(',');
            }

            return sb.ToString();
        }

        /// <summary>
        /// 可選權限樹狀選單(新增/編輯)
        /// </summary>
        public void PrimissionList()
        {
            StringBuilder sb = new StringBuilder();
            var userData = db.Premissions.ToList();
            sb.Append("[");
            sb.Append(GetPrimission(userData.Where(x => x.Pid == null).ToList()));
            sb.Append("]");
            ViewBag.tree = sb.ToString();
        }

        /// <summary>
        /// 驗證票登入者Id
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public int GetInsertAdminID() 
        {
            string id = ((FormsIdentity)HttpContext.User.Identity).Name;
            return int.Parse(id);
        }


        // POST: Admin/Users/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [MyAuthorizeAttribute(Roles = "C01")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel UM, HttpPostedFileBase uploadFile)
        {
            User user = new User();

            if (UM.UserPassword == null)
            {
                ModelState.AddModelError("UserPassword", "請輸入密碼,至少3字元");
            }
            if (uploadFile == null)
            {
                ModelState.AddModelError("UserImg", "請選擇照片");
            }

            if (ModelState.IsValid)
            {
                if (uploadFile != null)
                {
                    uploadFile.SaveAs(HttpContext.Server.MapPath("~/Images/user/") + uploadFile.FileName);
                    user.UserImg = "~/Images/user/" + uploadFile.FileName;
                }

                user.Account = UM.Account;
                user.Sticky = StickyType.顯示;
                user.Authority = UM.Authority;
                user.TypeId = UM.TypeId;
                user.UserName = UM.UserName;
                user.UserPhone = UM.UserPhone;
                user.UserEmail = UM.UserEmail;
                user.Sex = UM.Sex;
                user.PasswordSalt = Utility.CreateSalt();
                user.UserPassword = Utility.GenerateHashWithSalt(UM.UserPassword, user.PasswordSalt);
                user.Ramark = UM.Ramark;
                //新增InsertAdminID
                user.InsertAdminID=GetInsertAdminID();
                user.InsertTime = DateTime.Now;

                //興趣複選單
                List<Hobbys> hobbysList = new List<Hobbys>();
                //透過CheckBoxItem集合以IsChecked屬性為true抓出興趣循環加入hobbysList
                foreach (var item in UM.HobbyItems)
                {
                    if (item.IsChecked)
                    {
                        Hobbys h = db.Hobbys.FirstOrDefault(x => x.Id == item.Id);
                        hobbysList.Add(h);
                    }
                }

                //User u = db.Users.FirstOrDefault(x => x.Id == user.Id);
                foreach (Hobbys hobbys in hobbysList)
                {
                    user.Hobbys.Add(hobbys);
                }

                db.Users.Add(user);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // ViewBag.TypeId = new SelectList(db.UserTypes, "Id", "TypeName", user.TypeId);
            ViewBag.TypeId = new SelectList(db.UserTypes.OrderBy(x => x.Id).Take(2).ToList(), "Id", "TypeName");

            var hobbysitem = db.Hobbys.ToList();
            UM.HobbyItems = hobbysitem.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = vm.HobbyName,
                IsChecked = false
            }).ToList();

            //用戶類型
            ViewBag.TypeId = new SelectList(db.UserTypes, "Id", "TypeName");
            //權限選單
            PrimissionList();

            return View(UM);
        }

        // GET: Admin/Users/Edit/1018?page=2
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserViewModel UME = new UserViewModel();
            var allCourses = db.Hobbys.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = vm.HobbyName,
                IsChecked = vm.Users.Any(x => x.Id == id) ? true : false
            }).ToList();

            User user = db.Users.Find(id);
            //Session["imgPath"] = user.UserImg;
            UME.Account = user.Account;
            UME.Authority = user.Authority;
            UME.UserImg = user.UserImg;
            UME.TypeId = user.TypeId;
            UME.UserName = user.UserName;
            UME.UserPhone = user.UserPhone;
            UME.UserEmail = user.UserEmail;
            UME.Sex = user.Sex;
            UME.Ramark = user.Ramark;
            UME.HobbyItems = allCourses;
            //EditAdminID
            user.EditAdminID=GetInsertAdminID();
            user.EditTime = DateTime.Now;

            if (user == null)
            {
                return HttpNotFound();
            }

            //判斷是否是管理員,返回管理者列表是否顯示
            string data = ((FormsIdentity)HttpContext.User.Identity).Ticket.UserData;
            User admin = JsonConvert.DeserializeObject<User>(data);
            if (admin.Authority.Contains("C02"))
            {
                ViewBag.Title = "編輯用戶";
                ViewBag.display = "display:display";
            }
            else
            {
                ViewBag.Title = "個人資料設定";
                ViewBag.display = "display: none";
            }
            //TypeId綁值
            ViewBag.TypeId = new SelectList(db.UserTypes.OrderBy(x => x.Id).Take(2).ToList(), "Id", "TypeName", user.TypeId);
            //權限選單
            PrimissionList();
            return View(UME);
        }

        // POST: Admin/Users/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel UVME, HttpPostedFileBase uploadFile1)
        {
            //拿掉[Remote("CheckAccount", "Users", HttpMethod = "POST")]驗證
            ModelState["Account"].Errors.Clear();

            //關聯中間表
            User user = db.Users
                .Include(x => x.Hobbys)
                .FirstOrDefault(x => x.Id == UVME.Id);

            if (ModelState.IsValid)
            {
                if (uploadFile1 != null)
                {
                    uploadFile1.SaveAs(HttpContext.Server.MapPath("~/Images/user/") + uploadFile1.FileName);
                    user.UserImg = "~/Images/user/" + uploadFile1.FileName;
                    //user.Hobby = string.Join(",", user.HobbyArray);
                }

                user.Authority = UVME.Authority;
                user.TypeId = UVME.TypeId;
                user.UserName = UVME.UserName;
                user.UserPhone = UVME.UserPhone;
                user.UserEmail = UVME.UserEmail;
                user.Sex = UVME.Sex;
                user.Ramark = UVME.Ramark;

                //移除
                foreach (Hobbys oldH in user.Hobbys.ToList())
                {
                    user.Hobbys.Remove(oldH);
                    db.SaveChanges();
                }
                //數據放入集合
                List<Hobbys> hobbysList = new List<Hobbys>();
                foreach (var item in UVME.HobbyItems)
                {
                    if (item.IsChecked)
                    {
                        Hobbys newH = db.Hobbys.FirstOrDefault(x => x.Id == item.Id);
                        hobbysList.Add(newH);
                    }
                }
                //建立
                foreach (Hobbys hobbys in hobbysList)
                {
                    user.Hobbys.Add(hobbys);
                }

                db.SaveChanges();
                //判斷是否是管理員,回管理者列表,否則回登入
                string data = ((FormsIdentity)HttpContext.User.Identity).Ticket.UserData;
                User admin = JsonConvert.DeserializeObject<User>(data);
                if (admin.Authority.Contains("C02"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "編輯成功,請重新登入!";
                    return RedirectToAction("Login", "Home", new { area = "" });
                }
            }

            //編輯ModelState失敗
            //ModelState.錯誤訊息
            var msg = string.Empty;
            foreach (var value in ModelState.Values)
            {
                if (value.Errors.Count > 0)
                {
                    foreach (var error in value.Errors)
                    {
                        msg = msg + error.ErrorMessage;
                    }
                }
            }
            ViewBag.Error = msg;

            ViewBag.TypeId = new SelectList(db.UserTypes, "Id", "TypeName", user.TypeId);
            UVME.UserImg = user.UserImg;
            var allCourses = db.Hobbys.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = vm.HobbyName,
                IsChecked = vm.Users.Any(x => x.Id == UVME.Id) ? true : false
            }).ToList();

            UVME.HobbyItems = allCourses;
            //TypeId綁值
            ViewBag.TypeId = new SelectList(db.UserTypes.OrderBy(x => x.Id).Take(2).ToList(), "Id", "TypeName", user.TypeId);
            //權限選單
            PrimissionList();
            return View(UVME);
        }

        /// <summary>
        /// 帳號驗證
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult CheckAccount(string Account)
        {
            //兩張表上下關聯Users&Members 同一個帳號不可以重複
            var user1 = db.Users.Select(x => new { x.Account, x.Id });
            var user2 = db.Members.Select(x => new { x.Account, x.Id });
            var isAccount = user1.Union(user2).FirstOrDefault(y => y.Account == Account.Trim());
            if (!string.IsNullOrWhiteSpace(Account) && isAccount != null)
            {
                return Json("帳號" + Account + "已有人使用!", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Users/Delete/5
        [MyAuthorizeAttribute(Roles = "C02")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserViewModel UMDD = new UserViewModel();
            //關聯中間表
            User user = db.Users
                .Include(x => x.Hobbys)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }
            var allCourses = user.Hobbys.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = string.Format("{0},", vm.HobbyName),
            }).ToList();

            UMDD.Id = user.Id;
            UMDD.Account = user.Account;
            ViewBag.TypeId = db.UserTypes.FirstOrDefault(x => x.Id == user.TypeId).TypeName;
            UMDD.Authority = user.Authority;
            UMDD.UserImg = user.UserImg;
            UMDD.UserName = user.UserName;
            UMDD.UserPhone = user.UserPhone;
            UMDD.UserEmail = user.UserEmail;
            UMDD.Sex = user.Sex;
            UMDD.Ramark = user.Ramark;
            UMDD.HobbyItems = allCourses;

          
            return View(UMDD);
        }

        /// <summary>
        /// 管理員物理刪除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [MyAuthorizeAttribute(Roles = "C02")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //關聯中間表
            User user = db.Users
                .Include(x => x.Hobbys)
                .FirstOrDefault(x => x.Id == id);

            //db.Users.Remove(user);
            //物理刪除
            user.Sticky = StickyType.刪除_垃圾箱;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 查看前台會員資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorizeAttribute(Roles = "C02")]
        public ActionResult DetailsMember(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //會員類型
            var members = db.UserTypes.OrderBy(x => x.Id).Skip(2).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            items = members.Select(vm => new SelectListItem()
            {
                Value = vm.Id.ToString(),
                Text = vm.TypeName,
                Selected = vm.Members.Any(x => x.Id == id) ? true : false
            }).ToList();
            ViewBag.RadioButtonList = items;

            Member member = db.Members.Include("Experience").FirstOrDefault(x => x.Id == id);
            if (member == null)
            {
                return HttpNotFound();
            }
            if (member.Experience.Count == 1)
            {
                member.Experience.Add(new Models.Experience());
            }
            return View(member);
        }

        /// <summary>
        /// 物理刪除前台會員資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Admin/DeleteMember/Delete/5
        [HttpGet]
        [MyAuthorizeAttribute(Roles = "C02")]
        public ActionResult DeleteMember(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Members.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.Sticky = StickyType.刪除_垃圾箱;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 後台登出
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Logout()
        {
            //登出表單驗證票卷
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            return RedirectToAction("Login", "Home", new { area = "" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}