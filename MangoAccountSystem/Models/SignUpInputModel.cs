namespace MangoAccountSystem.Models
{
    public class SignUpInputModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public bool IsAgree { get; set; }
    }
}
