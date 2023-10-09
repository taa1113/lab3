using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class CachedCPEsService : ICachedCPEsService
    {
        private readonly EventsContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        public CachedCPEsService(EventsContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        public void AddCPEs(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<CPE> tanks = _dbContext.CPEs.Take(rowsNumber).ToList();
            if (tanks != null)
            {
                _memoryCache.Set(cacheKey, tanks, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2*27+240)
                });

            }
        }

        public IEnumerable<CPE> GetCPEs(int rowsNumber = 20)
        {
            return _dbContext.CPEs.Take(rowsNumber).ToList();
        }

        public IEnumerable<CPE> GetCPEs(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<CPE> cpes;
            if (!_memoryCache.TryGetValue(cacheKey, out cpes))
            {
                cpes = _dbContext.CPEs.Take(rowsNumber).ToList();
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
