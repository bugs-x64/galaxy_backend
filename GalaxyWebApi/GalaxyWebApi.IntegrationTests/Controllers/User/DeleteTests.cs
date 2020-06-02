using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using GalaxyDto;
using TestsCore.Integration;
using Xunit;

namespace GalaxyWebApi.IntegrationTests.Controllers.User
{
    public class DeleteTests : ControllerTestsBase<Startup>, IClassFixture<WebApiFactory<Startup>>, IClassFixture<ContentProvider>
    {
        protected override string RouteTemplate { get; } = "[controller]";
        protected override string Controller { get; } = "User";

        public DeleteTests(WebApiFactory<Startup> factory) : base(factory)
        {
        }

        [Theory, AutoData]
        public async Task Delete_DeleteExistingUser_GetAfterDeleteReturnNotFoundAsync(NewUserDto data)
        {
            await UserCreator.CreateUserAsync(Factory, new ContentProvider(), data, Client);
            var currentUserPath = GetControllerActionPath(data.Username);

            var response = await Client.DeleteAsync(currentUserPath);
            response.EnsureSuccessStatusCode();

            response = await Client.GetAsync(currentUserPath);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}