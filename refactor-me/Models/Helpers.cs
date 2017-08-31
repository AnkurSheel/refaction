using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Web;

namespace Refactor_me.Models
{
    public class Helpers
    {
        public static void AddParameter(IDbCommand command, string name, object value)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        public static IDbConnection NewConnection()
        {
            if (HttpContext.Current != null)
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", HttpContext.Current.Server.MapPath("~/App_Data"));
            }
            else
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\");
            }

            var connectionString = ConfigurationManager.ConnectionStrings["Database"];
            var providerName = connectionString.ProviderName;
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            if (connection == null)
            {
                return null;
            }

            connection.ConnectionString = connectionString.ConnectionString;
            return connection;
        }
    }
}
