using ContentRate.Application.Contracts.Content;
using ReactiveUI;

namespace ContentRate.ViewModels.Content
{
    public class RatingViewModel : ViewModelBase
    {
        private readonly ContentRating rating;

        public RatingViewModel(ContentRating rating)
        {
            this.rating = rating;
        }
        public Guid AssessorId => rating.AssessorId;
        public double Value
        {
            get => rating.Value;
            set
            {
                rating.Value = value;
                this.RaisePropertyChanged();
            }
        }

    }
}
