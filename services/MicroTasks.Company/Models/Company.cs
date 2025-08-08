using MicroTasks.Core;
namespace MicroTasks.CompanyApi.Models;

// Aggregate Root for Company
public class Company : BaseEntity
{
    public string Name { get; private set; }
    private readonly List<Tag> _tags = new();
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
    public bool IsActive { get; private set; }
    public int ProjectCount { get; private set; }

    // DDD: Business logic to increment/decrement project count
    public void SetProjectCount(int count)
    {
        ProjectCount = count;
    }

    // DDD: Constructor for new Company
    private Company() { }
    public Company(string name, IEnumerable<Tag>? tags = null)
    {
        Name = name;
        if (tags != null)
        {
            foreach (Tag tag in tags)
            {
                AddTag(tag);
            }
        }
        IsActive = true;
    }

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
