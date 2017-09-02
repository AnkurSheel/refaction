using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Web;
using Refactor_me.Data.Interfaces;

namespace Refactor_me.Data.Helpers
{
    public class ConnectionCreator : IConnectionCreator
    {
        public ConnectionCreator()
        {
            if (HttpContext.Current != null)
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", HttpContext.Current.Server.MapPath("~/App_Data"));
            }
            else
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\");
            }
        }

        public IDbConnection GetOpenConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Database"];
            var providerName = connectionString.ProviderName;
            var factory = DbProviderFactories.GetFactory((string)providerName);
            var connection = factory.CreateConnection();
            if (connection == null)
            {
                return null;
            }

            connection.ConnectionString = connectionString.ConnectionString;
            connection.Open();
            return connection;
        }
    }
}
