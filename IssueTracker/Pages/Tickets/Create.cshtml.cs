using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using IssueTracker.Data;
using IssueTracker.Models;

namespace IssueTracker.Pages.Tickets
{
    public class CreateModel : PageModel
    {
        private readonly IssueTracker.Data.IssueTrackerContext _context;

        public CreateModel(IssueTracker.Data.IssueTrackerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
        ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "Name");
        ViewData["AssigneeId"] = new SelectList(_context.Users, "UserId", "Name");
        ViewData["CreatorId"] = new SelectList(_context.Users, "UserId", "Name");
            return Page();
        }

        [BindProperty]
        public Ticket Ticket { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Ticket.Add(Ticket);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
