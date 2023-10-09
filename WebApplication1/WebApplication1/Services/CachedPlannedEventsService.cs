using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class CachedPlannedEventsService : ICachedPlannedEventsService
    {
        private readonly EventsContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        public CachedPlannedEventsService(EventsContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        public void AddPlannedEvents(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<PlannedEvent> tanks = _dbContext.PlannedEvents.Take(rowsNumber).ToList();
            if (tanks != null)
            {
                _memoryCache.Set(cacheKey, tanks, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 27 + 240)
                });

            }
        }

        public IEnumerable<PlannedEvent> GetPlannedEvents(int rowsNumber = 20)
        {
            return _dbContext.PlannedEvents.Take(rowsNumber).ToList();
        }

        public IEnumerable<PlannedEvent> GetPlannedEvents(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<PlannedEvent> cpes;
            if (!_memoryCache.TryGetValue(cacheKey, out cpes))
            {
                cpes = _dbContext.PlannedEvents.Take(rowsNumber).ToList();
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
