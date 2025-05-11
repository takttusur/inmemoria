namespace TaktTusur.InMemoria.MigrationApp.Services;

public interface IPersonExporter
{
	IEnumerable<int> GetEntityIdsForExport();

	void Export(int id, string fileName);
}
