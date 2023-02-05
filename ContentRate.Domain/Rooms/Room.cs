using ContentRate.Domain.Users;

namespace ContentRate.Domain.Rooms
{
    public class Room
    {
        // Не забудь добавить соостояние комнаты, типо Completed
        public Room(Guid id, string name, RoomDetails roomDetails, IEnumerable<Assessor> assessors, IEnumerable<Content> contentCollection)
        {
            Id = id;
            Name = name;
            RoomDetails = roomDetails;
            _contentList = new();
            _assessors = new List<Assessor>(assessors);
            AddContentRange(contentCollection);
        }

        public Guid Id { get; }
        public string Name { get; set; }
        public RoomDetails RoomDetails { get; }
        private List<Content> _contentList;
        public IReadOnlyCollection<Content> ContentList { get => _contentList; }
        private List<Assessor> _assessors;
        public IReadOnlyCollection<Assessor> Assessors { get  => _assessors;  }
        public void AddContent(Content content)
        {
            if (_contentList.Any(c => c.Id == content.Id))
                return;
            foreach (var userId in _assessors.Select(c=>c.Id))
                content.AddRating(new Rating(userId, 0));
            _contentList.Add(content);
        }
        //todo оптимизировать
        public void AddContentRange(IEnumerable<Content> contentCollection)
        {
            foreach (var content in contentCollection)
            {
                AddContent(content);
            }
        }
        public void RemoveContent(IEnumerable<Content> contentToDelete)
        {
            foreach (var content in contentToDelete)
            {
                var findedContent = _contentList.Find(c => c.Id == content.Id);
                if (findedContent is not null)
                {
                    _contentList.Remove(findedContent);
                }
            }
        }
        public void AssessorJoin(Assessor assessor)
        {
            if (Assessors.Any(c=>c.Id == assessor.Id))
                return;
            _assessors.Add(assessor);
            foreach (var content in ContentList)
            {
                content.AddRating(new Rating(assessor.Id, 0));
            }
        }
        public void AssessorLeave(Guid assessor)
        {
            var userForLeave = Assessors.FirstOrDefault(c => c.Id == assessor);
            if (userForLeave is null)
                return;
            if (RoomDetails.CreatorId == assessor)
                throw new ArgumentException("Creator can't leave own room!");
            _assessors.Remove(userForLeave);
            foreach (var content in ContentList)
            {
                content.DeleteRatingByUser(assessor);
            }
        }
    }
}
