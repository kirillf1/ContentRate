using ContentRate.Application.Contracts.Rooms;
using ReactiveUI;

namespace ContentRate.ViewModels.Users
{
    public class AssessorTitleViewModel : ViewModelBase
    {
        private readonly AssessorTitle assessorTitle;

        public AssessorTitleViewModel(AssessorTitle assessorTitle)
        {
            this.assessorTitle = assessorTitle;
        }
        public Guid Id => assessorTitle.Id;
        public string Name
        {
            get => assessorTitle.Name;
            set
            {
                assessorTitle.Name = value;
                this.RaisePropertyChanged();
            }
        }
        public bool IsMock
        {
            get => assessorTitle.IsMock;
            set
            {
                assessorTitle.IsMock = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
