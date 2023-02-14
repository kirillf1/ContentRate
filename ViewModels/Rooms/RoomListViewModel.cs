using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Rooms;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace ContentRate.ViewModels.Rooms
{
    public class RoomListViewModel : ViewModelBase
    {
        public RoomListViewModel(IRoomQueryService roomQueryService)
        {
            this.roomQueryService = roomQueryService;
            roomTitlesSource = new();
            var filter = this.WhenAnyValue(x => x.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(300), RxApp.MainThreadScheduler)
                .Select(BuildNameFilter);

            SearchText = "";
            var itemsChanged = roomTitlesSource.Connect()
                .Transform(c => c)
                .Filter(filter)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out roomTitles)
                .Subscribe();
        }
        [Reactive]
        public bool IsBusy { get; set; }
        [Reactive]
        public string SearchText { get; set; }
        public ReadOnlyObservableCollection<RoomTitle> RoomTitles => roomTitles;
        private ReadOnlyObservableCollection<RoomTitle> roomTitles;
        private readonly IRoomQueryService roomQueryService;
        private SourceList<RoomTitle> roomTitlesSource;
        public async Task LoadRooms()
        {
            try
            {
                IsBusy = true;
                var roomsResult = await roomQueryService.GetRoomTitles();
                if (!roomsResult.IsSuccess)
                    throw new Exception(string.Join(",", roomsResult.Errors));
                IsBusy = false;
                roomTitlesSource.AddRange(roomsResult.Value);
            }
            finally
            {
                IsBusy = false;
            }

        }
        private Func<RoomTitle, bool> BuildNameFilter(string searchText)
        {
            if (string.IsNullOrEmpty(searchText)) return room => true;
            return room => room.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }
    }
}
