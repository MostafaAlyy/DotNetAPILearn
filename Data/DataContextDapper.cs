using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotNetAPILearn.Data
{

    class DataContextDapper
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _dbConnection;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
            _dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        }

        public IEnumerable<T> LoadData<T>(string query)
        {
            return _dbConnection.Query<T>(query);
        }
        public T LoadDataSingle<T>(string query)
        {
            return _dbConnection.QuerySingle<T>(query);
        }

        public bool ExecuteSql(string sql)
        {
            return _dbConnection.Execute(sql) > 0;
        }
        public int ExecuteSqlWithCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return _dbConnection.Execute(sql);
        }




    }
}