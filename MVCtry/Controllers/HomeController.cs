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
        Context db = new Context();
        // GET: Home
        [Route("Home/Index")]
        [Route("Home")]
        public ActionResult Index()
        {
            var data = db.Products.ToList();
            return View(data);
        }

        [Route("Home/Login")]
        [Route("")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        public ActionResult Login(string email,string password)
        {
            if (email.IsEmpty() || password.IsEmpty())
            {
                if (email.IsEmpty()) ModelState.AddModelError("email", "Please enter email");
                if (password.IsEmpty()) ModelState.AddModelError("password", "Please enter password");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = db.Users.FirstOrDefault(u => u.email == email);
            if (user != null)
            {
                if (user.password == password)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("password", "Incorrect password");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("email", "User does not exist");
                return View();
            }
        }
        [HttpPost]
        [Route("Home/Logout")]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        [Route("Home/Signup")]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/Signup")]
        public ActionResult Signup(UserModel userModel)
        {   
            if(ModelState.IsValid)
            {
                User user = ModelToUser(userModel);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View("Signup", userModel);
        }
        private User ModelToUser(UserModel userModel)
        {
            User user = new User();
            user.email = userModel.email;
            user.firstname = userModel.firstname;
            user.lastname = userModel.lastname;
            user.gender = userModel.gender;
            user.phone = userModel.phone;
            user.password = userModel.password;
            return user;
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
            if (ModelState.IsValid)
            {
                Product product = ModelToProduct(productModel);
                db.Products.Add(product);
                db.SaveChanges();
                // return RedirectToAction("Index");
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
                Product product = ModelToProduct(productModel);
                var existingProduct = db.Products.FirstOrDefault(p => p.itemtype == productModel.itemtype && p.itemsize == productModel.itemsize);
                if (existingProduct != null)
                {
                    existingProduct.itempiece -= product.itempiece;
                    db.SaveChanges();
                    // return RedirectToAction("Index");
                }
            }
            return View("Sell", productModel);
        }

        [Route("Home/Edit")]
        public ActionResult Edit(int id)
        {
            var row = db.Products.Where(p => p.id == id).FirstOrDefault();
            ProductModel product = ProductToModel(row);
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
                Product product = ModelToProduct(productModel);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", productModel);
        }

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
        private Product ModelToProduct(ProductModel productModel)
        {
            Product product = new Product();
            product.id = productModel.id;
            product.itemtype = productModel.itemtype;
            product.itemsize = productModel.itemsize;
            product.itempiece = productModel.itempiece;
            product.itemprice = productModel.itemprice;
            return product;
        }

        private ProductModel ProductToModel(Product product)
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