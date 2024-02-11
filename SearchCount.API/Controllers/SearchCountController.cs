using SearchCount.API.RequestValidations;
using Microsoft.AspNetCore.Mvc;
using SearchCount.Handlers.Interfaces;
using SearchCount.Shared.Models;

namespace SearchCount.API.Controllers
{
    [ApiController]
    [Route("search")]
    public class SearchCountController : ControllerBase
    {
        private readonly ISearchCountHandler _searchCountHandler;
        private readonly IValidator<SearchCountRequest> _searchCountRequestValidator;

        public SearchCountController(ISearchCountHandler searchHandler, IValidator<SearchCountRequest> searchCountRequestValidator)
        {
            _searchCountHandler = searchHandler;
            _searchCountRequestValidator = searchCountRequestValidator;
        }

        [HttpPost]
        [Route("count")]
        public ActionResult<SearchCountResponse> GetSearchCountAsync([FromBody] SearchCountRequest searchCountRequest)
        {
            _searchCountRequestValidator.Validate(searchCountRequest);

            var result = _searchCountHandler.GetSearchCountAsync(searchCountRequest).Result;

            return Ok(new SearchCountResponse { Indices = result });
        }
    }
}
