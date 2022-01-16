using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PanelSelector.Models.Login
{
    public class ChangePasswordViewModel
    {
        public string LoginName { get; set; }
        [Required]
        [PasswordPropertyText]
        public string OldPassword { get; set; }
        [Required]
        [PasswordPropertyText]
        public string NewPassword { get; set; }
        [Required]
        [PasswordPropertyText]
        public string ConfirmPassword { get; set; }
    }
}