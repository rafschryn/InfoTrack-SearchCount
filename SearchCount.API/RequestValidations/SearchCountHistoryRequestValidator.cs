using SearchCount.Shared;
using SearchCount.Shared.Models;
using System.Text.RegularExpressions;

namespace SearchCount.API.RequestValidations
{
    public class SearchCountRequestValidator : IValidator<SearchCountRequest>
    {
        public void Validate(SearchCountRequest searchCountRequest)
        {
            ArgumentNullException.ThrowIfNull(searchCountRequest);

            if (string.IsNullOrEmpty(searchCountRequest.SearchTerm)) 
            {
                throw new RequestValidationException("Search Term must be populated");
            }

            if (searchCountRequest.SearchTerm.Count() > 150)
            {
                throw new RequestValidationException("Search Term must have fewer than 150 characters");
            }

            if (string.IsNullOrEmpty(searchCountRequest.Url))
            {
                throw new RequestValidationException("URL must be populated");
            }

            var isMatch = Regex.IsMatch(searchCountRequest.Url, "^(?:(ftp|http|https)?:\\/\\/)?(?:[\\w-]+\\.)+([a-z]|[A-Z]|[0-9]){2,6}$");
            if (!isMatch)
            {
                throw new RequestValidationException("URL is not valid");
            }
        }
    }
}
