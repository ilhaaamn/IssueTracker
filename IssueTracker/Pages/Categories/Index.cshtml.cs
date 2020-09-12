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

namespace IssueTracker.Pages.Categories
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IssueTracker.Data.IssueTrackerContext _context;

        public IndexModel(IssueTracker.Data.IssueTrackerContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; }

        public async Task OnGetAsync()
        {
            Category = await _context.Categories.ToListAsync();
        }
    }
}
