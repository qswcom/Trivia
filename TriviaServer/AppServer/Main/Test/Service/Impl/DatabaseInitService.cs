using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace Com.Qsw.TriviaServer.AppServer.Main.Test
{
    public class DatabaseInitService : IDatabaseInitService
    {
        private readonly IConfiguration configuration;

        public DatabaseInitService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task InitDatabase()
        {
            var nhibernateConfiguration = new Configuration();
            nhibernateConfiguration.SetProperties(
                new Dictionary<string, string>
                {
                    {"connection.provider", configuration["DatabaseConnection:Provider"]},
                    {"connection.driver_class", configuration["DatabaseConnection:DriverClass"]},
                    {"connection.connection_string", configuration["DatabaseConnection:ConnectionString"]},
                    {"dialect", configuration["DatabaseConnection:Dialect"]},
                });
            nhibernateConfiguration.CurrentSessionContext<AsyncLocalSessionContext>();

            IEnumerable<string> dataMappingFiePaths =
                Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Mappings"));

            foreach (string dataMappingFiePath in dataMappingFiePaths)
            {
                nhibernateConfiguration.AddXmlFile(dataMappingFiePath);
            }

            using ISessionFactory sessionFactory = nhibernateConfiguration.BuildSessionFactory();
            using IStatelessSession session = sessionFactory.OpenStatelessSession();
            await new SchemaExport(nhibernateConfiguration).CreateAsync(true, true);
        }
    }
}