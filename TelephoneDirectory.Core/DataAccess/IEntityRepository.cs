﻿using System.Linq.Expressions;
using TelephoneDirectory.Core.Entities;

namespace TelephoneDirectory.Core.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity, new()

    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        T Add(T entity);
        T Update(T entity);
        T Delete(T entity);

    }
}
