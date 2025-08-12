using CareerBuilder.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CareerBuilder.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<JobPosting> JobPostings { get; set; }
    public DbSet<Certification> Certifications { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<WorkExperience> WorkExperiences { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<ContactInformation> ContactInformations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure ApplicationUser
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure JobPosting
        builder.Entity<JobPosting>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(4000).IsRequired();
            entity.Property(e => e.Location).HasMaxLength(100).IsRequired();
            entity.Property(e => e.CompanyName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.JobType).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Requirements).HasMaxLength(3000);
            entity.Property(e => e.Benefits).HasMaxLength(2000);
            entity.Property(e => e.SalaryMin).HasColumnType("decimal(18,2)");
            entity.Property(e => e.SalaryMax).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.User)
                  .WithMany(u => u.JobPostings)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<WorkExperience>()
       .HasOne(w => w.Resume)
       .WithMany(r => r.Experiences)
       .HasForeignKey(w => w.ResumeId);
    }
}
