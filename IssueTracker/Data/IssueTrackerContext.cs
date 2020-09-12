using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IssueTracker.Areas.Identity.Data;

namespace IssueTracker.Data
{
    public class IssueTrackerContext : IdentityDbContext<IssueTrackerUser>
    {
        public IssueTrackerContext (DbContextOptions<IssueTrackerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
