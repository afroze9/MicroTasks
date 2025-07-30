namespace MicroTasks.CompanyApi.Models;

using System.ComponentModel.DataAnnotations;

public class Tag
{
    public Guid Id { get; set; }

    [Required]
    public string Value { get; set; } = string.Empty;

    public Tag() { }

    public Tag(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Tag value must not be empty.", nameof(value));
        Value = value;
    }
    // DDD: Tags should be unique by Value
}
