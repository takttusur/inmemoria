using TaktTusur.InMemoria.MigrationApp.OldModels;

namespace TaktTusur.InMemoria.MigrationApp.Services;

public interface ISettingsExporter
{
	/// <summary>
	/// Exports settings to the specified file.
	/// </summary>
	/// <param name="fileName">The name of the file to export settings to.</param>
	/// <returns>Count of export entities.</returns>
	int ExportTo(string fileName);
}
