using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SoftwareCatalog.Api.Techs.Endpoints;

public static class GettingATech
{
    public static async Task<Results<Ok<TechDetailsResponseModel>, NotFound>> GetTechAsync(Guid id, IDocumentSession session)
    {
        var tech = await session.Query<TechEntity>()
            .Where(t => t.Id == id)
            .Select(t => new TechDetailsResponseModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber
            })
            .SingleOrDefaultAsync();

        return tech is null ? TypedResults.NotFound() : TypedResults.Ok(tech);
    }

    public static async Task<Ok<IReadOnlyList<TechDetailsResponseModel>>> GetTechsAsync(IDocumentSession session)
    {
        var techs = await session.Query<TechEntity>()
            .Select(t => new TechDetailsResponseModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber
            })
            .ToListAsync();

        return TypedResults.Ok(techs);
    }
}
