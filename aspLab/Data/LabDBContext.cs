using Microsoft.EntityFrameworkCore;
using aspLab.Models;

namespace LabDB.Data
{
public class LabDBContext: DbContext
{
        public LabDBContext(DbContextOptions <LabDBContext> options)
: base(options)
{
        }
        public DbSet<UserException> UserException{ get; set; }
        public DbSet<aspLab.Models.AnalisReport> AnalisReport { get; set; }
    }
}