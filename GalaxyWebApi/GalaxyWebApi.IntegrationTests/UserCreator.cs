using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GalaxyDto;
using GalaxyRepository.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TestsCore.Integration;

namespace GalaxyWebApi.IntegrationTests
{
    public static class UserCreator
    {
        public static async Task CreateUserAsync(WebApplicationFactory<Startup> factory, ContentProvider contentProvider, NewUserDto data, HttpClient client)
        {
            var isUserExists = await IsUserExists(data, factory);
            if (!isUserExists)
            {
                await SendPostRequestAndSetTokenHeader(contentProvider, data, client, "user");
                return;
            }

            var authData = new AuthorizationDto()
            {
                Password = data.Password,
                Username = data.Username
            };

            await SendPostRequestAndSetTokenHeader(contentProvider, authData, client, "auth/GetToken");
        }

        private static async Task SendPostRequestAndSetTokenHeader<TInput>(ContentProvider contentProvider, TInput data, HttpClient client, string path)
        {
            var content = contentProvider.GetJsonStringContent(data);
            var response = await client.PostAsync(path, content);
            response.EnsureSuccessStatusCode();
            await AddHeaderToken(client, response);
        }

        private static async Task<bool> IsUserExists(NewUserDto user, WebApplicationFactory<Startup> factory)
        {
            using var scope = factory.Services.CreateScope();
            var repository = scope.ServiceProvider.GetService<IUserRepository>();
            var userDto = await repository.GetAsync(user.Username);
            return userDto != null;
        }

        private static async Task AddHeaderToken(HttpClient client, HttpResponseMessage create)
        {
            var tokenDto = await create.Content.ReadAsAsync<TokenDto>();
            client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse("Bearer " + tokenDto.access_token);
        }
    }
}