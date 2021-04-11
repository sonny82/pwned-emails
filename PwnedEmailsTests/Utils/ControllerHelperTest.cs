using Microsoft.AspNetCore.Mvc;
using PwnedEmailsApi;
using PwnedEmailsApi.Utils;
using Xunit;

namespace PwnedEmailsTests.Utils
{
    public class ControllerHelperTest
    {
        [Fact]
        public void ShouldCreateProblemDetails()
        {
            ProblemDetails actual = ControllerHelper.CreateProblemDetails(400, "title", "instance");

            Assert.NotNull(actual);
            Assert.Equal(400, actual.Status);
            Assert.Equal("title", actual.Title);
            Assert.Equal("instance", actual.Instance);
        }

        [Fact]
        public void ShouldExtractDomain()
        {
            string domain = ControllerHelper.ExtractDomain("john.doe@gmail.com");
            Assert.NotNull(domain);
            Assert.Equal("gmail.com", domain);
        }

        [Fact]
        public void ShouldSuccessfullyValidateEmail()
        {
            Assert.True(ControllerHelper.IsValidEmail("john.doe@gmail.com"));
        }

        [Fact]
        public void ShouldReturnFalseIfEmailIsNotValid()
        {
            Assert.False(ControllerHelper.IsValidEmail("john.doe"));
        }

        [Fact]
        public void ShouldCreateResponseBody()
        {
            var actual = ControllerHelper.CreateResponseBody(200, "Message");
            Assert.IsType<ResponseBody>(actual);
            Assert.Equal(200, actual.Status);
            Assert.Equal("Message", actual.Message);
        }
    }
}