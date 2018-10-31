using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Learning.Data.Repositories;
using Learning.WebApi.Models;

namespace Learning.WebApi.Controllers
{
    public class BaseApiController : ApiController
    {
        private readonly ILearningRepository _repo;
        private ModelFactory _modelFactory;

        public BaseApiController(ILearningRepository repo)
        {
            _repo = repo;
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(Request, TheRepository);
                }
                return _modelFactory;
            }
        }

        protected ILearningRepository TheRepository
        {
            get
            {
                return _repo;
            }
        }
    }
}
