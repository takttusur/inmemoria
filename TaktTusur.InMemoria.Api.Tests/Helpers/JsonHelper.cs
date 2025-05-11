using System.Text.Json;

namespace TaktTusur.InMemoria.Api.Tests.Helpers;

public static class JsonHelper
{
	public static T? FromJson<T>(this string json)
	{
		return System.Text.Json.JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions()
		{
			PropertyNameCaseInsensitive = true
		});
	}
}
