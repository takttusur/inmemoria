using Microsoft.EntityFrameworkCore;
using TaktTusur.InMemoria.DataAccess.Context;
using Microsoft.Extensions.Configuration;
using TaktTusur.InMemoria.Api.ServiceTasks;

namespace TaktTusur.InMemoria.Api;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddTransient<InitialSeedData>();

        var connectionString = builder.Configuration.GetConnectionString("InMemoriaDb");
        builder.Services.AddDbContext<InMemoriaDbContext>(options =>
            options.UseNpgsql(connectionString));

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		// Register SeedDataOptions from configuration
		builder.Services.Configure<SeedDataOptions>(builder.Configuration.GetSection("SeedData"));

		var app = builder.Build();

		app.Services.GetRequiredService<InitialSeedData>().RestoreDatabase();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}
