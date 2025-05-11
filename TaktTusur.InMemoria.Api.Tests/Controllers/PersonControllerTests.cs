using System.Net;
using FluentAssertions;
using TaktTusur.InMemoria.Api;
using TaktTusur.InMemoria.Api.Tests.Helpers;
using TaktTusur.InMemoria.Api.Tests.Infrastructure;
using TaktTusur.InMemoria.DataAccess.Context;

namespace TaktTusur.Eda.Api.Tests.Controllers;

[TestFixture]
public class PersonControllerTests : TestsBase
{
	public const string BASE_URL = "api/v1/Person";

	[SetUp]
	public void SetUp()
	{
		WebAppFactory = CustomWebApplicationFactory<Program>
			.CreateWithInMemoryDb<Program, InMemoriaDbContext>();
	}

	[Test]
	public async Task GetWithoutParameters()
	{
		var httpClient = WebAppFactory!.CreateClient();
		var response = await httpClient.GetAsync($"{BASE_URL}/-1");
		response.StatusCode.Should().Be(HttpStatusCode.NotFound, "person id is invalid(less than 0)");
	}
}
