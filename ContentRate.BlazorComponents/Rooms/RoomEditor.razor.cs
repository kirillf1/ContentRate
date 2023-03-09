using ContentRate.Application.ContentServices;
using ContentRate.Application.Users;
using ContentRate.Domain.Rooms;
using ContentRate.ViewModels.Rooms;
using DynamicData;
using DynamicData.Binding;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace ContentRate.BlazorComponents.Rooms
{
    public partial class RoomEditor
    {
        private static readonly Dictionary<ContentType, string> ContentNames = new()
        {
            {ContentType.Audio, "Аудио" },
            {ContentType.Video, "Видео" },
            {ContentType.Image, "Фото" }
        };
        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;
        [Inject]
        public IUserContext UserContext { get; set; } = default!;
        public RoomEditor()
        {
            this.WhenActivated(disposableRegistration =>
            {
                this.EditViewModel.Content.ToObservableChangeSet()
                .ToCollection()
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => StateHasChanged())
                .DisposeWith(disposableRegistration);

                this.EditViewModel.MockAssessors.ToObservableChangeSet()
               .ToCollection()
               .Throttle(TimeSpan.FromMilliseconds(100))
               .Subscribe(_ => StateHasChanged())
               .DisposeWith(disposableRegistration);
            });
        }
        [Inject]
        public RoomEditViewModel EditViewModel
        {
            get => ViewModel!;
            set => ViewModel = value;
        }
        [Parameter]
        public Guid? Id { get; set; }

        public string Title => Id is null ? "Создать комнату" : "Редактировать комнату";
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        bool isShow;
        protected async override Task OnInitializedAsync()
        {
            if (Id is null)
            {
                var user = await UserContext.TryGetCurrentUser();
                if (user is null)
                    throw new Exception("No user");
                EditViewModel.CreateNewRoom(user);
                return;
            }
            await EditViewModel.LoadRoom(Id.Value);
        }
        private void PasswordShowClicked()
        {
            if (isShow)
            {
                isShow = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isShow = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }
        private async Task ImportContent(IContentImporter contentImporter)
        {
            await EditViewModel.ImportContentCommand.Execute(contentImporter).ToTask();
        }
        private async Task RemoveRoom()
        {
           var isDeleted = await EditViewModel.RemoveRoomCommand.Execute().ToTask();
            if (isDeleted)
                NavigationManager.NavigateTo("/roomList");
        }
    }
}
