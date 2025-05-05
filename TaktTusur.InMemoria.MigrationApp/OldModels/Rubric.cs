using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TaktTusur.InMemoria.MigrationApp.OldModels;

public enum Rubric
{
    [EnumMember(Value = "gallery")]
	Gallery,

    [EnumMember(Value = "memoir")]
	Memoir,

    [EnumMember(Value = "hike")]
	Hike
}
