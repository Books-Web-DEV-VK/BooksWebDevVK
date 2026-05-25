using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }   
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public Guid? CompanyId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;

        [NotMapped] 
        public string Role { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
    }
}
