using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4j_Sample.Repositories;
using Neo4j_Sample.Helpers;
using Neo4j_Sample.Models;
using Neo4j.Driver.V1;

namespace Neo4j_Sample.Tests
{
    [TestClass]
    public class TestPersonRepository
    {
        Connection connection;

        [TestInitialize]
        public void Setup()
        {
            connection = new Connection
            {
                Url = "bolt://127.0.0.1:7687/",
                UserName = "neo4j",
                Password = "pwd"
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //Remove all Relationships & Nodes
            var driver = GraphDatabase.Driver(connection.Url, AuthTokens.Basic(connection.UserName, connection.Password));
            using (var session = driver.Session())
            {
                session.Run("MATCH (n) DETACH DELETE n;");
            }
        }

        [TestMethod]
        public void Add_Person()
        {
            var person = new Person { Name = "Person1", Age = 25 };
            using(var personRepository = new PersonRepository(connection))
            {
                personRepository.Add(person.Name, person.Age);
                var personFromDb = personRepository.FindByName(person.Name);
                Assert.AreEqual(person.Name, personFromDb.Name);
                Assert.AreEqual(person.Age, personFromDb.Age);
            }
        }

        [TestMethod]
        public void Update_Person()
        {
            var person = new Person { Name = "Person1", Age = 25 };
            using (var personRepository = new PersonRepository(connection))
            {
                personRepository.Add(person.Name, person.Age);

                var newAge = 30;
                personRepository.UpdateAge(person.Name, newAge);
                var personFromDb = personRepository.FindByName(person.Name);
                Assert.AreEqual(personFromDb.Age, newAge);
            }
        }
    }
}
