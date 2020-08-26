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
        public string Name { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
