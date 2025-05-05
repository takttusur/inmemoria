using System.Runtime.Serialization;

namespace TaktTusur.InMemoria.MigrationApp.OldModels;

public enum ResourceType
{
	[EnumMember(Value = "img")]
	Image,
	[EnumMember(Value = "video")]
	Video,
	[EnumMember(Value = "audio")]
	Audio,
	[EnumMember(Value = "url")]
	Url,
	[EnumMember(Value = "text")]
	Text,
	[EnumMember(Value = "html")]
	Html,
	[EnumMember(Value = "doc")]
	Document
}
