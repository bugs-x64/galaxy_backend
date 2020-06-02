using System;
using System.Net;
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
    public class ValidateTokenTests : ControllerTestsBase<Startup>, IClassFixture<WebApiFactory<Startup>>, IClassFixture<ContentProvider>
    {
        private readonly ContentProvider _contentProvider;
        protected override string RouteTemplate { get; } = "[controller]/[action]";
        protected override string Controller { get; } = "Auth";

        public ValidateTokenTests(WebApiFactory<Startup> factory, ContentProvider contentProvider) : base(factory)
        {
            _contentProvider = contentProvider;
        }

        [Theory, AutoData]
        public async Task ValidateToken_CorrectToken_ResponseSuccessStatusAsync(NewUserDto data)
        {
            await UserCreator.CreateUserAsync(Factory, _contentProvider, data, Client);
            var authData = new AuthorizationDto
            {
                Username = data.Username,
                Password = data.Password
            };
            var getTokenPath = GetControllerActionPath("GetToken");
            var validatePath = GetControllerActionPath("ValidateToken");
            var authDataContent = _contentProvider.GetJsonStringContent(authData);

            var response = await Client.PostAsync(getTokenPath, authDataContent);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<TokenDto>();

            var query = $"?token={result.access_token}";
            response = await Client.GetAsync(validatePath + query);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ValidateToken_IncorrectToken_ResponseBadRequestAsync()
        {
            const string badToken = "badtoken";
            var path = GetControllerActionPath("ValidateToken");
            var query = $"?token={badToken}";

            var response = await Client.GetAsync(path + query);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}