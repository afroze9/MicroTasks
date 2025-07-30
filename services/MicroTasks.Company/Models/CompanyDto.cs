namespace MicroTasks.CompanyApi.Models
{

    public class CompanyDto
    {
        public string Name { get; set; } = string.Empty;
        public List<TagDto> Tags { get; set; } = new();
    }
}
