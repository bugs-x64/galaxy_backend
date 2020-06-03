using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using GalaxyDto;
using TestsCore.Integration;
using Xunit;

namespace GalaxyWebApi.IntegrationTests.Controllers.User
{
    public class GetAllUsersTests : ControllerTestsBase<Startup>, IClassFixture<WebApiFactory<Startup>>, IClassFixture<ContentProvider>
    {
        private readonly ContentProvider _contentProvider;
        protected override string RouteTemplate { get; } = "[controller]";
        protected override string Controller { get; } = "User";

        public GetAllUsersTests(WebApiFactory<Startup> factory, ContentProvider contentProvider) : base(factory)
        {
            _contentProvider = contentProvider;
        }

        [Theory, AutoData]
        public async Task GetAllUsers_AllPropertiesFilled_RepositoryUsernameEqualsRequestedAsync(NewUserDto data)
        {
            await UserCreator.CreateUserAsync(Factory, _contentProvider, data, Client);
            var path = GetControllerActionPath();

            var response = await Client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<IEnumerable<UserDto>>();

            Assert.Contains(data.Username, result.Select(dto => dto.Username));
        }
    }
}