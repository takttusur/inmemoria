using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaktTusur.InMemoria.Api.Models;
using TaktTusur.InMemoria.DataAccess.Context;
using TaktTusur.InMemoria.Domain.Entities;

namespace TaktTusur.InMemoria.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AttachmentsController: Controller
{
	private readonly InMemoriaDbContext _context;

	public AttachmentsController(InMemoriaDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	[ProducesResponseType<PagedResultModel<Attachment>>(200)]
	public IActionResult Get(AttachmentCategory? category = null, int skip = 0, int take = 10, bool random = false)
	{
		IQueryable<Attachment> attachments = _context.Attachments.AsNoTracking().Where(x => x.IsActive);
		if (category != null)
		{
			attachments = attachments.Where(a => a.Category == category);
		}

		var count = attachments.Count();
		if (random)
		{
			attachments = attachments.OrderBy(x => Guid.NewGuid()).Skip(skip).Take(take);
		}
		else
		{
			attachments = attachments.OrderBy(x => x.CreatedAt)
				.Skip(skip).Take(take);
		}

		var result = new PagedResultModel<Attachment>()
		{
			Items = attachments,
			TotalCount = count,
			Skip = skip,
			Take = take
		};

		return Ok(result);
	}
}
