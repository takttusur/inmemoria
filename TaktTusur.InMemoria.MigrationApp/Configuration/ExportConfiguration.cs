namespace TaktTusur.InMemoria.MigrationApp.Configuration;

public class ExportConfiguration
{
	public const string SECTION_NAME = "Export";

	public string SettingsFile { get; set; }

	public string PersonsDirectory { get; set; }
}
