using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MangoAccountSystem.Models;
using Microsoft.EntityFrameworkCore;
using MangoAccountSystem.Helper;

namespace MangoAccountSystem.Dao
{
	/// <summary>
	/// 实现自定义角色管理器
	/// </summary>
	public class MangoUserRoleStore : IdentityRoleStore
	{

		private readonly UserDbContext _userDbContext = null;

		public MangoUserRoleStore(UserDbContext userDbContext)
		{
			_userDbContext = userDbContext;
		}

		public override async Task<IdentityResult> CreateAsync(MangoUserRole role, CancellationToken cancellationToken)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}

			try
			{
				UserRoleEntity userRoleEntity = MEConversion.UserRoleM2E(role);
				await _userDbContext.MangoUserRoles.AddAsync(userRoleEntity);
				await _userDbContext.SaveChangesAsync();
			}
			catch (DbUpdateException e)
			{
				return IdentityResult.Failed(new IdentityError { Description = e.Message });
			}
			return IdentityResult.Success;
		}

		public override async Task<IdentityResult> DeleteAsync(MangoUserRole role, CancellationToken cancellationToken)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}
			try
			{
				var userrole = await _userDbContext.MangoUserRoles.Where(r => r.RoleName == role.RoleName).SingleOrDefaultAsync();
				_userDbContext.MangoUserRoles.Remove(userrole);
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

		public override async Task<MangoUserRole> FindByIdAsync(int roleId, CancellationToken cancellationToken)
		{
			var userrole = await _userDbContext.MangoUserRoles.Where(r => r.Id == roleId).SingleOrDefaultAsync();
			if (userrole == null)
			{
				return null;
			}
			return MEConversion.UserRoleE2M(userrole);
		}

		public override async Task<MangoUserRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
		{
			var userrole = await _userDbContext.MangoUserRoles.Where(r => r.RoleName == normalizedRoleName).SingleOrDefaultAsync();
			if (userrole == null)
			{
				return null;
			}
			return MEConversion.UserRoleE2M(userrole);
		}

		public override async Task<string> GetNormalizedRoleNameAsync(MangoUserRole role, CancellationToken cancellationToken)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}

			return await Task.FromResult(role.RoleName);
		}

		public override async Task<string> GetRoleNameAsync(MangoUserRole role, CancellationToken cancellationToken)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}

			return await Task.FromResult(role.RoleName);
		}

		public override async Task<string> GetRoleIdAsync(MangoUserRole role, CancellationToken cancellationToken)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}
			var userrole = await FindByNameAsync(role.RoleName, cancellationToken);
			if (userrole == null)
			{
				return null;
			}
			return userrole.Id.ToString();
		}

		public override async Task SetNormalizedRoleNameAsync(MangoUserRole role, string normalizedName, CancellationToken cancellationToken)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}
			await Task.Run(() =>
			{
				role.RoleName = normalizedName;
			});
		}

		public override async Task SetRoleNameAsync(MangoUserRole role, string roleName, CancellationToken cancellationToken)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}
			await Task.Run(() =>
			{
				role.RoleName = roleName;
			});
		}

		public override async Task<IdentityResult> UpdateAsync(MangoUserRole role, CancellationToken cancellationToken)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}

			try
			{
				var userrole = await _userDbContext.MangoUserRoles.Where(r => r.RoleName == role.RoleName).SingleOrDefaultAsync();
				_userDbContext.MangoUserRoles.Remove(userrole);
				await _userDbContext.SaveChangesAsync();
			}
			catch (DbUpdateException e)
			{
				return IdentityResult.Failed(new IdentityError { Description = e.Message });
			}
			return IdentityResult.Success;
		}
	}
}
