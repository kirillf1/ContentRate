using ContentRate.Domain.Rooms;
using ContentRate.Infrastructure.Contexts;
using ContentRate.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ContentRate.Infrastructure.Repositories.EfRepositories
{
    public class ContentRepositoryEf : IContentRepository
    {
        private readonly ContentRateDbContext context;

        public ContentRepositoryEf(ContentRateDbContext context)
        {
            this.context = context;
        }
        public async Task<Content> GetById(Guid id)
        {
            var contentModel = await context.Content.SingleAsync(c => c.Id == id);
            return ContentConventer.ConvertModelToContent(contentModel);
        }

        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateContent(Content content)
        {
            var contentModel = context.Content.Local.FirstOrDefault(c => c.Id == content.Id)
            ?? await context.Content.SingleAsync(c => c.Id == content.Id);
            ContentConventer.UpdateContentModel(contentModel, content);
            context.Content.Update(contentModel);
        }
    }
}
