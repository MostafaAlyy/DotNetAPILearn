using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotNetAPILearn.Data
{

    class DataContextDapper
    {
        private readonly IConfiguration _config;
        private readonly SqlConnection _dbConnection;

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
            return _dbConnection.Execute(sql);
        }


        public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
        {
            SqlCommand sqlCommand = new(sql);

            foreach (SqlParameter parameter in parameters)
                sqlCommand.Parameters.Add(parameter);

            _dbConnection.Open();
            sqlCommand.Connection = _dbConnection;

            int rowAffected = sqlCommand.ExecuteNonQuery();
            _dbConnection.Close();
            return rowAffected > 0;
        }

    }
}