using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using Karasu.DTO;
using Karasu.Repositories;

namespace Karasu.Controllers
{
    [RoutePrefix("v1/songs")]
    public class SongController : ApiController
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly ISongRepository _songRepository;

        public SongController(ISettingsRepository settingsRepository, ISongRepository songRepository)
        {
            _settingsRepository = settingsRepository;
            _songRepository = songRepository;
        }

        [HttpGet, Route, ResponseType(typeof(IEnumerable<SongDTO>))]
        public HttpResponseMessage ListAll(string category = null, string search = null, int skip = 0, int take = 20)
        {
            skip = skip < 0 ? 0 : skip;
            take = take < 1 ? 20 : take;

            var totalSkip = skip == 0 ? 0 : skip - 1;
            var totalTake = Math.Max(1, take) + 1 + (skip > 0 ? 1 : 0);

            var songs = _songRepository.SearchSongs(category, search, totalSkip, totalTake).ToArray();
            var slice = songs.Skip(skip == 0 ? 0 : 1).Take(take).ToArray();

            var uriBuilder = new UriBuilder(Request.RequestUri.GetLeftPart(UriPartial.Path));
            var baseQuery = $"category={WebUtility.UrlEncode(category)}&search={WebUtility.UrlEncode(search)}";

            var linkBuilder = new StringBuilder();

            uriBuilder.Query = baseQuery;
            linkBuilder.Append("<");
            linkBuilder.Append(uriBuilder.Uri);
            linkBuilder.Append(">; rel=\"start\"");

            if (skip > 0)
            {
                uriBuilder.Query = skip - take < 1 ? $"{baseQuery}&take={take}" : $"{baseQuery}&skip={skip - take}&take={take}";

                linkBuilder.Append(", <");
                linkBuilder.Append(uriBuilder.Uri);
                linkBuilder.Append(">; rel=\"previous\"");
            }

            if (songs.Length == totalTake)
            {
                uriBuilder.Query = $"{baseQuery}&skip={skip + take}&take={take}";

                linkBuilder.Append(", <");
                linkBuilder.Append(uriBuilder.Uri);
                linkBuilder.Append(">; rel=\"next\"");
            }

            var response = Request.CreateResponse(slice);

            response.Headers.Add("Link", linkBuilder.ToString());

            return response;
        }

        [HttpGet, Route("categories")]
        public IEnumerable<CategoryDTO> GetCategories()
        {
            return _songRepository.GetCategories().Select(c => new CategoryDTO
            {
                Name = c.Name,
                SongCount = c.Songs.Count
            });
        }

        [HttpPost, Route("reindex")]
        public bool Reindex()
        {
            var paths = _settingsRepository.ListPaths();

            _songRepository.Reindex(paths.ToArray());

            return true;
        }
    }
}
