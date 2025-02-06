using FluentValidation;
using Marten;

namespace SoftwareCatalog.Api.Techs;

public record TechCreateModel
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}

public class TechCreateModelValidator : AbstractValidator<TechCreateModel>
{
    public TechCreateModelValidator()
    {
        RuleFor(t => t.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(t => t.Email).EmailAddress().When(t => !string.IsNullOrEmpty(t.Email));
        RuleFor(t => t.PhoneNumber).Matches(@"^\+?[1-9]\d{1,14}$").When(t => !string.IsNullOrEmpty(t.PhoneNumber));
        RuleFor(t => t).Must(t => !string.IsNullOrEmpty(t.Email) || !string.IsNullOrEmpty(t.PhoneNumber))
            .WithMessage("Either Email or PhoneNumber must be provided.");
    }
}

public record TechDetailsResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}

public class TechDataService(IDocumentSession session)
{
    public async Task<bool> DoesTechExistAsync(Guid id) =>
        await session.Query<TechEntity>().AnyAsync(t => t.Id == id);
}
