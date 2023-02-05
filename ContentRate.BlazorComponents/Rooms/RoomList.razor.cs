using ContentRate.ViewModels.Rooms;
using Microsoft.AspNetCore.Components;
using ReactiveUI;
using System.Reactive.Linq;

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
        public RoomList()
        {
           
        }
        protected override async Task OnInitializedAsync()
        {

            //this.WhenAnyObservable(x => x.ViewModel.RoomTitles.CountChanged).
            //    ObserveOn(RxApp.MainThreadScheduler).
            //   //Throttle(TimeSpan.FromMilliseconds(1), RxApp.MainThreadScheduler).
            //   Subscribe(x => StateHasChanged());
            await ViewModel!.LoadRooms();
        }
    }
}
