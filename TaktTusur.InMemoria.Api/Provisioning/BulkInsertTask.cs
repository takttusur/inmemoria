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

		var people = Directory.GetFiles(directory, "person*", SearchOption.TopDirectoryOnly);
		if (people.Length == 0)
		{
			_logger.LogInformation("No People found. Search pattern: {searchPattern}. Skipping People provisioning.", "person*");
		}
		else
		{
			ProvisionPeople(people);
		}

	}

	private void ProvisionEpigraphs(string[] filePaths)
	{
		int changesCounter = 0;
		foreach (var file in filePaths)
		{
			_logger.LogDebug("Reading {path} file", file);

			var json = File.ReadAllText(file);
			Epigraph? jsonModel = System.Text.Json.JsonSerializer.Deserialize<Epigraph>(json);

			if (jsonModel == null)
			{
				_logger.LogWarning("File {path} deserialized to null Epigraph model. Skipping.", file);
				continue;
			}

			var key = jsonModel.Text[..25];

			var dbModel = _dbContext.Epigraphs.FirstOrDefault(e => e.Text.StartsWith(key));
			if (dbModel != null)
			{
				_logger.LogDebug("The Epigraph with text '{text}' already exists", key);
				continue;
			}
			var model = new Epigraph
			{
				Text = jsonModel.Text
			};

			_dbContext.Add<Epigraph>(model);
			var changesCount = _dbContext.SaveChanges();
			changesCounter += changesCount;

			_logger.LogDebug("The Epigraph with text '{text}' processed. Changes count: {changes}",key, changesCount);
		}

		_logger.LogInformation("Finished Epigraphs provisioning. Changes count: {changes}", changesCounter);
	}

	private void ProvisionPeople(string[] filePaths)
	{
		int changesCounter = 0;
		foreach (var file in filePaths)
		{
			_logger.LogDebug("Reading {path} file", file);

			var json = File.ReadAllText(file);
			var jsonModel = System.Text.Json.JsonSerializer.Deserialize<Person>(json);

			if (jsonModel == null)
			{
				_logger.LogWarning("File {path} deserialized to null Person model. Skipping.", file);
				continue;
			}


			var dbModel = _dbContext.Persons
				.FirstOrDefault(p => p.FirstName == jsonModel.FirstName && p.LastName == jsonModel.LastName && p.Birthday == jsonModel.Birthday);
			if (dbModel != null)
			{
				_logger.LogDebug(
					"The Person '{firstName}' '{lastName}' '{dob}' already exists",
					jsonModel.FirstName,
					jsonModel.LastName,
					jsonModel.Birthday);
				continue;
			}

			var model = new Person
			{
				FirstName = jsonModel.FirstName,
				LastName = jsonModel.LastName,
				MaidenName = jsonModel.MaidenName,
				Patronymic = jsonModel.Patronymic,
				Nickname = jsonModel.Nickname,
				Birthday = jsonModel.Birthday,
				DeathDay = jsonModel.DeathDay,
				Biography = jsonModel.Biography,
				Active = jsonModel.Active,
				AttachmentsStorageId = jsonModel.AttachmentsStorageId
			};

			_dbContext.Add<Person>(model);
			var changesCount = _dbContext.SaveChanges();
			changesCounter += changesCount;

			_logger.LogDebug("The Person with full name '{name}' processed. Changes count: {changes}", string.Concat(jsonModel.FirstName,"/", jsonModel.LastName), changesCount);
		}

		_logger.LogInformation("Finished People provisioning. Changes count: {changes}", changesCounter);
	}
}