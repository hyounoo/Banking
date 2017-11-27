using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}