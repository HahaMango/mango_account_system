using MangoAccountSystem.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public sealed class UserManagerTest : UserManagerTestBase
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="except"></param>
        [Theory]
        [MemberData(nameof(UserTestList))]
        public void CreateUserTest(string username,string email,MangoUser except,bool result)
        {
            MangoUser mangoUser = new MangoUser
            {
                UserName = username,
                Email = email
            };
            UserManagerAction(usermanager =>
            {
                var flag = usermanager.CreateAsync(mangoUser).Result;

                var user = usermanager.FindByNameAsync(username).Result;

                Assert.Equal(result, flag.Succeeded);
                AssertUserEqual(except, user);
            });
        }

        /// <summary>
        /// 根据用户名搜索测试
        /// </summary>
        [Theory]
        [MemberData(nameof(FindByNameDataList))]
        public void FindByName(string userName, MangoUser except)
        {
            UserManagerAction(usermanager =>
            {
                var user = usermanager.FindByNameAsync(userName).Result;
                
                AssertUserEqual(except, user);
            });
        }

        [Theory]
        [MemberData(nameof(UpdataDataList))]
        public void Update(string id, DateTime dateTime)
        {
            UserManagerAction(usermanager =>
            {
                var user = usermanager.FindByIdAsync(id).Result;
                user.LastLoginDate = dateTime;
                usermanager.UpdateAsync(user).Wait();

                user = usermanager.FindByIdAsync(id).Result;
                Assert.Equal(dateTime, user.LastLoginDate);
            });
        }
    }
}
