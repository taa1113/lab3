using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Enterprise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnershipForm { get; set; }
        public string Adress {  get; set; }
        [Column("Manager")]
        public int ManagerId {  get; set; }
        public Employee Manager { get; set; }
        [Column("СhiefPowerEngineer")]
        public int CPEId { get; set; }
        public Employee CPE { get; set; }
    }
}
