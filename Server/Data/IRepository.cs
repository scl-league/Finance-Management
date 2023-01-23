using System;
using FinanceManagement.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.Server.Data
{
    public interface IRepository<T>
    {
        //Task<Earning[]> GetEarning();
        //Task<Earning> GetEarningById();
        //Task RemoveEarning();
        //Task AddEarning();

        void Add(T entity);
        void Remove(T entity);
        IEnumerable<T> GetAll();

        Task<ActionResult<Earning>> AddAsync(T entity);
        Task RemoveAsync(T entity);
        Task<ActionResult<IEnumerable<T>>> GetAllAsync();
    }
}

