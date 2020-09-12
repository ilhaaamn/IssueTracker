using IssueTracker.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Date Created")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Assignee")]
        [ForeignKey("UserAssigned")]
        public string? AssigneeId { get; set; }
        [DisplayName("Owner")]
        [ForeignKey("UserCreator")]
        public string CreatorId { get; set; }

        [DisplayName("Assignee")]
        public virtual IssueTrackerUser UserAssigned { get; set; }
        [DisplayName("Owner")]
        public virtual IssueTrackerUser UserCreator { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [DisplayName("Status")]
        public int StatusId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
    }
}
