using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.MVVM.Model
{
    class ModContext : DbContext
    {
        public DbSet<ModModel> Mods { get; set; }
        public ModContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=modding_assistant.db");
        }
    }
}
