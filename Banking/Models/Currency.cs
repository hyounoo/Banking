using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public double CurrencyVSUSD { get; set; }
    }
}