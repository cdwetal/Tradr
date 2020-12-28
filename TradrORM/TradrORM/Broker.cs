using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TradrORM
{
    public class Broker
    {
        private int _id = 0;
        public int Id
        {
            get
            {
                return _id;
            }
        }
        public string Name { get; set; }
        public string AccountId { get; set; }
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        public bool HasChanged
        {
            get
            {
                bool result = false;
                if (Id != _idOrig) result = true;
                if (Name != _nameOrig) result = true;
                if (AccountId != _accountIdOrig) result = true;
                if (ApiKey != _apiKeyOrig) result = true;
                if (BaseUrl != _baseUrlOrig) result = true;
                return result;
            }
        }

        private int _idOrig;
        private string _nameOrig;
        private string _accountIdOrig;
        private string _apiKeyOrig;
        private string _baseUrlOrig;

        public Broker() { }

        public Broker(int id)
        {
            Broker b = GetById(id);
            if (b != null)
            {
                _id = b.Id;
                Name = b.Name;
                AccountId = b.AccountId;
                ApiKey = b.ApiKey;
                BaseUrl = b.BaseUrl;
            }
            else
            {
                b._id = id;
            }
            SetOriginalState(this);
        }

        public Broker(string name, string accountId, string apiKey, string baseUrl)
        {
            Name = name;
            AccountId = accountId;
            ApiKey = apiKey;
            BaseUrl = baseUrl;
             
            SetOriginalState(this);
        }

        private void SetOriginalState(Broker broker)
        {
            _idOrig = broker.Id;
            _nameOrig = broker.Name;
            _accountIdOrig = broker.AccountId;
            _apiKeyOrig = broker.ApiKey;
            _baseUrlOrig = broker.BaseUrl;
        }

        private static Broker GetById(int id)
        {
            Broker result = null;
            using (SqlConnection con = new SqlConnection(Global.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText =
                        @"
                            SELECT * 
                            FROM Broker 
                            WHERE 
                                ID = @id
                        ";
                    cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt));
                    cmd.Parameters["@id"].Value = id;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Broker();
                            result.Name = reader["Name"].ToString();
                            result.AccountId = reader["AccountID"].ToString();
                            result.ApiKey = reader["APIKey"].ToString();
                            result.BaseUrl = reader["BaseURL"].ToString();
                            reader.Close();
                        }
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
                                    Name = @name,
                                    AccountID = @accountId,
                                    APIKey = @apiKey,
                                    BaseURL = @baseUrl
                                WHERE
                                    ID = @id
                            ";
                        cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt));
                        cmd.Parameters["@id"].Value = Id;
                        cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar, 255));
                        cmd.Parameters["@name"].Value = Name;
                        cmd.Parameters.Add(new SqlParameter("@accountId", System.Data.SqlDbType.VarChar, 100));
                        cmd.Parameters["@accountId"].Value = AccountId;
                        cmd.Parameters.Add(new SqlParameter("@apiKey", System.Data.SqlDbType.VarChar, 1000));
                        cmd.Parameters["@APIKey"].Value = ApiKey;
                        cmd.Parameters.Add(new SqlParameter("@BaseURL", System.Data.SqlDbType.VarChar, 1000));
                        cmd.Parameters["@baseUrl"].Value = BaseUrl;

                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            cmd.CommandText =
                                @"
                                    INSERT INTO Stock 
                                    (
                                        Name,
                                        AccountId,
                                        APIKey,
                                        BaseURL
                                    )
                                    OUTPUT INSERTED.ID
                                    VALUES 
                                    (
                                        @name,
                                        @accountId,
                                        @apiKey,
                                        @baseUrl
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
