using Microsoft.EntityFrameworkCore;
using TaktTusur.InMemoria.Domain.Entities;

namespace TaktTusur.InMemoria.DataAccess.Context;

public class InMemoriaDbContext : DbContext
{
	public DbSet<Person> Persons { get; set; }

	public DbSet<Attachment> Attachments { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Person>(entity =>
		{
			entity.ToTable("Persons");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.FirstName).IsRequired();
			entity.Property(e => e.LastName).IsRequired();
			entity.Property(e => e.MaidenName);
			entity.Property(e => e.Patronymic);
			entity.Property(e => e.Nickname);
			entity.Property(e => e.Birthday);
			entity.Property(e => e.DeathDay);
			entity.Property(e => e.Biography);
			entity.Property(e => e.Active).IsRequired();
		});

		modelBuilder.Entity<Attachment>(entity =>
		{
			entity.ToTable("Attachments");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Title).IsRequired();
			entity.Property(e => e.Subtitle);
			entity.Property(e => e.IsActive).IsRequired();
			entity.Property(e => e.Data);
			entity.Property(e => e.Order).IsRequired();
			entity.Property(e => e.CreatedAt);
			entity.HasOne(e => e.Person)
			      .WithMany(p => p.Attachments)
			      .HasForeignKey(e => e.PersonId)
			      .HasConstraintName("FK_Attachments_Person");
		});
	}
}
