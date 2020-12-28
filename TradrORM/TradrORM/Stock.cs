using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TradrORM
{
    public class Stock
    {
        private int _id = 0;
        public int Id
        {
            get
            {
                return _id;
            }
        }   
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
        public bool HasChanged
        {
            get
            {
                bool result = false;
                if (Id != _idOrig) result = true;
                if (Symbol != _symbolOrig) result = true;
                if (Name != _nameOrig) result = true;
                if (Sector != _sectorOrig) result = true;
                if (Industry != _industryOrig) result = true;
                return result;
            }
        }

        private int _idOrig;
        private string _symbolOrig;
        private string _nameOrig;
        public int _yearOrig;
        private string _sectorOrig;
        private string _industryOrig;

        public Stock() { }

        public Stock(int id)
        {
            Stock s = GetById(id);
            if (s != null)
            {
                _id = s.Id;
                Symbol = s.Symbol;
                Name = s.Name;
                Year = s.Year;
                Sector = s.Sector;
                Industry = s.Industry;
            } 
            else
            {
                s._id = id;
            }
            SetOriginalState(this);
        }

        public Stock(string symbol)
        {
            Stock s = GetBySymbol(symbol);
            if (s != null)
            {
                _id = s.Id;
                Symbol = s.Symbol;
                Name = s.Name;
                Year = s.Year;
                Sector = s.Sector;
                Industry = s.Industry;
            }
            else
            {
                Symbol = symbol;
            }
            SetOriginalState(this);
        }

        public Stock(string symbol, string name, int ipoYear, string sector, string industry)
        {
            Symbol = symbol;
            Name = name;
            Year = ipoYear;
            Sector = sector;
            Industry = industry;

            SetOriginalState(this);
        }

        private void SetOriginalState(Stock stock)
        {
            _idOrig = stock.Id;
            _symbolOrig = stock.Symbol;
            _nameOrig = stock.Name;
            _yearOrig = stock.Year;
            _sectorOrig = stock.Sector;
            _industryOrig = stock.Industry;
        }

        private static Stock GetById(int id)
        {
            Stock result = null;
            using (SqlConnection con = new SqlConnection(Global.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = 
                        @"
                            SELECT * 
                            FROM Stock 
                            WHERE 
                                ID = @id
                        ";
                    cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt));
                    cmd.Parameters["@id"].Value = id;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Stock();
                            result._id = Convert.ToInt32(reader["ID"].ToString());
                            result.Symbol = reader["Symbol"].ToString();
                            result.Name = reader["Name"].ToString();
                            result.Year = Convert.ToInt32(reader["Year"].ToString());
                            result.Sector = reader["Sector"].ToString();
                            result.Industry = reader["Industry"].ToString();
                            result.SetOriginalState(result);
                        }
                        reader.Close();
                    }
                }
                con.Close();
            }
            return result;
        }

        private static Stock GetBySymbol(string symbol)
        {
            Stock result = null;
            using (SqlConnection con = new SqlConnection(Global.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = 
                        @"
                            SELECT * 
                            FROM Stock 
                            WHERE 
                                Symbol = @symbol
                        ";
                    cmd.Parameters.Add(new SqlParameter("@symbol", System.Data.SqlDbType.VarChar, 10));
                    cmd.Parameters["@symbol"].Value = symbol;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Stock();
                            result._id = Convert.ToInt32(reader["ID"].ToString());
                            result.Symbol = reader["Symbol"].ToString();
                            result.Name = reader["Name"].ToString();
                            result.Year = Convert.ToInt32(reader["Year"].ToString());
                            result.Sector = reader["Sector"].ToString();
                            result.Industry = reader["Industry"].ToString();
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
                                UPDATE Stock
                                SET
                                    Symbol = @symbol,
                                    Name = @name,
                                    Year = @year,
                                    Sector = @sector,
                                    Industry = @industry
                                WHERE
                                    ID = @id
                            ";
                        cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt));
                        cmd.Parameters["@id"].Value = Id;
                        cmd.Parameters.Add(new SqlParameter("@symbol", System.Data.SqlDbType.VarChar, 10));
                        cmd.Parameters["@symbol"].Value = Symbol;
                        cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar, 255));
                        cmd.Parameters["@name"].Value = Name;
                        cmd.Parameters.Add(new SqlParameter("@year", System.Data.SqlDbType.Int));
                        cmd.Parameters["@year"].Value = Year;
                        cmd.Parameters.Add(new SqlParameter("@sector", System.Data.SqlDbType.VarChar, 255));
                        cmd.Parameters["@sector"].Value = Sector;
                        cmd.Parameters.Add(new SqlParameter("@industry", System.Data.SqlDbType.VarChar, 255));
                        cmd.Parameters["@industry"].Value = Industry;

                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            cmd.CommandText =
                                @"
                                    INSERT INTO Stock 
                                    (
                                        Symbol, 
                                        Name, 
                                        Year, 
                                        Sector, 
                                        Industry
                                    )
                                    OUTPUT INSERTED.ID
                                    VALUES 
                                    (
                                        @symbol, 
                                        @name, 
                                        @year, 
                                        @sector, 
                                        @industry
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
