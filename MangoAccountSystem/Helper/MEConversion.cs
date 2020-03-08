using MangoAccountSystem.Dao;
using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace MangoAccountSystem.Helper
{
	/// <summary>
	/// 数据库对象和业务对象之间的转换类
	/// </summary>
	public static class MEConversion
	{
		public static UserEntity UserM2E(MangoUser mangoUser)
		{
			if (mangoUser == null)
			{
				throw new ArgumentNullException(nameof(mangoUser));
			}
			return new UserEntity
			{
				Id = mangoUser.Id,
				LoginName = mangoUser.LoginName,
				UserName = mangoUser.UserName,
				Password = mangoUser.Password,
				Email = mangoUser.Email,				
				NormalizedEmail = mangoUser.NormalizedEmail,
				EmailConfirmed = mangoUser.EmailConfirmed,
				CreateDate = mangoUser.CreateDate,
				LastLoginDate = mangoUser.LastLoginDate
			};
		}

		public static UserRoleEntity UserRoleM2E(MangoUserRole mangoUserRole)
		{
			if (mangoUserRole == null)
			{
				throw new ArgumentNullException(nameof(mangoUserRole));
			}
			return new UserRoleEntity
			{
				Id = mangoUserRole.Id,
				RoleName = mangoUserRole.RoleName
			};
		}

		public static UserClaimEntity UserClaimM2E(MangoUserClaim mangoUserClaim)
		{
			if (mangoUserClaim == null)
			{
				throw new ArgumentNullException(nameof(mangoUserClaim));
			}
			return new UserClaimEntity
			{
				Id = mangoUserClaim.Id,
				UserId = mangoUserClaim.UserId,
				ClaimType = mangoUserClaim.ClaimType,
				ClaimValue = mangoUserClaim.ClaimValue
			};
		}

		public static MangoUser UserE2M(UserEntity userEntity)
		{
			if (userEntity == null)
			{
				throw new ArgumentNullException(nameof(userEntity));
			}
			return new MangoUser
			{
				Id = userEntity.Id,
				LoginName = userEntity.LoginName,
				UserName = userEntity.UserName,
				Password = userEntity.Password,
				Email = userEntity.Email,
				NormalizedEmail = userEntity.NormalizedEmail,
				EmailConfirmed = userEntity.EmailConfirmed,
				CreateDate = userEntity.CreateDate,
				LastLoginDate = userEntity.LastLoginDate
			};
		}

		public static MangoUserRole UserRoleE2M(UserRoleEntity userRoleEntity)
		{
			if (userRoleEntity == null)
			{
				throw new ArgumentNullException(nameof(userRoleEntity));
			}
			return new MangoUserRole
			{
				Id = userRoleEntity.Id,
				RoleName = userRoleEntity.RoleName
			};
		}

		public static MangoUserClaim UserClaimE2M(UserClaimEntity userClaimEntity)
		{
			if (userClaimEntity == null)
			{
				throw new ArgumentNullException(nameof(userClaimEntity));
			}
			return new MangoUserClaim
			{
				Id = userClaimEntity.Id,
				UserId = userClaimEntity.UserId,
				ClaimType = userClaimEntity.ClaimType,
				ClaimValue = userClaimEntity.ClaimValue
			};
		}

		public static ExternalLoginEntity ExternalLoginM2E(UserLoginInfo userLoginInfo)
		{
			if (userLoginInfo == null)
			{
				throw new ArgumentNullException(nameof(userLoginInfo));
			}
			return new ExternalLoginEntity
			{
				LoginProvider = userLoginInfo.LoginProvider,
				ProviderKey = userLoginInfo.ProviderKey,
				ProviderDisplayName = userLoginInfo.ProviderDisplayName
			};
		}

		public static UserLoginInfo ExternalLoginE2M(ExternalLoginEntity externalLoginEntity)
		{
			if (externalLoginEntity == null)
			{
				throw new ArgumentNullException(nameof(externalLoginEntity));
			}
			return new UserLoginInfo(externalLoginEntity.LoginProvider, externalLoginEntity.ProviderKey, externalLoginEntity.ProviderDisplayName);
		}
	}
}
