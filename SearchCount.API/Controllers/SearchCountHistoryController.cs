using Microsoft.AspNetCore.Mvc;
using SearchCount.Handlers.Interfaces;
using SearchCount.Shared.Models;

namespace SearchCount.API.Controllers
{
    [ApiController]
    [Route("history")]
    public class SearchCountHistoryController : ControllerBase
    {
        private readonly ISearchCountHistoryHandler _searchCountHistoryHandler;

        public SearchCountHistoryController(ISearchCountHistoryHandler searchCountHistoryHandler)
        {
            _searchCountHistoryHandler = searchCountHistoryHandler;
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<IEnumerable<SearchCountHistory>> GetAllSearchCountHistory()
        { 
            var result = _searchCountHistoryHandler.GetAllSearchCountHistory();

            return Ok(result);
        }
    }
}
