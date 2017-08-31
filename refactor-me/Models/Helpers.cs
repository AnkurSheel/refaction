using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web;

namespace Refactor_me.Models
{
    public class Helpers
    {
        private const string ConnectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\{DatabaseName};Integrated Security=True";

        public static SqlConnection NewConnection()
        {
            var dataDirectory = string.Empty;
            var databaseName = string.Empty;
            if (HttpContext.Current != null)
            {
                dataDirectory = HttpContext.Current.Server.MapPath("~/App_Data");
                databaseName = "Database.mdf";
            }
            else
            {
                dataDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                databaseName = "Database_Test.mdf";
            }

            var connstr = ConnectionString.Replace("{DataDirectory}", dataDirectory).Replace("{DatabaseName}", databaseName);
            return new SqlConnection(connstr);
        }

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
    }
}
