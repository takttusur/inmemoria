namespace TaktTusur.InMemoria.Domain.Entities;

public class Person : BaseEntity
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string MaidenName { get; set; }

	public string Patronymic { get; set; }

	public string Nickname { get; set; }

	public DateTime Birthday { get; set; }

	public DateTime DeathDay { get; set; }

	public string Biography { get; set; }

	public bool Active { get; set; }

	public int AttachmentsStorageId { get; set; }

	public virtual List<Attachment> Attachments { get; set; }
}
