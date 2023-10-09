using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Services.Interfaces
{
    public interface ICachedPlannedEventsService
    {
        public IEnumerable<PlannedEvent> GetPlannedEvents(int rowsNumber = 20);
        public void AddPlannedEvents(string cacheKey, int rowsNumber = 20);
        public IEnumerable<PlannedEvent> GetPlannedEvents(string cacheKey, int rowsNumber = 20);
    }
}
