using System.Data.Entity;
using Learning.Data.Entities;
using Learning.Data.Mappers;
using Learning.Data.Migrations;

namespace Learning.Data
{
    public class LearningContext : DbContext
    {
        public LearningContext() :
            base("LearningConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LearningContext, Configuration>());
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Tutor> Tutors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new StudentMapper());
            modelBuilder.Configurations.Add(new SubjectMapper());
            modelBuilder.Configurations.Add(new TutorMapper());
            modelBuilder.Configurations.Add(new CourseMapper());
            modelBuilder.Configurations.Add(new EnrollmentMapper());

            base.OnModelCreating(modelBuilder);
        }
    }
}