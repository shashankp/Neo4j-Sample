using Neo4j.Driver.V1;
using Neo4j_Sample.Helpers;
using Neo4j_Sample.Models;
using System;
using System.Linq;

namespace Neo4j_Sample.Repositories
{
    public class PersonRepository : IDisposable
    {
        private IDriver _driver;

        public PersonRepository(Connection connection)
        {
            _driver = GraphDatabase.Driver(connection.Url, AuthTokens.Basic(connection.UserName, connection.Password));
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }

        public void Add(string name, int age)
        {
            string query = $"CREATE (n:Person {{ name:'{name}', age: {age} }});";
            using (var session = _driver.Session())
            {
                session.Run(query);
            }
        }

        public Person FindByName(string name)
        {
            string query = $"MATCH (p {{name:'{name}'}}) RETURN p.name, p.age;";
            using (var session = _driver.Session())
            {
                try
                {
                    var resultRecord = session.Run(query).Single();
                    return new Person { Name = resultRecord[0].ToString(), Age = Convert.ToInt32(resultRecord[1]) };
                }
                catch
                {

                }
                return null;
            }
        }

        public void UpdateAge(string name, int newAge)
        {
            string query = $"MATCH (p {{name:'{name}'}}) SET p.age = {newAge};";
            using (var session = _driver.Session())
            {
                session.Run(query);
            }
        }

        public void Delete(string name)
        {
            string query = $"MATCH (p {{name:'{name}'}}) DELETE p;";
            using (var session = _driver.Session())
            {
                session.Run(query);
            }
        }

        public void AssignToGroup(string personName, string groupName)
        {
            string query = $"MATCH (p {{name:'{personName}'}}) " + 
                $"MATCH (g {{name:'{groupName}'}}) " +
                $"CREATE (p)-[:IS_PART_OF {{startDate: '{DateTime.UtcNow}'}}]->(g);";
            using (var session = _driver.Session())
            {
                session.Run(query);
            }
        }
    }
}
