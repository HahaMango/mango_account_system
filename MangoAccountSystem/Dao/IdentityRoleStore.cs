using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MangoAccountSystem.Dao
{
    public abstract class IdentityRoleStore : IRoleStore<MangoUserRole>
    {

        #region 接口适配方法

        public Task<MangoUserRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(MangoUserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 抽象方法

        public Task<IdentityResult> CreateAsync(MangoUserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(MangoUserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<MangoUserRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(MangoUserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(MangoUserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(MangoUserRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(MangoUserRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(MangoUserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

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
