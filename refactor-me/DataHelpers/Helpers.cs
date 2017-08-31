﻿using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Web;

namespace Refactor_me.DataHelpers
{
    public static class Helpers
    {
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
