using System;
using System.Collections.Generic;

namespace Refactor_me.Data
{
    public interface IRepository<T>
        where T : new()
    {
        void Add(T entity);

        IEnumerable<T> FindAll();

        T FindById(Guid id);

        void Remove(Guid id);

        void Update(Guid id, T updatedEntity);
    }
}
