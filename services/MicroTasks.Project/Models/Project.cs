using System.Collections.Generic;
using MicroTasks.Core;

namespace MicroTasks.ProjectApi.Models;

public class Project : BaseEntity
{
    // Id is inherited from BaseEntity
    public Guid CompanyId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; } = string.Empty;
    private readonly List<WorkItem> _workItems = new();
    public IReadOnlyCollection<WorkItem> WorkItems => _workItems.AsReadOnly();
    public ProjectStatus Status { get; private set; } = ProjectStatus.New;

    // DDD: Private constructor for EF Core
    private Project()
    {
        Name = string.Empty;
    }

    // DDD: Public constructor for new Project
    public Project(Guid companyId, string name, string description, IEnumerable<WorkItem> workItems)
    {
        Id = Guid.NewGuid();
        CompanyId = companyId;
        Name = name;
        Description = description;
        foreach (WorkItem wi in workItems)
        {
            AddWorkItem(wi);
        }
        Status = ProjectStatus.New;
    }

    // Business logic
    public void AddWorkItem(WorkItem workItem)
    {
        if (workItem != null && !_workItems.Any(w => w.Id == workItem.Id))
        {
            _workItems.Add(workItem);
        }
    }

    public void AddWorkItems(IEnumerable<WorkItem> workItems)
    {
        foreach (WorkItem wi in workItems)
        {
            AddWorkItem(wi);
        }
    }

    public void RemoveWorkItem(Guid workItemId)
    {
        _workItems.RemoveAll(w => w.Id == workItemId);
    }

    public void ClearWorkItems()
    {
        _workItems.Clear();
    }

    public void ChangeName(string newName)
    {
        if (!string.IsNullOrWhiteSpace(newName) && newName != Name)
        {
            Name = newName;
        }
    }

    public void ChangeDescription(string newDescription)
    {
        if (newDescription != Description)
        {
            Description = newDescription;
        }
    }

    public void ChangeStatus(ProjectStatus newStatus)
    {
        if (newStatus != Status)
        {
            Status = newStatus;
        }
    }
}
