using Microsoft.EntityFrameworkCore;
using Modding_Assistant.MVVM.Model;

namespace Modding_Assistant.Core.Data.Models
{
    public class ModContext(DbContextOptions<ModContext> options) : DbContext(options)
    {
        public DbSet<ModModel> Mods { get; set; }
    }
}
