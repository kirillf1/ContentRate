using Ardalis.Result;
using ContentRate.Application.ContentServices;
using ContentRate.Application.Contracts.Content;
using System.Net.Http.Json;
using Youtube.Extensions.Models;

namespace Youtube.Extensions
{
    public class YoutubeContentImporter : IContentImporter
    {
        private readonly HttpClient httpClient;
        private string _playlistId = "";
        private string _apiKey = "";
        private const string PlaylistUrl = "https://www.googleapis.com/youtube/v3/playlistItems";

        public YoutubeContentImporter(HttpClient httpClient)
        {
            this.httpClient = httpClient;

        }
        /// <summary>
        /// TODO временная реализация, нужно ее убрать и инициализировать через factory или как-то по-другому
        /// </summary>
        /// <param name="playlistId"></param>
        /// <param name="apiKey"></param>
        public void Init(string playlistId, string apiKey)
        {
            _playlistId = playlistId;
            _apiKey = apiKey;
        }
        public async Task<Result<IEnumerable<ContentImport>>> Import()
        {
            try
            {
                var videos = await GetAllVideosFromPlaylist(_playlistId, _apiKey);
                return Result.Success(videos.Select(v => new ContentImport()
                {
                    ContentType = ContentRate.Domain.Rooms.ContentType.Video,
                    Name = v.Name ?? "",
                    Path = $"https://www.youtube.com/embed/{v.Id}"
                }));
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
        private async Task<IEnumerable<Video>> GetAllVideosFromPlaylist(string playlistId, string apiKey)
        {
            List<Video> videos = new List<Video>();
            var playlist = await httpClient.GetFromJsonAsync<PlaylistModel>(PlaylistUrl + $"?part=snippet&maxResults=50&playlistId={playlistId}&key={apiKey}");
            if (playlist == null)
                return Enumerable.Empty<Video>();
            if (playlist.Items != null)
                videos.AddRange(playlist.Items.Select(p => new Video(p.Snippet?.Title, p.Snippet!.ResourceId!.VideoId!)));
            while (playlist != null && !string.IsNullOrEmpty(playlist?.NextPageToken))
            {
                playlist = await httpClient.GetFromJsonAsync<PlaylistModel>(PlaylistUrl + $"?part=snippet&maxResults=50&playlistId={playlistId}&key={apiKey}&pageToken={playlist.NextPageToken}");
                if (playlist != null)
                    videos.AddRange(playlist.Items!.Select(p => new Video(p.Snippet?.Title, p.Snippet!.ResourceId!.VideoId!)));
            }
            return videos;
        }
    }
}
