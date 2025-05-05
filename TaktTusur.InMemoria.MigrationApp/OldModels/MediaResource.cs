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

	/// <summary>
	/// 'img','video','audio','url','text','html','doc'
	/// </summary>
	[Column("type")]
	public string ResourceType { get; set; }

	/// <summary>
	/// 'gallery','memoir','hike'
	/// </summary>
	[Column("rubric")]
	public string Rubric { get; set; }

	/// <summary>
	/// Based on <see cref="ResourceType"/> value:
	/// 'html' - HTML code
	/// 'text' - text
	/// 'url' - external link
	/// 'img' - inner link with domain name  and person id
	/// 'video' - inner link with domain name and person id
	/// 'audio' - inner link with domain name and person id
	/// 'doc' - inner link with domain name and person id
	/// </summary>
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
