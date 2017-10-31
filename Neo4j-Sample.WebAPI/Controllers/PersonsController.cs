using Neo4j_Sample.Models;
using Neo4j_Sample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Neo4j_Sample.WebAPI.Controllers
{
    public class PersonsController : ApiController
    {
        private PersonRepository repo;

        public PersonsController(PersonRepository repo)
        {
            this.repo = repo;
        }

        public Person Get()
        {
            return repo.FindByName("1");
        }
    }
}
