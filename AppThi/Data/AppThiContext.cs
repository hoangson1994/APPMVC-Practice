using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppThi.Models
{
    public class AppThiContext : DbContext
    {
        public AppThiContext (DbContextOptions<AppThiContext> options)
            : base(options)
        {
        }

        public DbSet<AppThi.Models.Employee> Employee { get; set; }
    }
}
