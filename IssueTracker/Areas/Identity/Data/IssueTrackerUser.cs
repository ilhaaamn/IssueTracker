using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace IssueTracker.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the IssueTrackerUser class
    public class IssueTrackerUser : IdentityUser
    {
        public virtual ICollection<Ticket> OwnedTickets { get; set; }
        public virtual ICollection<Ticket> AssignedTickets { get; set; }
    }
}
