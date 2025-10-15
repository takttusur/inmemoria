using TaktTusur.InMemoria.Domain.Entities;

namespace TaktTusur.InMemoria.Api.Models;

public class PersonFullViewModel
{
	public int Id { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string MaidenName { get; set; }

	public string Patronymic { get; set; }

	public string Nickname { get; set; }

	public string BirthDay { get; set; }

	public string DeathDay { get; set; }

	public string? Layout { get; set; }

	public string? PhotoImage { get; set; }

	public string Biography { get; set; }

	public Attachment[] Memories { get; set; }

	public Attachment[] Photos { get; set; }
}
