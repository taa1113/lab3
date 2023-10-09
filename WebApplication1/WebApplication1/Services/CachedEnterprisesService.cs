using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class CachedEnterprisesService : ICachedEnterprisesService
    {
        private readonly EventsContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        public CachedEnterprisesService(EventsContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        public void AddEnterprises(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Enterprise> tanks = _dbContext.Enterprises.Take(rowsNumber).ToList();
            if (tanks != null)
            {
                _memoryCache.Set(cacheKey, tanks, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 27 + 240)
                });

            }
        }

        public IEnumerable<Enterprise> GetEnterprises(int rowsNumber = 20)
        {
            return _dbContext.Enterprises.Take(rowsNumber).ToList();
        }

        public IEnumerable<Enterprise> GetEnterprises(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Enterprise> cpes;
            if (!_memoryCache.TryGetValue(cacheKey, out cpes))
            {
                cpes = _dbContext.Enterprises.Take(rowsNumber).ToList();
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
