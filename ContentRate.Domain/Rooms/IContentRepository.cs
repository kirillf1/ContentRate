namespace ContentRate.Domain.Rooms
{
    public interface IContentRepository
    {
        public Task<Content> GetById(Guid id);
        public Task UpdateContent(Content content);
        public Task SaveChanges(CancellationToken cancellationToken = default);
    }
}
