﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class EditModel : BasePageModel
    {
        public EditModel(IssueTrackerContext context,
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

            if (Ticket == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, Ticket,
                                                        TicketOperations.Edit);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            ViewData["CategoryId"] = new SelectList(Context.Categories, "CategoryId", "Name");
            ViewData["StatusId"] = new SelectList(Context.Statuses, "StatusId", "Name");
            ViewData["AssigneeId"] = new SelectList(Context.Users, "Id", "UserName");
            ViewData["CreatorId"] = new SelectList(Context.Users, "Id", "UserName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch Contact from DB to get OwnerID.
            var ticket = await Context
                .Ticket.AsNoTracking()
                .FirstOrDefaultAsync(m => m.TicketId == id);

            if (ticket == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, Ticket,
                                                        TicketOperations.Edit);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            /*Ticket.CreatorId = ticket.CreatorId;*/
            Context.Attach(Ticket).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
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
            return Context.Ticket.Any(e => e.TicketId == id);
        }
    }
}
