using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace CryptoBackend.Utils
{
    public static class Database
    {
        public static IDbConnection Master
        {
            get
            {
                var connectionString = Config.Default.ConnectionString;
                var connection = new NpgsqlConnection(connectionString);
                
                connection.Open();

                return connection;
            }
        }

        public static IEnumerable<T> Many<T>(this IDbConnection connection, string sql, object param = null)
        {
            var result = connection.Query<T>(sql, param);

            connection.Close();

            return result;
        }

        public static T One<T>(this IDbConnection connection, string sql, object param = null)
        {
            IEnumerable<T> enumerable = connection.Query<T>(sql, param);
            IEnumerator<T> enumerator = enumerable.GetEnumerator();

            if(enumerator.MoveNext())
            {
                // As we are only interested in the first element, return it if it exists.
                connection.Close();

                return enumerator.Current;
            }

            connection.Close();
            return default(T);
        }

        public static int Run(this IDbConnection connection, string sql, object param = null)
        {
            var result = connection.Execute(sql, param);

            connection.Close();

            return result;
        }
    }
}
