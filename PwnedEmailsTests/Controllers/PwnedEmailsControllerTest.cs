using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace PwnedEmailsTests.Controllers
{
    public class PwnedEmailsControllerTest : PwnedEmailsControllerTestBase
    {
        private const string InvalidEmail = "invalidEmailAddress";

        [Fact]
        public async Task Get_ShouldReturnOkWhenEmailIsFound()
        {
            MockGrain.Setup(m => m.CheckEmailAddress(Email)).ReturnsAsync(true);
            Assert.IsType<OkObjectResult>(await Controller.Get(Email));
        }

        [Fact]
        public async Task Get_ShouldReturnNotFoundWhenEmailIsFound()
        {
            MockGrain.Setup(m => m.CheckEmailAddress(Email)).ReturnsAsync(false);
            Assert.IsType<NotFoundObjectResult>(await Controller.Get(Email));
        }

        [Fact]
        public async Task Get_ShouldReturnBadRequestWhenEmailIsInvalid()
        {
            Assert.IsType<BadRequestObjectResult>(await Controller.Get(InvalidEmail));
        }

        [Fact]
        public async Task Post_ShouldReturnOkWhenEmailIsAddedToTheList()
        {
            MockGrain.Setup(m => m.AddEmailAddress(Email)).ReturnsAsync(true);
            Assert.IsType<CreatedResult>(await Controller.Post(Email));
        }

        [Fact]
        public async Task Post_ShouldReturnConflictWhenEmailIsAlreadyOnTheList()
        {
            MockGrain.Setup(m => m.AddEmailAddress(Email)).ReturnsAsync(false);
            Assert.IsType<ConflictObjectResult>(await Controller.Post(Email));
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequestWhenEmailIsInvalid()
        {
            Assert.IsType<BadRequestObjectResult>(await Controller.Post(InvalidEmail));
        }
    }
}