using System.ComponentModel.DataAnnotations.Schema;

namespace TaktTusur.InMemoria.MigrationApp.OldModels;

/// <summary>
/// Table with settings in old db.
/// </summary>
[Table("ini")]
public class Settings
{
	[Column("key")]
	public string Key { get; set; }

	[Column("value")]
	public string Value { get; set; }

	[Column("update")]
	public DateTime Update { get; set; }
}
