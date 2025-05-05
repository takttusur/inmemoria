using System.Text.Json;
using System.Text.RegularExpressions;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using TaktTusur.InMemoria.MigrationApp.OldModels;

namespace TaktTusur.InMemoria.MigrationApp.Services;

public class PersonsExporter : IPersonExporter
{
	private readonly MySqlConnection _connection;
	private readonly ILogger<PersonsExporter> _logger;
	private readonly IMapper _mapper;

	public PersonsExporter(MySqlConnection connection, ILogger<PersonsExporter> logger,
		IMapper mapper)
	{
		_connection = connection;
		_logger = logger;
		_mapper = mapper;
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

		var converted = ConvertLinks(person);
		var mapped = _mapper.Map<OldModels.Person, Person>(converted);
		File.WriteAllText(fileName, JsonSerializer.Serialize(mapped));
	}

	private Person ConvertLinks(Person person)
	{
		// PhotoBig examples:
		// http://inmemoria.tusur.ru/media/20120704102731.jpg
		person.PhotoBig = ExtractPhoto(person.PhotoBig);

		// PhotoSmall examples:
		// http://inmemoria.tusur.ru/media/20120704102731s.jpg
		person.PhotoSmall = ExtractPhoto(person.PhotoSmall);

		// Media -> Data examples
		// http://inmemoria.tusur.ru/media/3/20120530114041.jpg

		var innerLinkingTypes = new[] { "img", "video", "audio", "doc" };
		foreach (var mediaResource in person.MediaResources)
		{
			if (!innerLinkingTypes.Contains(mediaResource.ResourceType.ToLower())) continue;
			mediaResource.Data = ExtractMedia(mediaResource.Data);
		}

		return person;
	}

	private string? ExtractPhoto(string url)
	{
		var regex = new Regex(@"(?<=media\/).+$");
		var match = regex.Match(url);

		return match.Success ? match.Value : null;
	}

	private string ExtractMedia(string url)
	{
		var regex = new Regex(@"(?<=\/)[^\/]+$");
		var match = regex.Match(url);

		if (match.Success)
		{
			return match.Value;
		}

		throw new ApplicationException("Unknown resource link:" + url);
	}
}
