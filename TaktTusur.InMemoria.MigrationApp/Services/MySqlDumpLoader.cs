using Dapper;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace TaktTusur.InMemoria.MigrationApp.Services;

public class MySqlDumpLoader : IMySqlDumpLoader
{
	private readonly MySqlConnection _connection;
	private readonly ILogger<MySqlDumpLoader> _logger;

	public MySqlDumpLoader(MySqlConnection connection, ILogger<MySqlDumpLoader> logger)
	{
		_connection = connection;
		_logger = logger;
	}

	public void Load(string sql)
	{
		_logger.LogInformation("Start loading MySQL dump...");
		var result = _connection.Execute(sql);
		if (result == 0) throw new ApplicationException("No rows affected, looks like script doesnt work");
		_logger.LogInformation("Loading finished, affected rows count: {0}", result);
	}
}
