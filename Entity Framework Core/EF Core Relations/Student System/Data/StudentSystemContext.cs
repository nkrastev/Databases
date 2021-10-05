using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        //ctor
        //DB Set
        //connect to SQL
        //fluent API
        public StudentSystemContext()
        {

        }
        public StudentSystemContext(DbContextOptions options)
            :base(options)
        {

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Judge Fails if check is skipped
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=StudentSystem;Integrated Security=True");
            }
            base.OnConfiguring(optionsBuilder);            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Set primary key for mapping table
            modelBuilder.Entity<StudentCourse>(x =>
            {
                x.HasKey(x => new { x.CourseId, x.StudentId });
            });
            base.OnModelCreating(modelBuilder);
        }

    }
}
