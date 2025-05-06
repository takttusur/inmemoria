using Microsoft.AspNetCore.Mvc;
using TaktTusur.InMemoria.DataAccess.Context;

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
	public IActionResult Get()
	{
		var persons = _context.Persons.ToList();
		return Ok(persons);
	}

	[HttpGet("{id}")]
	public IActionResult Get(int id)
	{
		var person = _context.Persons.Find(id);

		if (person == null)
		{
			return NotFound();
		}

		return Ok(person);
	}
}
