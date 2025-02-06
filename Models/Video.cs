namespace YouTubeVideoAPI.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string VideoUrl { get; set; }
        public int Duration { get; set; } // Em segundos
        public DateTime PublishedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
