using Neo4j_Sample.Helpers;
using Neo4j_Sample.Repositories;

namespace Neo4j_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new Connection
            {
                Url = "bolt://127.0.0.1:7687/",
                UserName = "neo4j",
                Password = "pwd"
            };

            /*
            //Using Unity DI
            
            IUnityContainer container = new UnityContainer();
            container.RegisterType<Connection>(new InjectionFactory(c => connection));
            var personRepository = container.Resolve<PersonRepository>();
            var groupRepository = container.Resolve<GroupRepository>();
            */

            using (var groupRepository = new GroupRepository(connection))
            {
                groupRepository.Add("Group1");

                using (var personRepository = new PersonRepository(connection))
                {
                    //personRepository.Add("Person1", 25);
                    //personRepository.UpdateAge("Person1", 30);

                    var person1 = personRepository.FindByName("Person1");
                    var group1 = groupRepository.FindByName("Group1");
                    personRepository.AssignToGroup(person1.Name, group1.Name);
                }
            }
        }
    }
}
