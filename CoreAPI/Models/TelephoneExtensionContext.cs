using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Models
{
    public class TelephoneExtensionContext : DbContext
    {
        public TelephoneExtensionContext(DbContextOptions<TelephoneExtensionContext> options) : base(options) { }
        public DbSet<TelephoneExtensionItem> TelephoneItems { get; set; }
    }
}
