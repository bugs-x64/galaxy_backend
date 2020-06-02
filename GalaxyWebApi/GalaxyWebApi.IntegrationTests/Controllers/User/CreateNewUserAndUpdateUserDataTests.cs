using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using GalaxyDto;
using GalaxyRepository.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TestsCore.Integration;
using Xunit;

namespace GalaxyWebApi.IntegrationTests.Controllers.User
{
    public class CreateNewUserAndUpdateUserDataTests : ControllerTestsBase<Startup>, IClassFixture<WebApiFactory<Startup>>, IClassFixture<ContentProvider>
    {
        private readonly ContentProvider _contentProvider;
        protected override string RouteTemplate { get; } = "[controller]";
        protected override string Controller { get; } = "User";

        public CreateNewUserAndUpdateUserDataTests(WebApiFactory<Startup> factory, ContentProvider contentProvider) : base(factory)
        {
            _contentProvider = contentProvider;
        }


        [Theory, AutoData]
        public async Task CreateNewUserAndUpdateUserData_UpdateAmount_RepositoryUsernameEqualsRequestedAsync(NewUserDto data)
        {
            var path = GetControllerActionPath();
            var concreteUserPath = $"{path}/{data.Username}";
            var content = _contentProvider.GetJsonStringContent(data);

            var updateData = ((IMapper)Factory.Services.GetService(typeof(IMapper))).Map<UserDto>(data);
            updateData.Amount = 100;
            var updateContent = _contentProvider.GetJsonStringContent(updateData);

            var response = await Client.PostAsync(path, content);
            await AddHeaderToken(response);

            response = await Client.PutAsync(concreteUserPath, updateContent);
            response.EnsureSuccessStatusCode();
            response = await Client.GetAsync(concreteUserPath);
            var result = await response.Content.ReadAsAsync<UserDto>();

            Assert.Equal(updateData.Amount, result.Amount);
        }

        private async Task AddHeaderToken(HttpResponseMessage create)
        {
            var tokenDto = await create.Content.ReadAsAsync<TokenDto>();
            Client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse("Bearer " + tokenDto.access_token);
        }
    }
}