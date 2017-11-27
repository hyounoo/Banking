using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.ViewModels
{
    public class CurrencyViewModel
    {
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public double CurrencyVSUSD { get; set; }
    }
}