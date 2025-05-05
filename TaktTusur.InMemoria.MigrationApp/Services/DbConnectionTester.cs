using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace TaktTusur.InMemoria.MigrationApp.Services;

public class DbConnectionTester : IDbConnectionTester
{
	private readonly MySqlConnection _connection;
	private readonly ILogger<DbConnectionTester> _logger;

	public DbConnectionTester(MySqlConnection connection, ILogger<DbConnectionTester> logger)
	{
		_connection = connection;
		_logger = logger;
	}

	public void Test(int maxRetry = 1)
	{
		while (maxRetry > 0)
		{
			try
			{
				_connection.Open();
				System.Threading.Thread.Sleep(500);
				_connection.Close();
				return;
			}
			catch (MySqlException)
			{
				_logger.LogInformation("Cannot connect to the database. Retrying in 5 seconds...");
				System.Threading.Thread.Sleep(5000);
				maxRetry--;
			}
		}

		throw new ApplicationException("DB was not reached after few retries.");
	}
}
