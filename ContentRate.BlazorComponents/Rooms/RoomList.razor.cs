using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Users;
using ContentRate.ViewModels.Rooms;
using Microsoft.AspNetCore.Components;
using ReactiveUI;
using System.Reactive.Disposables;

namespace ContentRate.BlazorComponents.Rooms
{
    public partial class RoomList
    {
        [Inject]
        IUserContext UserContext { get; set; } = default!;
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
        // если будет много логики проверки то можно создать отдельный сервис, а не подгружать пользователя
        private UserTitle? userTitle;
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
            await RoomViewModel.LoadRooms();
            userTitle = await UserContext.TryGetCurrentUser();
            await base.OnInitializedAsync();
        }
        private bool CanEditRoom(Guid creatorId)
        {
            if (userTitle is null)
                return false;
            return userTitle.Id == creatorId;
        }
    }
}
