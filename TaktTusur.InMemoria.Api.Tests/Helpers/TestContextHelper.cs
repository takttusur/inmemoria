using NUnit.Framework.Internal;

namespace TaktTusur.InMemoria.Api.Tests.Helpers;

public static class TestContextHelper
{
	public static void SetTestContextProperty(string key, object? value)
	{
		TestExecutionContext.CurrentContext.CurrentTest.Properties.Set(key, value);
	}

	public static T? GetTestContextProperty<T>(string key)
	{
		if (!TestExecutionContext.CurrentContext.CurrentTest.Properties.ContainsKey(key)) return default;
		var obj = TestExecutionContext.CurrentContext.CurrentTest.Properties.Get(key);
		if (obj is null) return default;

		return (T)obj;
	}
}
