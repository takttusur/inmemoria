using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using TaktTusur.InMemoria.MigrationApp.OldModels;

namespace TaktTusur.InMemoria.MigrationApp.Services;

public class PersonsExporter : IPersonExporter
{
	private readonly MySqlConnection _connection;
	private readonly ILogger<PersonsExporter> _logger;

	public PersonsExporter(MySqlConnection connection, ILogger<PersonsExporter> logger)
	{
		_connection = connection;
		_logger = logger;
	}

	public IEnumerable<int> GetEntityIdsForExport()
	{
		var sql = "SELECT id FROM person";
		var entities = _connection.Query<int>(sql);
		return entities;
	}

	public void Export(int id, string fileName)
	{
		var personSql = "SELECT * FROM person WHERE id = @id";
		var person = _connection.QuerySingle<Person>(personSql, new
		{
			id
		});
		var mediaSql = "SELECT * FROM resource where person_id = @personId";
		var personId = person.Id;
		person.MediaResources = _connection.Query<MediaResource>(mediaSql, new
		{
			personId
		}).ToList();
		File.WriteAllText(fileName, JsonSerializer.Serialize(person));
	}
}
