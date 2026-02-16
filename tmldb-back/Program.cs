using System;
using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace tmldb_back;

public class Program
{
	private static NpgsqlDataSource dataSource;

	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		string? connectionString = Environment.GetEnvironmentVariable("DB_CONN");
		if (connectionString == null) {
			throw new Exception("Connection string not found");
		}
		
		dataSource = NpgsqlDataSource.Create(connectionString);
		Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
		
		// Add services to the container.
		builder.Services.AddAuthorization();
		builder.Services.AddControllers().AddJsonOptions(opts =>
		{
			opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
			opts.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
		});

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment()) {
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();
		app.UseAuthorization();
		app.MapControllers();

		app.Run();
	}

	[Pure]
	public static async Task<NpgsqlConnection> OpenConnection() => await dataSource.OpenConnectionAsync();
}