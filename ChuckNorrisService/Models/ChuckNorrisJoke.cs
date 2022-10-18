namespace ChuckNorrisApi.Models
{
    //This class is based on the properties
    //returned by https://api.chucknorris.io/jokes/random
    public class ChuckNorrisJoke
    {
        public object[] Categories { get; set; }
        public string Created_at { get; set; }
        public string Icon_url { get; set; }
        public string Id { get; set; }
        public string Updated_at { get; set; }
        public string Url { get; set; }
        public string Value { get; set; }
    }
}
