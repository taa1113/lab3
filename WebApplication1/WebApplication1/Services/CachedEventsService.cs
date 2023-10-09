using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class CachedEventsService : ICachedEventsService
    {
        private readonly EventsContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        public CachedEventsService(EventsContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        public void AddEvents(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Event> tanks = _dbContext.Events.Take(rowsNumber).ToList();
            if (tanks != null)
            {
                _memoryCache.Set(cacheKey, tanks, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 27 + 240)
                });

            }
        }

        public IEnumerable<Event> GetEvents(int rowsNumber = 20)
        {
            return _dbContext.Events.Take(rowsNumber).ToList();
        }

        public IEnumerable<Event> GetEvents(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Event> cpes;
            if (!_memoryCache.TryGetValue(cacheKey, out cpes))
            {
                cpes = _dbContext.Events.Take(rowsNumber).ToList();
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
