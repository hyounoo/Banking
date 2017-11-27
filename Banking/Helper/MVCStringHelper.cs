using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Helper
{
    public static class MVCStringHelper
    {
        public static string ConvertSortDirection(string sortParameter)
        {
            return sortParameter.Contains("_desc") ? sortParameter.Replace("_desc", "") : sortParameter + "_desc";
        }
    }
}