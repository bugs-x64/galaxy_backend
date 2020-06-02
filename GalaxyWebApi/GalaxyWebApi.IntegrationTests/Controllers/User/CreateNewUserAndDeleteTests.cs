using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using GalaxyDto;
using GalaxyRepository.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TestsCore.Integration;
using Xunit;

namespace GalaxyWebApi.IntegrationTests.Controllers.User
{
    public class CreateNewUserAndDeleteTests : ControllerTestsBase<Startup>, IClassFixture<WebApiFactory<Startup>>, IClassFixture<ContentProvider>
    {
        private readonly ContentProvider _contentProvider;
        protected override string RouteTemplate { get; } = "[controller]";
        protected override string Controller { get; } = "User";

        public CreateNewUserAndDeleteTests(WebApiFactory<Startup> factory, ContentProvider contentProvider) : base(factory)
        {
            _contentProvider = contentProvider;
        }


        [Theory, AutoData]
        public async Task UserPostRequest_AllPropertiesFilled_RepositoryUsernameEqualsRequestedAsync(NewUserDto data)
        {
            var path = GetControllerActionPath();
            var currentUserPath = $"{path}/{data.Username}";
            var content = _contentProvider.GetJsonStringContent(data);


            var response = await Client.PostAsync(path, content);
            await AddHeaderToken(response);
            response = await Client.DeleteAsync(currentUserPath);
            response.EnsureSuccessStatusCode();
            response = await Client.GetAsync(currentUserPath);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        private async Task AddHeaderToken(HttpResponseMessage create)
        {
            var tokenDto = await create.Content.ReadAsAsync<TokenDto>();
            Client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse("Bearer " + tokenDto.access_token);
        }
    }
}