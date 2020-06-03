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
    public class UpdateTests : ControllerTestsBase<Startup>, IClassFixture<WebApiFactory<Startup>>, IClassFixture<ContentProvider>
    {
        private readonly ContentProvider _contentProvider;
        protected override string RouteTemplate { get; } = "[controller]";
        protected override string Controller { get; } = "User";

        public UpdateTests(WebApiFactory<Startup> factory, ContentProvider contentProvider) : base(factory)
        {
            _contentProvider = contentProvider;
        }

        [Theory, AutoData]
        public async Task Update_UpdateAmount_ResultAmountEqualsUpdateRequestedAsync(NewUserDto data)
        {
            await UserCreator.CreateUserAsync(Factory, _contentProvider, data, Client);
            var concreteUserPath = GetControllerActionPath(data.Username);
            var mapper = Factory.Services.GetService<IMapper>();
            var updateData = mapper.Map<UserDto>(data);
            updateData.Amount = 100;
            var updateContent = _contentProvider.GetJsonStringContent(updateData);

            var response = await Client.PutAsync(concreteUserPath, updateContent);
            response.EnsureSuccessStatusCode();

            response = await Client.GetAsync(concreteUserPath);
            var result = await response.Content.ReadAsAsync<UserDto>();
            Assert.Equal(updateData.Amount, result.Amount);
        }

    }
}