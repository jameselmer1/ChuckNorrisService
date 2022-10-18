using ChuckNorrisApi.Models;
using ChuckNorrisApi.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChuckNorrisApi.Services
{
    public class ChuckNorrisService : IChuckNorrisService
    {
        private readonly ChuckNorrisApiSettings _settings;
        private readonly ILogger<ChuckNorrisService> _logger;
        private readonly IChuckNorrisRepository _cnRepository;
        private readonly HttpClient _httpClient;

        public ChuckNorrisService(IOptions<ChuckNorrisApiSettings> settings, ILogger<ChuckNorrisService> logger,
            IChuckNorrisRepository chuckNorrisRepository, IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value;
            _logger = logger;
            _cnRepository = chuckNorrisRepository;
            _httpClient = httpClientFactory.CreateClient("cnClient");
        }

        public async Task<ChuckNorrisSavedJoke> GetRandomJoke()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_settings.Url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            ChuckNorrisSavedJoke newJoke = JsonConvert.DeserializeObject<ChuckNorrisSavedJoke>(responseBody);

            return newJoke;
        }

        public async Task<ChuckNorrisSavedJoke> SaveJoke(ChuckNorrisSavedJoke newJokeToSave)
        {
            //validate there is something to save
            if (newJokeToSave == null || string.IsNullOrWhiteSpace(newJokeToSave?.Id) || string.IsNullOrEmpty(newJokeToSave?.Value))
            {
                _logger.LogWarning($"Could not save joke with id: {newJokeToSave?.Id} and Value: {newJokeToSave?.Value}");
                throw new ArgumentException($"Can't save the joke, it is null or missing required fields.");
            }

            ChuckNorrisSavedJoke savedJoke = await _cnRepository.Save(newJokeToSave);

            return savedJoke;
        }

        //Gets all saved jokes.  If there are no jokes, this will return an empty collection
        public Collection<ChuckNorrisSavedJoke> GetAllJokes()
        {
            Collection<ChuckNorrisSavedJoke> allJokes = _cnRepository.GetAllJokes();

            return allJokes ?? new Collection<ChuckNorrisSavedJoke>();
        }

        //Deletes all jokes from the datastore
        public void DeleteAllJokes()
        {
            _cnRepository.DeleteAllJokes();
        }
    }
}
