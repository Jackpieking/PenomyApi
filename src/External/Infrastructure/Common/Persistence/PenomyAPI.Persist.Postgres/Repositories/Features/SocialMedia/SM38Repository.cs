using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class SM38Repository : ISM38Repository
{
    private readonly DbSet<SocialGroup> _socialGroupDbSet;
    private readonly DbSet<SocialGroupMember> _socialGroupMemberDbSet;

    public SM38Repository(DbContext dbContext)
    {
        _socialGroupDbSet = dbContext.Set<SocialGroup>();
        _socialGroupMemberDbSet = dbContext.Set<SocialGroupMember>();
    }

    /// <summary>
    /// Updates the cover photo of the specified group if the user has the necessary permissions.
    /// </summary>
    /// <param name="userId">The ID of the user attempting to update the cover photo.</param>
    /// <param name="groupId">The ID of the group whose cover photo is to be updated.</param>
    /// <param name="coverPhotoUrl">The new cover photo URL.</param>
    /// <returns>A task representing the asynchronous operation, containing the number of affected records.</returns>
    public async Task<int> UpdateGroupCoverPhotoAsync(
        long userId,
        long groupId,
        string coverPhotoUrl
    )
    {
        try
        {
            // Check if the user is a member of the group
            var user = _socialGroupMemberDbSet
                .Where(gm => gm.MemberId == userId && gm.GroupId == groupId)
                .FirstOrDefault();

            // Validate user role and permissions
            if (user == null || user.RoleId != 1)
                return 0;

            // Update the cover photo URL for the group
            return await _socialGroupDbSet
                .Where(gr => gr.Id == groupId)
                .ExecuteUpdateAsync(record =>
                    record.SetProperty(g => g.CoverPhotoUrl, coverPhotoUrl)
                );
        }
        catch
        {
            // Return 0 if an error occurs
            return 0;
        }
    }

    /// <summary>
    ///     Updates the detail of the group with the given ID.
    /// </summary>
    /// <param name="userId">The ID of the user who is performing the update.</param>
    /// <param name="groupId">The ID of the group to update.</param>
    /// <param name="name">The new name of the group.</param>
    /// <param name="description">The new description of the group.</param>
    /// <param name="RequireApprovedWhenPost">Indicates whether the group requires approval when posting.</param>
    /// <returns>The number of rows affected if the update was successful, otherwise <see cref="null"/>.</returns>
    public async Task<int> UpdateGroupDetailAsync(
        long userId,
        long groupId,
        string name,
        string description,
        bool RequireApprovedWhenPost
    )
    {
        int result = 0;
        try
        {
            // Check user permission
            var user = _socialGroupMemberDbSet
                .Where(gm => gm.MemberId == userId && gm.GroupId == groupId)
                .FirstOrDefault();
            if (user == null)
                return 0;
            else if (user.RoleId != 1)
                return 0;

            if (name != "")
                result = await _socialGroupDbSet
                    .Where(gr => gr.Id == groupId)
                    .ExecuteUpdateAsync(record => record.SetProperty(g => g.Name, name));

            result = await _socialGroupDbSet
                .Where(gr => gr.Id == groupId)
                .ExecuteUpdateAsync(record =>
                    record
                        .SetProperty(g => g.Description, description)
                        .SetProperty(g => g.RequireApprovedWhenPost, RequireApprovedWhenPost)
                );

            return result;
        }
        catch
        {
            return 0;
        }
    }
}
