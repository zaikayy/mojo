using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonClassLibrary
{
    public static class ParcerPrice
    {
        public static double ParcePrice(string sPrice)
        {
            string s = Regex.Match(sPrice ?? "", "[0-9]*[.,]?[0-9]+").Value;
            double price_ = 0;
            double.TryParse(s, out price_);
            return price_;
        }
    }
}
