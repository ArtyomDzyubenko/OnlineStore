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

        // GET: basket
        public ActionResult Index()
        {
            return View(GetGoodsFromCookies());
        }
        
        public ActionResult AddToBasket(long? id)
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

            HttpCookie basket = Request.Cookies[BASKET_NAME];

            if (basket == null)
            {
                basket = new HttpCookie(BASKET_NAME);
            }

            String goodsId = "g_" + goods.goods_id;

            if (!basket.Values.AllKeys.Contains(goodsId))
            {
                basket.Values.Add(goodsId, id.ToString());
            }

            Response.Cookies.Set(basket);

            return Redirect("/Goods/Index");
        }

        public ActionResult DeleteFromBasket(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            String goodsId = "g_" + id;

            if (!Request.Cookies[BASKET_NAME].Values.AllKeys.Contains(goodsId))
            {
                return HttpNotFound();
            }

            NameValueCollection goods = Request.Cookies[BASKET_NAME].Values;
            goods.Remove(goodsId);

            HttpCookie basket = new HttpCookie(BASKET_NAME);
            basket.Values.Add(goods);

            Response.Cookies.Set(basket);

            return RedirectToAction("Index");
        }

        public ActionResult DeleteAllFromBasket()
        {
            Response.Cookies.Set(new HttpCookie(BASKET_NAME));

            return RedirectToAction("Index");
        }

        protected List<Goods> GetGoodsFromCookies()
        {
            List<Goods> goodsList = new List<Goods> { };

            HttpCookie basket = Request.Cookies[BASKET_NAME];

            if (basket == null)
            {
                Response.Cookies.Add(new HttpCookie(BASKET_NAME));
                return goodsList;
            }

            foreach (String key in basket.Values.AllKeys)
            {
                if (key == null)
                {
                    continue;
                }

                if (!Int64.TryParse(basket.Values.Get(key), out long id))
                {
                    return goodsList;
                }

                Goods goods = db.Goods.Find(id);

                if (goods != null)
                {
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