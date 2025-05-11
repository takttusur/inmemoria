using System.ComponentModel.DataAnnotations.Schema;

namespace TaktTusur.InMemoria.MigrationApp.OldModels;

/// <summary>
/// Table with people records.
/// </summary>
[Table("person")]
public class Person
{
	[Column("id")]
	public int Id { get; set; }

	[Column("card_name")]
	public string CardName { get; set; }

	[Column("firstname")]
	public string FirstName { get; set; }

	[Column("lastname")]
	public string LastName { get; set; }

	[Column("lastname2")]
	public string LastName2 { get; set; }

	[Column("patronymic")]
	public string Patronymic { get; set; }

	[Column("nickname")]
	public string Nickname { get; set; }

	[Column("birthday")]
	public DateTime Birthday { get; set; }

	[Column("death_day")]
	public DateTime DeathDay { get; set; }

	[Column("biography")]
	public string Biography { get; set; }

	[Column("photo_big")]
	public string? PhotoBig { get; set; }

	[Column("photo_small")]
	public string? PhotoSmall { get; set; }

	[Column("active")]
	public int Active { get; set; }

	public List<MediaResource> MediaResources { get; set; }
}
