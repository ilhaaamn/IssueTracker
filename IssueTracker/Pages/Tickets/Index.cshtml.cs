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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IssueTracker.Pages.Tickets
{
    [Authorize]
    public class IndexModel : BasePageModel
    {
        public IndexModel(IssueTrackerContext context,
                        IAuthorizationService authorizationService,
                        UserManager<IssueTrackerUser> userManager)
                        : base(context, authorizationService, userManager)
        {
        }

        public string CurrentView { get; set; }
        public int CurrentFilter { get; set; }
        public string CurrentSearch { get; set; }
        public IList<Status> StatusList { get; set; }

        public PaginatedList<Ticket> IssueTickets { get; set; }

        public async Task OnGetAsync(string view, int currentFilter, int filterId, 
                        string currentSearch, string searchString, int? pageIndex)
        {
            if (searchString != null || filterId != 0)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentSearch;
                filterId = currentFilter;
            }


            StatusList = Context.Statuses.ToList();
            CurrentView = "All Issues";
            CurrentSearch = searchString;
            CurrentFilter = filterId;

            var currentUserId = UserManager.GetUserId(User);
            IQueryable<Ticket> ticketsIQ = from s in Context.Ticket
                                             select s;
            ticketsIQ = ticketsIQ.OrderByDescending(s => s.CreatedDate);
            // For Selecting View (My Tickets or Assigned Ticket)
            if (view == "My Tickets")
            {
                ticketsIQ = ticketsIQ.Where(c => c.CreatorId == currentUserId);
                CurrentView = view;
            }
            else if (view == "Assigned Tickets")
            {
                ticketsIQ = ticketsIQ.Where(c => c.AssigneeId == currentUserId);
                CurrentView = view;
            }

            if (!String.IsNullOrEmpty(searchString)) ticketsIQ = ticketsIQ.Where(s => s.Name.Contains(searchString));
            if (filterId != 0) ticketsIQ = ticketsIQ.Where(s => s.StatusId == filterId);

            int pageSize = 3;

            IssueTickets = await PaginatedList<Ticket>.CreateAsync(
                ticketsIQ
                .Include(t => t.Category)
                .Include(t => t.Status)
                .Include(t => t.UserAssigned)
                .Include(t => t.UserCreator).AsNoTracking(), 
                pageIndex ?? 1, pageSize);
        }
    }
}
