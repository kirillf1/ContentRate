namespace ContentRate.UnitTests.Helpers
{
    public static class RoomFactory
    {
        public static Room CreateRoom(Guid id,int contentCount, Guid? creatorId = null,bool isPrivate=false)
        {
            var assessor = AssessorFactory.CreateAssessor(creatorId);
            var details = new RoomDetails(assessor.Id, isPrivate);
            var content = Enumerable.Repeat(() => ContentFactory.CreateContent(Guid.NewGuid()), contentCount);
            return new Room(id, id.ToString(),details,new List<Assessor> {assessor },content.Select(c=>c.Invoke()));
        }
    }
}
