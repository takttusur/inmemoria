namespace TaktTusur.InMemoria.MigrationApp.Services;

public interface IDbConnectionTester
{
	void Test(int maxRetry = 1);
}
