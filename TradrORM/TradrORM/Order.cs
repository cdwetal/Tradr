using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TradrORM
{
    public class Order
    {
        private int _id = 0;
        public int Id
        {
            get
            {
                return _id;
            }
        }
        public int BrokerId { get; set; }
        public string BrokerOrderId { get; set; }
        public string BrokerAccountId { get; set; }
        public int StockId { get; set; }
        public string Symbol { get; set; }
        public OrderSide Side { get; set; }
        public int Quantity { get; set; }
        public OrderType Type { get; set; }
        public OrderDuration Duration { get; set; }
        public decimal Price { get; set; }
        public decimal Stop { get; set; }
        public OrderStatus Status { get; set; }
        public bool HasChanged
        {
            get
            {
                bool result = false;
                if (_id != _idOrig) result = true;
                if (BrokerId != _brokerIdOrig) result = true;
                if (BrokerOrderId != _brokerOrderIdOrig) result = true;
                if (BrokerAccountId != _brokerAccountIdOrig) result = true;
                if (StockId != _stockIdOrig) result = true;
                if (Symbol != _symbolOrig) result = true;
                if (Side != _sideOrig) result = true;
                if (Quantity != _quantityOrig) result = true;
                if (Type != _typeOrig) result = true;
                if (Duration != _durationOrig) result = true;
                if (Price != _priceOrig) result = true;
                if (Stop != _stopOrig) result = true;
                if (Status != _statusOrig) result = true;
                return result;
            }
        }

        private int _idOrig;
        private int _brokerIdOrig;
        private string _brokerOrderIdOrig;
        private string _brokerAccountIdOrig;
        private int _stockIdOrig;
        private string _symbolOrig;
        private OrderSide _sideOrig;
        private int _quantityOrig;
        private OrderType _typeOrig;
        private OrderDuration _durationOrig;
        private decimal _priceOrig;
        private decimal _stopOrig;
        private OrderStatus _statusOrig;

        public enum OrderSide
        {
            Buy,
            BuyToCover,
            Sell,
            SellShort
        }

        public enum OrderType
        {
            Market,
            Limit,
            Stop,
            StopLimit
        }

        public enum OrderDuration
        {
            Day,
            Gtc,
            Pre,
            Post
        }

        public enum OrderStatus
        {
            Pending,
            Placed,
            Completed,
            Cancelled
        }

        public Order() { }

        public Order(int id)
        {
            Order o = GetById(id);
            if (o != null)
            {
                _id = o.Id;
                BrokerId = o.BrokerId;
                BrokerOrderId = o.BrokerOrderId;
                BrokerAccountId = o.BrokerAccountId;
                StockId = o.StockId;
                Symbol = o.Symbol;
                Side = o.Side;
                Quantity = o.Quantity;
                Type = o.Type;
                Duration = o.Duration;
                Price = o.Price;
                Stop = o.Stop;
                Status = o.Status;
            }
            else
            {
                o._id = id;
            }
            SetOriginalState(this);
        }

        public Order(int brokerId, string brokerOrderId)
        {
            Order o = GetByBrokerOrderId(brokerId, brokerOrderId);
            if (o != null)
            {
                _id = o.Id;
                BrokerId = o.BrokerId;
                BrokerOrderId = o.BrokerOrderId;
                BrokerAccountId = o.BrokerAccountId;
                StockId = o.StockId;
                Symbol = o.Symbol;
                Side = o.Side;
                Quantity = o.Quantity;
                Type = o.Type;
                Duration = o.Duration;
                Price = o.Price;
                Stop = o.Stop;
                Status = o.Status;
            }
            else
            {
                o.BrokerId = brokerId;
                o.BrokerOrderId = brokerOrderId;
            }
            SetOriginalState(this);
        }

        public Order(
            int brokerId,
            string brokerOrderId,
            string brokerAccountId,
            int stockId,
            string symbol,
            OrderSide side,
            int quantity,
            OrderType type,
            OrderDuration duration,
            decimal price,
            decimal stop,
            OrderStatus status)
        {
            BrokerId = brokerId;
            BrokerOrderId = brokerOrderId;
            BrokerAccountId = brokerAccountId;
            StockId = stockId;
            Symbol = symbol;
            Side = side;
            Quantity = quantity;
            Type = type;
            Duration = duration;
            Price = price;
            Stop = stop;
            Status = status;

            SetOriginalState(this);
        }

        private void SetOriginalState(Order order)
        {
            _idOrig = order.Id;
            _brokerIdOrig = order.BrokerId;
            _brokerOrderIdOrig = order.BrokerOrderId;
            _brokerAccountIdOrig = order.BrokerAccountId;
            _stockIdOrig = order.StockId;
            _symbolOrig = order.Symbol;
            _sideOrig = order.Side;
            _quantityOrig = order.Quantity;
            _typeOrig = order.Type;
            _durationOrig = order.Duration;
            _priceOrig = order.Price;
            _stopOrig = order.Stop;
            _statusOrig = order.Status;
        }

        private static Order GetById(int id)
        {
            Order result = null;
            using (SqlConnection con = new SqlConnection(Global.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText =
                        @"
                            SELECT 
                                [Order].* ,
                                AccountID,
                                Symbol
                            FROM [Order] 
                            LEFT JOIN Broker ON [Order].BrokerID = Broker.ID
                            LEFT JOIN Stock ON [Order].StockID = Stock.ID
                            WHERE 
                                [Order].ID = @id
                        ";
                    cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt));
                    cmd.Parameters["@id"].Value = id;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Order();
                            result._id = Convert.ToInt32(reader["ID"].ToString());
                            result.BrokerId = Convert.ToInt32(reader["BrokerID"].ToString());
                            result.BrokerOrderId = reader["BrokerOrderID"].ToString();
                            result.BrokerAccountId = reader["AccountID"].ToString();
                            result.StockId = Convert.ToInt32(reader["StockID"].ToString());
                            result.Symbol = reader["Symbol"].ToString();
                            result.Side = (OrderSide)reader["Side"];
                            result.Quantity = Convert.ToInt32(reader["Quantity"].ToString());
                            result.Type = (OrderType)reader["Type"];
                            result.Duration = (OrderDuration)reader["Duration"];
                            result.Price = Convert.ToDecimal(reader["Price"].ToString());
                            result.Stop = Convert.ToDecimal(reader["Stop"].ToString());
                            result.Status = (OrderStatus)reader["Status"];
                            result.SetOriginalState(result);
                        }
                        reader.Close();
                    }
                }
                con.Close();
            }
            return result;
        }

        private static Order GetByBrokerOrderId(int brokerId, string brokerOrderId)
        {
            Order result = null;
            using (SqlConnection con = new SqlConnection(Global.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText =
                        @"
                            SELECT 
                                [Order].* ,
                                AccountID,
                                Symbol
                            FROM [Order] 
                            LEFT JOIN Broker ON [Order].BrokerID = Broker.ID
                            LEFT JOIN Stock ON [Order].StockID = Stock.ID
                            WHERE 
                                BrokerID = @brokerId AND
                                BrokerOrderID = @brokerOrderId
                        ";
                    cmd.Parameters.Add(new SqlParameter("@brokerId", System.Data.SqlDbType.VarChar, 100));
                    cmd.Parameters["@brokerId"].Value = brokerId;
                    cmd.Parameters.Add(new SqlParameter("@brokerOrderId", System.Data.SqlDbType.VarChar, 100));
                    cmd.Parameters["@brokerOrderId"].Value = brokerOrderId;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Order();
                            result._id = Convert.ToInt32(reader["ID"].ToString());
                            result.BrokerId = Convert.ToInt32(reader["BrokerID"].ToString());
                            result.BrokerOrderId = reader["BrokerOrderID"].ToString();
                            result.BrokerAccountId = reader["AccountID"].ToString();
                            result.StockId = Convert.ToInt32(reader["StockID"].ToString());
                            result.Symbol = reader["Symbol"].ToString();
                            result.Side = (OrderSide)reader["Side"];
                            result.Quantity = Convert.ToInt32(reader["Quantity"].ToString());
                            result.Type = (OrderType)reader["Type"];
                            result.Duration = (OrderDuration)reader["Duration"];
                            result.Price = Convert.ToDecimal(reader["Price"].ToString());
                            result.Stop = Convert.ToDecimal(reader["Stop"].ToString());
                            result.Status = (OrderStatus)reader["Status"];
                            result.SetOriginalState(result);
                        }
                        reader.Close();
                    }
                }
                con.Close();
            }
            return result;
        }

        public void Save()
        {
            if (HasChanged)
            {
                using (SqlConnection con = new SqlConnection(Global.ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText =
                            @"
                                UPDATE [Order]
                                SET
                                    BrokerID = @brokerId,
                                    BrokerOrderID = @brokerOrderId,
                                    StockID = @stockId,
                                    Side = @side,
                                    Quantity = @quantity,
                                    Type = @type,
                                    Duration = @duration,
                                    Price = @price,
                                    Stop = @stop,
                                    Status = @status
                                WHERE
                                    ID = @id
                            ";
                        cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt));
                        cmd.Parameters["@id"].Value = _id;
                        cmd.Parameters.Add(new SqlParameter("@brokerId", System.Data.SqlDbType.BigInt));
                        cmd.Parameters["@brokerId"].Value = BrokerId;
                        cmd.Parameters.Add(new SqlParameter("@brokerOrderId", System.Data.SqlDbType.VarChar, 100));
                        cmd.Parameters["@brokerOrderId"].Value = BrokerOrderId;
                        cmd.Parameters.Add(new SqlParameter("@stockId", System.Data.SqlDbType.BigInt));
                        cmd.Parameters["@stockId"].Value = StockId;
                        cmd.Parameters.Add(new SqlParameter("@side", System.Data.SqlDbType.Int));
                        cmd.Parameters["@side"].Value = (int)Side;
                        cmd.Parameters.Add(new SqlParameter("@quantity", System.Data.SqlDbType.Int));
                        cmd.Parameters["@quantity"].Value = Quantity;
                        cmd.Parameters.Add(new SqlParameter("@type", System.Data.SqlDbType.Int));
                        cmd.Parameters["@type"].Value = (int)Type;
                        cmd.Parameters.Add(new SqlParameter("@duration", System.Data.SqlDbType.Int));
                        cmd.Parameters["@duration"].Value = (int)Duration;
                        cmd.Parameters.Add(new SqlParameter("@price", System.Data.SqlDbType.Decimal));
                        cmd.Parameters["@price"].Value = Price;
                        cmd.Parameters.Add(new SqlParameter("@stop", System.Data.SqlDbType.Decimal));
                        cmd.Parameters["@stop"].Value = Stop;
                        cmd.Parameters.Add(new SqlParameter("@status", System.Data.SqlDbType.Int));
                        cmd.Parameters["@status"].Value = (int)Status;

                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            cmd.CommandText =
                                @"
                                    INSERT INTO [Order] 
                                    (
                                        BrokerID,
                                        BrokerOrderID,
                                        StockID,
                                        Side,
                                        Quantity,
                                        Type,
                                        Duration,
                                        Price,
                                        Stop,
                                        Status
                                    )
                                    OUTPUT INSERTED.ID
                                    VALUES 
                                    (
                                        @brokerId,
                                        @brokerOrderId,
                                        @stockId,
                                        @side,
                                        @quantity,
                                        @type,
                                        @duration,
                                        @price,
                                        @stop,
                                        @status
                                    )
                                ";

                            using (SqlDataReader reader = cmd.ExecuteReader()) 
                            {
                                if (reader.Read())
                                {
                                    _id = Convert.ToInt32(reader["ID"].ToString());
                                    SetOriginalState(this);
                                }
                            }
                        } 
                        else
                        {
                            SetOriginalState(this);
                        }
                    }
                    con.Close();
                }
            }
        }
    }
}
