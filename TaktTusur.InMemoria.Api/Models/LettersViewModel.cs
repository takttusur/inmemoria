using TaktTusur.InMemoria.Domain.Entities;

namespace TaktTusur.InMemoria.Api.Models;

public class LettersViewModel
{
	public string Letter { get; set; }

	public Person[] Persons { get; set; }
}
