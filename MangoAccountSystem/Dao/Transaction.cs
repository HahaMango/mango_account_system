using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace MangoAccountSystem.Dao
{
	public class Transaction
	{
		private readonly UserDbContext _userDbContext;

		public Transaction(UserDbContext userDbContext)
		{
			_userDbContext = userDbContext;
		}

		public IDbContextTransaction BeginTransaction()
		{
			return _userDbContext.Database.BeginTransaction();
		}

		public async Task<IDbContextTransaction> BeginTransactionAsync()
		{
			return await _userDbContext.Database.BeginTransactionAsync();
		}
	}
}
