using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanelSelector.Models.Login
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string DisplayName { get; set; }
        public string EmailId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}