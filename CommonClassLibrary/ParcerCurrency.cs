using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonClassLibrary
{
    public static class ParcerCurrency
    {
        public static string ParceCurrency(string sPrice)
        {
            return Regex.Match(sPrice ?? "", "[^.\\d]\\D+").Value;
        }
    }
}
