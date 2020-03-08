using MangoAccountSystem.Dao;
using MangoAccountSystem.Models;
using MangoAccountSystem.Test;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace Test
{
    public class UserManagerTestBase
    {
        private IWebHost host;

        #region 测试数据

        /// <summary>
        /// CreateUserTest测试数据
        /// </summary>
        public static IList<object[]> UserTestList
        {
            get
            {
                IList<object[]> mangoUsers = new List<object[]>
                {
                    new object[]{ "test1", "test1@email.com",UserData.TestUser1,true}
                };

                return mangoUsers;
            }
        }

        /// <summary>
        /// FindByName测试
        /// </summary>
        public static IList<object[]> FindByNameDataList
        {
            get
            {
                IList<object[]> dataList = new List<object[]>
                {
                    new object[]{ "mango", UserData.Mango},
                };

                return dataList;
            }
        }

        public static IList<object[]> UpdataDataList
        {
            get
            {
                IList<object[]> dataList = new List<object[]>
                {
                    new object[]{ "4", new DateTime(2020,05,10)},
                };

                return dataList;
            }
        }

        #endregion

        public UserManagerTestBase()
        {
            string[] args = { };
            var host = CreateWebHostBuilder(args).Build();
            this.host = host;
        }

        protected void UserManagerAction(Action<UserManager<MangoUser>> userManagerAction)
        {
            using (var _scope = host.Services.CreateScope())
            {
                var services = _scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<UserDbContext>();
                    var user = new UserEntity
                    {
                        Id = 4,
                        UserName = "mango",
                        LoginName = "MANGO",
                        Email = "mango@mango.com",
                        NormalizedEmail = "MANGO@MANGO.COM",
                        EmailConfirmed = false,
                        Password = "dsfwfdsfw",
                        CreateDate = new DateTime(2020, 03, 07),
                        LastLoginDate = new DateTime(2020, 04, 07)
                    };
                    context.MangoUsers.Add(user);                    
                    context.SaveChanges();

                    var es = context.ChangeTracker.Entries();
                    while(es.Count() > 0)
                    {
                        es.First().State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    }

                    var _userManager = services.GetRequiredService<UserManager<MangoUser>>();
                    userManagerAction(_userManager);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<TestSetup>();
        }

        /// <summary>
        /// 判断用户相等的辅助方法
        /// </summary>
        /// <param name="excepted"></param>
        /// <param name="actual"></param>
        protected static void AssertUserEqual(MangoUser excepted, MangoUser actual)
        {
            Assert.Equal(excepted.UserName, actual.UserName);
            Assert.Equal(excepted.LoginName, actual.LoginName);
            Assert.Equal(excepted.Email, actual.Email);
            Assert.Equal(excepted.NormalizedEmail, actual.NormalizedEmail);
            Assert.Equal(excepted.EmailConfirmed, actual.EmailConfirmed);
            Assert.Equal(excepted.Password,actual.Password);
            Assert.Equal(excepted.LastLoginDate, actual.LastLoginDate);
            Assert.Equal(excepted.CreateDate, actual.CreateDate);
        }
    }
}
