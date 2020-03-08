using MangoAccountSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public static class UserData
    {
        public static MangoUser TestUser1 { get; set; }

        public static MangoUser TestUser2 { get; set; }

        public static MangoUser Mango { get; set; }

        static UserData()
        {
            TestUser1 = new MangoUser
            {
                Id = 3,
                UserName = "test1",
                Email = "test1@email.com",
                NormalizedEmail = "TEST1@EMAIL.COM",
                LoginName = "TEST1",
            };

            TestUser2 = new MangoUser
            {
                Id = 2,
                UserName = "test2",
                Email = "test2@email.com",
                NormalizedEmail = "TEST2@EMAIL.COM",
                LoginName = "TEST2",
            };

            Mango = new MangoUser
            {
                Id = 4,
                UserName = "mango",
                LoginName = "MANGO",
                Email = "mango@mango.com",
                NormalizedEmail = "MANGO@MANGO.COM",
                Claims = null,
                Password = "dsfwfdsfw",
                EmailConfirmed = false,
                CreateDate = new DateTime(2020, 03, 07),
                LastLoginDate = new DateTime(2020, 04, 07)
            };
        }
    }
}
