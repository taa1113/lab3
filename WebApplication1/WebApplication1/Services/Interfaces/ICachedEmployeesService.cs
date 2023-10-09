using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Services.Interfaces
{
    public interface ICachedEmployeesService
    {
        public IEnumerable<Employee> GetEmployees(int rowsNumber = 20);
        public void AddEmployees(string cacheKey, int rowsNumber = 20);
        public IEnumerable<Employee> GetEmployees(string cacheKey, int rowsNumber = 20);
    }
}
