using TaktTusur.InMemoria.MigrationApp.OldModels;

namespace TaktTusur.InMemoria.MigrationApp.Services;

public class DbMediaResourceReader : IDbReader<MediaResource>
{
	public IEnumerable<MediaResource> GetAll()
	{
		throw new NotImplementedException();
	}

	public IEnumerable<MediaResource> GetById(int id)
	{
		throw new NotImplementedException();
	}

	public int GetCount()
	{
		throw new NotImplementedException();
	}
}
