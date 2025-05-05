using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TaktTusur.InMemoria.MigrationApp.Configuration;

namespace TaktTusur.InMemoria.MigrationApp.Services;

public class ApplicationService : IApplicationService
{
	private readonly IMySqlDumpLoader _mySqlDumpLoader;
	private readonly IConfiguration _configuration;
	private readonly ISettingsExporter _settingsExporter;
	private readonly IPersonExporter _personExporter;
	private readonly ILogger<ApplicationService> _logger;

	public ApplicationService(IMySqlDumpLoader mySqlDumpLoader, IConfiguration configuration,
		ISettingsExporter settingsExporter, IPersonExporter personExporter,
		ILogger<ApplicationService> logger)
	{
		_mySqlDumpLoader = mySqlDumpLoader;
		_configuration = configuration;
		_settingsExporter = settingsExporter;
		_personExporter = personExporter;
		_logger = logger;
	}

	public void DoExport()
	{
		// Restore DB using sql file
		var backupSql = File.ReadAllText("./Resources/inmemoria.sql");
		_mySqlDumpLoader.Load(backupSql);

		// Read export settings
		var exportConfiguration = _configuration.GetSection(ExportConfiguration.SECTION_NAME)
			.Get<ExportConfiguration>();

		// Saving Settings to file
		var exported = _settingsExporter.ExportTo(exportConfiguration.SettingsFile);
		_logger.LogInformation("Settings Exported: {settingsCount}", exported);

		// Saving Persons
		var ids = _personExporter.GetEntityIdsForExport();
		int personCounter = 0;
		foreach (var id in ids)
		{
			const string ext = ".json";
			var file = Path.Combine(exportConfiguration.PersonsDirectory, id + ext);
			if (File.Exists(file)) continue;
			_personExporter.Export(id, file);
			personCounter++;
		}
		_logger.LogInformation("Person exported: {personCount}", personCounter);
	}
}
