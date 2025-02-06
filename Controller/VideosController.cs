using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using YouTubeVideoAPI.Models;
using YouTubeVideoAPI.Services;

namespace YouTubeVideoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly YouTubeApiService _youTubeApiService;

        public VideosController(AppDbContext context, YouTubeApiService youTubeApiService)
        {
            _context = context;
            _youTubeApiService = youTubeApiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetVideos([FromQuery] string title, [FromQuery] int? duration, [FromQuery] string author, [FromQuery] DateTime? date, [FromQuery] string q)
        {
            var query = _context.Videos.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(v => v.Title.Contains(title));
            if (duration.HasValue)
                query = query.Where(v => v.Duration == duration);
            if (!string.IsNullOrEmpty(author))
                query = query.Where(v => v.Author.Contains(author));
            if (date.HasValue)
                query = query.Where(v => v.PublishedAt > date.Value);
            if (!string.IsNullOrEmpty(q))
                query = query.Where(v => v.Title.Contains(q) || v.Description.Contains(q) || v.Author.Contains(q));

            var videos = await query.ToListAsync();
            return Ok(videos);
        }

        [HttpPost]
        public async Task<IActionResult> AddVideo([FromBody] Video video)
        {
            _context.Videos.Add(video);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVideos), new { id = video.Id }, video);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideo(int id, [FromBody] Video video)
        {
            var existingVideo = await _context.Videos.FindAsync(id);
            if (existingVideo == null)
                return NotFound();

            existingVideo.Title = video.Title;
            existingVideo.Description = video.Description;
            existingVideo.Author = video.Author;
            existingVideo.VideoUrl = video.VideoUrl;
            existingVideo.Duration = video.Duration;
            existingVideo.PublishedAt = video.PublishedAt;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
                return NotFound();

            video.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
