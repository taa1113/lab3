using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class CachedSourceOfFinancingsService : ICachedSourceOfFinancingsService
    {
        private readonly EventsContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        public CachedSourceOfFinancingsService(EventsContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        public void AddSourceOfFinancings(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<SourceOfFinancing> tanks = _dbContext.SourcesOfFinancing.Take(rowsNumber).ToList();
            if (tanks != null)
            {
                _memoryCache.Set(cacheKey, tanks, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 27 + 240)
                });

            }
        }

        public IEnumerable<SourceOfFinancing> GetSourceOfFinancings(int rowsNumber = 20)
        {
            return _dbContext.SourcesOfFinancing.Take(rowsNumber).ToList();
        }

        public IEnumerable<SourceOfFinancing> GetSourceOfFinancings(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<SourceOfFinancing> cpes;
            if (!_memoryCache.TryGetValue(cacheKey, out cpes))
            {
                cpes = _dbContext.SourcesOfFinancing.Take(rowsNumber).ToList();
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
