using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PanelSelector.Models.Login;
using PanelSelectorData;

namespace PanelSelector.Controllers
{
   [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UsersList()
        {
            var connectionString = ConfigurationManager.AppSettings["SqlConnection"];
            var userService = new UserService(connectionString);
            var dtUsers = userService.GetAllUsers();
            var users =new List<UserViewModel>();
            foreach (DataRow dtUsersRow in dtUsers.Rows)
            {
                var user = new UserViewModel()
                {
                    Id = (Guid) dtUsersRow["Id"], 
                    DisplayName = dtUsersRow["DisplayName"].ToString(), 
                    EmailId = dtUsersRow["Emailid"].ToString(),
                    IsAdmin = (bool)dtUsersRow["IsAdmin"],
                   PasswordHash = dtUsersRow["PasswordHash"].ToString(),
                   Username = dtUsersRow["LoginName"].ToString(),
                   IsActive = (bool)dtUsersRow["IsActive"]
                };
                users.Add(user);
            }
            return View(users);
        }
        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var defaultPassword = ConfigurationManager.AppSettings["DefaultPassword"];
                var connectionString = ConfigurationManager.AppSettings["SqlConnection"];
                var userService = new UserService(connectionString);
                var count = userService.CreateUser(model.Username, GetHash(defaultPassword), model.DisplayName,
                    model.EmailId, model.IsAdmin,  (Guid) Session["UserId"]);
                if (count > 0)
                {
                    return RedirectToAction("UsersList");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var connectionString = ConfigurationManager.AppSettings["SqlConnection"];
            var userService = new UserService(connectionString);
            var userDt = userService.GetUserDetails(id);
            if (userDt.Rows.Count > 0)
            {
                DataRow dtUsersRow = userDt.Rows[0];
                var user = new UserViewModel()
                {
                    Id = (Guid)dtUsersRow["Id"],
                    DisplayName = dtUsersRow["DisplayName"].ToString(),
                    EmailId = dtUsersRow["Emailid"].ToString(),
                    IsAdmin = (bool)dtUsersRow["IsAdmin"],
                    Username = dtUsersRow["LoginName"].ToString(),
                    IsActive =  (bool)dtUsersRow["IsActive"]
                };
                return View(user);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var connectionString = ConfigurationManager.AppSettings["SqlConnection"];
                var userService = new UserService(connectionString);
                var count = userService.UpdateUser((Guid) model.Id, model.Username,  model.DisplayName,
                    model.EmailId, model.IsAdmin,model.IsActive, (Guid)Session["UserId"]);
                if (count > 0)
                {
                    return RedirectToAction("UsersList");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var connectionString = ConfigurationManager.AppSettings["SqlConnection"];
            var userService = new UserService(connectionString);
            var result = userService.DeleteUser(id);
            return RedirectToAction("UsersList");
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