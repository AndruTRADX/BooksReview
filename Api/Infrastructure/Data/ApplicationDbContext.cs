using Core.Common;
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<BookGenre> BookGenres => Set<BookGenre>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ReviewReport> ReviewReports => Set<ReviewReport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Review>()
            .Property(r => r.Status)
            .HasConversion<string>();
        
        modelBuilder.Entity<ReviewReport>()
            .Property(rr => rr.Status)
            .HasConversion<string>();
        
        modelBuilder.Entity<User>()
            .Property(u => u.Status)
            .HasConversion<string>();

        modelBuilder.Entity<BookGenre>()
            .HasKey(bg => new { bg.BookId, bg.GenreId });

        modelBuilder.Entity<BookGenre>()
            .HasOne(bg => bg.Book)
            .WithMany(b => b.BookGenres)
            .HasForeignKey(bg => bg.BookId);

        modelBuilder.Entity<BookGenre>()
            .HasOne(bg => bg.Genre)
            .WithMany(g => g.BookGenres)
            .HasForeignKey(bg => bg.GenreId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Book)
            .WithMany(b => b.Reviews)
            .HasForeignKey(r => r.BookId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<ReviewReport>()
            .HasOne(rr => rr.Review)
            .WithMany(r => r.Reports)
            .HasForeignKey(rr => rr.ReviewId);

        modelBuilder.Entity<ReviewReport>()
            .HasOne(rr => rr.ReportedByUser)
            .WithMany()
            .HasForeignKey(rr => rr.ReportedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Book>()
            .HasIndex(b => b.ISBN)
            .IsUnique()
            .HasFilter("[ISBN] IS NOT NULL");

        modelBuilder.Entity<Genre>()
            .HasIndex(g => g.Name)
            .IsUnique();

        modelBuilder.Entity<Review>()
            .HasIndex(r => new { r.BookId, r.UserId })
            .IsUnique();

        modelBuilder.Entity<Book>().Property<DateTime>("CreatedAt").HasDefaultValueSql("GETUTCDATE()");
        modelBuilder.Entity<Genre>().Property<DateTime>("CreatedAt").HasDefaultValueSql("GETUTCDATE()");
        modelBuilder.Entity<Review>().Property<DateTime>("CreatedAt").HasDefaultValueSql("GETUTCDATE()");
        modelBuilder.Entity<ReviewReport>().Property<DateTime>("CreatedAt").HasDefaultValueSql("GETUTCDATE()");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && 
                (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}