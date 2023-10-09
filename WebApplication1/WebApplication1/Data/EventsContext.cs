using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class EventsContext : DbContext
    {
        public EventsContext(DbContextOptions<EventsContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<CPE> CPEs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<SourceOfFinancing> SourcesOfFinancing { get; set; }
        public DbSet<PlannedEvent> PlannedEvents { get; set; }
    }

}
