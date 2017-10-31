using Neo4j.Driver.V1;
using Neo4j_Sample.Helpers;
using Neo4j_Sample.Models;
using System;
using System.Linq;

namespace Neo4j_Sample.Repositories
{
    class GroupRepository : IDisposable
    {
        private IDriver _driver;

        public GroupRepository(Connection connection)
        {
            _driver = GraphDatabase.Driver(connection.Url, AuthTokens.Basic(connection.UserName, connection.Password));
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }

        public void Add(string name)
        {
            string query = $"CREATE (g:Group {{ name:'{name}'}});";
            using (var session = _driver.Session())
            {
                session.Run(query);
            }
        }

        public Group FindByName(string name)
        {
            string query = $"MATCH (g:Group {{name:'{name}'}}) RETURN g.name;";
            using (var session = _driver.Session())
            {
                var resultRecord = session.Run(query).Single();
                return new Group { Name = resultRecord[0].ToString() };
            }
        }
    }
}
