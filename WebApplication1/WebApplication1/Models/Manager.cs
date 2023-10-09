using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Manager
    {
        public int Id { get; set; }
        [Column("Manager")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
