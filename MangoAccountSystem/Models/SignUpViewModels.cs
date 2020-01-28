using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangoAccountSystem.Models
{
    public class SignUpViewModels
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public bool IsAgree { get; set; }

        public string UserNameErrors { get; set; }
        public bool IsUserNameError
        {
            get
            {
                return UserNameErrors != null && UserNameErrors != "";
            }
        }

        public string PasswordErrors { get; set; }
        public bool IsPasswordError
        {
            get
            {
                return PasswordErrors != null && PasswordErrors != "";
            }
        }

        public string AgreeErrors { get; set; }
        public bool IsAgreeError
        {
            get
            {
                return AgreeErrors != null && AgreeErrors != "";
            }
        }
    }
}
