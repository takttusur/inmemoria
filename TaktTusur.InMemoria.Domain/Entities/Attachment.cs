namespace TaktTusur.InMemoria.Domain.Entities;

public class Attachment : BaseEntity
{
	public string Title { get; set; }

	public string Subtitle { get; set; }

	public bool IsActive { get; set; }

	public string Data { get; set; }

	public int Order { get; set; }

	public DateTime CreatedAt { get; set; }

	public virtual Person Person { get; set; }
}
