using MangoAccountSystem.Dao;
using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace MangoAccountSystem.Helper
{
    public static class MEConversion
    {
        public static UserEntity UserM2E(MangoUser mangoUser)
        {
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
            return new UserRoleEntity
            {
                Id = mangoUserRole.Id,
                RoleName = mangoUserRole.RoleName
            };
        }

        public static UserClaimEntity UserClaimM2E(MangoUserClaim mangoUserClaim)
        {
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
            return new MangoUser
            {
                Id = userEntity.Id,
                LoginName = userEntity.LoginName,
                UserName = userEntity.UserName,
                Password = null,
                Email = userEntity.Email,
                NormalizedEmail = userEntity.NormalizedEmail,
                EmailConfirmed = userEntity.EmailConfirmed,
                CreateDate = userEntity.CreateDate,
                LastLoginDate = userEntity.LastLoginDate
            };
        }

        public static MangoUserRole UserRoleE2M(UserRoleEntity userRoleEntity)
        {
            return new MangoUserRole
            {
                Id = userRoleEntity.Id,
                RoleName = userRoleEntity.RoleName
            };
        }

        public static MangoUserClaim UserClaimE2M(UserClaimEntity userClaimEntity)
        {
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
            return new ExternalLoginEntity
            {
                LoginProvider = userLoginInfo.LoginProvider,
                ProviderKey = userLoginInfo.ProviderKey,
                ProviderDisplayName = userLoginInfo.ProviderDisplayName
            };
        }

        public static UserLoginInfo ExternalLoginE2M(ExternalLoginEntity externalLoginEntity)
        {
            return new UserLoginInfo(externalLoginEntity.LoginProvider, externalLoginEntity.ProviderKey, externalLoginEntity.ProviderDisplayName);
        }
    }
}
