using TaktTusur.InMemoria.MigrationApp.OldModels;

namespace TaktTusur.InMemoria.MigrationApp.Services;

public class DbPersonReader : IDbReader<Person>
{
	public IEnumerable<Person> GetAll()
	{
		throw new NotImplementedException();
	}

	public IEnumerable<Person> GetById(int id)
	{
		throw new NotImplementedException();
	}

	public int GetCount()
	{
		throw new NotImplementedException();
	}
}
