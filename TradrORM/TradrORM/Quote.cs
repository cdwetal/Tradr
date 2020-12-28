using System;
using System.Collections.Generic;
using System.Text;

namespace TradrORM
{
    public class Quote
    {
        private int _stockId;
        private string _symbol;
        private DateTime _tradeDate;
        private decimal _last;
        private int _lastVolume;
        private decimal _change;
        private decimal _changePercentage;
        private decimal _open;
        private decimal _high;
        private decimal _low;
        private decimal _previousClose;
        private decimal _close;
        private int _volume;
        private int _averageVolume;
        private decimal _bid;
        private int _bidSize;
        private DateTime _bidDate;
        private decimal _ask;
        private int _askSize;
        private DateTime _askDate;
        private decimal _week52High;
        private decimal _week52Low;

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
        public DateTime TradeDate
        {
            get
            {
                return _tradeDate;
            }
        }
        public decimal Last
        {
            get
            {
                return _last;
            }
        }
        public int LastVolume
        {
            get
            {
                return _lastVolume;
            }
        }
        public decimal Change
        {
            get
            {
                return _change;
            }
        }
        public decimal ChangePercentage
        {
            get
            {
                return _changePercentage;
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
        public decimal PreviousClose
        {
            get
            {
                return _previousClose;
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
        public int AverageVolume
        {
            get
            {
                return _averageVolume;
            }
        }
        public decimal Bid
        {
            get
            {
                return _bid;
            }
        }
        public int BidSize
        {
            get
            {
                return _bidSize;
            }
        }
        public DateTime BidDate
        {
            get
            {
                return _bidDate;
            }
        }
        public decimal Ask
        {
            get
            {
                return _ask;
            }
        }
        public int AskSize
        {
            get
            {
                return _askSize;
            }
        }
        public DateTime AskDate
        {
            get
            {
                return _askDate;
            }
        }
        public decimal Week52High
        {
            get
            {
                return _week52High;
            }
        }
        public decimal Week52Low
        {
            get
            {
                return _week52Low;
            }
        }

        public Quote(
            int symbolId,
            string symbol,
            DateTime tradeDate,
            decimal last,
            int lastVolume,
            decimal change,
            decimal changePercentage,
            decimal open,
            decimal high,
            decimal low,
            decimal previousClose,
            decimal close,
            int volume,
            int averageVolume,
            decimal bid,
            int bidSize,
            DateTime bidDate,
            decimal ask,
            int askSize,
            DateTime askDate,
            decimal week52High,
            decimal week52Low)
        {
            _stockId = symbolId;
            _symbol = symbol;
            _tradeDate = tradeDate;
            _last = last;
            _lastVolume = lastVolume;
            _change = change;
            _changePercentage = changePercentage;
            _open = open;
            _high = high;
            _low = low;
            _previousClose = previousClose;
            _close = close;
            _volume = volume;
            _averageVolume = averageVolume;
            _bid = bid;
            _bidSize = bidSize;
            _bidDate = bidDate;
            _ask = ask;
            _askSize = askSize;
            _askDate = askDate;
            _week52High = week52High;
            _week52Low = week52Low;
        }
    }
}
