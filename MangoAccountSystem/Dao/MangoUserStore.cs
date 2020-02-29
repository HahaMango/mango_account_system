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
	/// <summary>
	/// 自定义用户管理器
	/// </summary>
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
			if (claims == null)
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

		public override async Task AddLoginAsync(MangoUser user, UserLoginInfo login, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (login == null)
			{
				throw new ArgumentNullException(nameof(login));
			}

			var mangouser = await FindByNameEntityAsync(user.UserName);
			if (mangouser != null)
			{
				ExternalLoginEntity externalLogin = MEConversion.ExternalLoginM2E(login);
				externalLogin.UserId = mangouser.Id;

				await _userDbContext.ExternalLogins.AddAsync(externalLogin);
				await _userDbContext.SaveChangesAsync();
			}
		}

		public override async Task AddToRoleAsync(MangoUser user, string roleName, CancellationToken cancellationToken)
		{
			if(user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}

			var role = await _userDbContext.MangoUserRoles.Where(r => r.RoleName == roleName).SingleOrDefaultAsync();
			if (role == null)
			{
				throw new RoleNotFoundException($"没有找到角色为：{roleName}的对象");
			}
			var mangouser = await FindByNameEntityAsync(user.UserName);
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
				throw new ArgumentNullException(nameof(user));
			}
			try
			{
				await _userDbContext.MangoUsers.AddAsync(MEConversion.UserM2E(user));
				await _userDbContext.SaveChangesAsync();
			}
			catch (DbUpdateException e)
			{
				return IdentityResult.Failed(new IdentityError { Description = e.Message });
			}
			return IdentityResult.Success;
		}

		public override async Task<IdentityResult> DeleteAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			try
			{
				var mangouser = await FindByNameEntityAsync(user.LoginName);
				if (mangouser == null)
				{
					throw new UserNotFoundException(user.UserName);
				}
				_userDbContext.Remove(mangouser);
				await _userDbContext.SaveChangesAsync();
			}
			catch (DbUpdateException e)
			{
				return IdentityResult.Failed(new IdentityError { Description = e.Message });
			}
			return IdentityResult.Success;
		}

		public override void Dispose()
		{
			_userDbContext.Dispose();
		}

		public override async Task<MangoUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
		{
			if (normalizedEmail == null)
			{
				throw new ArgumentNullException(nameof(normalizedEmail));
			}

			UserEntity userEntity = await _userDbContext.MangoUsers.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
			if (userEntity == null)
			{
				return null;
			}
			return MEConversion.UserE2M(userEntity);
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

		public override async Task<MangoUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
		{
			if (loginProvider == null)
			{
				throw new ArgumentNullException(nameof(loginProvider));
			}
			if (providerKey == null)
			{
				throw new ArgumentNullException(nameof(providerKey));
			}

			ExternalLoginEntity exl = await _userDbContext.ExternalLogins.FirstOrDefaultAsync(ex => ex.LoginProvider == loginProvider && ex.ProviderKey == providerKey);
			if (exl == null)
			{
				return null;
			}
			UserEntity userEntity = await _userDbContext.MangoUsers.FirstOrDefaultAsync(ex => ex.Id == exl.UserId);
			if (userEntity == null)
			{
				throw new UserNotFoundException($"method:{nameof(FindByLoginAsync)},没有找到id为：{exl.UserId}的用户");
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

			if (userEntity == null)
			{
				return null;
			}
			return MEConversion.UserE2M(userEntity);
		}

		public override async Task<IList<Claim>> GetClaimsAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			UserEntity userEntity = await FindByNameEntityAsync(user.LoginName);

			IList<UserClaimEntity> userClaimEntities = await _userDbContext.MangoUserClaims.Where(c => c.UserId == userEntity.Id).ToListAsync();
			IList<Claim> mangoUserClaims = new List<Claim>();
			foreach (UserClaimEntity claimEntity in userClaimEntities)
			{
				Claim claim = new Claim(claimEntity.ClaimType, claimEntity.ClaimValue);
				mangoUserClaims.Add(claim);
			}
			return mangoUserClaims;
		}

		public override async Task<string> GetEmailAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			return await Task.FromResult(user.Email);
		}

		public override async Task<bool> GetEmailConfirmedAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			UserEntity userEntity = await FindByNameEntityAsync(user.UserName);
			if (userEntity == null)
			{
				return false;
			}
			return userEntity.EmailConfirmed;
		}

		public override async Task<string> GetLoginNameAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (user.LoginName == null)
			{
				return user.LoginName;
			}
			var mangouser = await FindByIdAsync(user.Id, cancellationToken);
			return mangouser.LoginName;
		}

		public override async Task<IList<UserLoginInfo>> GetLoginsAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}

			IList<UserLoginInfo> userLoginInfos;
			UserEntity userEntity = await FindByNameEntityAsync(user.UserName);
			if (userEntity == null)
			{
				return null;
			}
			var logins = await _userDbContext.ExternalLogins.Where(ex => ex.UserId == userEntity.Id).ToListAsync();
			userLoginInfos = new List<UserLoginInfo>();
			foreach (ExternalLoginEntity externalLogin in logins)
			{
				userLoginInfos.Add(MEConversion.ExternalLoginE2M(externalLogin));
			}

			return userLoginInfos;
		}

		public override async Task<string> GetNormalizedEmailAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			UserEntity userEntity = await FindByNameEntityAsync(user.UserName);
			if (userEntity == null)
			{
				return null;
			}
			return userEntity.NormalizedEmail;
		}

		public override async Task<string> GetPasswordHashAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			UserEntity userEntity = await FindByNameEntityAsync(user.UserName);
			if (userEntity == null)
			{
				return null;
			}
			return userEntity.Password;
		}

		public override async Task<IList<string>> GetRolesAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			UserEntity userEntity = await FindByNameEntityAsync(user.LoginName);

			IList<string> roles = new List<string>();
			var roleentities = await _userDbContext.User2Roles.Where(u2r => u2r.UserId == userEntity.Id).Select(u2r => u2r.RoleId).ToListAsync();
			foreach (int i in roleentities)
			{
				var role = await _userDbContext.MangoUserRoles.Where(r => r.Id == i).SingleOrDefaultAsync();
				if (role != null)
				{
					roles.Add(role.RoleName);
				}
			}
			return roles;
		}

		public override async Task<string> GetUserIdAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			UserEntity userEntity = await FindByNameEntityAsync(user.UserName);
			if (userEntity == null)
			{
				return null;
			}
			return userEntity.Id.ToString();
		}

		public override async Task<string> GetUserNameAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			return await Task.FromResult(user.UserName);
		}

		public override Task<IList<MangoUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public override async Task<IList<MangoUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
		{
			if (roleName == null)
			{
				throw new ArgumentNullException(nameof(roleName));
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

			foreach (UserEntity userEntity in userEntities)
			{
				mangoUsers.Add(MEConversion.UserE2M(userEntity));
			}

			return mangoUsers;
		}

		public override async Task<bool> HasPasswordAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			UserEntity userEntity = await FindByNameEntityAsync(user.UserName);

			return (userEntity.Password != null) ? true : false;
		}

		public override async Task<bool> IsInRoleAsync(MangoUser user, string roleName, CancellationToken cancellationToken)
		{
			if (user == null || roleName == null)
			{
				return false;
			}

			var userentity = await FindByNameEntityAsync(user.UserName);
			if (userentity == null)
			{
				return false;
			}

			int userid = userentity.Id;
			var flag = await _userDbContext.User2Roles.Where(u2r => u2r.UserId == userid).AnyAsync();
			return flag;
		}

		public override async Task RemoveClaimsAsync(MangoUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (claims == null)
			{
				throw new ArgumentNullException(nameof(claims));
			}
			UserEntity userEntity = await FindByNameEntityAsync(user.UserName);
			if (userEntity == null)
			{
				throw new UserNotFoundException(user.UserName);
			}
			int userid = userEntity.Id;
			IList<UserClaimEntity> userClaimEntities = await _userDbContext.MangoUserClaims.Where(c => c.UserId == userid).ToListAsync();
			IList<UserClaimEntity> removeList = new List<UserClaimEntity>();

			foreach (Claim claim in claims)
			{
				foreach (UserClaimEntity userClaim in userClaimEntities)
				{
					if (claim.Type == userClaim.ClaimType && claim.Value == userClaim.ClaimValue)
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
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}

			if(roleName == null)
			{
				throw new ArgumentNullException(nameof(roleName));
			}

			var userentity = await FindByNameEntityAsync(user.UserName);
			if (userentity == null)
			{
				throw new UserNotFoundException(user.UserName);
			}
			IList<User2Role> user2Roles = await _userDbContext.User2Roles.Where(u2r => u2r.UserId == userentity.Id).ToListAsync();
			_userDbContext.User2Roles.RemoveRange(user2Roles);
			await _userDbContext.SaveChangesAsync();
		}

		public override async Task RemoveLoginAsync(MangoUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (loginProvider == null)
			{
				throw new ArgumentNullException(nameof(loginProvider));
			}
			if (providerKey == null)
			{
				throw new ArgumentNullException(nameof(providerKey));
			}

			UserEntity userEntity = await FindByNameEntityAsync(user.UserName);
			if (userEntity == null)
			{
				return;
			}
			var logins = await _userDbContext.ExternalLogins.Where(ex => ex.UserId == userEntity.Id && ex.LoginProvider == loginProvider && ex.ProviderKey == providerKey).ToListAsync();
			_userDbContext.RemoveRange(logins);
			await _userDbContext.SaveChangesAsync();
		}

		public override async Task ReplaceClaimAsync(MangoUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (claim == null)
			{
				throw new ArgumentNullException(nameof(claim));
			}
			if (newClaim == null)
			{
				throw new ArgumentNullException(nameof(newClaim));
			}

			UserEntity userEntity = await FindByNameEntityAsync(user.UserName);
			if (userEntity == null)
			{
				throw new UserNotFoundException(user.UserName);
			}
			int userid = userEntity.Id;
			UserClaimEntity oldclaim = await _userDbContext.MangoUserClaims.Where(c => c.UserId == userid && c.ClaimType == claim.Type && c.ClaimValue == claim.Value).SingleOrDefaultAsync();
			if (oldclaim == null)
			{
				throw new ClaimNotFoundException(claim.Type, claim.Value);
			}
			oldclaim.ClaimType = newClaim.Type;
			oldclaim.ClaimValue = newClaim.Value;
			await _userDbContext.SaveChangesAsync();
		}

		public override async Task SetEmailAsync(MangoUser user, string email, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (email == null)
			{
				throw new ArgumentNullException(nameof(email));
			}
			await Task.Run(() =>
			{
				user.Email = email;
			});
		}

		public override async Task SetEmailConfirmedAsync(MangoUser user, bool confirmed, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			await Task.Run(() =>
			{
				user.EmailConfirmed = confirmed;
			});
		}

		public override async Task SetLoginNameAsync(MangoUser user, string loginName, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if(loginName == null)
			{
				throw new ArgumentNullException(nameof(user));
			}

			await Task.Run(() =>
			{
				user.LoginName = loginName;
			});
		}

		public override async Task SetNormalizedEmailAsync(MangoUser user, string normalizedEmail, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (normalizedEmail == null)
			{
				throw new ArgumentNullException(nameof(normalizedEmail));
			}
			await Task.Run(() =>
			{
				user.NormalizedEmail = normalizedEmail;
			});
		}

		public override async Task SetPasswordHashAsync(MangoUser user, string passwordHash, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if(passwordHash == null)
			{
				throw new ArgumentNullException(nameof(passwordHash));
			}

			await Task.Run(() =>
			{
				user.Password = passwordHash;
			});
		}

		public override async Task SetUserNameAsync(MangoUser user, string userName, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if(userName == null)
			{
				throw new ArgumentNullException(nameof(userName));
			}

			await Task.Run(() =>
			{
				user.UserName = userName;
			});
		}

		public override async Task<IdentityResult> UpdateAsync(MangoUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}

			try
			{
				UserEntity userEntity = await FindByNameEntityAsync(user.UserName);
				userEntity.LoginName = user.LoginName;
				userEntity.UserName = user.UserName;
				userEntity.Email = user.Email;
				userEntity.CreateDate = user.CreateDate;
				userEntity.LastLoginDate = user.LastLoginDate;
				await _userDbContext.SaveChangesAsync();
			}
			catch (DbUpdateException e)
			{
				return IdentityResult.Failed(new IdentityError { Description = e.Message });
			}
			return IdentityResult.Success;
		}

		private async Task<UserEntity> FindByNameEntityAsync(string username)
		{
			UserEntity userEntity = await _userDbContext.MangoUsers.FirstOrDefaultAsync(u => u.UserName == username);
			return userEntity;
		}
	}
}
