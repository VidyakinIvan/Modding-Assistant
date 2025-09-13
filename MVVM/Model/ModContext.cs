using Microsoft.EntityFrameworkCore;

namespace Modding_Assistant.MVVM.Model
{
    public class ModContext : DbContext
    {
        public DbSet<ModModel> Mods { get; set; }
        public ModContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=modding_assistant.db");
        }
    }
}
