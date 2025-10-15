using Microsoft.EntityFrameworkCore;
using TaktTusur.InMemoria.Api.Provisioning;
using TaktTusur.InMemoria.DataAccess.Context;

namespace TaktTusur.InMemoria.Api;

public class Program
{
	private const string DbProvider = "DbProvider";
	private const string DbProviderSqlite = "Sqlite";
	private const string DbProviderPostgreSQL = "PostgreSQL";

	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		var dbProvider = builder.Configuration.GetValue<string>(DbProvider);
		var connectionString = builder.Configuration.GetConnectionString("InMemoriaDb");
		switch (dbProvider)
		{
			case DbProviderSqlite:
				builder.Services.AddDbContext<InMemoriaDbContext>(options =>
					options.UseSqlite(connectionString));
				break;
			case DbProviderPostgreSQL:
				builder.Services.AddDbContext<InMemoriaDbContext>(options =>
					options.UseNpgsql(connectionString));
				break;
			default:
				throw new InvalidOperationException($"Unsupported database provider: {dbProvider}");
		}

		builder.Services.AddTransient<SqliteDbCreateTask>();
		builder.Services.AddTransient<BulkInsertTask>();

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		var scope = ((IEndpointRouteBuilder)app).ServiceProvider.CreateAsyncScope();
		Provisioning(scope);
		scope.Dispose();

		app.Run();
	}

	public static void Provisioning(IServiceScope scope)
	{
		var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
		var logger = loggerFactory.CreateLogger<Program>();

		// Add your provisioning tasks here
		// Ensure tasks are registered in the DI container
		// e.g., services.AddTransient<YourProvisioningTask>();
		var provisioningList = new List<IProvisioningTask>
		{
			scope.ServiceProvider.GetRequiredService<SqliteDbCreateTask>(),
			scope.ServiceProvider.GetRequiredService<BulkInsertTask>(),
		};

		foreach (var task in provisioningList)
		{
			try
			{
				logger.LogInformation("Starting provisioning task: {TaskName}", task.GetType().Name);
				task.Provision();
				logger.LogInformation("Completed provisioning task: {TaskName}", task.GetType().Name);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error during provisioning task: {TaskName}", task.GetType().Name);
				throw;
			}
		}
	}
}
