using System;
using System.Collections.Generic;
using System.Data;
using Refactor_me.Data.Helpers;
using Refactor_me.Data.Interfaces;

namespace Refactor_me.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T>
        where T : new()
    {
        private readonly IConnectionCreator _connectionCreator;

        protected BaseRepository()
        {
            _connectionCreator = new ConnectionCreator();
        }

        public void Add(T entity)
        {
            using (var connection = _connectionCreator.GetOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    Create(entity, command);
                }
            }
        }

        public IEnumerable<T> FindAll()
        {
            using (var connection = _connectionCreator.GetOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    var tableName = GetTableName();
                    command.CommandText = $"select * from {tableName}";
                    var entities = new List<T>();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        entities.Add(Map(reader));
                    }

                    return entities;
                }
            }
        }

        public T FindById(Guid id)
        {
            using (var connection = _connectionCreator.GetOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    return FindById(id, command);
                }
            }
        }

        public void Remove(Guid id)
        {
            using (var connection = _connectionCreator.GetOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    Delete(id, command);
                }
            }
        }

        public void Update(Guid id, T updatedT)
        {
            using (var connection = _connectionCreator.GetOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    Update(id, updatedT, command);
                }
            }
        }

        protected abstract void Create(T entity, IDbCommand command);

        protected abstract void Delete(Guid id, IDbCommand command);

        protected abstract T FindById(Guid id, IDbCommand command);

        protected abstract string GetTableName();

        protected abstract T Map(IDataRecord record);

        protected abstract void Update(Guid id, T updatedT, IDbCommand command);
    }
}
