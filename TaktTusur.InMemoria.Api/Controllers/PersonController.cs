using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaktTusur.InMemoria.Api.Models;
using TaktTusur.InMemoria.DataAccess.Context;
using TaktTusur.InMemoria.Domain.Entities;

namespace TaktTusur.InMemoria.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PersonController : Controller
{
	private readonly InMemoriaDbContext _context;

	public PersonController(InMemoriaDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	[ProducesResponseType<PagedResultModel<Person>>(200)]
	public IActionResult Get(string query = "", int skip = 0, int take = 10, bool random = false)
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

		var count = persons.Count();
		if (random)
		{
			persons = persons.OrderBy(x => Guid.NewGuid()).Skip(skip).Take(take);
		}
		else
		{
			persons = persons.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.Patronymic)
				.Skip(skip).Take(take);
		}

		var result = new PagedResultModel<Person>()
		{
			Items = persons,
			TotalCount = count,
			Skip = skip,
			Take = take
		};

		return Ok(result);
	}

	[HttpGet("letters")]
	[ProducesResponseType<LettersViewModel[]>(200)]
	public IActionResult Get()
	{
		var persons = _context.Persons.AsNoTracking().Where(x => x.Active).ToList();
		var groups = persons
			.GroupBy(p => p.FirstName[0], p => p)
			.OrderBy(g => g.Key)
			.Select(g => new LettersViewModel()
			{
				Letter = g.Key.ToString(),
				Persons = g.ToArray()
			});

		var result = groups.ToArray();

		return Ok(result);
	}

	[HttpGet("{id}")]
	[ProducesResponseType<Person>(200)]
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

		// To avoid cycle referencing
		foreach (var attachment in person.Attachments)
		{
			attachment.Person = null;
		}

		var model = new PersonFullViewModel()
		{
			Id = person.Id,
			FirstName = person.FirstName,
			LastName = person.LastName,
			Biography = person.Biography,
			BirthDay = person.Birthday.ToString("O"),
			DeathDay = person.DeathDay.ToString("O"),
			Layout = null,
			MaidenName = person.MaidenName,
			Nickname = person.Nickname,
			Patronymic = person.Patronymic,
			Memories = person.Attachments?.Where(a => a.Category == AttachmentCategory.Memories).ToArray() ?? [],
			Photos = person.Attachments?.Where(a => a.Category == AttachmentCategory.Gallery).ToArray() ?? []
		};

		return Ok(model);
	}
}