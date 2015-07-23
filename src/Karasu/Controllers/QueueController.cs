using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Karasu.DTO;
using Karasu.Repositories;

namespace Karasu.Controllers
{
    [RoutePrefix("v1/queue")]
    public class QueueController : ApiController
    {
        private readonly ISongRepository _songRepository;

        public QueueController(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        [HttpGet, Route]
        public IQueryable<EnqueuedSongDTO> ListAll()
        {
            return _songRepository.ListQueue().Select(q => new EnqueuedSongDTO
            {
                Artist = q.Secret ? string.Empty : q.Song.Artist,
                Title = q.Secret ? "Secret Song" : q.Song.Title,
                Player = new PlayerDTO
                {
                    Name = q.Player.Name,
                    HexColor = q.Player.HexColor
                },
                Secret = q.Secret
            });
        }

        [HttpPost, Route]
        public async Task<EnqueuedSongDTO> Enqueue(EnqueueSongDTO song)
        {
            if (song == null) throw new HttpResponseException(HttpStatusCode.BadRequest);

            var item = _songRepository.EnqueueSong(song.Player.Name, song.Player.HexColor, song.SongId, song.Secret);

            if (item == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The specified song is invalid."));

            await Task.Delay(TimeSpan.FromMilliseconds(100));

            return new EnqueuedSongDTO
            {
                Artist = item.Secret ? String.Empty : item.Song.Artist,
                Title = item.Secret ? "Secret Song" : item.Song.Title,
                Player = new PlayerDTO
                {
                  Name = item.Player.Name,
                  HexColor = item.Player.HexColor
                },
                Secret = item.Secret
            };
        }
    }
}
