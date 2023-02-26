using Ardalis.Result;
using ContentRate.Application.ContentServices;
using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Rooms;
using ContentRate.Domain.Rooms;
using ContentRate.ViewModels.Content;
using ContentRate.ViewModels.Users;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace ContentRate.ViewModels.Rooms
{
    /// <summary>
    /// ViewModel для редактирования комнаты, для инициализации нужно вызвать LoadRoom или CreateNewRoom
    /// </summary>
    public class RoomEditViewModel : ViewModelBase
    {
        public RoomEditViewModel(IRoomService roomService)
        {
            this.roomService = roomService;
            Content = new();
            MockAssessors = new();
            SaveRoomCommand = ReactiveCommand.CreateFromTask(Save);
            RemoveAssessorCommand = ReactiveCommand.Create<Guid>(RemoveAssessor);
            AddAssessorCommand = ReactiveCommand.Create(AddAssessor);
            AddContentCommand = ReactiveCommand.Create(CreateNewContent);
            RemoveContentCommand = ReactiveCommand.Create<Guid>(RemoveContent);
            ImportContentCommand = ReactiveCommand.CreateFromTask<IContentImporter>(ImportContent);
            room = new RoomUpdate() { Id = Guid.NewGuid(), Name = "" };
        }
        private RoomUpdate room;
        private readonly IRoomService roomService;
        private bool isNewRoom = true;
        public ReactiveCommand<Unit, Unit> SaveRoomCommand { get; }
        public ReactiveCommand<Unit, Unit> AddContentCommand { get; }
        public ReactiveCommand<Unit, Unit> AddAssessorCommand { get; }
        public ReactiveCommand<IContentImporter, Unit> ImportContentCommand { get; }
        public ReactiveCommand<Guid, Unit> RemoveAssessorCommand { get; }
        public ReactiveCommand<Guid, Unit> RemoveContentCommand { get; }
        public string? Password
        {
            get => room.Password;
            set
            {
                room.Password = value;
                this.RaisePropertyChanged();
            }
        }
        public bool IsPrivate
        {
            get => room.IsPrivate;
            set
            {
                room.IsPrivate = value;
                this.RaisePropertyChanged();
            }
        }
        public string Name
        {
            get => room.Name;
            set
            {
                room.Name = value;
                this.RaisePropertyChanged();
            }
        }
        public ObservableCollection<ContentViewModel> Content { get; private set; }
        public ObservableCollection<AssessorTitleViewModel> MockAssessors { get; private set; }
        public async Task LoadRoom(Guid roomId)
        {
            var roomResult = await roomService.OpenRoomToUpdate(roomId);
            if (!roomResult.IsSuccess)
                throw new Exception($"Can't find room by id {roomId}");
            InitViewModel(roomResult.Value);
            isNewRoom = false;
        }
        public void CreateNewRoom(Guid creatorId)
        {
            InitViewModel(new RoomUpdate() { Id = creatorId, Name = "" });
            isNewRoom = true;
        }
        private void InitViewModel(RoomUpdate roomUpdate)
        {
            room = roomUpdate;
            MockAssessors.Clear();
            MockAssessors.AddRange(roomUpdate.Assessors.Select(c => new AssessorTitleViewModel(c)).Where(c => c.IsMock));

            Content.Clear();
            Content.AddRange(roomUpdate.Content.Select(c => new ContentViewModel(c)));
        }
        #region commandMethods
        private async Task Save()
        {
            Result result = isNewRoom ? await roomService.CreateRoom(room) : await roomService.UpdateRoom(room);
            if (!result.IsSuccess)
                throw new Exception($"Can't save room {string.Join(",", result.Errors)}");

        }
        private async Task ImportContent(IContentImporter contentImporter)
        {
            var importResult = await contentImporter.Import();
            if (!importResult.IsSuccess)
                throw new Exception(importResult.Errors.FirstOrDefault() ?? string.Empty);
            foreach (var contentImport in importResult.Value)
            {
                if (Content.Any(c => c.Name == contentImport.Name && c.Path == contentImport.Path))
                    continue;
                AddContent(new ContentDetails
                {
                    ContentType = contentImport.ContentType,
                    Id = Guid.NewGuid(),
                    Name = contentImport.Name,
                    Path = contentImport.Path,
                });
            }
        }
        private void CreateNewContent()
        {
            var content = new ContentDetails() { Id = Guid.NewGuid(), Name = "", Path = "" };
            AddContent(content);
        }

        private void AddContent(ContentDetails content)
        {
            foreach (var assessorId in MockAssessors.Select(c => c.Id))
                content.Ratings.Add(new ContentRating() { AssessorId = assessorId, Value = 0 });
            room.Content.Add(content);
            Content.Insert(0, new ContentViewModel(content));
        }

        private void AddAssessor()
        {
            var assessor = new AssessorTitle() { Id = Guid.NewGuid(), Name = "", IsMock = true };
            MockAssessors.Add(new AssessorTitleViewModel(assessor));
            room.Assessors.Add(assessor);
            foreach (var content in Content)
                content.AddRating(new ContentRating() { AssessorId = Guid.NewGuid(), Value = 0 });
        }
        private void RemoveContent(Guid contentId)
        {
            var content = Content.FirstOrDefault(c => c.Id == contentId);
            if (content is null)
                return;
            Content.Remove(content);
            var roomDelete = room.Content.Find(c => c.Id == contentId);
            if (roomDelete is not null)
                room.Content.Remove(roomDelete);

        }
        private void RemoveAssessor(Guid assessorId)
        {
            var assessor = MockAssessors.FirstOrDefault(c => c.Id == assessorId);
            if (assessor is null || !assessor.IsMock)
                return;
            foreach (var content in Content)
                content.RemoveRating(assessorId);
            MockAssessors.Remove(assessor);
            var assessorDelete = room.Assessors.Find(c => c.Id == assessorId);
            if (assessorDelete is not null)
                room.Assessors.Remove(assessorDelete);
        }
        #endregion
    }
}
