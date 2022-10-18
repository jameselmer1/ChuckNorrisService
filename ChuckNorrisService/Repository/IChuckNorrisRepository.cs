using ChuckNorrisApi.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ChuckNorrisApi.Repository
{
    public interface IChuckNorrisRepository
    {
        Task<ChuckNorrisSavedJoke> Save(ChuckNorrisSavedJoke jokeToSave);

        Collection<ChuckNorrisSavedJoke> GetAllJokes();

        void DeleteAllJokes();
    }
}
