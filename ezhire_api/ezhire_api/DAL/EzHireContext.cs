using ezhire_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.DAL;

public class EzHireContext : DbContext
{
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<RecruitmentCampaign> Campaigns { get; set; }

    protected EzHireContext()
    {
    }

    public EzHireContext(DbContextOptions options) : base(options)
    {
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    /**
     * Automatically sets `created_at` and `updated_at` columns to new timestamps
     */
    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;

            if (entry.State == EntityState.Added) entity.CreatedAt = DateTime.UtcNow;

            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}