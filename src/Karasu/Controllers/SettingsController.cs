using System.Collections.Generic;
using System.Web.Http;
using Karasu.Repositories;

namespace Karasu.Controllers
{
    [RoutePrefix("api/v1/settings")]
    public class SettingsController : ApiController
    {
        private readonly ISettingsRepository _repository;

        public SettingsController(ISettingsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet, Route("paths")]
        public IEnumerable<string> ListPaths()
        {
            return _repository.ListPaths();
        }
    }
}
