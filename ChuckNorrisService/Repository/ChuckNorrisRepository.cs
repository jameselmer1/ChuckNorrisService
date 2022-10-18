using ChuckNorrisApi.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuckNorrisApi.Repository
{
    //Operations required to Save, Get, and Delete Chuck Norris Jokes.  This repo saves the data
    //to a file in the app's base directory.  This Repo can be replaced to save this data
    //to any other datastore...e.g. a database.
    public class ChuckNorrisRepository : IChuckNorrisRepository
    {
        private readonly ILogger<ChuckNorrisRepository> _logger;
        private const string FileName = "SavedJokes.json";
        private readonly string _fileNameAndPath;

        public ChuckNorrisRepository(ILogger<ChuckNorrisRepository> logger)
        {
            _logger = logger;
            _fileNameAndPath = $"{AppContext.BaseDirectory}\\{FileName}";
        }

        public async Task<ChuckNorrisSavedJoke> Save(ChuckNorrisSavedJoke newJokeToSave)
        {
            //validate there is something to save
            if (newJokeToSave == null)
            {
                throw new ArgumentException($"Can't save the joke, it is empty.");
            }

            Collection<ChuckNorrisSavedJoke> allJokes = new Collection<ChuckNorrisSavedJoke>();

            //create or open file
            var fi = new FileInfo(_fileNameAndPath);
            if (fi.Exists)
            {
                //read contents
                var contents = ReadFile(fi);
                allJokes = JsonConvert.DeserializeObject<Collection<ChuckNorrisSavedJoke>>(contents);
            }

            //add date saved and new joke
            newJokeToSave.Created_at = DateTime.UtcNow.ToString("o");
            allJokes.Add(newJokeToSave);
            string serializedJokeToSave = JsonConvert.SerializeObject(allJokes);

            //save file
            _logger.LogTrace($"Saving file {serializedJokeToSave}");
            await SaveFile(fi, serializedJokeToSave);

            //Get the saved item to return
            var savedItem = GetAllJokes().Where(j => j.Id == newJokeToSave.Id).FirstOrDefault();
            if (savedItem != null)
            {
                _logger.LogTrace($"New joke {savedItem?.Id} saved successfully");
            }

            //better to go get this from the saved file, but in the interest of time, I'll cheat.
            return savedItem;
        }

        //Gets all saved jokes.  If the file does not exist, this will return an empty collection
        public Collection<ChuckNorrisSavedJoke> GetAllJokes()
        {
            Collection<ChuckNorrisSavedJoke> allJokes = new Collection<ChuckNorrisSavedJoke>();

            var fi = new FileInfo(_fileNameAndPath);
            if (fi.Exists)
            {
                var contents = ReadFile(fi);
                allJokes = JsonConvert.DeserializeObject<Collection<ChuckNorrisSavedJoke>>(contents);
            }
            else
            {
                _logger.LogTrace($"GetAllJokes: File {_fileNameAndPath} was not found.");
            }
            return allJokes;
        }

        //If the file exists, delete it
        public void DeleteAllJokes()
        {
            var fi = new FileInfo(_fileNameAndPath);

            if (fi.Exists)
            {
                fi.Delete();
            }
            else
            {
                _logger.LogTrace($"DeleteAllJokes: Cannot delete file. File {_fileNameAndPath} was not found.");
            }
        }

        private async Task SaveFile(FileInfo fi, string content)
        {
            using (FileStream fs = File.Create(fi.FullName))
            {
                byte[] text = new UTF8Encoding(false).GetBytes(content);
                await fs.WriteAsync(text, 0, text.Length);
            }

            _logger.LogTrace($"Saved File: {fi.Name}");
        }

        private string ReadFile(FileInfo fileInfo)
        {
            string fileText = string.Empty;
            string s = string.Empty;
            using (StreamReader sr = File.OpenText(fileInfo.FullName))
            {
                while ((s = sr.ReadLine()) != null)
                {
                    fileText += s;
                }
            }

            return fileText;
        }
    }
}
