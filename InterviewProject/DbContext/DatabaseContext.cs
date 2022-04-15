using InterviewProject.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewProject.DbContext
{
    public class DatabaseContext:Microsoft.EntityFrameworkCore.DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Firm> Firms { get; set; }
    }
}
