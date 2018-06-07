using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TargetFinder.Models.Target;

namespace TargetFinder.Models
{
    public class TargetFinderContext : DbContext
    {
        public TargetFinderContext (DbContextOptions<TargetFinderContext> options)
            : base(options)
        {
        }

        public DbSet<TargetFinder.Models.Target.Target> Target { get; set; }
    }
}
