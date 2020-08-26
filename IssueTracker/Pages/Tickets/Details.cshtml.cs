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
    public class DetailsModel : PageModel
    {
        private readonly IssueTracker.Data.IssueTrackerContext _context;

        public DetailsModel(IssueTracker.Data.IssueTrackerContext context)
        {
            _context = context;
        }

        public Ticket Ticket { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket = await _context.Ticket
                .Include(t => t.Category)
                .Include(t => t.Status)
                .Include(t => t.UserAssigned)
                .Include(t => t.UserCreator).FirstOrDefaultAsync(m => m.TicketId == id);

            if (Ticket == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
