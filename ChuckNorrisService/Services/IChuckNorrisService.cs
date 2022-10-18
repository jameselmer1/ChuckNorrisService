using ChuckNorrisApi.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ChuckNorrisApi.Services
{
    public interface IChuckNorrisService
    {
        Task<ChuckNorrisSavedJoke> GetRandomJoke();

        Task<ChuckNorrisSavedJoke> SaveJoke(ChuckNorrisSavedJoke jokeToSave);

        Collection<ChuckNorrisSavedJoke> GetAllJokes();

        void DeleteAllJokes();
    }
}
