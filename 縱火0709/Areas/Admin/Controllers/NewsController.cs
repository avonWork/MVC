using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using 縱火0709.Models;
using PagedList; //分頁套件
using 縱火0709.Filters;
using System.Net;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Ast.Selectors;
using static 縱火0709.Models.Enum;

namespace 縱火0709.Areas.Admin.Controllers
{
    [PriemissionAttrbute]
    public class NewsController : Controller
    {
        private Model1 db = new Model1();

        private int pageSize = 5; //分頁

        // GET: Admin/News/Index
        [MyAuthorizeAttribute(Roles = "B02")]
        [NoCache] // 將使「上一頁」無效 --- 掛上Attribute即可
        public ActionResult Index(string searchStartDate, string searchEndDate, string searchTitle, string sortBy, int page = 1)
        {
            //page用來記憶目前是哪一頁
            int currentPage = page < 1 ? 1 : page;
            //排序
            ViewBag.DateSort = String.IsNullOrEmpty(sortBy) ? "Date desc" : "";
            ViewBag.TitleSort = sortBy == "Title" ? "Title desc" : "Title";
            //包含顯示 未顯示
            var newsList = db.News.Where(y => y.Sticky < StickyType.刪除_垃圾箱).AsQueryable();

            if (searchStartDate != null && searchStartDate != "" || searchEndDate != null && searchEndDate != "")
            {
                DateTime start = searchStartDate.AsDateTime();

                DateTime end = searchEndDate.AsDateTime();

                newsList = newsList.Where(m => m.Date >= start && m.Date <= end);
            }

            if (searchTitle != null)
            {
                newsList = newsList.Where(x => x.New_Title.StartsWith(searchTitle));
            }

            //第一次postback     sortBy為null跑去default
            switch (sortBy)
            {
                case "Date desc":
                    newsList = newsList.OrderByDescending(x => x.Date);
                    break;

                case "Title desc":
                    newsList = newsList.OrderByDescending(x => x.New_Title);
                    break;

                case "Title":
                    newsList = newsList.OrderBy(x => x.New_Title);
                    break;

                default:
                    newsList = newsList.OrderBy(x => x.Date);
                    break;
            }

            var PageResult = newsList.ToPagedList(currentPage, pageSize);

            return View(PageResult);
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [MyAuthorizeAttribute(Roles = "B02")]
        public ActionResult ClearAll(string searchStartDate, string searchEndDate, string searchTitle)
        {
            return RedirectToAction("Index", "News");
        }

        // GET: Admin/NewsDemo/Details/5
        [MyAuthorizeAttribute(Roles = "B02")]
        public ActionResult Details(int? id)
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

        // GET: Admin/News/Create
        [MyAuthorizeAttribute(Roles = "B01")]
        public ActionResult Create()
        {
            NewsViewModel dataModel = new NewsViewModel();
            return View(dataModel);
        }

        // POST: Admin/News/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [MyAuthorizeAttribute(Roles = "B01")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] //设置取消验
        public ActionResult Create(NewsViewModel NVM)
        {
            if (NVM.image1 == null)
            {
                //自訂義要在ModelState.IsValid前面 才不會進入 if (ModelState.IsValid)
                ModelState.AddModelError("image1", "請選擇照片");
            }

            if (ModelState.IsValid)
            {
                News news = new News();
                if (NVM.image1 != null)
                {
                    NVM.image1.SaveAs(HttpContext.Server.MapPath("~/Images/news/") + NVM.image1.FileName);
                    news.New_img = "~/Images/news/" + NVM.image1.FileName;
                }
                news.New_Title = NVM.New_Title;
                news.Date = NVM.Date;
                news.New_Message = NVM.New_Message;
                news.New_Content = NVM.New_Content;
                news.Sticky = NVM.Sticky;
                news.AddTime = DateTime.Now;
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(NVM);
        }

        // GET: Admin/Users/Edit/5
        [MyAuthorizeAttribute(Roles = "B02")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NewsViewModel NVME = new NewsViewModel();
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            NVME.New_img = news.New_img;
            NVME.New_Title = news.New_Title;
            NVME.Date = news.Date;
            NVME.New_Message = news.New_Message;
            NVME.New_Content = news.New_Content;
            NVME.Sticky = news.Sticky;
            //POST 紀錄會遺失,必須Session傳遞紀錄
            Session["recordPage"] = Request["page"];
            Session["recordSearchTitle"] = Request["searchTitle"];
            Session["recordSearchStartDate"] = Request["searchStartDate"];
            Session["recordSearchEndDate"] = Request["searchEndDate"];
            Session["recordSortBy"] = Request["sortBy"];
            return View(NVME);
        }

        // POST: Admin/Users/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorizeAttribute(Roles = "B02")]
        [ValidateInput(false)] //设置取消验
        public ActionResult Edit(NewsViewModel NVME)
        {
            if (ModelState.IsValid)
            {
                News news = db.News.Find(NVME.id);
                if (NVME.image1 != null)
                {
                    NVME.image1.SaveAs(HttpContext.Server.MapPath("~/Images/news/") + NVME.image1.FileName);
                    news.New_img = "~/Images/news/" + NVME.image1.FileName;
                }
                news.New_Title = NVME.New_Title;
                news.Date = NVME.Date;
                news.New_Message = NVME.New_Message;
                news.New_Content = NVME.New_Content;
                news.Sticky = NVME.Sticky;
                news.EditTime = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index", new { page = Session["recordPage"], searchTitle = Session["recordSearchTitle"], searchStartDate = Session["recordSearchStartDate"], searchEndDate = Session["recordSearchEndDate"], sortBy = Session["recordSortBy"] });

            }
            return View(NVME);
        }

        // GET: Admin/NewsDemo/Delete/5
        [MyAuthorizeAttribute(Roles = "B02")]
        public ActionResult Delete(int? id)
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

        // POST: Admin/NewsDemo/Delete/5
        [HttpPost, ActionName("Delete")]
        [MyAuthorizeAttribute(Roles = "B02")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            News news = db.News.Find(id);
            //刪除語法
            //db.News.Remove(news);
            //物理刪除
            news.Sticky = StickyType.刪除_垃圾箱;
            db.SaveChanges();
            return RedirectToAction("Index");
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