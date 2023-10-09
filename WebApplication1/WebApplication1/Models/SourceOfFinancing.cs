using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class SourceOfFinancing
    {
        public int Id { get; set; }
        public decimal Enterprise { get; set; }
        public decimal Organisation { get; set; }
        public decimal Ministry { get; set; }
        public decimal RepublicBudget { get; set; }
        public decimal LocalBudget { get; set; }
    }
}
