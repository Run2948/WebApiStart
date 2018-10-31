﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Learning.Data;
using Learning.Data.Entities;
using Learning.Data.Repositories;
using Learning.WebApi.Filters;
using Learning.WebApi.Models;

namespace Learning.WebApi.Controllers
{
    public class StudentsV2Controller : BaseApiController
    {

        public StudentsV2Controller(ILearningRepository repo)
            : base(repo)
        {
        }

        public IEnumerable<StudentV2BaseModel> Get(int page = 0, int pageSize = 10)
        {
            IQueryable<Student> query;

            query = TheRepository.GetAllStudentsWithEnrollments().OrderBy(c => c.LastName);

            var totalCount = query.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);

            var urlHelper = new UrlHelper(Request);
            var prevLink = page > 0 ? urlHelper.Link("Students", new { page = page - 1, pageSize = pageSize }) : "";
            var nextLink = page < totalPages - 1 ? urlHelper.Link("Students", new { page = page + 1, pageSize = pageSize }) : "";

            var paginationHeader = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PrevPageLink = prevLink,
                NextPageLink = nextLink
            };

            System.Web.HttpContext.Current.Response.Headers.Add("X-Pagination",Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

            var results = query
                        .Skip(pageSize * page)
                        .Take(pageSize)
                        .ToList()
                        .Select(s => TheModelFactory.CreateV2Summary(s));

            return results;
        }

        [LearningAuthorize]
        public HttpResponseMessage Get(string userName)
        {
            try
            {
                var student = TheRepository.GetStudentEnrollments(userName);
                if (student != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(student));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage Post([FromBody] Student student)
        {

            try
            {

                TheRepository.Insert(student);

                if (TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(student));
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not save to the database.");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpPatch]
        [HttpPut]
        public HttpResponseMessage Put(string userName, [FromBody] Student student)
        {
            try
            {

                var originalStudent = TheRepository.GetStudent(userName);

                if (originalStudent == null || originalStudent.UserName != userName)
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Studnet is not found");
                }
                else
                {
                    student.Id = originalStudent.Id;
                }

                if (TheRepository.Update(originalStudent, student) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(student));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(string userName)
        {
            try
            {
                var student = TheRepository.GetStudent(userName);

                if (student == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                if (student.Enrollments.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Can not delete student, student has enrollments in courses.");
                }

                if (TheRepository.DeleteStudent(student.Id) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}
