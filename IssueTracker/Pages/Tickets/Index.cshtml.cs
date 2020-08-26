using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Data;
using IssueTracker.Models;

namespace IssueTracker.Pages.Tickets
{
    public class IndexModel : PageModel
    {
        private readonly IssueTracker.Data.IssueTrackerContext _context;

        public IndexModel(IssueTracker.Data.IssueTrackerContext context)
        {
            _context = context;
        }

        public IList<Ticket> Ticket { get;set; }

        public async Task OnGetAsync()
        {
            Ticket = await _context.Ticket
                .Include(t => t.Category)
                .Include(t => t.Status)
                .Include(t => t.UserAssigned)
                .Include(t => t.UserCreator).ToListAsync();
        }
    }
}
