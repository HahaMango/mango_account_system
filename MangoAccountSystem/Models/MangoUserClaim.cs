using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MangoAccountSystem.Models
{
    public class MangoUserClaim
    {
        public int Id { get; set; }
        public int LoginName { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
