using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MangoAccountSystem.Dao
{
    public class MangoUserStore : IdentityUserStore
    {
        public override Task AddClaimsAsync(MangoUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task AddToRoleAsync(MangoUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityResult> CreateAsync(MangoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityResult> DeleteAsync(MangoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override Task<MangoUser> FindByIdAsync(int userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<MangoUser> FindByNameAsync(string loginName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<Claim>> GetClaimsAsync(MangoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<string> GetLoginNameAsync(MangoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<string> GetPasswordHashAsync(MangoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<string>> GetRolesAsync(MangoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<int> GetTheUserIdAsync(MangoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<string> GetUserNameAsync(MangoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<MangoUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<MangoUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> HasPasswordAsync(MangoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> IsInRoleAsync(MangoUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task RemoveClaimsAsync(MangoUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task RemoveFromRoleAsync(MangoUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task ReplaceClaimAsync(MangoUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task SetLoginNameAsync(MangoUser user, string loginName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task SetPasswordHashAsync(MangoUser user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task SetUserNameAsync(MangoUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityResult> UpdateAsync(MangoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
