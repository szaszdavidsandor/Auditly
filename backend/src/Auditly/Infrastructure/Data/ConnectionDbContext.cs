using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Infrastructure.Data
{
    public class ConnectionDbContext : DbContext
    {
        public ConnectionDbContext(DbContextOptions<ConnectionDbContext> options) : base(options)
        {

        }
        
    }
}
