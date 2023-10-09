using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class PlannedEvent
    {
        public int Id {  get; set; }
        [Column("Enterprise")]
        public int EnterpriseId {  get; set; }
        public Enterprise Enterprise { get; set; }
        public DateTime DateOfStart { get; set; }
        public DateTime DateOfEnd { get; set; }
        public int Scope { get; set; }
        public decimal Expenses { get; set; }
        public decimal EconomicEffect { get; set; }
        [Column("Responsible")]
        public int EmployeeId { get; set; }
        public Employee Responsible { get; set; }
        [Column("Event")]
        public int EventId { get; set; }
        public Event Event { get; set; }
        [Column("Finance")]
        public int FinanceId { get; set; }
        public SourceOfFinancing Finance { get; set; }
    }
}
