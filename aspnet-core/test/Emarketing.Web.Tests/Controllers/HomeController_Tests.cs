using System.Threading.Tasks;
using Emarketing.Models.TokenAuth;
using Emarketing.Web.Controllers;
using Shouldly;
using Xunit;

namespace Emarketing.Web.Tests.Controllers
{
    public class HomeController_Tests: EmarketingWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}