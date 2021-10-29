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
    [MyAuthorizeAttribute(Roles = "E01")]
    [PriemissionAttrbute]
    public class FileController : Controller
    {
        private Model1 db = new Model1();
        private int pageSize = 5;

        // GET: FileClass
        [HttpGet]
        [NoCache] // 將使「上一頁」無效 --- 掛上Attribute即可
        public ActionResult Index(int page = 1)
        {
            //page用來記憶目前是哪一頁
            int currentPage = page < 1 ? 1 : page;

            //資料必須先經過排序
            //圖片列表
            var list = db.FileClasses.OrderBy(x => x.fileorder);

            //分頁資料使用 ToPagedList()
            //參數分別是傳入所要分頁的頁碼以及分頁資料量
            //此範例就是每一頁會有10筆資料
            var PageResult = list.ToPagedList(currentPage, pageSize);
            return View(PageResult);
        }

        [HttpPost]
        public ActionResult Index(string addfileText, HttpPostedFileBase file, int page = 1)
        {
            if (file == null)
            {
                ModelState.AddModelError("fileName", "請選擇檔案");
            }

            if (ModelState.IsValid)
            {
                FileClass files = new FileClass();
                if (file != null)
                {
                    string time = DateTime.Now.ToString("yyyyMMdd");
                    string filePath = HttpContext.Server.MapPath("~/Files/file/") + time + file.FileName;
                    file.SaveAs(filePath);
                    files.fileUrl = "~/Files/file/" + time + file.FileName;
                    files.fileName = time + file.FileName;
                }
                var orderid = db.FileClasses.Count() + 1;
                files.fileorder = orderid;
                files.fileText = addfileText;
                files.AddUserId = GetInsertAdminID();
                files.AddTime = DateTime.Now;
                db.FileClasses.Add(files);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            int currentPage = page < 1 ? 1 : page;
            var list = db.FileClasses.OrderBy(x => x.fileorder);
            var PageResult = list.ToPagedList(currentPage, pageSize);
            return View(PageResult);
        }

       
        //更新文字
        public ActionResult UpdateFileTxt(int id, string txt, int page)
        {
            FileClass file = db.FileClasses.Find(id);
            file.fileText = txt;
            file.EditUserId = GetInsertAdminID();
            file.EditTime = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("index", new { page = page });
        }

        //更新排序編號
        public JsonResult OrderUpdate(int[] ids, int[] text)
        {
            JsonResult json = new JsonResult();
            for (int i = 0; i < ids.Length; i++)
            {
                FileClass file = db.FileClasses.Find(ids[i]);
                file.fileorder = text[i];
            }
            db.SaveChanges();

            return new JsonResult
            {
                Data = "ok",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
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
        // POST: FileClass/Delete/5
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            FileClass FileClass = db.FileClasses.Find(id);
            db.FileClasses.Remove(FileClass);
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