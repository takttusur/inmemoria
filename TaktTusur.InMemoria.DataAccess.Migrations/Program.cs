using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TaktTusur.InMemoria.DataAccess.Migrations;

public class Program
{
	public static void Main(string[] args)
	{
		var connectionString =
			args.FirstOrDefault(a => a.StartsWith("connectionString=", StringComparison.OrdinalIgnoreCase))?.Split('=')[1]
			?? Environment.GetEnvironmentVariable("ConnectionStrings__InMemoriaDb")
			?? throw new InvalidOperationException("Connection string must be provided via argument or environment variable.");

		var serviceProvider = new ServiceCollection()
			.AddFluentMigratorCore()
			.ConfigureRunner(rb => rb
				.AddPostgres()
				.WithGlobalConnectionString(connectionString)
				.ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
			.AddLogging(lb => lb.AddFluentMigratorConsole())
			.BuildServiceProvider();

		using var scope = serviceProvider.CreateScope();
		var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
		runner.MigrateUp();
	}
}
