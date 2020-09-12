using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using IssueTracker.Data;
using IssueTracker.Models;
using IssueTracker.Areas.Identity.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IssueTracker.Authorization;

namespace IssueTracker.Pages.Tickets
{
    [Authorize]
    public class CreateModel : BasePageModel
    {
        public CreateModel(IssueTrackerContext context,
                        IAuthorizationService authorizationService,
                        UserManager<IssueTrackerUser> userManager)
                        : base(context, authorizationService, userManager)
        {
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(Context.Categories, "CategoryId", "Name");
            ViewData["StatusId"] = new SelectList(Context.Statuses, "StatusId", "Name");
            ViewData["AssigneeId"] = new SelectList(Context.Users, "Id", "UserName");
            ViewData["CreatorId"] = new SelectList(Context.Users, "Id", "UserName");
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

            Ticket.CreatorId = UserManager.GetUserId(User);

            // requires using ContactManager.Authorization;
            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, Ticket,
                                                        TicketOperations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            var user = await UserManager.GetUserAsync(User);
            Ticket.CreatorId = user.Id;
            Ticket.StatusId = 1;
            Ticket.CreatedDate = DateTime.Now;

            Context.Ticket.Add(Ticket);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
