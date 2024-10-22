using CatchUpPlatform.API.News.Domain.Model.Aggregates;
using CatchUpPlatform.API.News.Domain.Repositories;
using CatchUpPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using CatchUpPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CatchUpPlatform.API.News.Infrastructure.Repositories
{
    public class FavoriteSourceRepository(AppDbContext context) : BaseRepository<FavoriteSource>(context), IFavoriteSourceRepository
    {
        /// <inheritdoc />
        public async Task<IEnumerable<FavoriteSource>> FindByNewsApiKeyAsync(string newsApiKey)
        {
            return await context.Set<FavoriteSource>().Where(f => f.NewsApiKey == newsApiKey).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<FavoriteSource?> FindByNewsApiKeyAndSourceIdAsync(string newsApiKey, string sourceId)
        {
            return await context.Set<FavoriteSource>().FirstOrDefaultAsync(f => f.NewsApiKey == newsApiKey && f.SourceId == sourceId);
        }
    }
}
