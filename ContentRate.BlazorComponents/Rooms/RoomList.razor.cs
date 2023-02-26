using ContentRate.ViewModels.Rooms;
using Microsoft.AspNetCore.Components;
using ReactiveUI;
using System.Reactive.Disposables;

namespace ContentRate.BlazorComponents.Rooms
{
    public partial class RoomList
    {      
        [Inject]
        public RoomListViewModel RoomViewModel
        {
            get => ViewModel!;
            set => ViewModel = value;

        }
        public bool CanSearch
        {
            get => canSearch;
            set
            {
                canSearch = !value;
                StateHasChanged();
            }
        }
        private bool canSearch;
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
