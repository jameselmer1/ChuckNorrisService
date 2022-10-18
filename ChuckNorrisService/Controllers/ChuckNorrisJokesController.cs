using ChuckNorrisApi.Models;
using ChuckNorrisApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;

namespace ChuckNorrisApi.Controllers
{
    /// <summary>
    /// These controller routes expose CRUD operations (except for update) for the Chuck Norris service.  Currently 
    /// 400 and 500 response codes are returned on exception.
    /// If this would be a public facing API, it may be better to simply return
    /// some type of success status so no information is given out that things didn't work.
    /// </summary>
    [Route("api")]
    [ApiController]
    public class ChuckNorrisJokesController : ControllerBase
    {
        private readonly ILogger<ChuckNorrisJokesController> _logger;
        private readonly IChuckNorrisService _chuckNorrisService;

        public ChuckNorrisJokesController(ILogger<ChuckNorrisJokesController> logger,
            IChuckNorrisService chuckNorrisService)
        {
            _logger = logger;
            _chuckNorrisService = chuckNorrisService;
        }

        [Route("v1/[controller]/next")]
        [HttpGet]
        public async Task<IActionResult> GetRandomJokeAsync()
        {
            ChuckNorrisSavedJoke result;
            try
            {
                result = await _chuckNorrisService.GetRandomJoke();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling {nameof(_chuckNorrisService.GetRandomJoke)}: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Ok(result);
        }

        [Route("v1/[controller]")]
        [HttpPost]
        public async Task<IActionResult> SaveJokeAsync([FromBody] ChuckNorrisSavedJoke jokeToSave)
        {
            ChuckNorrisSavedJoke result;
            try
            {
                result = await _chuckNorrisService.SaveJoke(jokeToSave);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError(ae, $"Error calling {nameof(_chuckNorrisService.SaveJoke)}: {ae.Message}");
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling {nameof(_chuckNorrisService.SaveJoke)}: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Ok(result);
        }


        [Route("v1/[controller]")]
        [HttpGet]
        public async Task<IActionResult> GetAllJokes()
        {
            Collection<ChuckNorrisSavedJoke> result;
            try
            {
                result = await Task.Run(() => _chuckNorrisService.GetAllJokes());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling {nameof(_chuckNorrisService.GetAllJokes)}: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Ok(result);
        }

        [Route("v1/[controller]")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAllJokes()
        {
            try
            {
                await Task.Run(() => _chuckNorrisService.DeleteAllJokes());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling {nameof(_chuckNorrisService.DeleteAllJokes)}: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Ok();
        }
    }
}
