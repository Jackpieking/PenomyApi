﻿using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation
{
    public interface IArt7Repository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(
        CancellationToken cancellationToken);

        Task<IEnumerable<ArtworkOrigin>> GetAllOriginsAsync(
            CancellationToken cancellationToken);

        Task<Artwork> GetComicDetailByIdAsync(
            long comicId,
            CancellationToken cancellationToken);

        Task<bool> UpdateComicAsync(
            Artwork comic,
            IEnumerable<ArtworkCategory> artworkCategories,
            CancellationToken cancellationToken);
    }
}