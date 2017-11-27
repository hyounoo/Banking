using Banking.Areas.Admin.Validators;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Banking.Areas.Admin.ViewModels
{
    [Validator(typeof(AccountViewModelValidator))]
    public class AccountViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Balance")]
        public decimal Balance { get; set; }
        public string Type { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
        [Display(Name = "Client Id")]
        public int ClientId { get; set; }
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Display(Name = "Currency Id")]
        public int CurrencyId { get; set; }        
    }
}