using System;
using System.Data;

namespace Refactor_me.Data
{
    public static class CommandExtensions
    {
        public static void AddParameter(IDbCommand command, string name, object value)
        {
            if (command == null)
            {
                throw new ArgumentNullException($"command");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException($"name");
            }

            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }
    }
}
