using System.Threading.Tasks;
using AutoFixture.Xunit2;
using GalaxyDto;
using GalaxyRepository.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TestsCore.Integration;
using Xunit;

namespace GalaxyWebApi.IntegrationTests.Controllers.User
{
    public class CreateNewUserTests : ControllerTestsBase<Startup>, IClassFixture<WebApiFactory<Startup>>, IClassFixture<ContentProvider>
    {
        private readonly ContentProvider _contentProvider;
        protected override string RouteTemplate { get; } = "[controller]";
        protected override string Controller { get; } = "User";

        public CreateNewUserTests(WebApiFactory<Startup> factory, ContentProvider contentProvider) : base(factory)
        {
            _contentProvider = contentProvider;
        }


        [Theory, AutoData]
        public async Task CreateNewUser_AllPropertiesFilled_RepositoryUsernameEqualsRequestedAsync(NewUserDto data)
        {
            var path = GetControllerActionPath();
            var content = _contentProvider.GetJsonStringContent(data);

            await Client.PostAsync(path, content);

            using var scope = Factory.Services.CreateScope();
            var user = await scope.ServiceProvider.GetService<IUserRepository>().GetAsync(data.Username);

            Assert.Equal(user.Username, data.Username);
        }
    }
}