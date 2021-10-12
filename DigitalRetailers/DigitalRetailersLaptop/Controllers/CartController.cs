using DigitalRetailersLaptop.Helpers;
using DigitalRetailersLaptop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalRetailersLaptop.Controllers
{
    public class CartController : Controller
    {




        ApplicationDBContext context;
        public CartController()
        {
            context = new ApplicationDBContext();
        }



        public IActionResult Index()
        {
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                return View("CartEmpty");
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                ViewBag.cart = cart;
                ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
                return View();
            }

        }




        public IActionResult Buy(int id)
        {
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)//Session object is cart
            {
                //No item in the cart session object and no session object with cart

                //now creating new list of items because there is no new product items
                List<Item> cart = new List<Item>();

                cart.Add(new Item { Product = context.Products.Find(id), Quantity = 1 });

                //cart need to store as setObjectAsJson,cart id is generated as cart,item is list of cart
                SessionHelper.setObjectAsJson(HttpContext.Session, "cart", cart);
                
            }
            else
            {
                //The cart is already saved in session,getting that cart and stroing into list of items

                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = isExists(id);

                if (index != -1) //item is already added to the cart
                {
                    cart[index].Quantity++;//So add the quantity.
                }
                else
                {
                    cart.Add(new Item { Product = context.Products.Find(id), Quantity = 1 });
                }
                SessionHelper.setObjectAsJson(HttpContext.Session, "cart", cart);//Changed Session need to be updated
            }
            return RedirectToAction("Index");//Once added to the cart we need to redirect to the Index
        }



        public int isExists(int id)     //sending product id
        {
            //Getting item from the cart

            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id == id)
                {
                    return i;
                }
            }
            return -1;     //Means item does not exists
        }

        public IActionResult Checkout()
        {
            if (SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user") != null)
            {
                //Only if the user is loggedin,The payment procedure might proceed
                return View();
            }
            else
            {
                //or will be redirected to the account page
                return RedirectToAction("Index", "Account");
            }
             
        }
        public IActionResult Thankyou()
        {
            return View();
        }
    }
}
