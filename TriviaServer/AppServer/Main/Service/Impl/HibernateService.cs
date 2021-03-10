using System.Collections.Generic;
using System.IO;
using Com.Qsw.Framework.Session.Impl;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;

namespace Com.Qsw.TriviaServer.AppServer.Main
{
    public class HibernateService : IHibernateService
    {
        private readonly IConfiguration configuration;
        private ISessionFactory sessionFactory;

        public HibernateService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void InitHibernate()
        {
            var nhibernateConfiguration = new Configuration();
            nhibernateConfiguration.SetProperties(
                new Dictionary<string, string>
                {
                    {"connection.provider", configuration["DatabaseConnection:Provider"]},
                    {"connection.driver_class", configuration["DatabaseConnection:DriverClass"]},
                    {"connection.connection_string", configuration["DatabaseConnection:ConnectionString"]},
                    {"dialect", configuration["DatabaseConnection:Dialect"]},
                    {"connection.release_mode", configuration["DatabaseConnection:ReleaseMode"]}
                });
            nhibernateConfiguration.CurrentSessionContext<AsyncLocalSessionContext>();

            IEnumerable<string> dataMappingFiePaths =
                Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Mappings"));

            foreach (string dataMappingFiePath in dataMappingFiePaths)
            {
                nhibernateConfiguration.AddXmlFile(dataMappingFiePath);
            }

            nhibernateConfiguration.SetInterceptor(new EntityInterceptor());
            sessionFactory = nhibernateConfiguration.BuildSessionFactory();
        }

        public ISessionFactory GetSessionFactory()
        {
            return sessionFactory;
        }
    }
}