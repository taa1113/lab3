using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class CachedEmployeesService : ICachedEmployeesService
    {
        private readonly EventsContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        public CachedEmployeesService(EventsContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        public void AddEmployees(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Employee> tanks = _dbContext.Employees.Take(rowsNumber).ToList();
            if (tanks != null)
            {
                _memoryCache.Set(cacheKey, tanks, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 27 + 240)
                });

            }
        }

        public IEnumerable<Employee> GetEmployees(int rowsNumber = 20)
        {
            return _dbContext.Employees.Take(rowsNumber).ToList();
        }

        public IEnumerable<Employee> GetEmployees(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Employee> cpes;
            if (!_memoryCache.TryGetValue(cacheKey, out cpes))
            {
                cpes = _dbContext.Employees.Take(rowsNumber).ToList();
                if (cpes != null)
                {
                    _memoryCache.Set(cacheKey, cpes,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 27 + 240)));
                }
            }
            return cpes;
        }
    }
}
