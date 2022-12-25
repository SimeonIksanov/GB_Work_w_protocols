using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.Data;
public class ClinicServiceDbContext : IdentityDbContext<User>
{
    public ClinicServiceDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Consultation> Consultations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Consultation>()
            .HasOne<Pet>(c => c.Pet).WithMany(b => b.Consultations)
            .HasForeignKey(p => p.PetId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
