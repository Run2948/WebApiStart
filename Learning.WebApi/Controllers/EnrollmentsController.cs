﻿using Learning.Data.Entities;
using Learning.Data.Repositories;
using Learning.WebApi.Filters;
using Learning.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Learning.WebApi.Controllers
{
    public class EnrollmentsController : BaseApiController
    {
        public EnrollmentsController(ILearningRepository repo)
            : base(repo)
        {
        }

        public IEnumerable<StudentBaseModel> Get(int courseId)
        {
            IQueryable<Student> query;

            query = TheRepository.GetEnrolledStudentsInCourse(courseId).OrderBy(s => s.LastName);

            var totalCount = query.Count();

            System.Web.HttpContext.Current.Response.Headers.Add("X-InlineCount", totalCount.ToString());

            var results = query

                        .ToList()
                        .Select(s => TheModelFactory.Create(s));

            return results;
        }

        [Route("api/enrollments/{courseName}/{studentName?}")]
        public IEnumerable<StudentBaseModel> GetStudentsInfo(string courseName, string studentName = "")
        {
            IQueryable<Student> query;
            Course course = TheRepository.GetAllCourses().FirstOrDefault(c => c.Name == courseName);
            if (course == null)
            {
                return null;
            }
            query = TheRepository.GetEnrolledStudentsInCourse(course.Id).OrderBy(s => s.LastName);
            if (!string.IsNullOrWhiteSpace(studentName))
            {
                query = query.Where(s => s.FirstName == studentName);
            }
            var totalCount = query.Count();

            System.Web.HttpContext.Current.Response.Headers.Add("X-InlineCount", totalCount.ToString());

            var results = query

                        .ToList()
                        .Select(s => TheModelFactory.Create(s));

            return results;
        }

        //TODO: Apply Security Here
        [LearningAuthorize]
        public HttpResponseMessage Post(int courseId, [FromUri]string userName, [FromBody]Enrollment enrollment)
        {
            try
            {
                if (!TheRepository.CourseExists(courseId)) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find Course");

                var student = TheRepository.GetStudent(userName);
                if (student == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find Student");

                var result = TheRepository.EnrollStudentInCourse(student.Id, courseId, enrollment);

                if (result == 1)
                {
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                else if (result == 2)
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Student already enrolled in this course");
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}