using FluentMigrator;

namespace TaktTusur.InMemoria.DataAccess.Migrations.Migrations;

[Migration(20250505)]
public class InitialMigration : Migration
{
	public override void Up()
	{
		Create.Table("Persons")
		    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
		    .WithColumn("FirstName").AsString()
		    .WithColumn("LastName").AsString()
		    .WithColumn("MaidenName").AsString()
		    .WithColumn("Patronymic").AsString()
		    .WithColumn("Nickname").AsString()
		    .WithColumn("Birthday").AsDateTime()
		    .WithColumn("DeathDay").AsDateTime()
		    .WithColumn("Biography").AsString()
		    .WithColumn("AttachmentsStorageId").AsInt32()
		    .WithColumn("Active").AsBoolean();

		Create.Table("Attachments")
		    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
		    .WithColumn("PersonId").AsInt32().ForeignKey("FK_Attachments_Person", "Persons", "Id")
		    .WithColumn("Title").AsString()
		    .WithColumn("Subtitle").AsString()
		    .WithColumn("IsActive").AsBoolean()
		    .WithColumn("Data").AsString()
		    .WithColumn("Order").AsInt32()
		    .WithColumn("CreatedAt").AsDateTime()
		    .WithColumn("Category").AsInt32()
		    .WithColumn("AttachmentType").AsInt32();
	}

	public override void Down()
	{
		Delete.Table("Attachments");
		Delete.Table("Person");
	}
}
