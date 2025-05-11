namespace TaktTusur.InMemoria.Domain.Entities;

public class Attachment : BaseEntity
{
	public string Title { get; set; }

	public string Subtitle { get; set; }

	public bool IsActive { get; set; }

	public string Data { get; set; }

	public AttachmentCategory Category { get; set; }

	public AttachmentTypes AttachmentType { get; set; }

	public int Order { get; set; }

	public DateTime CreatedAt { get; set; }

	public int PersonId { get; set; }

	public virtual Person Person { get; set; }
}
