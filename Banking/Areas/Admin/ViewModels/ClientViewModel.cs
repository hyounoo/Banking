using Banking.Areas.Admin.Validators;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Banking.Areas.Admin.ViewModels
{
    [Validator(typeof(ClientViewModelValidator))]
    public class ClientViewModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int Accounts
        {
            get
            {
                return AccountList != null ? AccountList.Count() : 0;
            }
        }
        public decimal AccountTotal
        {
            get
            {
                return AccountList != null ? AccountList.Sum(a => a.Balance) : 0;
            }
        }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
        public List<AccountViewModel> AccountList { get; set; }
    }
}