using Microsoft.EntityFrameworkCore;
using TaktTusur.InMemoria.DataAccess.Context;

namespace TaktTusur.InMemoria.Api.Provisioning;

public class SqliteDbCreateTask : IProvisioningTask
{
	private readonly ILogger<SqliteDbCreateTask> _logger;
	private readonly InMemoriaDbContext _dbContext;

	public SqliteDbCreateTask(ILogger<SqliteDbCreateTask> logger, InMemoriaDbContext dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
	}

	public void Seed()
	{
		_dbContext.Database.EnsureCreated();
	}

	public void Provision()
	{
		if (!_dbContext.Database.IsSqlite())
		{
			_logger.LogInformation("The provided database is not SQLite. Skipping SQLite provisioning task.");
			return;
		}
		try
		{
			_logger.LogInformation("Starting SQLite database provisioning task.");
			Seed();
			_logger.LogInformation("SQLite database provisioning task completed successfully.");
		}
		catch (Exception e)
		{
			_logger?.LogError(e, "Error in creating Database.");
			throw;
		}
	}
}
