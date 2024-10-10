using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G52Repository : IG52Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkComment> _artworkCommentDbSet;
    private readonly DbSet<ArtworkCommentParentChild> _commentParentChildDbSet;
    private readonly DbSet<ArtworkCommentReference> _commentReferenceDbSet;

    public G52Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _commentParentChildDbSet = dbContext.Set<ArtworkCommentParentChild>();
        _artworkCommentDbSet = dbContext.Set<ArtworkComment>();
        _commentReferenceDbSet = dbContext.Set<ArtworkCommentReference>();
    }

    public async Task<long> CreateCommentAsync(ArtworkComment Comment)
    {
        //var option = new SnowflakeIdOptions();
        //var Generator = new AppSnowflakeIdGenerator(option);
        Comment.Id = 123123123;
        try
        {
            _artworkCommentDbSet.Add(Comment);
            await _dbContext.SaveChangesAsync();
            return Comment.Id;
        }
        catch
        {
            return 0;
        }
    }
}
