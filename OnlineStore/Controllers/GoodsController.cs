using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Util;

namespace OnlineStore.Controllers
{
    public class GoodsController : Controller
    {
        private online_storeEntities db = new online_storeEntities();

        // GET: Goods
        public ActionResult Index()
        {
            return View(db.Goods.ToList());
        }

        // GET: Goods/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Goods goods = db.Goods.Find(id);

            if (goods == null)
            {
                return HttpNotFound();
            }

            return View(goods);
        }

        // GET: Goods/Create
        [Authorize(Roles ="ORA_DBA")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Goods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="ORA_DBA")]
        public ActionResult Create([Bind(Include = "goods_id,name,description,price")] Goods goods)
        {
            if (ModelState.IsValid)
            {
                db.Goods.Add(goods);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goods);
        }

        // GET: Goods/Edit/5
        [Authorize(Roles = "ORA_DBA")]
        public ActionResult Edit(long? id)
        {
            if (!UserRoleChecker.IsUserAdmin())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Goods goods = db.Goods.Find(id);

            if (goods == null)
            {
                return HttpNotFound();
            }

            return View(goods);
        }

        // POST: Goods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ORA_DBA")]
        public ActionResult Edit([Bind(Include = "goods_id,name,description,price")] Goods goods)
        {
            if (!UserRoleChecker.IsUserAdmin())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                db.Entry(goods).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goods);
        }

        // GET: Goods/Delete/5
        [Authorize(Roles = "ORA_DBA")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Goods goods = db.Goods.Find(id);

            if (goods == null)
            {
                return HttpNotFound();
            }

            return View(goods);
        }

        // POST: Goods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ORA_DBA")]
        public ActionResult DeleteConfirmed(long id)
        {
            Goods goods = db.Goods.Find(id);
            db.Goods.Remove(goods);
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
