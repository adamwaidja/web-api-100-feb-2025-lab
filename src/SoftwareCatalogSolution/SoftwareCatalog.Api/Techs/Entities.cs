namespace SoftwareCatalog.Api.Techs;

public class TechEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string ManagerSubClaim { get; set; } = string.Empty;
}
