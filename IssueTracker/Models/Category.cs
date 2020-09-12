using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "This field cannot be empty.")]
        public string Name { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
