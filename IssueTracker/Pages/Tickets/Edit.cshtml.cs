using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Data;
using IssueTracker.Models;

namespace IssueTracker.Pages.Tickets
{
    public class EditModel : PageModel
    {
        private readonly IssueTracker.Data.IssueTrackerContext _context;

        public EditModel(IssueTracker.Data.IssueTrackerContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
           ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "Name");
           ViewData["AssigneeId"] = new SelectList(_context.Users, "UserId", "Name");
           ViewData["CreatorId"] = new SelectList(_context.Users, "UserId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(Ticket.TicketId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.TicketId == id);
        }
    }
}
