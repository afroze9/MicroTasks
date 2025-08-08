using MicroTasks.Core;
using System.ComponentModel.DataAnnotations;
namespace MicroTasks.CompanyApi.Models;

public class Tag : BaseEntity
{
    [Required]
    public string Value { get; set; } = string.Empty;

    public Tag() { }

    public Tag(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Tag value must not be empty.", nameof(value));
        Value = value;
    }
}
