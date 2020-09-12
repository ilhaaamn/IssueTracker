using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Data;
using IssueTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace IssueTracker.Pages.Categories
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IssueTracker.Data.IssueTrackerContext _context;

        public DeleteModel(IssueTracker.Data.IssueTrackerContext context)
        {
            _context = context;
        }

        public const string DeleteStatus = nameof(DeleteStatus);
        public const string StatusMessage = nameof(StatusMessage);

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);

            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories.FindAsync(id);

            if (Category != null)
            {
                IQueryable<Ticket> ticketsIQ = from s in _context.Ticket
                                               select s;
                var IssueTickets = ticketsIQ.Where(a => a.CategoryId == Category.CategoryId).AsNoTracking();
                if (!IssueTickets.Any())
                {
                    _context.Categories.Remove(Category);
                    await _context.SaveChangesAsync();
                    TempData[DeleteStatus] = "Success";
                    TempData[StatusMessage] = "Category has been deleted!";
                } 
                else
                {
                    TempData[DeleteStatus] = "Fail";
                    TempData[StatusMessage] = "Unable to delete category because it's still related to some Issue Tickets";
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
