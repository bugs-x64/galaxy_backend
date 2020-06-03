using AutoFixture.Xunit2;
using GalaxyCore.Services;
using Xunit;

namespace GalaxyCore.UnitTests.Services
{
    public class PasswordServiceTests
    {
        private readonly PasswordService _service = new PasswordService();

        [Theory]
        [InlineData("123")]
        [InlineData("qweerwfddfgfdgdgdfgxccc")]
        [InlineData("jiwejiwefjwef--123131n4")]
        [InlineData("AD418FB0-FA49-4831-BF45-AA6FDAE1A6F8")]
        public void ComparePasswords_qwe_qwe(string password)
        {
            var hash = _service.GetHashedPasswordBytes(password);

            var condition = _service.ComparePasswords(hash, password);

            Assert.True(condition);
        }

        [Theory, AutoData]
        [InlineData("123")]
        [InlineData("qweerwfddfgfdgdgdfgxccc")]
        [InlineData("jiwejiwefjwef--123131n4")]
        [InlineData("AD418FB0-FA49-4831-BF45-AA6FDAE1A6F8")]
        public void ComparePasswords_qwe_qwe1(string password)
        {
            var hash = _service.GetHashedPasswordBytes(password);

            var condition = _service.ComparePasswords(hash, password);

            Assert.True(condition);
        }
    }
}
