using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MangoAccountSystem.Dao
{
    public abstract class IdentityUserStore : IUserRoleStore<MangoUser>, IUserPasswordStore<MangoUser>, IUserClaimStore<MangoUser>
    {

        #region 接口适配

        public Task<MangoUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return FindByIdAsync(String2Id(userId), cancellationToken);
        }

        public abstract Task<MangoUser> FindByIdAsync(int userId, CancellationToken cancellationToken);

        public abstract Task<string> GetUserIdAsync(MangoUser user, CancellationToken cancellationToken);

        public Task<string> GetNormalizedUserNameAsync(MangoUser user, CancellationToken cancellationToken)
        {
            return GetLoginNameAsync(user, cancellationToken);
        }

        public abstract Task<string> GetLoginNameAsync(MangoUser user, CancellationToken cancellationToken);

        public Task SetNormalizedUserNameAsync(MangoUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return SetLoginNameAsync(user, normalizedName, cancellationToken);
        }

        public abstract Task SetLoginNameAsync(MangoUser user, string loginName, CancellationToken cancellationToken);

        #endregion

        #region 抽象方法

        public abstract Task AddClaimsAsync(MangoUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken);

        public abstract Task AddToRoleAsync(MangoUser user, string roleName, CancellationToken cancellationToken);

        public abstract Task<IdentityResult> CreateAsync(MangoUser user, CancellationToken cancellationToken);

        public abstract Task<IdentityResult> DeleteAsync(MangoUser user, CancellationToken cancellationToken);

        public abstract void Dispose();

        public abstract Task<MangoUser> FindByNameAsync(string loginName, CancellationToken cancellationToken);

        public abstract Task<IList<Claim>> GetClaimsAsync(MangoUser user, CancellationToken cancellationToken);       

        public abstract Task<string> GetPasswordHashAsync(MangoUser user, CancellationToken cancellationToken);

        public abstract Task<IList<string>> GetRolesAsync(MangoUser user, CancellationToken cancellationToken);

        public abstract Task<string> GetUserNameAsync(MangoUser user, CancellationToken cancellationToken);

        public abstract Task<IList<MangoUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken);

        public abstract Task<IList<MangoUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken);

        public abstract Task<bool> HasPasswordAsync(MangoUser user, CancellationToken cancellationToken);

        public abstract Task<bool> IsInRoleAsync(MangoUser user, string roleName, CancellationToken cancellationToken);

        public abstract Task RemoveClaimsAsync(MangoUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken);

        public abstract Task RemoveFromRoleAsync(MangoUser user, string roleName, CancellationToken cancellationToken);

        public abstract Task ReplaceClaimAsync(MangoUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken);

        public abstract Task SetPasswordHashAsync(MangoUser user, string passwordHash, CancellationToken cancellationToken);

        public abstract Task SetUserNameAsync(MangoUser user, string userName, CancellationToken cancellationToken);

        public abstract Task<IdentityResult> UpdateAsync(MangoUser user, CancellationToken cancellationToken);

        #endregion

        #region id类型转换辅助方法

        private string Id2String(int id)
        {
            return id.ToString();
        }

        private int String2Id(string id)
        {
            int result = 0;
            try
            {
                result = int.Parse(id);
            }
            catch
            {
                throw;
            }
            return result;
        }

        #endregion
    }
}
