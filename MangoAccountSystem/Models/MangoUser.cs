using System;
using System.Collections.Generic;

namespace MangoAccountSystem.Models
{
    public class MangoUser
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public IList<MangoUserClaim> Claims { get; set; }
    }
}
