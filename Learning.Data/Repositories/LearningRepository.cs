using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learning.Data.Entities;

namespace Learning.Data.Repositories
{
    public class LearningRepository : ILearningRepository
    {
        private readonly LearningContext _ctx;
        public LearningRepository(LearningContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<Subject> GetAllSubjects()
        {
            return _ctx.Subjects
                    .AsQueryable();
        }

        public Subject GetSubject(int subjectId)
        {
            return _ctx.Subjects.Find(subjectId);
        }

        public IQueryable<Course> GetCoursesBySubject(int subjectId)
        {
            return _ctx.Courses
                    .Include("CourseSubject")
                    .Include("CourseTutor")
                    .Where(c => c.CourseSubject.Id == subjectId)
                    .AsQueryable();
        }

        public IQueryable<Course> GetAllCourses()
        {
            return _ctx.Courses
                    .Include("CourseSubject")
                    .Include("CourseTutor")
                    .AsQueryable();
        }

        public Course GetCourse(int courseId)
        {
            return _ctx.Courses
                     .Include("Enrollments")
                    .Include("Enrollments.Student")
                    .Include("CourseSubject")
                    .Include("CourseTutor")
                    .SingleOrDefault(c => c.Id == courseId);

        }

        public bool CourseExists(int courseId)
        {
            return _ctx.Courses.Any(c => c.Id == courseId);
        }

        public IQueryable<Student> GetAllStudentsWithEnrollments()
        {
            return _ctx.Students
                    .Include("Enrollments")
                    .Include("Enrollments.Course")
                     .Include("Enrollments.Course.CourseSubject")
                     .Include("Enrollments.Course.CourseTutor")
                    .AsQueryable();
        }

        public IQueryable<Student> GetAllStudentsSummary()
        {
            return _ctx.Students
                    .AsQueryable();
        }

        public Student GetStudentEnrollments(string userName)
        {
            var student = _ctx.Students
                           .Include("Enrollments")
                           .Include("Enrollments.Course")
                           .Include("Enrollments.Course.CourseSubject")
                           .Include("Enrollments.Course.CourseTutor")
                           .SingleOrDefault(s => s.UserName == userName);

            return student;
        }

        public Student GetStudent(string userName)
        {
            var student = _ctx.Students
                           .Include("Enrollments")
                           .SingleOrDefault(s => s.UserName == userName);

            return student;
        }

        public IQueryable<Student> GetEnrolledStudentsInCourse(int courseId)
        {
            return _ctx.Students
                    .Include("Enrollments")
                    .Where(c => c.Enrollments.Any(s => s.Course.Id == courseId))
                    .AsQueryable();

        }

        public Tutor GetTutor(int tutorId)
        {
            return _ctx.Tutors.Find(tutorId);
        }

        public int EnrollStudentInCourse(int studentId, int courseId, Enrollment enrolment)
        {
            try
            {
                if (_ctx.Enrollments.Any(e => e.Course.Id == courseId && e.Student.Id == studentId))
                {
                    return 2;
                }

                _ctx.Database.ExecuteSqlCommand("INSERT INTO Enrollments VALUES (@p0, @p1, @p2)",
                    enrolment.EnrollmentDate, courseId.ToString(), studentId.ToString());

                return 1;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbex)
            {
                foreach (var eve in dbex.EntityValidationErrors)
                {
                    string line =
                        $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:";

                    foreach (var ve in eve.ValidationErrors)
                    {
                        line = $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"";

                    }
                }
                return 0;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool LoginStudent(string userName, string password)
        {
            var student = _ctx.Students.SingleOrDefault(s => s.UserName == userName);

            if (student != null)
            {
                if (student.Password == password)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Insert(Student student)
        {
            try
            {
                _ctx.Students.Add(student);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Student originalStudent, Student updatedStudent)
        {
            _ctx.Entry(originalStudent).CurrentValues.SetValues(updatedStudent);
            return true;
        }

        public bool DeleteStudent(int id)
        {
            try
            {
                var entity = _ctx.Students.Find(id);
                if (entity != null)
                {
                    _ctx.Students.Remove(entity);
                    return true;
                }
            }
            catch
            {
                // TODO Logging
            }

            return false;
        }

        public bool Insert(Course course)
        {
            try
            {
                _ctx.Courses.Add(course);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Course originalCourse, Course updatedCourse)
        {
            _ctx.Entry(originalCourse).CurrentValues.SetValues(updatedCourse);
            //To update child entites in Course entity
            originalCourse.CourseSubject = updatedCourse.CourseSubject;
            originalCourse.CourseTutor = updatedCourse.CourseTutor;

            return true;
        }

        public bool DeleteCourse(int id)
        {
            try
            {
                var entity = _ctx.Courses.Find(id);
                if (entity != null)
                {
                    _ctx.Courses.Remove(entity);
                    return true;
                }
            }
            catch
            {
                //ToDo: Logging
            }

            return false;
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }
    }
}
