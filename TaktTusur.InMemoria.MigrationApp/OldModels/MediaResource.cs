using System.ComponentModel.DataAnnotations.Schema;

namespace TaktTusur.InMemoria.MigrationApp.OldModels;

/// <summary>
/// Table with media resources which are linked with people.
/// </summary>
[Table("resource")]
public class MediaResource
{
	[Column("id")]
	public int Id { get; set; }

	[Column("person_id")]
	public int PersonId { get; set; }

	[Column("name")]
	public string Name { get; set; }

	[Column("type")]
	public string ResourceType { get; set; }

	[Column("rubric")]
	public string Rubric { get; set; }

	[Column("data")]
	public string Data { get; set; }

	[Column("position")]
	public int Position { get; set; }

	[Column("create_time")]
	public DateTime CreateTime { get; set; }

	[Column("comment")]
	public string Comment { get; set; }

	[Column("active")]
	public int Active { get; set; }
}
