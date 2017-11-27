using Banking.Areas.Admin.Validators;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using X.PagedList;

namespace Banking.Areas.Admin.ViewModels
{
    [Validator(typeof(AccountSearchViewModelValidator))]
    public class AccountSearchViewModel
    {
        #region Sort Parameters
        public string SortParameter { get; set; }
        public string AccountNumberSortParameter { get; set; } = "AccountNumberSortParameter";
        public string TitleSortParameter { get; set; } = "TitleSortParameter";
        public string ClientNameSortParameter { get; set; } = "ClientNameSortParameter";
        public string BalanceSortParameter { get; set; } = "BalanceSortParameter";
        public string CurrencySortParameter { get; set; } = "CurrencySortParameter";
        public string CreatedDateSortParameter { get; set; } = "CreatedDateSortParameter";
        public string ModifiedDateSortParameter { get; set; } = "ModifiedDateSortParameter";
        #endregion

        [Display(Name = "Account Number")]
        public int? AccountNumber { get; set; }

        
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Display(Name = "Min Balance")]
        public decimal? MinBalance { get; set; }

        [Display(Name = "Max Balance")]
        public decimal? MaxBalance { get; set; }

        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        public int? page { get; set; }
        public int? pageSize { get; set; }

        public IPagedList<AccountViewModel> AccountList { get; set; }
    }
}