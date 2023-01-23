using System;
using System.Threading.Tasks;
using FinanceManagement.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.Server.Data
{
    public class MemoryRepository<T> : IRepository<T>
    {
        private  readonly IList<Task<T>> _entitiesTask;
        private  readonly IList<T> _entities;

        public MemoryRepository()
        {
            _entities = new List<T>();
            _entitiesTask = new List<Task<T>>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entities;
        }

        public void  Add(T entity)
        {
            _entities.Add(entity);
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        // Async

        public Task<ActionResult<Earning>> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<IEnumerable<T>>> GetAllAsync()
        {
            //return  _entitiesTask.ToList();
            throw new NotImplementedException();
        }
    }
}

