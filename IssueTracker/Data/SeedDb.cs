using IssueTracker.Areas.Identity.Data;
using IssueTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker.Data
{
    public static class SeedDb
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new IssueTrackerContext(
                serviceProvider.GetRequiredService<DbContextOptions<IssueTrackerContext>>()))
            {
                var user1 = await EnsureUser(serviceProvider, testUserPw, "user1@test.com");
                var user2 = await EnsureUser(serviceProvider, testUserPw, "user2@test.com");
                var user3 = await EnsureUser(serviceProvider, testUserPw, "user3@test.com");

                SeedDB(context, user1, user2, user3);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IssueTrackerUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IssueTrackerUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        public static void SeedDB(IssueTrackerContext context, string user1, string user2, string user3)
        {
            if (context.Ticket.Any())
            {
                return;   // DB has been seeded
            }

            context.Statuses.AddRange(
                new Status
                {
                    Name = "OPEN"
                },
                new Status
                {
                    Name = "ON PROGRESS"
                },
                new Status
                {
                    Name = "DONE"
                },
                new Status
                {
                    Name = "CLOSED"
                }
            );

            context.Categories.AddRange(
                new Category
                {
                    Name = "Backend"
                },
                new Category
                {
                    Name = "FrontEnd"
                },
                new Category
                {
                    Name = "Database"
                },
                new Category
                {
                    Name = "Project"
                }
            );

            context.Ticket.AddRange(
                 new Ticket
                 {
                     Name = "Help: No API Found for Testing Class",
                     Description = "Only for testing purpose.",
                     StatusId = 3,
                     CategoryId = 1,
                     AssigneeId = user1,
                     CreatorId = user2,
                     CreatedDate = new DateTime(2020, 8, 1),

                 },
                 new Ticket
                 {
                     Name = "Help: Layout error when using testing module",
                     Description = "Only for testing purpose.",
                     StatusId = 2,
                     CategoryId = 2,
                     AssigneeId = user2,
                     CreatorId = user1,
                     CreatedDate = new DateTime(2020, 8, 2),

                 },
                 new Ticket
                 {
                     Name = "Help: Database conflict need to drop all of them",
                     Description = "Only for testing purpose.",
                     StatusId = 4,
                     CategoryId = 3,
                     AssigneeId = user3,
                     CreatorId = user2,
                     CreatedDate = new DateTime(2020, 8, 3),

                 },
                 new Ticket
                 {
                     Name = "Help: Issues in Flowchart",
                     Description = "Only for testing purpose.",
                     StatusId = 1,
                     CategoryId = 4,
                     AssigneeId = user2,
                     CreatorId = user3,
                     CreatedDate = new DateTime(2020, 8, 4),

                 },
                 new Ticket
                 {
                     Name = "Help: Cannot Install Windows Media Player",
                     Description = "Only for testing purpose.",
                     StatusId = 1,
                     CategoryId = 2,
                     AssigneeId = user1,
                     CreatorId = user3,
                     CreatedDate = new DateTime(2020, 8, 5),

                 }
             );
            context.SaveChanges();
        }
    }
}
