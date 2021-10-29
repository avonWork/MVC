using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using MvcPaging;
using 縱火0709.Filters;
using 縱火0709.Migrations;
using 縱火0709.Models;
using static 縱火0709.Models.Enum;
using System.Web;

namespace 縱火0709.Controllers
{
    public class MembersController : Controller
    {
        private Model1 db = new Model1();
        private const int DefaultPageSize = 5;

        /// <summary>
        /// 討論區列表(前台會員首頁)
        /// </summary>
        /// <returns></returns>
        [FontTicketAttrbute]
        [ActionName("member_forum")]
        public ActionResult Index(int page = 1)
        {
            int currentPageIndex = page < 0 ? 0 : page - 1;
            var articleList = db.Article.Include("Replys").OrderByDescending(x => x.AddTime).ToList();

            #region 取出Cookie身分

            //cookie資料
            var data = CookieData();
            //轉物件
            Member Myperson = JsonConvert.DeserializeObject<Member>(data);

            #endregion 取出Cookie身分

            ViewBag.memberId = Myperson.UserId;

            Member member = db.Members.FirstOrDefault(x => x.UserId == Myperson.UserId);
            //登入成功 錯誤歸0
            member.Error = 0;
            db.SaveChanges();
            return View(PagingExtensions.ToPagedList(articleList, currentPageIndex, DefaultPageSize));
        }

        private string CookieData()
        {
            //自定義cookie取值
            HttpCookie _cookie = HttpContext.Request.Cookies["Font"];
            //解碼
            FormsAuthenticationTicket _ticket = FormsAuthentication.Decrypt(_cookie.Value);
            //資料
            string data = _ticket.UserData;
            return data;
        }

        /// <summary>
        /// 呈現討論區文章
        /// </summary>
        /// <returns></returns>
        ///    [HttpGet]
        [HttpGet]
        [FontTicketAttrbute]
        [ActionName("member_showArticle")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            #region 取出Cookie身分

            //cookie資料
            var data = CookieData();
            //轉物件
            Member Myperson = JsonConvert.DeserializeObject<Member>(data);

            #endregion 取出Cookie身分

            ViewBag.memberId = Myperson.UserId;
            Article article = db.Article.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        /// <summary>
        /// 新增討論區文章
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [FontTicketAttrbute]
        [ActionName("member_createArticle")]
        public ActionResult Create()
        {
            #region 取出Cookie身分

            //cookie資料
            var data = CookieData();
            //轉物件
            Member Myperson = JsonConvert.DeserializeObject<Member>(data);

            #endregion 取出Cookie身分

            ViewBag.memberId = Myperson.UserId;

            return View();
        }

        // POST: Members/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)] //设置取消验
        [ActionName("member_createArticle")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                #region 取出Cookie身分

                //cookie資料
                var data = CookieData();
                //轉物件
                Member Myperson = JsonConvert.DeserializeObject<Member>(data);

                #endregion 取出Cookie身分

                article.Article_name = Myperson.UserName;
                article.AddUserId = Myperson.Id;

                article.AddTime = DateTime.Now;
                db.Article.Add(article);
                db.SaveChanges();
                return RedirectToAction("member_forum");
            }

            return View(article);
        }

        /// <summary>
        /// 回覆討論區文章
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [FontTicketAttrbute]
        [ActionName("member_ReArticle")]
        public ActionResult Edit(int? ArtID)
        {
            #region 取出Cookie身分

            //cookie資料
            var data = CookieData();
            //轉物件
            Member Myperson = JsonConvert.DeserializeObject<Member>(data);

            #endregion 取出Cookie身分

            ViewBag.memberId = Myperson.UserId;

            //關聯兩張表 作法一
            //Reply table = db.Reply.Include("Article").FirstOrDefault(x => x.ArtID == id);
            //Reply reply = new Reply();
            //reply.Article = table.Article;

            //作法二
            Reply reply2 = new Reply();
            ViewBag.Article_title = db.Article.FirstOrDefault(x => x.id == ArtID).Article_Title;
            Session["ArtID"] = ArtID;
            return View(reply2);
        }

        // POST: Members/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)] //设置取消验
        [ActionName("member_ReArticle")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reply dataReply)
        {
            #region 取出Cookie身分

            //cookie資料
            var data = CookieData();
            //轉物件
            Member Myperson = JsonConvert.DeserializeObject<Member>(data);

            #endregion 取出Cookie身分

            dataReply.AddUserId = Myperson.Id;
            dataReply.AddTime = DateTime.Now;
            dataReply.Reply_name = Myperson.UserName;
            db.Reply.Add(dataReply);
            db.SaveChanges();
            return RedirectToAction("member_showArticle",new { id = Session["ArtID"]});
        }

