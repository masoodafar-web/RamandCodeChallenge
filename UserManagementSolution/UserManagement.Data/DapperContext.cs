using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace UserManagement.Data;

public class DapperContext
{
    private DbSettings _dbSettings;

    public DapperContext(IOptions<DbSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = $"Server={_dbSettings.Server}; Database={_dbSettings.Database};ConnectRetryCount={_dbSettings.ConnectRetryCount};";
        return new SqlConnection(connectionString);
    }

    public async Task Init()
    {
        await _initDatabase();
        await _initTables();
    }

    private async Task _initDatabase()
    {
        // create database if it doesn't exist
        var connectionString = $"Server={_dbSettings.Server}; Database=master;ConnectRetryCount={_dbSettings.ConnectRetryCount};";
        using var connection = new SqlConnection(connectionString);
        var sql = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{_dbSettings.Database}') CREATE DATABASE [{_dbSettings.Database}];";
        await connection.ExecuteAsync(sql);
    }

    private async Task _initTables()
    {
        // create tables if they don't exist
        using var connection = CreateConnection();
        await _initUsers();

        async Task _initUsers()
        {
            var sql = """
                      IF OBJECT_ID('Users', 'U') IS NULL
                      BEGIN
                          CREATE TABLE Users (
                              Id BIGINT NOT NULL PRIMARY KEY IDENTITY,
                              UserName NVARCHAR(MAX),
                              FirstName NVARCHAR(MAX),
                              LastName NVARCHAR(MAX),
                              NationalId NVARCHAR(10) NOT NULL,
                              Age INT,
                              PasswordHash NVARCHAR(MAX)
                          );
                      
                          CREATE UNIQUE INDEX IX_Users_NationalId ON Users(NationalId);
                      END
                      """;

            await connection.ExecuteAsync(sql);
        }
    }

}