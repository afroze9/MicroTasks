namespace MicroTasks.CompanyApi.Models;

// Aggregate Root for Company
public class Company
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    private readonly List<Tag> _tags = new();
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; private set; }

    // DDD: Constructor for new Company
    public Company(string name, IEnumerable<Tag>? tags = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        if (tags != null)
        {
            foreach (Tag tag in tags)
            {
                AddTag(tag);
            }
        }
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
        IsActive = true;
    }

    // DDD: For EF Core
    private Company() { }

    // Business logic
    public void AddTag(Tag tag)
    {
        if (tag != null && !_tags.Any(t => t.Value == tag.Value))
        {
            _tags.Add(tag);
        }
    }

    public void AddTags(IEnumerable<Tag> tags)
    {
        foreach (Tag tag in tags)
        {
            AddTag(tag);
        }
    }

    public void RemoveTag(string value)
    {
        Tag? tag = _tags.FirstOrDefault(t => t.Value == value);
        if (tag != null)
            _tags.Remove(tag);
    }

    public void ClearTags()
    {
        _tags.Clear();
    }

    public void ChangeName(string newName)
    {
        if (!string.IsNullOrWhiteSpace(newName) && newName != Name)
        {
            Name = newName;
        }
    }

    public void Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
        }
    }

    public void Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
        }
    }
}
