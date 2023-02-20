using Microsoft.EntityFrameworkCore;

namespace WalletApp.Model
{
    public class DBClass : DbContext
    {
        public DBClass(DbContextOptions<DBClass> options)
      : base(options)
        { }
        public DbSet<User> Users { get; set; }
    }
}
