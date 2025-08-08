namespace MicroTasks.CompanyApi.Dtos;

public class CompanyDto
{
    public string Name { get; set; } = string.Empty;
    public List<TagDto> Tags { get; set; } = new();
    public int ProjectCount { get; set; }
}
