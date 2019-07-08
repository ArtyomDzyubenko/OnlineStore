using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class GoodsController : Controller
    {
        private online_storeEntities db = new online_storeEntities();

        //Главная страница витрины
        public ActionResult Index()
        {   //Добавляем в модель список всех товаров
            return View(db.Goods.ToList());
        }

        //Получение детальной информации о товаре: Goods/Details/5
        public ActionResult Details(long? id)
        {   //Проверяем Id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Извлекам товар из БД
            Goods goods = db.Goods.Find(id);
            //Если товар не найден, посылаем NotFound
            if (goods == null)
            {
                return HttpNotFound();
            }
            //Добавляем товар в модель и возвращаем страницу с детальной информацией
            return View(goods);
        }

        //Создавать товары может только администратор
        [Authorize(Roles ="ORA_DBA")]
        //Получение страницы создания нового товара (GET): Goods/Create
        public ActionResult Create()
        {   //Возвращаем страницу создания нового товара
            return View();
        }

        //Контроллер приемa данных с формы создания товара (POST): Goods/Create
        [HttpPost]
        //Токен защиты формы от CSRF
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="ORA_DBA")]
        //Создание товара на основе биндинга данных формы
        public ActionResult Create([Bind(Include = "goods_id,name,description,price")] Goods goods)
        {   
            if (ModelState.IsValid)
            {   //Сохраняем товар в БД
                db.Goods.Add(goods);
                db.SaveChanges();
                //Перенаправляем на витрину
                return RedirectToAction("Index");
            }
            //Если модель не валидна, возвращаем обратно
            return View(goods);
        }

        //Получение страницы редактирования товара (GET) : Goods/Edit/5
        //Редактировать товары может только администратор
        [Authorize(Roles = "ORA_DBA")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Извлекаем товар из БД
            Goods goods = db.Goods.Find(id);
            //Товар не найден
            if (goods == null)
            {
                return HttpNotFound();
            }
            //Отправляем на страницу редактирования
            return View(goods);
        }

        //Контроллер приемa данных с формы создания товара (POST): Goods/Create POST: Goods/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ORA_DBA")]
        public ActionResult Edit([Bind(Include = "goods_id,name,description,price")] Goods goods)
        {

            if (ModelState.IsValid)
            {
                db.Entry(goods).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(goods);
        }

        //Удаление товара (GET): Goods/Delete/5
        //Удалять товары может только администратор
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

        //Контроллер подтверждения удаления товара (POST): Goods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ORA_DBA")]
        public ActionResult DeleteConfirmed(long id)
        {   
            Goods goods = db.Goods.Find(id);
            //Удаляем товар из БД
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
