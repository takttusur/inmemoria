using System.Text.Json;
using System.Text.Json.Serialization;
using Dapper;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using TaktTusur.InMemoria.MigrationApp.OldModels;

namespace TaktTusur.InMemoria.MigrationApp.Services;

public class SettingsExporter : ISettingsExporter
{
	private readonly MySqlConnection _connection;
	private readonly ILogger<SettingsExporter> _logger;

	public SettingsExporter(MySqlConnection connection, ILogger<SettingsExporter> logger)
	{
		_connection = connection;
		_logger = logger;
	}

	public int ExportTo(string fileName)
	{
		var sql = "SELECT * FROM ini";
		var settings = _connection.Query<Settings>(sql).ToList();

		var json = JsonSerializer.Serialize(settings);
		File.WriteAllText(fileName, json);

		return settings.Count;
	}
}
