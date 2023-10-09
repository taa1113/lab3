using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class CachedManagersService : ICachedManagersService
    {
        private readonly EventsContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        public CachedManagersService(EventsContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        public void AddManagers(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Manager> tanks = _dbContext.Managers.Take(rowsNumber).ToList();
            if (tanks != null)
            {
                _memoryCache.Set(cacheKey, tanks, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 27 + 240)
                });

            }
        }

        public IEnumerable<Manager> GetManagers(int rowsNumber = 20)
        {
            return _dbContext.Managers.Take(rowsNumber).ToList();
        }

        public IEnumerable<Manager> GetManagers(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Manager> cpes;
            if (!_memoryCache.TryGetValue(cacheKey, out cpes))
            {
                cpes = _dbContext.Managers.Take(rowsNumber).ToList();
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
