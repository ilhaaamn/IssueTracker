using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Models;

namespace IssueTracker.Data
{
    public class IssueTrackerContext : DbContext
    {
        public IssueTrackerContext (DbContextOptions<IssueTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne(ticket => ticket.UserAssigned)
                .WithMany(user => user.AssignedTickets)
                .HasForeignKey(ticket => ticket.AssigneeId);

            modelBuilder.Entity<Ticket>()
                .HasOne(p => p.UserCreator)
                .WithMany(b => b.OwnedTickets)
                .HasForeignKey(ticket => ticket.CreatorId);
        }
    }
}
