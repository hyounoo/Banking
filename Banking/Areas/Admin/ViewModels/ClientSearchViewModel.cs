using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using X.PagedList;

namespace Banking.Areas.Admin.ViewModels
{
    public class ClientSearchViewModel
    {
        #region Sort Parameters
        public string SortParameter { get; set; }
        public string NameSortParameter { get; set; } = "NameSortParameter";
        public string AccountsSortParameter { get; set; } = "AccountsSortParameter";
        public string AccountTotalSortParameter { get; set; } = "AccountTotalSortParameter";
        public string CreatedDateSortParameter { get; set; } = "CreatedDateSortParameter";
        public string ModifiedDateSortParameter { get; set; } = "ModifiedDateSortParameter";
        #endregion

        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        public int? page { get; set; }
        public int? pageSize { get; set; }

        public IPagedList<ClientViewModel> ClientList { get; set; }
    }
}