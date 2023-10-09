using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Services.Interfaces
{
    public interface ICachedSourceOfFinancingsService
    {
        public IEnumerable<SourceOfFinancing> GetSourceOfFinancings(int rowsNumber = 20);
        public void AddSourceOfFinancings(string cacheKey, int rowsNumber = 20);
        public IEnumerable<SourceOfFinancing> GetSourceOfFinancings(string cacheKey, int rowsNumber = 20);
    }
}
