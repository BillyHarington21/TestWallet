using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWallet.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Column(TypeName = "decimal(18,8)")]
        public decimal Balance { get; set; }
    }
}
