using Microsoft.EntityFrameworkCore;
using MinimalAPIPostgresSQL.Models;

namespace MinimalAPIPostgresSQL.Data
{
    public class OfficeDb : DbContext
    {
        //Representacion de la tabla en la base de datos
        public OfficeDb(DbContextOptions<OfficeDb> options) : base(options) {
        }
        public DbSet<Employer> Employers => Set<Employer>();
    }
}
