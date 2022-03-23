using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace Bytewizer.Huxley.Api.Models
{
    class HuxleyContext : DbContext
    {
        public HuxleyContext(DbContextOptions<HuxleyContext> options) 
            : base(options) { }    
        public DbSet<DeviceModel> Devices => Set<DeviceModel>();
    }
}
