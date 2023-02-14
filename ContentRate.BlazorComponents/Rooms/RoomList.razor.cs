using ContentRate.ViewModels.Rooms;
using Microsoft.AspNetCore.Components;
using ReactiveUI;
using System.Reactive.Disposables;

namespace ContentRate.BlazorComponents.Rooms
{
    public partial class RoomList
    {
        public bool CanSearch
        {
            get => canSearch;
            set
            {
                canSearch = value ? false : true;
                StateHasChanged();
            }
        }
        private bool canSearch;
        [Inject]
        public RoomListViewModel FetchViewModel
        {
            get => ViewModel!;
            set => ViewModel = value;

        }
        public RoomList()
        {
            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(ViewModel,
                        viewModel => viewModel.IsBusy,
                        view => view.CanSearch)
                    .DisposeWith(disposableRegistration);
                
            });
        }
        protected override async Task OnInitializedAsync()
        {
            await ViewModel!.LoadRooms();
            await base.OnInitializedAsync();
        }
    }
}
