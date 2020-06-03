using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using GalaxyDto;
using GalaxyRepository.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TestsCore.Integration;
using Xunit;

namespace GalaxyWebApi.IntegrationTests.Controllers.Auth
{
    public class GetTokenTests : ControllerTestsBase<Startup>, IClassFixture<WebApiFactory<Startup>>, IClassFixture<ContentProvider>
    {
        private readonly ContentProvider _contentProvider;
        protected override string RouteTemplate { get; } = "[controller]/[action]";
        protected override string Controller { get; } = "Auth";

        public GetTokenTests(WebApiFactory<Startup> factory, ContentProvider contentProvider) : base(factory)
        {
            _contentProvider = contentProvider;
        }


        private async Task<TokenDto> GetToken(NewUserDto data)
        {
            await UserCreator.CreateUserAsync(Factory, _contentProvider, data, Client);

            var authData = new AuthorizationDto
            {
                Username = data.Username,
                Password = data.Password
            };
            var getTokenPath = GetControllerActionPath("GetToken");
            var authDataContent = _contentProvider.GetJsonStringContent(authData);

            var response = await Client.PostAsync(getTokenPath, authDataContent);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<TokenDto>();
            return result;
        }

        [Theory, AutoData]
        public async Task GetToken_CorrectData_AccessTokenNotNullOrEmptyAsync(NewUserDto data)
        {
            var result = await GetToken(data);

            Assert.False(string.IsNullOrEmpty(result.access_token));
        }

        [Theory, AutoData]
        public async Task GetToken_CorrectData_ResponseUsernameEqualsRequestedAsync(NewUserDto data)
        {
            var result = await GetToken(data);

            Assert.Equal(data.Username, result.username);
        }
    }
}