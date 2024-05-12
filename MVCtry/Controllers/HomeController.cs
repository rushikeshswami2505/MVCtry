using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCtry.Models;
using MVCtry.Data;
using System.Web.WebPages;
using System.Web.Script.Serialization;
using System.Collections;

namespace MVCtry.Controllers
{
    public class HomeController : Controller
    {
        ProductContext db = new ProductContext();
        // GET: Home


        [Route("Home/Index")]
        [Route("Home")]
        // [Route("")]
        public ActionResult Index()
        {
            var data = db.Products.ToList();
            return View(data);
        }
        [Route("Home/Buy")]


        [Route("")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        public ActionResult Login(string email,string password)
        {
            var cookie = Request.Cookies["LoginCookies"];
            if (email.IsEmpty() || password.IsEmpty())
            {
                if (email.IsEmpty()) ModelState.AddModelError("email", "Please enter email");
                if (password.IsEmpty()) ModelState.AddModelError("password", "Please enter password");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (cookie != null)
            {
                var json = cookie.Value;
                var jsonSerializer = new JavaScriptSerializer();
                var userList = jsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
                
                foreach (var user in userList)
                {
                    if (user["email"] == email && user["password"] == password)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }

        [Route("Home/Signup")]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/Signup")]
        public ActionResult Signup(string email, string firstname, string lastname, string gender, string phone, string password, string cpassword)
        {
            TempData["Email"] = email;
            TempData["FirstName"] = firstname;
            TempData["LastName"] = lastname;
            TempData["Gender"] = gender;
            TempData["Phone"] = phone;
            Dictionary<string, string> user = new Dictionary<string, string>
            {
                { "email", email },
                { "firstname", firstname },
                { "lastname", lastname },
                { "gender", gender },
                { "phone", phone },
                { "password", password },
                { "cpassword", cpassword }
            };

            var cookie = Request.Cookies["LoginCookies"] ?? new HttpCookie("LoginCookies");
            var jsonSerializer = new JavaScriptSerializer();
            var myList = cookie.Value != null ? jsonSerializer.Deserialize<List<Dictionary<string, string>>>(cookie.Value) : new List<Dictionary<string, string>>();
            myList.Add(user);
            var json = jsonSerializer.Serialize(myList);
            cookie.Value = json;
            Response.Cookies.Add(cookie);

            return RedirectToAction("Login");
        }

        [Route("Home/Buy")]
        public ActionResult Buy()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/Buy")]
        public ActionResult Buy(ProductModel productModel, string goHome)
        {
            if (goHome != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid == true)
            {
                Product product = ModelToDataBase(productModel);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Buy", productModel);
        }

        [Route("Home/Sell")]
        public ActionResult Sell()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/Sell")]
        public ActionResult Sell(ProductModel productModel, string goHome)
        {
            if (goHome != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid == true)
            {
                Product product = ModelToDataBase(productModel);
                var existingProduct = db.Products.FirstOrDefault(p => p.itemtype == productModel.itemtype && p.itemsize == productModel.itemsize);
                if (existingProduct != null)
                {
                    existingProduct.itempiece -= product.itempiece;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View("Sell", productModel);
        }

        [Route("Home/Edit")]
        public ActionResult Edit(int id)
        {
            var row = db.Products.Where(p => p.id == id).FirstOrDefault();
            ProductModel product = DataBaseToModel(row);
            return View(product);
        }

        [HttpPost]
        [Route("Home/Edit")]
        public ActionResult Edit(ProductModel productModel, string goHome)
        {
            if (goHome != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid == true)
            {
                Product product = ModelToDataBase(productModel);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", productModel);
        }

        // [HttpDelete]
        [Route("Home/Delete")]
        public ActionResult Delete(int id)
        {
            var productToDelete = db.Products.Find(id);
            if (productToDelete != null)
            {
                db.Products.Remove(productToDelete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }
        private Product ModelToDataBase(ProductModel productModel)
        {
            Product product = new Product();
            product.id = productModel.id;
            product.itemtype = productModel.itemtype;
            product.itemsize = productModel.itemsize;
            product.itempiece = productModel.itempiece;
            product.itemprice = productModel.itemprice;
            return product;
        }

        private ProductModel DataBaseToModel(Product product)
        {
            ProductModel productModel = new ProductModel();
            productModel.id = product.id;
            productModel.itemtype = product.itemtype;
            productModel.itemsize = product.itemsize;
            productModel.itempiece = product.itempiece;
            productModel.itemprice = product.itemprice;
            return productModel;
        }

    }
}