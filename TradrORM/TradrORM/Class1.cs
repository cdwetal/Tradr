using System;
using System.Collections.Generic;
using System.Text;

namespace TradrORM
{
    public class Price
    {
        public int ID { get; set; }
        public Stock Stock { get; set }
        public DateTime DateTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public int Volume { get; set; }
        public int VolumeAdjusted { get; set }

        public Price() { }

        public enum Interval
        {
            1Min = 0,
            5Min = 1
        }

        public Price(int id, Stock stock, DateTime dateTime, decimal open, decimal high, decimal low, decimal close, int volume, int volumeAdjusted)
        {
            this.ID = id;
            this.Stock = stock;
            this.DateTime = dateTime;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;
            this.VolumeAdjusted = volumeAdjusted
        }

        public static Price Get(int id)
        {
            return null;
        }

        public static bool Save(Price price)
        {
            return true;
        }
    }
}
