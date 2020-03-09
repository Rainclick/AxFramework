using System;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Data.Repositories
{
    public class QueryExecutor : IDisposable
    {
        const string ConnectionString = "Server=185.211.57.94;Database=Nid_BPMS;User Id=sa;Password=pint@p1398energy;";
        public SqlConnection Connection;
        public QueryExecutor()
        {
            Connection = new SqlConnection(ConnectionString);
        }

        public T RunFirstOrDefault<T>(string query)
        {

            var data = Connection.QueryFirstOrDefault<T>(query);
            return data;
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
