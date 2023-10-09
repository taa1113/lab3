using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Services.Interfaces
{
    public interface ICachedEnterprisesService
    {
        public IEnumerable<Enterprise> GetEnterprises(int rowsNumber = 20);
        public void AddEnterprises(string cacheKey, int rowsNumber = 20);
        public IEnumerable<Enterprise> GetEnterprises(string cacheKey, int rowsNumber = 20);
    }
}
