using FluentValidation;
using SoftwareCatalog.Api.Techs.Endpoints;

namespace SoftwareCatalog.Api.Techs;

public static class Extensions
{
    public static IServiceCollection AddTechs(this IServiceCollection services)
    {
        services.AddScoped<IValidator<TechCreateModel>, TechCreateModelValidator>();
        services.AddScoped<TechDataService>();

        services.AddAuthorizationBuilder()
            .AddPolicy("canAddTechs", p => p.RequireRole("software-center-manager"));

        return services;
    }

    public static IEndpointRouteBuilder MapTechs(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("techs").WithTags("Software Center Techs");

        group.MapPost("/", AddingATech.AddTechAsync).RequireAuthorization("canAddTechs");
        group.MapGet("/{id}", GettingATech.GetTechAsync);
        group.MapGet("/", GettingATech.GetTechsAsync);

        return group;
    }
}
