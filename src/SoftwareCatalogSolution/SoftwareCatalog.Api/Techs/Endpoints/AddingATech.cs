using FluentValidation;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace SoftwareCatalog.Api.Techs.Endpoints;

public static class AddingATech
{
    public static async Task<Results<Created<TechDetailsResponseModel>, BadRequest>> AddTechAsync(
        [FromBody] TechCreateModel request,
        [FromServices] IValidator<TechCreateModel> validator,
        [FromServices] IDocumentSession session,
        [FromServices] IHttpContextAccessor httpContextAccessor)
    {
        var managerSub = httpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (managerSub is null) return TypedResults.BadRequest();

        var validationResults = await validator.ValidateAsync(request);
        if (!validationResults.IsValid) return TypedResults.BadRequest();

        var tech = new TechEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            ManagerSubClaim = managerSub
        };

        session.Store(tech);
        await session.SaveChangesAsync();

        var response = new TechDetailsResponseModel
        {
            Id = tech.Id,
            Name = tech.Name,
            Email = tech.Email,
            PhoneNumber = tech.PhoneNumber
        };

        return TypedResults.Created($"/techs/{tech.Id}", response);
    }
}
