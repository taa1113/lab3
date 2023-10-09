using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Services.Interfaces
{
    public interface ICachedCPEsService
    {
        public IEnumerable<CPE> GetCPEs(int rowsNumber = 20);
        public void AddCPEs(string cacheKey, int rowsNumber = 20);
        public IEnumerable<CPE> GetCPEs(string cacheKey, int rowsNumber = 20);
    }
}
