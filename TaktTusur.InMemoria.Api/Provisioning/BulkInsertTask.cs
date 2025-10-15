using TaktTusur.InMemoria.DataAccess.Context;
using TaktTusur.InMemoria.Domain.Entities;

namespace TaktTusur.InMemoria.Api.Provisioning;

public class BulkInsertTask : IProvisioningTask
{
	private readonly ILogger<BulkInsertTask> _logger;
	private readonly InMemoriaDbContext _dbContext;
	private readonly IConfiguration _configuration;

	public BulkInsertTask(ILogger<BulkInsertTask> logger, InMemoriaDbContext dbContext, IConfiguration configuration)
	{
		_logger = logger;
		_dbContext = dbContext;
		_configuration = configuration;
	}

	public async Task InsertEpighraphs(IList<Epigraph> data, CancellationToken cancellationToken = default)
	{
		var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

		foreach (var e in data)
		{
			_dbContext.Epigraphs.Add(e);
		}

		await transaction.CommitAsync(cancellationToken);
	}

	public void Provision()
	{
		_logger.LogInformation("Starting Bulk Insert Task...");

		const string provisioningDirKey = "ProvisioningDirectory";
		const string epigraphFileSearchPattern = "epigraph*";
		var directory = _configuration.GetValue<string>(provisioningDirKey);

		if (!Directory.Exists(directory))
		{
			_logger.LogInformation("Provisioning directory '{path}' doesn't exist. Skipping provisioning.", directory);
			return;
		}

		var epigraphs = Directory.GetFiles(directory, epigraphFileSearchPattern, SearchOption.TopDirectoryOnly);
		if (epigraphs.Length == 0)
		{
			_logger.LogInformation("No Epigraphs found. Search pattern: {searchPattern}. Skipping Epighraphs provisioning.", epigraphFileSearchPattern);
		}
		else
		{
			ProvisionEpigraphs(epigraphs);
		}

	}

	private void ProvisionEpigraphs(string[] filePaths)
	{
		int changesCounter = 0;
		foreach (var file in filePaths)
		{
			_logger.LogDebug("Reading {path} file", file);

			var json = File.ReadAllText(file);
			var model = new Epigraph(); //TODO:
			var key = model.Text[..50];

			var dbModel = _dbContext.Epigraphs.FirstOrDefault(e => e.Text == model.Text);
			if (dbModel != null)
			{
				_logger.LogDebug("The Epigraph with text '{text}' already exists", key);
				continue;
			}

			_dbContext.Add<Epigraph>(model);
			var changesCount = _dbContext.SaveChanges();
			changesCounter += changesCount;

			_logger.LogDebug("The Epigraph with text '{text}' processed. Changes count: {changes}",key, changesCount);
		}

		_logger.LogInformation("Finished Epigraphs provisioning. Changes count: {changes}", changesCounter);
	}
}
