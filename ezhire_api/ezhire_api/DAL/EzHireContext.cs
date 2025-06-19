using ezhire_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.DAL;

public class EzHireContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<RecruitmentCampaign> Campaigns { get; set; }
    public DbSet<JobPosting> JobPostings { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<RecruitmentStage> RecruitmentStages { get; set; }
    public DbSet<TechnicalMeeting> TechnicalMeetings { get; set; }
    public DbSet<TeamMeeting> TeamMeetings { get; set; }
    public DbSet<CultureMeeting> CultureMeetings { get; set; }
    public DbSet<RecruitmentStageMeeting> RecruitmentStageMeetings { get; set; }
    public DbSet<Offer> Offers { get; set; }
    
    
    public DbSet<User> Users { get; set; }
    public DbSet<HiringManager> HiringManagers { get; set; }
    public DbSet<Recruiter> Recruiters { get; set; }

    protected EzHireContext()
    {
    }

    public EzHireContext(DbContextOptions<EzHireContext> options) : base(options)
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