        //會員忘記密碼
        //寄信-新密碼
        [HttpGet]
        [ActionName("member_ForgotPassword")]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AcceptVerbs("Post", "Put")]
        [ActionName("member_ForgotPassword")]
        public ActionResult ForgotPassword(LoginViewModel login)
        {
            var result = db.Members.FirstOrDefault(x => x.Account == login.Account);
            if (result == null)
            {
                TempData["error"] = "尚未註冊";
                return View(login);
            }
            if (!result.CheckAccount)
            {
                TempData["error"] = "會員帳號尚未開通";
                return View(login);
            }
            if (login.Email != result.Email)
            {
                TempData["error"] = "資料不符,請確認!";
                return View(login);
            }
            //暫時密碼
            result.PasswordSalt = Utility.CreateSalt();
            string newPassword = Guid.NewGuid().ToString();
            result.Password1 = Utility.GenerateHashWithSalt(newPassword, result.PasswordSalt);
            result.Password2 = result.Password1;
            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}
            //密碼更新成功
            if (db.SaveChanges() > 0)
            {
                //寄信給申請人
                string subjectTitle = "國際縱火調查人員協會臺灣分會_會員成功申請新密碼通知:";
                string bodycontent = $"<p>新密碼:" + newPassword + "<p><p>請回到登入!重新登入新密碼~<p><br/><a href ='https://localhost:44362/Home/member_login'>登入</a>";
                Utility.SendEmail(login.Email, subjectTitle, bodycontent);
                TempData["message"] = "已成功寄出,請至Email收信~";
            }
            return RedirectToAction("member_login", "Home");
        }

        /// <summary>
        ///     會員註冊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("member_reg")]
        public ActionResult CreateMember()
        {
            var member = new Member();
            var members = db.UserTypes.OrderBy(x => x.Id).Skip(2).ToList();
            var items = new List<SelectListItem>();
            foreach (var c in members)
                items.Add(new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.TypeName,
                    Selected = c.Id % 2 == 0
                });
            ViewBag.RadioButtonList = items;

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("member_reg")]
        public ActionResult CreateMember(Member member, Models.Experience Exp, Models.Experience Exp2)
        {
            //圖片驗證碼機制
            var code = Request.Form["code"];
            if (code != Session["code"].ToString())
            {
              ViewBag.Result = "驗證錯誤";

                var members = db.UserTypes.OrderBy(x => x.Id).Skip(2).ToList();
                var items = new List<SelectListItem>();
                foreach (var c in members)
                    items.Add(new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.TypeName,
                        Selected = c.Id % 2 == 0
                    });
                ViewBag.RadioButtonList = items;
                ModelState.AddModelError("Password1", "請重新填寫密碼");
                ModelState.AddModelError("TypeId", "請選擇申請類別");
                return View(member);
            }

            if (ModelState.IsValid)
            {
                member.PasswordSalt = Utility.CreateSalt();
                member.Password1 = Utility.GenerateHashWithSalt(member.Password1, member.PasswordSalt);
                member.Password2 = member.Password1;
                member.Sticky = StickyType.顯示;
                member.InsertTime = DateTime.Now;
                member.UserId = Guid.NewGuid().ToString();
                db.Members.Add(member);

                #region 抓取錯誤提示

                //try
                //{
                //    db.SaveChanges();
                //}
                //catch (Exception e)
                //{
                //    string aa = e.Message;
                //}

                #endregion 抓取錯誤提示

                if (db.SaveChanges() > 0)//儲存成功寄信
                {
                    //寄信給註冊人
                    string subjectTitle = "國際縱火調查人員協會臺灣分會_會員帳號開通通知:";
                    string bodycontent = $"<p>謝謝你的註冊, 請點擊以下連結:<p><br/><a href ='https://localhost:44362/Members/PutAccount?userid={member.UserId}'>開通你的帳號</a>";
                    Utility.SendEmail(member.Email, subjectTitle, bodycontent);
                    TempData["message"] = "已成功寄出,請至Email開通您的帳號~";
                }
                Exp.MemUserId = member.UserId;
                db.Experience.Add(Exp);

                var IsSuccess = true; //Exp2 全部不為空=true
                Exp2.MemUserId = member.UserId; //屬性先給值 不然屬性為空跑陣列會異常

                //判斷全不為空語法(過去工作經驗第二大欄 所有屬性都要填寫否則不儲存)
                //一旦為空就不儲存
                var properties = Exp2.GetType().GetProperties();

                #region 移除陣列不想要元素

                var indexToRemove = 8; //移除陣列第幾個
                properties = properties.Where((source, index) => index != indexToRemove).ToArray();

                #endregion 移除陣列不想要元素

                foreach (var item in properties)
                    if (item.GetValue(Exp2, null) == null)
                    {
                        IsSuccess = false;
                        break;
                    }

                if (IsSuccess) db.Experience.Add(Exp2);
                db.SaveChanges();

                return RedirectToAction("member_login", "Home");
            }

            return View(member);
        }

        /// <summary>
        /// 修改會員資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [FontTicketAttrbute]
        [ActionName("member_edit")]
        public ActionResult EditMember(string userid)
        {
            if (userid == null)
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
                Selected = vm.Members.Any(x => x.UserId == userid) ? true : false
            }).ToList();
            ViewBag.RadioButtonList = items;

            Member member = db.Members.Include("Experience").FirstOrDefault(x => x.UserId == userid);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("member_edit")]
        public ActionResult EditMember(Member member)
        {
            //會員資料庫資料
            var aa = db.Members.Include("Experience").FirstOrDefault(x => x.UserId == member.UserId);
            ModelState["Account"].Errors.Clear();
            //驗證成功
            if (ModelState.IsValid)
            {
                //資料庫_集合資料
                foreach (var item in aa.Experience.ToList())
                {
                    //資料刪除(資料庫數據)
                    db.Experience.Remove(item);
                }

                //表單集合資料
                foreach (Models.Experience experience in member.Experience)
                {
                    //資料加入(表單集合數據)
                    aa.Experience.Add(experience);
                }
                //資料更改
                aa.UserName = member.UserName;
                aa.Address = member.Address;
                aa.Sex = member.Sex;
                aa.Birthday = member.Birthday;
                aa.TotalRelevantYears = member.TotalRelevantYears;
                aa.TotalRelevantMonths = member.TotalRelevantMonths;
                aa.TypeId = member.TypeId;
                aa.InternationalMembership = member.InternationalMembership;
                aa.CurrentEmployer = member.CurrentEmployer;
                aa.Email = member.Email;
                aa.HighestEducation = member.HighestEducation;
                aa.JobTitle = member.JobTitle;

                #region 取出Cookie身分

                //cookie資料
                var data = CookieData();

                //轉物件
                Member Myperson = JsonConvert.DeserializeObject<Member>(data);

                #endregion 取出Cookie身分

                member.EditAdminID = Myperson.Id;
                aa.EditTime = DateTime.Now;

                //密碼更改情況下(回到登入頁)
                if (aa.Password1 != member.Password1)
                {
                    //密碼變更
                    aa.PasswordSalt = Utility.CreateSalt();
                    aa.Password1 = Utility.GenerateHashWithSalt(member.Password1, aa.PasswordSalt);
                    aa.Password2 = aa.Password1;
                    if (db.SaveChanges() > 0)
                    {
                        TempData["message"] = "修改成功!請重新登入新密碼";
                        return RedirectToAction("member_login", "Home");
                    }
                }
                //沒有修改密碼下(頁面重新刷新)
                db.SaveChanges();
                TempData["message"] = "修改資料成功!";

                //重新刷新判斷
                if (Request.UrlReferrer.ToString() != "https://localhost:44362/Members/member_edit")
                {
                    //重新刷新
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else
                {
                    //重新刷新(html元素被刪)
                    return Redirect(Request.UrlReferrer.ToString() + "?userid=" + member.UserId);
                }
            }

            //驗證失敗

            #region 會員類型賦值

            //會員類型
            var members = db.UserTypes.OrderBy(x => x.Id).Skip(2).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            items = members.Select(vm => new SelectListItem()
            {
                Value = vm.Id.ToString(),
                Text = vm.TypeName,
                Selected = vm.Members.Any(x => x.UserId == member.UserId) ? true : false
            }).ToList();
            ViewBag.RadioButtonList = items;

            #endregion 會員類型賦值

            member.Account = aa.Account;
            TempData["message"] = "資料有誤,請檢查資料!";
            return View(member);
        }

        /// <summary>
        ///   會員帳號開通
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PutAccount(string userid)
        {
            var member = db.Members.FirstOrDefault(x => x.UserId == userid);
            member.CheckAccount = true;
            if (db.SaveChanges() > 0)
            {
                TempData["message"] = "帳號開通成功!請登入您的帳密~";
            }
            return RedirectToAction("member_login", "Home");
        }

        /// <summary>
        /// 帳號驗證
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        [HttpPost]
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

        /// <summary>
        /// 圖片驗證碼
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckVerification(string code)
        {
            if (code != Session["code"].ToString())
            {
                return Json("驗證碼錯誤!", JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 畫出 圖形驗證碼
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidateCode()
        {
            byte[] data = null;
            //圖片長度設定
            string code = Utility.RandomCode(3);
            Session["code"] = code;
            //定義一個畫板
            MemoryStream ms = new MemoryStream();
            using (Bitmap map = new Bitmap(100, 40))
            {
                //畫筆,在指定畫板畫板上畫圖
                //g.Dispose();
                using (Graphics g = Graphics.FromImage(map))
                {
                    g.Clear(Color.White);
                    g.DrawString(code, new Font("黑體", 18.0F), Brushes.Blue, new Point(10, 8));
                    //繪製干擾線(數字代表幾條)
                    Utility.PaintInterLine(g, 10, map.Width, map.Height);
                }
                map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            data = ms.GetBuffer();
            return File(data, "image/jpeg");
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