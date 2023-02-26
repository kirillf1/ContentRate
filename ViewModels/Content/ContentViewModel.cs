using ContentRate.Application.Contracts.Content;
using ContentRate.Domain.Rooms;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace ContentRate.ViewModels.Content
{
    public class ContentViewModel : ViewModelBase
    {
        private readonly ContentDetails contentDetails;

        public ContentViewModel(ContentDetails contentDetails)
        {
            this.contentDetails = contentDetails;
            ratings = new(contentDetails.Ratings.Select(c => new RatingViewModel(c)));
            Ratings = new(ratings);
            NeedPreWatchContent = false;
        }

        public bool NeedPreWatchContent { get; set; }
        public Guid Id => contentDetails.Id;
        public string Name
        {
            get => contentDetails.Name;
            set
            {
                contentDetails.Name = value;
                this.RaisePropertyChanged();
            }
        }
        public ContentType ContentType
        {
            get => contentDetails.ContentType;
            set
            {
                contentDetails.ContentType = value;
                this.RaisePropertyChanged();
            }
        }
        public string Path
        {
            get => contentDetails.Path;
            set
            {
                contentDetails.Path = ParsePath(value);
                this.RaisePropertyChanged();
            }
        }
        public ReadOnlyObservableCollection<RatingViewModel> Ratings { get; }
        private ObservableCollection<RatingViewModel> ratings;
        public void AddRating(ContentRating contentRating)
        {
            if (contentDetails.Ratings.Any(c => c.AssessorId == contentRating.AssessorId))
                return;
            contentDetails.Ratings.Add(contentRating);
            ratings.Add(new RatingViewModel(contentRating));
        }
        public void RemoveRating(Guid assessorId)
        {
            // можно потом переопределить Equals и передавать уже объект рейтинга
            var rating = contentDetails.Ratings.Find(c => c.AssessorId == assessorId);
            if (rating is null)
                return;
            contentDetails.Ratings.Remove(rating);
            ratings.Remove(ratings.First(c=>c.AssessorId == assessorId));
        }
        // TODO при расширении логики вынести в отдельный класс
        private static string ParsePath(string path)
        {
            switch (path)
            {
                case string u when u.Contains("youtube.com/watch"):
                    return YoutubeEmbedConvert(u);
                default:
                    return path;
            }
        }
        private static string YoutubeEmbedConvert(string url)
        {
            var youtubeVideoId = url[(url.IndexOf("?v=") + 3)..url.IndexOf("&")];
            return "https://www.youtube.com/embed/" + youtubeVideoId;
        }
    }
}
