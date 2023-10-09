using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Services.Interfaces
{
    public interface ICachedManagersService
    {
        public IEnumerable<Manager> GetManagers(int rowsNumber = 20);
        public void AddManagers(string cacheKey, int rowsNumber = 20);
        public IEnumerable<Manager> GetManagers(string cacheKey, int rowsNumber = 20);
    }
}
