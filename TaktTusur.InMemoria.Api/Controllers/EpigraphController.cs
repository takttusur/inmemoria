using Microsoft.AspNetCore.Mvc;
using TaktTusur.InMemoria.Api.Models;
using TaktTusur.InMemoria.DataAccess.Context;

namespace TaktTusur.InMemoria.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class EpigraphController : Controller
{
	private readonly InMemoriaDbContext _context;

	public EpigraphController(InMemoriaDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public IActionResult Get(int skip = 0, int take = 10)
	{
		var count = _context.Epigraphs.Count();
		var list = _context.Epigraphs
			.OrderBy(e => e.Id)
			.Skip(skip)
			.Take(take)
			.Select(e => new EpigraphViewModel()
		{
			Id = e.Id,
			Text = e.Text
		});
		return Json(new PagedResultModel<EpigraphViewModel>()
		{
			Items = list,
			Skip = skip,
			Take = take,
			TotalCount = count
		});
	}
}
