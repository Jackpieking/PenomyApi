using Microsoft.AspNetCore.Identity;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
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
    private UserManager<PgUser> _userManager;
    private RoleManager<PgRole> _roleManager;

    public UnitOfWork(
        AppDbContext dbContext,
        UserManager<PgUser> userManager,
        RoleManager<PgRole> roleManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }
}
