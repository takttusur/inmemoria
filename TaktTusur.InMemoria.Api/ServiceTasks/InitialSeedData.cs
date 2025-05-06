using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaktTusur.InMemoria.DataAccess.Context;
using TaktTusur.InMemoria.Domain.Entities;

namespace TaktTusur.InMemoria.Api.ServiceTasks;

public class InitialSeedData
{
    private readonly InMemoriaDbContext _dbContext;
    private readonly IOptions<SeedDataOptions> _options;
    private readonly ILogger<InitialSeedData> _logger;

    public InitialSeedData(InMemoriaDbContext dbContext, IOptions<SeedDataOptions> options, ILogger<InitialSeedData> logger)
    {
        _dbContext = dbContext;
        _options = options;
        _logger = logger;
    }

    public void RestoreDatabase()
    {
        // Get json files
        var directory = new DirectoryInfo(_options.Value.PersonsDirectory);
        var files = directory.GetFiles("*.json");

        _logger.LogInformation("Start seeding data. Found {records} records", files.Length);
        var counter = 0;

        foreach (var file in files)
        {
            var jsonData = File.ReadAllText(file.FullName);
            var person = JsonSerializer.Deserialize<Person>(jsonData);

            // Check if person already exists in the database
            if (!_dbContext.Persons
                .Any(p => p.FirstName == person.FirstName && p.LastName == person.LastName && p.Birthday == person.Birthday))
            {
                // Add person and attachments to context and save
                _dbContext.Persons.Add(person);
                _dbContext.SaveChanges();
                counter++;
            }
        }

        _logger.LogInformation("Finish seeding data. Inserted {records} records", counter);
    }
}
