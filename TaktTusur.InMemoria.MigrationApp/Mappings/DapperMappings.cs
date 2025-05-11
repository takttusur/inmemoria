using System.ComponentModel.DataAnnotations.Schema;
using Dapper;
using TaktTusur.InMemoria.MigrationApp.OldModels;

namespace TaktTusur.InMemoria.MigrationApp.Mappings;

public class DapperMappings
{
	public static void ApplyMappings()
	{
		Dapper.SqlMapper.SetTypeMap(
			typeof(Person),
			new CustomPropertyTypeMap(
				typeof(Person),
				(type, columnName) =>
					type.GetProperties().FirstOrDefault(prop =>
						prop.GetCustomAttributes(false)
							.OfType<ColumnAttribute>()
							.Any(attr => attr.Name == columnName)
					)
			)
		);
		Dapper.SqlMapper.SetTypeMap(
			typeof(MediaResource),
			new CustomPropertyTypeMap(
				typeof(MediaResource),
				(type, columnName) =>
					type.GetProperties().FirstOrDefault(prop =>
						prop.GetCustomAttributes(false)
							.OfType<ColumnAttribute>()
							.Any(attr => attr.Name == columnName)
					)
			)
		);
		Dapper.SqlMapper.SetTypeMap(
			typeof(Settings),
			new CustomPropertyTypeMap(
				typeof(Settings),
				(type, columnName) =>
					type.GetProperties().FirstOrDefault(prop =>
						prop.GetCustomAttributes(false)
							.OfType<ColumnAttribute>()
							.Any(attr => attr.Name == columnName)
					)
			)
		);
	}
}
