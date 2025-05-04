namespace TaktTusur.InMemoria.MigrationApp.Services;

public interface IDbReader<out T> where T : class
{
	public IEnumerable<T> GetAll();

	public IEnumerable<T> GetById(int id);

	public int GetCount();
}
