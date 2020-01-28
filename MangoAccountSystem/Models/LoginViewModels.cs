namespace MangoAccountSystem.Models
{
    public class LoginViewModels
    {
        public string ReturnUrl { get; set; }
        public string UserName { get; set; }
        public string ValidationErrors { get; set; }
        public bool IsError
        {
            get
            {
                return ValidationErrors != null && ValidationErrors != "";
            }
        }
    }
}
