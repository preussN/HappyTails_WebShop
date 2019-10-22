using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HappyTails_WebShop.Models;

namespace HappyTails_WebShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private HappyTailsEntities db = new HappyTailsEntities();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderNow(int id)
        {
            //Code for creating a Shopping cart is written after Youtube guide:
            //https://www.youtube.com/watch?v=ZBd0MnKb7u0&fbclid=IwAR2VvtqR6QaSysYKUjt1LlhIQPEOfvxdpjYsqebJp9IZAelJRO1mi_L21-E

            if (Session["cart"] == null)
            {
                List<CartItem> cart = new List<CartItem>();
                cart.Add(new CartItem(db.Product.Find(id), 1));
                Session["cart"] = cart;
            }
            else
            {
                List<CartItem> cart = (List<CartItem>) Session["cart"];
                int index = IsCartItemExisting(id);

                if (index == -1)
                {
                    //If the products does not exist in the shopping cart, it will be added
                    cart.Add(new CartItem(db.Product.Find(id), 1));
                }
                else
                {
                    //Adds the product one more time (for each time the user clicks on Order now)
                    cart[index].Quantity++;
                    Session["cart"] = cart;
                }
            }
            return View("Cart");
        }

        private int IsCartItemExisting(int id)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];

            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Prod.ProductID == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public ActionResult DeleteCartItem(int id)
        {
            //Deletes ALL the products of the same id (user has add 3 products of the id 2, then clicks on Delete -> all 3 products will be removed)
            int index = IsCartItemExisting(id);
            List<CartItem> cart = (List<CartItem>) Session["cart"];

            cart.RemoveAt(index);

            return View("Cart");
        }
    }
}