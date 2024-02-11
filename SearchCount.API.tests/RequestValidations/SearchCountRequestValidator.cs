using SearchCount.API.RequestValidations;
using SearchCount.Shared;
using SearchCount.Shared.Models;

namespace SearchCount.API.tests.RequestValidations
{
    public class SearchCountRequestValidatorTests
    {
        private readonly SearchCountRequestValidator _validator = new SearchCountRequestValidator();

        [Fact]
        public void Validate_ShouldNotThrow_WhenRequestIsValid()
        {
            var request = new SearchCountRequest
            {
                SearchTerm = "ValidTerm",
                Url = "http://www.validurl.com",
                SearchEngine = SearchEngine.Google
            };

            var exception = Record.Exception(() => _validator.Validate(request));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_ShouldThrowRequestValidationException_ForInvalidSearchTerm(string searchTerm)
        {
            var request = new SearchCountRequest
            {
                SearchTerm = searchTerm,
                Url = "http://www.validurl.com",
                SearchEngine = SearchEngine.Google
            };

            Assert.Throws<RequestValidationException>(() => _validator.Validate(request));
        }

        [Fact]
        public void Validate_ShouldThrowRequestValidationException_WhenSearchTermIsTooLong()
        {
            var request = new SearchCountRequest
            {
                SearchTerm = new string('a', 151),
                Url = "http://www.validurl.com",
                SearchEngine = SearchEngine.Google
            };

            Assert.Throws<RequestValidationException>(() => _validator.Validate(request));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_ShouldThrowRequestValidationException_ForInvalidUrl(string url)
        {
            var request = new SearchCountRequest
            {
                SearchTerm = "ValidTerm",
                Url = url,
                SearchEngine = SearchEngine.Google
            };

            Assert.Throws<RequestValidationException>(() => _validator.Validate(request));
        }

        [Theory]
        [InlineData("invalidurl")]
        [InlineData("http://incomplete")]
        public void Validate_ShouldThrowRequestValidationException_ForUrlNotMatchingRegex(string url)
        {
            var request = new SearchCountRequest
            {
                SearchTerm = "ValidTerm",
                Url = url,
                SearchEngine = SearchEngine.Google
            };

            Assert.Throws<RequestValidationException>(() => _validator.Validate(request));
        }
    }
}
