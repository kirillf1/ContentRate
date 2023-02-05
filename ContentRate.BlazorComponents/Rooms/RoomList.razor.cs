using ContentRate.ViewModels.Rooms;
using Microsoft.AspNetCore.Components;

namespace ContentRate.BlazorComponents.Rooms
{
    public partial class RoomList
    {
        [Inject]
        public RoomListViewModel FetchViewModel
        {
            get => ViewModel!;
            set => ViewModel = value;

        }
        protected override async Task OnInitializedAsync()
        {
            await ViewModel!.LoadRooms();
        }
    }
}
