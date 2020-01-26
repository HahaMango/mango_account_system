using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MangoAccountSystem.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MangoAccountSystem.Exception;
using MangoAccountSystem.Helper;

namespace MangoAccountSystem.Dao
{
    public class MangoUserStore : IdentityUserStore
    {
        private readonly UserDbContext _userDbContext = null;

        public MangoUserStore(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public override async Task AddClaimsAsync(MangoUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if(claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }
            MangoUser mangoUser = await FindByNameAsync(user.UserName, cancellationToken);
            int userid = mangoUser.Id;
            using (var trans = _userDbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (Claim claim in claims)
                    {
                        UserClaimEntity claimEntity = new UserClaimEntity
                        {
                            UserId = userid,
                            ClaimType = claim.Type,
                            ClaimValue = claim.Value
                        };
                        await _userDbContext.AddAsync(claimEntity);
                    }
                    await _userDbContext.SaveChangesAsync();

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public override async Task AddToRoleAsync(MangoUser user, string roleName, CancellationToken cancellationToken)
        {
            var role = await _userDbContext.MangoUserRoles.Where(r => r.RoleName == roleName).SingleOrDefaultAsync();
            if (role == null)
            {
                throw new RoleNotFoundException($"没有找到角色为：{roleName}的对象");
            }
            var mangouser = await FindByNameEntityAsync(user.UserName, cancellationToken);
            User2Role u2r = new User2Role
            {
                UserId = mangouser.Id,
                RoleId = role.Id
            };
            await _userDbContext.User2Roles.AddAsync(u2r);
            await _userDbContext.SaveChangesAsync();
        }

        public override async Task<IdentityResult> CreateAsync(MangoUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }
            try
            {
                await _userDbContext.MangoUsers.AddAsync(MEConversion.UserM2E(user));
                await _userDbContext.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                return IdentityResult.Failed(new IdentityError { Description = e.Message });
            }
            return IdentityResult.Success;
        }

        public override async Task<IdentityResult> DeleteAsync(MangoUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }
            try
            {
                var mangouser = await FindByNameEntityAsync(user.LoginName, cancellationToken);
                if (mangouser == null)
                {
                    throw new UserNotFoundException(user.UserName);
                }
                _userDbContext.Remove(mangouser);
                await _userDbContext.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                return IdentityResult.Failed(new IdentityError { Description = e.Message });
            }
            return IdentityResult.Success;
        }

        public override void Dispose()
        {            
        }

        public override async Task<MangoUser> FindByIdAsync(int userId, CancellationToken cancellationToken)
        {
            UserEntity userEntity = await _userDbContext.MangoUsers.Where(u => u.Id == userId).SingleOrDefaultAsync();
            if (userEntity == null)
            {
                return null;
            }
            return MEConversion.UserE2M(userEntity);
        }

        /// <summary>
        /// 输入参数的username经过UserManger的格式化处理
        /// </summary>
        /// <param name="loginName">用户名</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<MangoUser> FindByNameAsync(string username, CancellationToken cancellationToken)
        {
            UserEntity userEntity = await _userDbContext.MangoUsers.Where(u => u.LoginName == username).SingleOrDefaultAsync();

            if(userEntity == null)
            {
                return null;
            }
            return MEConversion.UserE2M(userEntity);
        }

        public override async Task<IList<Claim>> GetClaimsAsync(MangoUser user, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(MangoUser));
            }
            UserEntity userEntity = await FindByNameEntityAsync(user.LoginName,cancellationToken);

            IList<UserClaimEntity> userClaimEntities = await _userDbContext.MangoUserClaims.Where(c => c.UserId == userEntity.Id).ToListAsync();
            IList<Claim> mangoUserClaims = new List<Claim>();
            foreach(UserClaimEntity claimEntity in userClaimEntities)
            {
                Claim claim = new Claim(claimEntity.ClaimType, claimEntity.ClaimValue);
                mangoUserClaims.Add(claim);
            }
            return mangoUserClaims;
        }

