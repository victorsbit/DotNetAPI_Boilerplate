using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace PlaygroundAPI.Data;

public class DataContextDapper
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;
    public DataContextDapper(IConfiguration config)
    {
        _config = config;
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    public IEnumerable<T> LoadData<T>(string queryString)
    {
        IDbConnection dbConnection = new SqlConnection(_connectionString);
        return dbConnection.Query<T>(queryString);
    }

    public T LoadSingleData<T>(string queryString)
    {
        IDbConnection dbConnection = new SqlConnection(_connectionString);
        return dbConnection.QuerySingle<T>(queryString);
    }

    public bool ExecuteInsert(string queryString)
    {
        IDbConnection dbConnection = new SqlConnection(_connectionString);
        return dbConnection.Execute(queryString) > 0;
    }

    public int ExecuteInsertWithRowCount(string queryString)
    {
        IDbConnection dbConnection = new SqlConnection(_connectionString);
        return dbConnection.Execute(queryString);
    }
}
