using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Rooms;
using DynamicData;
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
        public List<RoomTitle> RoomTitles { get; private set; }
        public async Task LoadRooms()
        {
            try
            {
                IsBusy = false;
                var roomsResult = await roomQueryService.GetRoomTitles();
                if (!roomsResult.IsSuccess)
                    throw new Exception(string.Join(",", roomsResult.Errors));
                IsBusy = false;
                RoomTitles.AddRange(roomsResult.Value);
                //foreach (var item in roomsResult.Value)
                //{
                //    RoomTitles.Add(item);
                //}
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
