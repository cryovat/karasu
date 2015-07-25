using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Karasu.DTO;
using Karasu.Instrumentation;
using Karasu.Repositories;

namespace Karasu.Controllers
{
    [RoutePrefix("v1/playback")]
    public class PlaybackController : ApiController
    {
        private readonly SongRunner _songRunner;

        public PlaybackController(SongRunner songRunner)
        {
            _songRunner = songRunner;
        }

        [HttpGet, Route("current")]
        public EnqueuedSongDTO Current()
        {
            var current = _songRunner.CurrentSong;

            if (current == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new EnqueuedSongDTO
            {
                Artist = current.Song.Artist,
                Title = current.Song.Title,
                Player = new PlayerDTO
                {
                    Name = current.Player.Name,
                    HexColor = current.Player.HexColor
                },
                Secret = false
            };
        }

        [HttpPost, Route("restart")]
        public HttpResponseMessage Restart(SongControlDTO dto)
        {
            var success = _songRunner.RestartCurrent();

            return Request.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
        }

        [HttpPost, Route("pause")]
        public HttpResponseMessage Pause(SongControlDTO dto)
        {
            var success = _songRunner.PauseCurrent();

            return Request.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
        }

        [HttpPost, Route("skip")]
        public HttpResponseMessage Skip(SongControlDTO dto)
        {
            var success = _songRunner.SkipCurrent();

            return Request.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
        }
    }
}
