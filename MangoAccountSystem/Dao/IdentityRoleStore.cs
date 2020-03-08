using MangoAccountSystem.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MangoAccountSystem.Dao
{
	/// <summary>
	/// 自定义Identity的角色管理器储存逻辑，自定义表结构
	/// </summary>
	public abstract class IdentityRoleStore : IRoleStore<MangoUserRole>
	{

		#region 接口适配方法

		public Task<MangoUserRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
		{
			return FindByIdAsync(String2Id(roleId), cancellationToken);
		}

		public abstract Task<MangoUserRole> FindByIdAsync(int roleId, CancellationToken cancellationToken);

		public abstract Task<string> GetRoleIdAsync(MangoUserRole role, CancellationToken cancellationToken);

		#endregion

		#region 抽象方法

		public abstract Task<IdentityResult> CreateAsync(MangoUserRole role, CancellationToken cancellationToken);

		public abstract Task<IdentityResult> DeleteAsync(MangoUserRole role, CancellationToken cancellationToken);

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected abstract void Dispose(bool disposing);

        public abstract Task<MangoUserRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken);

        public abstract Task<string> GetNormalizedRoleNameAsync(MangoUserRole role, CancellationToken cancellationToken);

        public abstract Task<string> GetRoleNameAsync(MangoUserRole role, CancellationToken cancellationToken);

        public abstract Task SetNormalizedRoleNameAsync(MangoUserRole role, string normalizedName, CancellationToken cancellationToken);

        public abstract Task SetRoleNameAsync(MangoUserRole role, string roleName, CancellationToken cancellationToken);

        public abstract Task<IdentityResult> UpdateAsync(MangoUserRole role, CancellationToken cancellationToken);

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
