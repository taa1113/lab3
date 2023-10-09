using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Services.Interfaces
{
    public interface ICachedEventsService
    {
        public IEnumerable<Event> GetEvents(int rowsNumber = 20);
        public void AddEvents(string cacheKey, int rowsNumber = 20);
        public IEnumerable<Event> GetEvents(string cacheKey, int rowsNumber = 20);
    }
}
