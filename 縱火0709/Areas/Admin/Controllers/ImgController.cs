using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using 縱火0709.Filters;
using 縱火0709.Models;

namespace 縱火0709.Areas.Admin.Controllers
{
    [MyAuthorizeAttribute(Roles = "D01")]
    [PriemissionAttrbute]
    public class ImgController : Controller
    {
        private Model1 db = new Model1();
        private int pageSize = 5;

        [HttpGet]
        [NoCache] // 將使「上一頁」無效 --- 掛上Attribute即可
        //GET:Admin/Img/Index
        public ActionResult Index(int page = 1)
        {
            //page用來記憶目前是哪一頁
            int currentPage = page < 1 ? 1 : page;

            //資料必須先經過排序
            //圖片列表
            var list = db.ImgClasses.OrderBy(x => x.imgorder);

            //分頁資料使用 ToPagedList()
            //參數分別是傳入所要分頁的頁碼以及分頁資料量
            //此範例就是每一頁會有5筆資料
            var PageResult = list.ToPagedList(currentPage, pageSize);
            return View(PageResult);
        }

        [HttpPost]
        public ActionResult Index(string addImgTxt, HttpPostedFileBase file, int page = 1)
        {
            if (file == null)
            {
                ModelState.AddModelError("imgName", "請選擇圖片");
            }

            if (ModelState.IsValid)
            {
                ImgClass imgs = new ImgClass();
                if (imgs != null)
                {
                    string time = DateTime.Now.ToString("yyyyMMdd");
                    string filePath = HttpContext.Server.MapPath("~/Images/scroll/") + time + file.FileName;
                    file.SaveAs(filePath);
                    string img = filePath.Substring(filePath.LastIndexOf('\\') + 1); //檔名
                    Utility.GenerateThumbnailImage(img, HttpContext.Server.MapPath("~/Images/scroll/"), "D:/InterviewWorks/縱火0709/縱火0709/Images/small/", "", 70);
                    imgs.imgUrl = "~/Images/small/" + time + file.FileName;
                    imgs.imgName = time + file.FileName;
                }
                var orderid = db.ImgClasses.Count() + 1;
                imgs.imgorder = orderid;
                imgs.imgText = addImgTxt;
                imgs.AddUserId = GetInsertAdminID();
                imgs.AddTime = DateTime.Now;
                db.ImgClasses.Add(imgs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            int currentPage = page < 1 ? 1 : page;
            var list = db.ImgClasses.OrderBy(x => x.imgorder);
            var PageResult = list.ToPagedList(currentPage, pageSize);
            return View(PageResult);
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


        //更新文字
        public ActionResult UpdateFileTxt(int id, string txt, int page)
        {
            ImgClass imgs = db.ImgClasses.Find(id);
            imgs.imgText = txt;
            imgs.EditUserId = GetInsertAdminID();
            imgs.EditTime= DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("index", new { page = page });
        }

        //更新排序編號
        public JsonResult OrderUpdate(int[] ids, int[] text)
        {
            JsonResult json = new JsonResult();
            for (int i = 0; i < ids.Length; i++)
            {
                ImgClass imgs = db.ImgClasses.Find(ids[i]);
                imgs.imgorder = text[i];
            }
            db.SaveChanges();
            return new JsonResult
            {
                Data = "ok",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        // GET: Admin/Img/Delete/5
        [HttpGet]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ImgClass imgs = db.ImgClasses.Find(id);
            db.ImgClasses.Remove(imgs);
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