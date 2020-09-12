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
using Microsoft.AspNetCore.Identity;
using IssueTracker.Areas.Identity.Data;
using IssueTracker.Authorization;

namespace IssueTracker.Pages.Tickets
{
    [Authorize]
    public class DeleteModel : BasePageModel
    {

        public DeleteModel(IssueTrackerContext context,
                        IAuthorizationService authorizationService,
                        UserManager<IssueTrackerUser> userManager)
                        : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Ticket Ticket { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket = await Context.Ticket
                .Include(t => t.Category)
                .Include(t => t.Status)
                .Include(t => t.UserAssigned)
                .Include(t => t.UserCreator).FirstOrDefaultAsync(m => m.TicketId == id);

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                            User, Ticket,
                                            TicketOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            if (Ticket == null)
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

            Ticket = await Context.Ticket.FindAsync(id);

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, Ticket,
                                                        TicketOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            if (Ticket != null)
            {
                Context.Ticket.Remove(Ticket);
                await Context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
