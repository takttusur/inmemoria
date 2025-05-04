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

	// TODO: enum 'img','video','audio','url','text','html','doc'
	[Column("type")]
	public object ResourceType { get; set; }

	// TODO: enum 'gallery','memoir','hike'
	[Column("rubric")]
	public object Rubric { get; set; }

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

	public virtual Person Person { get; set; }
}
