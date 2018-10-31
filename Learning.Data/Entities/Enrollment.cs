using System;

namespace Learning.Data.Entities
{
    public class Enrollment
    {
        public Enrollment()
        {
            Student = new Student();
            Course = new Course();
        }
        public int Id { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}