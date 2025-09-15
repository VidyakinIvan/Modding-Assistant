using Microsoft.EntityFrameworkCore;

namespace Modding_Assistant.MVVM.Model
{
    public class ModContext(DbContextOptions<ModContext> options) : DbContext(options)
    {
        public DbSet<ModModel> Mods { get; set; }
    }
}
