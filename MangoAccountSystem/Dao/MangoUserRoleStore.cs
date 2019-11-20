using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MangoAccountSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace MangoAccountSystem.Dao
{
    public class MangoUserRoleStore : IRoleStore<MangoUserRole>
    {

        private readonly UserDbContext _userDbContext = null;

        public MangoUserRoleStore(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<IdentityResult> CreateAsync(MangoUserRole role, CancellationToken cancellationToken)
        {
            int result = 0;

            try
            {
                _userDbContext.MangoUserRoles.Add(new UserRoleEntity
                {
                    Id = role.Id,
                    RoleName = role.RoleName
                });

                result = await _userDbContext.SaveChangesAsync();
            }catch(Exception e)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"添加角色失败，错误内容{e.Message}" });
            }
            return (result > 0) ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "添加角色失败"});
        }

        public async Task<IdentityResult> DeleteAsync(MangoUserRole role, CancellationToken cancellationToken)
        {
            int result = 0;

            try
            {
                _userDbContext.MangoUserRoles.Remove(new UserRoleEntity
                {
                    Id = role.Id,
                    RoleName = role.RoleName
                });

                result = await _userDbContext.SaveChangesAsync();
            }catch(Exception e)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"删除角色失败，错误内容{e.Message}" });
            }

            return (result > 0) ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "删除角色失败" });
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<MangoUserRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _userDbContext.MangoUserRoles.Where(r => r.Id == int.Parse(roleId)).SingleOrDefaultAsync();
                if (role == null)
                {
                    return null;
                }
                else
                {
                    return new MangoUserRole
                    {
                        Id = role.Id,
                        RoleName = role.RoleName
                    };
                }
            }catch(Exception e)
            {
                throw new ApplicationException();
            }
        }

        public async Task<MangoUserRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _userDbContext.MangoUserRoles.Where(r => r.RoleName == normalizedRoleName).SingleOrDefaultAsync();
                if (role == null)
                {
                    return null;
                }
                else
                {
                    return new MangoUserRole
                    {
                        Id = role.Id,
                        RoleName = role.RoleName
                    };
                }
            }catch(Exception e)
            {
                throw new ApplicationException();
            }
        }

        public async Task<string> GetNormalizedRoleNameAsync(MangoUserRole role, CancellationToken cancellationToken)
        {
            if(role.RoleName != null)
            {
                return role.RoleName;
            }
            try
            {
                var resultRole = await _userDbContext.MangoUserRoles.Where(r => r.Id == (role.Id)).SingleOrDefaultAsync();
                return resultRole.RoleName;
            }
            catch(Exception e)
            {
                throw new ApplicationException();
            }
        }

        public async Task<string> GetRoleIdAsync(MangoUserRole role, CancellationToken cancellationToken)
        {
            if (role.Id != 0)
            {
                return role.Id.ToString();
            }
            try
            {
                var resultRole = await _userDbContext.MangoUserRoles.Where(r => r.RoleName == (role.RoleName)).SingleOrDefaultAsync();
                return resultRole.Id.ToString();
            }
            catch (Exception e)
            {
                throw new ApplicationException();
            }
        }

        public async Task<string> GetRoleNameAsync(MangoUserRole role, CancellationToken cancellationToken)
        {
            return await GetNormalizedRoleNameAsync(role, cancellationToken);
        }

        public async Task SetNormalizedRoleNameAsync(MangoUserRole role, string normalizedName, CancellationToken cancellationToken)
        {
            try
            {
                var resultRole = await _userDbContext.MangoUserRoles.Where(r => r.Id == role.Id).SingleOrDefaultAsync();
                resultRole.RoleName = normalizedName;
                await _userDbContext.SaveChangesAsync();
            }catch(Exception e)
            {
                throw new ApplicationException();
            }
        }

        public async Task SetRoleNameAsync(MangoUserRole role, string roleName, CancellationToken cancellationToken)
        {
            await SetNormalizedRoleNameAsync(role, roleName, cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(MangoUserRole role, CancellationToken cancellationToken)
        {
            try
            {
                var resultRole = await _userDbContext.MangoUserRoles.Where(r => r.Id == role.Id).SingleOrDefaultAsync();
                resultRole.RoleName = role.RoleName;
                await _userDbContext.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch(Exception e)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"更新失败，错误内容{e.Message}"});
            }
        }       
    }
}