        public override async Task<string> GetLoginNameAsync(MangoUser user, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                throw new ArgumentNullException();
            }
            if(user.LoginName == null)
            {
                return user.LoginName;
            }
            var mangouser = await FindByIdAsync(user.Id, cancellationToken);
            return mangouser.LoginName;
        }

        public override async Task<string> GetPasswordHashAsync(MangoUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }
            UserEntity userEntity = await FindByNameEntityAsync(user.UserName, cancellationToken);
            if(userEntity == null)
            {
                return null;
            }
            return userEntity.Password;
        }

        public override async Task<IList<string>> GetRolesAsync(MangoUser user, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                throw new ArgumentNullException();
            }
            UserEntity userEntity = await FindByNameEntityAsync(user.LoginName, cancellationToken);

            IList<string> roles = new List<string>();
            var roleentities = await _userDbContext.MangoUserRoles.Join(_userDbContext.User2Roles, r => r.Id, u2r => u2r.RoleId, (r, u2r) => r.RoleName).ToListAsync();
            return roleentities;
        }

        public override async Task<string> GetUserIdAsync(MangoUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }
            UserEntity userEntity = await FindByNameEntityAsync(user.UserName, cancellationToken);
            if(userEntity == null)
            {
                return null;
            }
            return userEntity.Id.ToString();
        }

        public override async Task<string> GetUserNameAsync(MangoUser user, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                throw new ArgumentNullException();
            }
            return await Task.FromResult(user.UserName);
        }

        public override Task<IList<MangoUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task<IList<MangoUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            if(roleName == null)
            {
                throw new ArgumentNullException();
            }
            IList<MangoUser> mangoUsers = new List<MangoUser>();
            var userEntities = await _userDbContext.MangoUsers.Join(_userDbContext.User2Roles, ue => ue.Id, u2r => u2r.UserId, (ue, u2r) => new UserEntity
            {
                Id = ue.Id,
                Email = ue.Email,
                LoginName = ue.LoginName,
                UserName = ue.UserName,
                CreateDate = ue.CreateDate,
                LastLoginDate = ue.LastLoginDate,
                Password = ue.Password
            }).ToListAsync();

            foreach(UserEntity userEntity in userEntities)
            {
                mangoUsers.Add(MEConversion.UserE2M(userEntity));
            }

            return mangoUsers;
        }

        public override async Task<bool> HasPasswordAsync(MangoUser user, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                throw new ArgumentNullException();
            }
            UserEntity userEntity = await FindByNameEntityAsync(user.UserName, cancellationToken);

            return (userEntity.Password != null) ? true : false;
        }

        public override async Task<bool> IsInRoleAsync(MangoUser user, string roleName, CancellationToken cancellationToken)
        {
            if (user == null || roleName == null)
            {
                return false;
            }

            var userentity = await FindByNameEntityAsync(user.UserName, cancellationToken);
            if (userentity == null)
            {
                return false;
            }

            int userid = userentity.Id;
            var flag = await _userDbContext.User2Roles.Where(u2r => u2r.UserId == userid).CountAsync() > 0;
            return flag;
        }

        public override async Task RemoveClaimsAsync(MangoUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(MangoUser));
            }
            if(claims == null)
            {
                throw new ArgumentNullException(nameof(IEnumerable<Claim>));
            }
            UserEntity userEntity = await FindByNameEntityAsync(user.UserName, cancellationToken);
            if(userEntity == null)
            {
                throw new UserNotFoundException(user.UserName);
            }
            int userid = userEntity.Id;
            IList<UserClaimEntity> userClaimEntities = await _userDbContext.MangoUserClaims.Where(c => c.UserId == userid).ToListAsync();
            IList<UserClaimEntity> removeList = new List<UserClaimEntity>();

            foreach(Claim claim in claims)
            {
                foreach(UserClaimEntity userClaim in userClaimEntities)
                {
                    if(claim.Type == userClaim.ClaimType && claim.Value == userClaim.ClaimValue)
                    {
                        removeList.Add(userClaim);
                    }
                }
            }

            _userDbContext.MangoUserClaims.RemoveRange(removeList);
            await _userDbContext.SaveChangesAsync();
        }

        public override async Task RemoveFromRoleAsync(MangoUser user, string roleName, CancellationToken cancellationToken)
        {
            if (user == null || roleName == null)
            {
                throw new ArgumentNullException();
            }

            var userentity = await FindByNameEntityAsync(user.UserName, cancellationToken);
            if(userentity == null)
            {
                throw new UserNotFoundException(user.UserName);
            }
            IList<User2Role> user2Roles = await _userDbContext.User2Roles.Where(u2r => u2r.UserId == userentity.Id).ToListAsync();
            _userDbContext.User2Roles.RemoveRange(user2Roles);
            await _userDbContext.SaveChangesAsync();
        }

        public override async Task ReplaceClaimAsync(MangoUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if(claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            if(newClaim == null)
            {
                throw new ArgumentNullException(nameof(newClaim));
            }

            UserEntity userEntity = await FindByNameEntityAsync(user.UserName, cancellationToken);
            if(userEntity == null)
            {
                throw new UserNotFoundException(user.UserName);
            }
            int userid = userEntity.Id;
            UserClaimEntity oldclaim = await _userDbContext.MangoUserClaims.Where(c => c.UserId == userid && c.ClaimType == claim.Type && c.ClaimValue == claim.Value).SingleOrDefaultAsync();
            if(oldclaim == null)
            {
                throw new ClaimNotFoundException(claim.Type, claim.Value);
            }
            oldclaim.ClaimType = newClaim.Type;
            oldclaim.ClaimValue = newClaim.Value;
            await _userDbContext.SaveChangesAsync();
        }

        public override async Task SetLoginNameAsync(MangoUser user, string loginName, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                throw new ArgumentNullException();
            }

            await Task.Run(() =>
            {
                user.LoginName = loginName;
            });
        }

        public override async Task SetPasswordHashAsync(MangoUser user, string passwordHash, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            await Task.Run(() =>
            {
                user.Password = passwordHash;
            });            
        }

        public override async Task SetUserNameAsync(MangoUser user, string userName, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                throw new ArgumentNullException();
            }

            await Task.Run(() =>
            {
                user.UserName = userName;
            });
        }

        public override async Task<IdentityResult> UpdateAsync(MangoUser user, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                UserEntity userEntity = await FindByNameEntityAsync(user.UserName, cancellationToken);
                userEntity.LoginName = user.LoginName;
                userEntity.UserName = user.UserName;
                userEntity.Email = user.Email;
                userEntity.CreateDate = user.CreateDate;
                userEntity.LastLoginDate = user.LastLoginDate;
                await _userDbContext.SaveChangesAsync();
            }
            catch(System.Exception e)
            {
                return IdentityResult.Failed(new IdentityError { Description = e.Message });
            }
            return IdentityResult.Success;
        }

        private async Task<UserEntity> FindByNameEntityAsync(string username, CancellationToken cancellationToken)
        {
            UserEntity userEntity = await _userDbContext.MangoUsers.Where(u => u.UserName == username).SingleOrDefaultAsync();
            return userEntity;
        }
    }
}
