using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required]
        public int AccountNumber { get; set; }
        [Required]
        public string Title { get; set; }
        public decimal Balance { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Foreign Key
        public int ClientId { get; set; }
        [JsonIgnore]
        public virtual Client Client { get; set; }

        public int CurrencyId { get; set; }
        [JsonIgnore]
        public virtual Currency Currency { get; set; }
        
    }
}