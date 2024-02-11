using Moq;
using Microsoft.AspNetCore.Mvc;
using SearchCount.Handlers.Interfaces;
using SearchCount.Shared.Models;
using SearchCount.API.RequestValidations;
using SearchCount.Shared;
using SearchCount.API.Controllers;

namespace SearchCount.API.Tests.Controllers
{
    public class SearchCountControllerTests
    {
        private readonly Mock<ISearchCountHandler> _mockSearchCountHandler = new();
        private readonly Mock<IValidator<SearchCountRequest>> _mockSearchCountRequestValidator = new();
        private readonly SearchCountController _controller;
        private SearchCountRequest _request;

        public SearchCountControllerTests()
        {
            Setup();

            _controller = new SearchCountController(_mockSearchCountHandler.Object, _mockSearchCountRequestValidator.Object);
        }

        private void Setup()
        {
            _request = new SearchCountRequest
            {
                SearchTerm = string.Empty,
                Url = string.Empty,
                SearchEngine = SearchEngine.Google
            };

            _mockSearchCountHandler.Setup(h => h.GetSearchCountAsync(It.IsAny<SearchCountRequest>()))
                .ReturnsAsync(new List<int> { 1, 2, 3 });
        }

        [Fact]
        public void GetSearchCountAsync_ReturnsExpectedResult()
        {
            var actionResult = _controller.GetSearchCountAsync(_request);

            Assert.NotNull(actionResult);
            Assert.True(actionResult.Result is OkObjectResult);

            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.Equal(new List<int> { 1, 2, 3 }, ((SearchCountResponse)okObjectResult!.Value!).Indices);
        }

        [Fact]
        public void GetSearchCountAsync_CallsHandler()
        {
            _controller.GetSearchCountAsync(_request);

            _mockSearchCountHandler.Verify(handler => handler.GetSearchCountAsync(It.IsAny<SearchCountRequest>()), Times.Once);
        }

        [Fact]
        public void GetSearchCountAsync_CallsValidator()
        {
            _controller.GetSearchCountAsync(_request);

            _mockSearchCountRequestValidator.Verify(validator => validator.Validate(_request), Times.Once);
        }

        [Fact]
        public void GetSearchCountAsync_FailsValidator_DoesNotCallHandler()
        {
            _mockSearchCountRequestValidator.Setup(v => v.Validate(It.IsAny<SearchCountRequest>())).Throws<RequestValidationException>();

            try
            {
                _controller.GetSearchCountAsync(_request);
            }
            catch { }

            _mockSearchCountHandler.Verify(handler => handler.GetSearchCountAsync(It.IsAny<SearchCountRequest>()), Times.Never);
        }
        [Fact]

        public void GetSearchCountAsync_FailsValidator_ReturnsError()
        {
            _mockSearchCountRequestValidator.Setup(v => v.Validate(It.IsAny<SearchCountRequest>())).Throws<RequestValidationException>();

            Assert.Throws<RequestValidationException>(() => _controller.GetSearchCountAsync(_request));
        }
    }
}