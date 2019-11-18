using Microsoft.EntityFrameworkCore;

namespace MangoAccountSystem.Dao
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions dbContextOptionsBuilder) : base(dbContextOptionsBuilder)
        {

        }
    }
}
