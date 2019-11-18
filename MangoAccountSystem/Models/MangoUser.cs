using System;
using System.Security.Principal;

namespace MangoAccountSystem.Models
{
    public class MangoUser : IIdentity
    {
        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }
    }
}
