using Microsoft.EntityFrameworkCore;

namespace PracticeSP1.Models
{
    public static class ModelBuilderExtention
    {
        public static void Seed(this ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1, CourseName = "C3" },
                new Course { CourseId = 2, CourseName = "NT" },
                new Course { CourseId = 3, CourseName = "WADA" },
                new Course { CourseId = 4, CourseName = "Web" }
                );
        }

    }
}
