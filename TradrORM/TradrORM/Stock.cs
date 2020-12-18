using System;
using System.Collections.Generic;
using System.Text;

namespace TradrORM
{
    public class Stock
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int IPOYear { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }

        public Stock() { }

        public Stock(string symbol, string name, int ipoYear, string sector, string industry)
        {
            this.Symbol = symbol;
            this.Name = name;
            this.IPOYear = ipoYear;
            this.Sector = sector;
            this.Industry = industry;
        }

        public static Stock Get(int id)
        {
            return null;
        }

        public static bool Save(Stock stock)
        {
            return true;
        }
    }
}
