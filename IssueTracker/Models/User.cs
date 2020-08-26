using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public virtual ICollection<Ticket> OwnedTickets { get; set; }
        public virtual ICollection<Ticket> AssignedTickets { get; set; }
    }
}
