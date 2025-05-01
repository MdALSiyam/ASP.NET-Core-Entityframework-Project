using Microsoft.EntityFrameworkCore;

namespace PracticeSP1.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options) { }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseModule> Modules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(s =>
            {
                s.Property(x => x.RegistrationFee).HasColumnType("decimal(18,2)");
            });
            modelBuilder.Seed();
        }


    }
}
