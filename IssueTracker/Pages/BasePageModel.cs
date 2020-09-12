using IssueTracker.Areas.Identity.Data;
using IssueTracker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker.Pages
{
    public class BasePageModel : PageModel
    {
        protected IssueTrackerContext Context { get; }
        protected IAuthorizationService AuthorizationService { get; }
        protected UserManager<IssueTrackerUser> UserManager { get; }

        public BasePageModel(
            IssueTrackerContext context,
            IAuthorizationService authorizationService,
            UserManager<IssueTrackerUser> userManager) : base()
        {
            Context = context;
            UserManager = userManager;
            AuthorizationService = authorizationService;
        }
    }
}
