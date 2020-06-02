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
    public class CreateNewUsersAndGetAllUsersTests : ControllerTestsBase<Startup>, IClassFixture<WebApiFactory<Startup>>, IClassFixture<ContentProvider>
    {
        private readonly ContentProvider _contentProvider;
        protected override string RouteTemplate { get; } = "[controller]";
        protected override string Controller { get; } = "User";

        public CreateNewUsersAndGetAllUsersTests(WebApiFactory<Startup> factory, ContentProvider contentProvider) : base(factory)
        {
            _contentProvider = contentProvider;
        }

        [Theory, AutoData]
        public async Task CreateNewUsersAndGetAllUsersTests_AllPropertiesFilled_RepositoryUsernameEqualsRequestedAsync(NewUserDto data)
        {
            var expected = data.Username;
            var path = GetControllerActionPath();
            var content = _contentProvider.GetJsonStringContent(data);

            var response = await Client.PostAsync(path, content);
            await AddHeaderToken(response);

            response = await Client.GetAsync(path);
            var result = await response.Content.ReadAsAsync<IEnumerable<UserDto>>();
            var usernames = result.Select(dto => dto.Username);

            Assert.Contains(expected, usernames);
        }

        private async Task AddHeaderToken(HttpResponseMessage create)
        {
            var tokenDto = await create.Content.ReadAsAsync<TokenDto>();
            Client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse("Bearer " + tokenDto.access_token);
        }
    }
}