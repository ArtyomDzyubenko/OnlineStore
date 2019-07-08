using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class BasketController : Controller
    {
        private readonly static String BASKET_NAME = "basket";
        private online_storeEntities db = new online_storeEntities();

        //Главная страница корзины
        public ActionResult Index()
        {   //Извлекам все товары из корзины, добавляем в модель и отправляем на страницу корзины
            return View(GetGoodsFromCookies());
        }
        //Добавление товара в корзину
        public ActionResult AddToBasket(long? id)
        {   //Неверный Id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Goods goods = db.Goods.Find(id);

            if (goods == null)
            {
                return HttpNotFound();
            }
            //Запрашиваем корзину из cookies
            HttpCookie basket = Request.Cookies[BASKET_NAME];
            //Корзина еще не создана либо удалена
            if (basket == null)
            {
                basket = new HttpCookie(BASKET_NAME);
            }
            //Id товара в корзине
            String goodsId = "g_" + goods.goods_id;
            //Товар уже есть в корзине
            if (!basket.Values.AllKeys.Contains(goodsId))
            {
                basket.Values.Add(goodsId, id.ToString());
            }
            //Добавляем корзину в ответ
            Response.Cookies.Set(basket);
            //Перенаправляем на витрину
            return Redirect("/Goods/Index");
        }
        //Удаление товара из корзины
        public ActionResult DeleteFromBasket(long? id)
        {   //Неверный Id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            String goodsId = "g_" + id;
            //Товара нет в корзине
            if (!Request.Cookies[BASKET_NAME].Values.AllKeys.Contains(goodsId))
            {
                return HttpNotFound();
            }
            //Извлекаем корзину из cookies и удаляем товар
            NameValueCollection goods = Request.Cookies[BASKET_NAME].Values;
            goods.Remove(goodsId);
            //Формируем новую корзину
            HttpCookie basket = new HttpCookie(BASKET_NAME);
            basket.Values.Add(goods);
            //Отправляем пользователю
            Response.Cookies.Set(basket);
            //Перенаправляем в корзину
            return RedirectToAction("Index");
        }
        //Очистка корзины
        public ActionResult DeleteAllFromBasket()
        {   //Перезаписываем корзину пустым cookie
            Response.Cookies.Set(new HttpCookie(BASKET_NAME));

            return RedirectToAction("Index");
        }
        //Извлекаем корзину из cookies
        protected List<Goods> GetGoodsFromCookies()
        {   //Вспомогательный массив для хранения информации о товаре
            //т.к размер cookies ограничен, в cookie храним только Id товаров
            List<Goods> goodsList = new List<Goods> { };
            //Извлекаем корзину
            HttpCookie basket = Request.Cookies[BASKET_NAME];
            //Проверяем, существует ли корзина в cookies
            if (basket == null)
            {
                Response.Cookies.Add(new HttpCookie(BASKET_NAME));
                return goodsList;
            }
            //
            foreach (String key in basket.Values.AllKeys)
            {
                if (key == null)
                {
                    continue;
                }
                //В сookies все данные строковые, преобразовываем Id в число
                if (!Int64.TryParse(basket.Values.Get(key), out long id))
                {
                    return goodsList;
                }
                //Находим информацию о товаре в БД
                Goods goods = db.Goods.Find(id);

                if (goods != null)
                {   //Добавляем во вспомогательный массив
                    goodsList.Add(goods);
                }
            }

            return goodsList;
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