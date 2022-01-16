using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI.WebControls;
using PanelSelector.Models;
using PanelSelector.Models.Login;
using PanelSelectorData;

namespace PanelSelector.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userName, string password)
        {
            var connectionString = ConfigurationManager.AppSettings["SqlConnection"];
            var defaultPassword = ConfigurationManager.AppSettings["DefaultPassword"];
            var userService = new UserService(connectionString);
            var data = userService.ValidateUserAndGetDetails(userName, GetHash(password));
            if (data.Rows.Count>0)
            {
                TempData["DisplayName"] = data.Rows[0]["displayname"];
                Session["Name"] = data.Rows[0]["displayname"];
                Session["UserId"] = data.Rows[0]["Id"];
                Session["isAdmin"] = data.Rows[0]["IsAdmin"];
                FormsAuthentication.SetAuthCookie(userName, false);
                if (password == defaultPassword)
                {
                    var loginName = data.Rows[0]["LoginName"].ToString();
                    return RedirectToAction("ChangePassword", new RouteValueDictionary(
                        new { controller = "Login", action = "ChangePassword", loginName = loginName }));
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Login Failed";
                TempData["Message"] = "Login or password incorrect!!";
                return View();
            }

        }

        [HttpGet]
        public ActionResult ChangePassword(string loginName)
        {
            return View();
        }


        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid && model.NewPassword == model.ConfirmPassword)
            {
                var connectionString = ConfigurationManager.AppSettings["SqlConnection"];
                var userService = new UserService(connectionString);
                var count = userService.UpdatePassword(model.LoginName, GetHash(model.OldPassword), GetHash(model.NewPassword));
                if (count > 0)
                {
                    //TempData["UserMessage"] = new MessageVM() { CssClassName = "alert-sucess", Title = "Success!", Message = "Operation Done." };
                    TempData["Message"] = "Password Updated Successfully";
                    return RedirectToAction("Logout");
                }
            }

            return View(model);

        }

        public ActionResult Logout()
        {
            Session["Name"] = null;
            Session["UserId"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ForgotPassword(LoginViewModel model)
        {
            TempData["Message"] = "Your password reset request has been submitted to Admin";
            return RedirectToAction("Index");
        }

        private string GetHash(string value)
        {
            HashAlgorithm hashAlg = new SHA256CryptoServiceProvider();
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(value);
            byte[] bytHash = hashAlg.ComputeHash(bytValue);
            return Convert.ToBase64String(bytHash);
        }
    }
}