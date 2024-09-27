using System;
using Microsoft.AspNetCore.Identity;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;

// Lưu ý namespace phải giống nhau.
namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     This UnitOfWork file is only contain the Common Repositories.
///     Please do not add any Feature Repositories in this UnitOfWork file.
/// </summary>
public sealed partial class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private readonly Lazy<UserManager<PgUser>> _userManager;
    private readonly Lazy<RoleManager<PgRole>> _roleManager;

    public UnitOfWork(
        AppDbContext dbContext,
        Lazy<UserManager<PgUser>> userManager,
        Lazy<RoleManager<PgRole>> roleManager
    )
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }
}
