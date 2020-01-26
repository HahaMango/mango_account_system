using MangoAccountSystem.Dao;
using MangoAccountSystem.Models;

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
    }
}
