using InterviewProject.IdentityAuth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewProject.DbContext
{
    public class ApplicationDatabaseContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
