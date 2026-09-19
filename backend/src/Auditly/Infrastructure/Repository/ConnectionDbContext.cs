using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    internal class ConnectionDbContext : DbContext
    {
        public ConnectionDbContext(DbContextOptions<ConnectionDbContext> options) : base(options)
        {

        }
    }
}
