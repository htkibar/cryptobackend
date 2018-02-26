using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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

        public static T Run<T>(this IDbConnection connection, string sql, object param = null)
        {
            var result = connection.Query<T>(sql, param).FirstOrDefault();
            connection.Close();
            return result;
        }

        public static void RunAsync(this IDbConnection connection, List<Tuple<string, object>> queries)
        {
            var aggregateQuery = "";
            foreach (Tuple<string, object> query in queries) {
                var sql = query.Item1;
                var param = query.Item2;
                var literalReplacedSql = sql;
                
                var properties = param.GetType().GetProperties();

                foreach (PropertyInfo prop in properties) {
                    var value = prop.GetValue(param);
                    var name = "@" + prop.Name;

                    if (value != null && (value.GetType().Name == "String" || value.GetType().Name == "Guid")) {
                        literalReplacedSql = literalReplacedSql.Replace(name, "'" + value.ToString() + "'");
                    } else if (value != null) {
                        literalReplacedSql = literalReplacedSql.Replace(name, value.ToString());
                    } else {
                        literalReplacedSql = literalReplacedSql.Replace(name, "NULL");
                    }
                }

                aggregateQuery += literalReplacedSql + ";";
            }

            connection.QueryMultiple(aggregateQuery);

            //connection.Close();
        }
    }
}
