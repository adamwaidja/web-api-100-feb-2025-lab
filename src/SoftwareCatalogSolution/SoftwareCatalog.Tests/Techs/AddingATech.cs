using Alba;
using Alba.Security;
using SoftwareCatalog.Api.Techs;
using System.Security.Claims;

namespace SoftwareCatalog.Tests.Techs;

[Trait("Category", "System")]
public class AddingATechTests
{
    [Fact]
    public async Task CanAddATech()
    {
        var fakeIdentity = new AuthenticationStub()
            .WithName("manager")
            .With(new Claim(ClaimTypes.Role, "software-center-manager"));

        var host = await AlbaHost.For<Program>(fakeIdentity);

        var requestModel = new TechCreateModel
        {
            Name = "Adam Aidja",
            Email = "Adam.Aidja@example.com"
        };

        var postResponse = await host.Scenario(api =>
        {
            api.Post.Json(requestModel).ToUrl("/techs");
            api.StatusCodeShouldBe(201);
        });

        var postBody = postResponse.ReadAsJson<TechDetailsResponseModel>();
        Assert.NotNull(postBody);
    }
}
