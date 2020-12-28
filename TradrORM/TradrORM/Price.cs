using System;
using System.Collections.Generic;
using System.Text;

namespace TradrORM
{
    public class Price
    {
        private int _id;
        private int _stockId;
        private string _symbol;
        private DateTime _dateTime;
        private decimal _open;
        private decimal _high;
        private decimal _low;
        private decimal _close;
        private int _volume;

        public int Id
        {
            get
            {
                return _id;
            }
        }
        public int StockId
        {
            get
            {
                return _stockId;
            }
        }
        public string Symbol
        {
            get
            {
                return _symbol;
            }
        }
        public DateTime DateTime
        {
            get
            {
                return _dateTime;
            }
        }
        public decimal Open
        {
            get
            {
                return _open;
            }
        }
        public decimal High
        {
            get
            {
                return _high;
            }
        }
        public decimal Low
        {
            get
            {
                return _low;
            }
        }
        public decimal Close
        {
            get
            {
                return _close;
            }
        }
        public int Volume
        {
            get
            {
                return _volume;
            }
        }

        public Price() { }

        public Price(int stockId, string symbol, DateTime dateTime, decimal open, decimal high, decimal low, decimal close, int volume)
        {
            this._id = 0;
            this._stockId = stockId;
            this._symbol = symbol;
            this._dateTime = dateTime;
            this._open = open;
            this._high = high;
            this._low = low;
            this._close = close;
            this._volume = volume;
        }

        public Price(int id, int stockId, string symbol, DateTime dateTime, decimal open, decimal high, decimal low, decimal close, int volume)
        {
            this._id = id;
            this._stockId = stockId;
            this._symbol = symbol;
            this._dateTime = dateTime;
            this._open = open;
            this._high = high;
            this._low = low;
            this._close = close;
            this._volume = volume;
        }

        public static Price Get(int id)
        {
            return null;
        }

        public void Save()
        {

        }
    }
}
