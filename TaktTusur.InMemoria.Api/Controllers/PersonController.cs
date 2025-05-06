using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaktTusur.InMemoria.DataAccess.Context;
using TaktTusur.InMemoria.Domain.Entities;

[Route("api/[controller]")]
[ApiController]
public class PersonController : Controller
{
	private readonly InMemoriaDbContext _context;

	public PersonController(InMemoriaDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public IActionResult Get(string query = "", int skip = 0, int take = 10)
	{
		IQueryable<Person> persons = _context.Persons.AsNoTracking().Where(x => x.Active);
		if (!string.IsNullOrWhiteSpace(query))
		{
			persons = persons.Where(x =>
				x.FirstName.Contains(query)
				|| x.LastName.Contains(query)
				|| x.Patronymic.Contains(query)
				|| x.MaidenName.Contains(query)
				|| x.Nickname.Contains(query));
		}

		persons = persons.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.Patronymic)
			.Skip(skip).Take(take);

		return Ok(persons);
	}

	[HttpGet("{id}")]
	public IActionResult Get(int id)
	{
		var person = _context.Persons
			.Include(x => x.Attachments)
			.AsNoTracking()
			.FirstOrDefault(x => x.Id == id);

		if (person == null)
		{
			return NotFound();
		}

		return Ok(person);
	}
}
