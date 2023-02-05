using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Rooms;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace ContentRate.ViewModels.Rooms
{
    public class RoomListViewModel : ViewModelBase
    {
        private readonly IRoomQueryService roomQueryService;

        public RoomListViewModel(IRoomQueryService roomQueryService)
        {
            this.roomQueryService = roomQueryService;
            RoomTitles = new();
        }
        public ObservableCollection<RoomTitle> RoomTitles { get; private set; }
        public async Task LoadRooms()
        {
            var roomsResult = await roomQueryService.GetRoomTitles();
            if (!roomsResult.IsSuccess)
                throw new Exception(string.Join(",", roomsResult.Errors));
            foreach (var item in roomsResult.Value)
            {
                RoomTitles.Add(item);
            }
        }
    }
}
