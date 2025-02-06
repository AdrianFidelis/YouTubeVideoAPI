using System.Net.Http;
using System.Threading.Tasks;
using YouTubeVideoAPI.Models;
using Newtonsoft.Json;

namespace YouTubeVideoAPI.Services
{
    public class YouTubeApiService
    {
        private readonly string apiKey = "sua-chave-api-aqui"; // Substitua pela chave da API

        public async Task<IEnumerable<Video>> GetYouTubeVideosAsync()
        {
            using (var client = new HttpClient())
            {
                var url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q=manipulação de medicamentos&regionCode=BR&publishedAfter=2025-01-01T00:00:00Z&type=video&key={apiKey}";
                var response = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<YouTubeApiResponse>(response);

                return result.Items.Select(item => new Video
                {
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    Author = item.Snippet.ChannelTitle,
                    VideoUrl = $"https://www.youtube.com/watch?v={item.Id.VideoId}",
                    PublishedAt = DateTime.Parse(item.Snippet.PublishedAt),
                    Duration = 0, // A duração pode ser obtida com uma chamada adicional à API
                    IsDeleted = false
                });
            }
        }
    }

    public class YouTubeApiResponse
    {
        public List<YouTubeItem> Items { get; set; }
    }

    public class YouTubeItem
    {
        public YouTubeId Id { get; set; }
        public YouTubeSnippet Snippet { get; set; }
    }

    public class YouTubeId
    {
        public string VideoId { get; set; }
    }

    public class YouTubeSnippet
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChannelTitle { get; set; }
        public string PublishedAt { get; set; }
    }
}
