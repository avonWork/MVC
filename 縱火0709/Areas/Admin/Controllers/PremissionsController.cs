using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using 縱火0709.Filters;
using 縱火0709.Models;

namespace 縱火0709.Areas.Admin.Controllers
{
    [PriemissionAttrbute]
    public class PremissionsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Admin/Premissions
        [MyAuthorizeAttribute(Roles = "A01")]
        [NoCache] // 將使「上一頁」無效 --- 掛上Attribute即可
        public ActionResult Index()
        {
            var premissions = db.Premissions.Include(p => p.Primissions);
            return View(premissions.ToList());
        }

        // GET: Admin/Premissions/Details/5
        [MyAuthorizeAttribute(Roles = "A01")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premission premission = db.Premissions.Find(id);
            if (premission == null)
            {
                return HttpNotFound();
            }
            return View(premission);
        }

        // GET: Admin/Premissions/Create
        [MyAuthorizeAttribute(Roles = "A02")]
        public ActionResult Create()
        {
            ViewBag.Pid = new SelectList(db.Premissions, "Id", "Name");
            return View();
        }

        // POST: Admin/Premissions/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorizeAttribute(Roles = "A02")]
        public ActionResult Create([Bind(Include = "Id,Name,Pid,PValue,Url")] Premission premission)
        {
            if (ModelState.IsValid)
            {
                db.Premissions.Add(premission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Pid = new SelectList(db.Premissions, "Id", "Name", premission.Pid);
            return View(premission);
        }

        // GET: Admin/Premissions/Edit/5
        [MyAuthorizeAttribute(Roles = "A01")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premission premission = db.Premissions.Find(id);
            if (premission == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pid = new SelectList(db.Premissions, "Id", "Name", premission.Pid);
            return View(premission);
        }

        // POST: Admin/Premissions/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [MyAuthorizeAttribute(Roles = "A01")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Pid,PValue,Url")] Premission premission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(premission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Pid = new SelectList(db.Premissions, "Id", "Name", premission.Pid);
            return View(premission);
        }

        // GET: Admin/Premissions/Delete/5
        [MyAuthorizeAttribute(Roles = "A01")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premission premission = db.Premissions.Find(id);
            if (premission == null)
            {
                return HttpNotFound();
            }
            return View(premission);
        }

        // POST: Admin/Premissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [MyAuthorizeAttribute(Roles = "A01")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Premission premission = db.Premissions.Find(id);
            db.Premissions.Remove(premission);
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